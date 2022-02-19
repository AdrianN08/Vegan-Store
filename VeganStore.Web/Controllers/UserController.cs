using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VeganStore.Models.Entities;
using VeganStore.Utility;

namespace VeganStore.Web.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            
            return View();
        }
        public IActionResult Orders()
        {

            return View();
        }
        public IActionResult OrderDetails(int orderId)
        {

            return View();
        }
        #region API CALLS
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            IEnumerable<Order> orders = new List<Order>();
            using (var client = new HttpClient())
            {
                orders = await client.GetFromJsonAsync<IEnumerable<Order>>(SD.localHost + $"Orders?appUserId={claim.Value}&" + SD.ApiKey);
            }
            return Json(new { data = orders });
        }
        #endregion
    }

}
