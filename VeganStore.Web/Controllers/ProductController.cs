using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using System.Net;
using System.Security.Claims;
using VeganStore.Models.Entities;
using VeganStore.Models.Product;
using VeganStore.Models.ShoppingCart;
using VeganStore.Models.SubCategory;
using VeganStore.Utility;
using VeganStore.Web.Models.ViewModels;

namespace VeganStore.Web.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {

        public ProductController()
        {

        }

        #region IndexPage[Anonymous]
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            IEnumerable<ProductViewModel> viewModel = new List<ProductViewModel>();
            using (var client = new HttpClient())
            {
                viewModel = await client.GetFromJsonAsync<IEnumerable<ProductViewModel>>(SD.localHost + "Products?" + SD.ApiKey);
            }
            return View(viewModel);
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        #endregion

        #region ProductCategory
        [HttpGet]
        public async Task<IActionResult> ProductCategory(string category)
        {
            IEnumerable<ProductViewModel> viewModel = new List<ProductViewModel>();
            IEnumerable<SubCategoryModel> viewData = new List<SubCategoryModel>();
            using (var client = new HttpClient())
            {
                viewModel = await client.GetFromJsonAsync<IEnumerable<ProductViewModel>>(SD.localHost + $"Products?category={category}&" + SD.ApiKey);
                viewData = await client.GetFromJsonAsync<IEnumerable<SubCategoryModel>>(SD.localHost + $"SubCategories?category={category}&" + SD.ApiKey);
            }
            ViewData["SubCategoryName"] = new SelectList(viewData, "Name", "Name");
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> ProductCategory(string category, string subCategory)
        {
            IEnumerable<ProductViewModel> viewModel = new List<ProductViewModel>();
            IEnumerable<SubCategoryModel> viewData = new List<SubCategoryModel>();
            using (var client = new HttpClient())
            {
                viewModel = await client.GetFromJsonAsync<IEnumerable<ProductViewModel>>(SD.localHost + $"Products?category={category}&subcategory={subCategory}&" + SD.ApiKey);
                viewData = await client.GetFromJsonAsync<IEnumerable<SubCategoryModel>>(SD.localHost + $"SubCategories?category={category}&" + SD.ApiKey);
            }
            ViewData["SubCategoryName"] = new SelectList(viewData, "Name", "Name");
            return View(viewModel);
        }
        #endregion

        #region Details
        [HttpGet]
        public async Task<IActionResult> Details(int productId)
        {
            ShoppingCart cart = new ShoppingCart();
            ProductModel product = new ProductModel();
            SubCategoryModel subCategory = new SubCategoryModel();
            using (var client = new HttpClient())
            {
                product = await client.GetFromJsonAsync<ProductModel>(SD.localHost + "Products/" + productId + "?" + SD.ApiKey);
                subCategory = await client.GetFromJsonAsync<SubCategoryModel>(SD.localHost + $"SubCategories/GetByName/{product.SubCategoryName}?" + SD.ApiKey);
            }
            cart.Product = new Product(product.Id,product.Name, product.ArticleNumber , product.RegularPrice, product.SalePrice, subCategory.Id);
            cart.ProductId = product.Id;
            cart.Product.SubCategory = new SubCategory(subCategory.Id,subCategory.Name);
            cart.Quantity = 1;
            

            return View(cart);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details(ShoppingCartModel shoppingCart)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            shoppingCart.AppUserId = claim.Value;

            using (var client = new HttpClient())
            {
                var task = client.GetAsync(SD.localHost + $"ShoppingCarts/GetCurrent/{claim.Value}&{shoppingCart.ProductId}?" + SD.ApiKey);
                var result = task.Result;
                if (result.StatusCode == HttpStatusCode.NoContent)
                {
                    var add = await client.PostAsJsonAsync(SD.localHost + "ShoppingCarts?" + SD.ApiKey, shoppingCart);
                }
                else
                {
                    var cartFromDb = await client.GetFromJsonAsync<ShoppingCartModel>(SD.localHost + $"ShoppingCarts/GetCurrent/{claim.Value}&{shoppingCart.ProductId}?" + SD.ApiKey);
                    var update = await client.PutAsJsonAsync(SD.localHost + $"ShoppingCarts/{cartFromDb.Id}?increment={shoppingCart.Quantity}&" + SD.ApiKey, cartFromDb);
                }
                TempData["success"] = "Produkt lades till i varukorg";
            }
            return RedirectToAction(nameof(Index));
        }
        #endregion

    }
}