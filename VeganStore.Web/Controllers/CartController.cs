using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using System.Security.Claims;
using VeganStore.Models.Entities;
using VeganStore.Models.ShoppingCart;
using VeganStore.Utility;
using VeganStore.Web.Models;

namespace VeganStore.Web.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly AppDbContext _db;

        [BindProperty]
        public ShoppingCartViewModel ShoppingCartVM { get; set; }
        public int OrderTotal { get; set; }
        public CartController(AppDbContext db)
        {
            _db = db;
        }

        #region CartIndex
        public async Task<IActionResult> Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            IEnumerable<ShoppingCartModel> viewModel = new List<ShoppingCartModel>();

            using (var client = new HttpClient())
            {
                viewModel = await client.GetFromJsonAsync<IEnumerable<ShoppingCartModel>>(SD.localHost + $"ShoppingCarts?appUserId={claim.Value}&" + SD.ApiKey);
            }
            ShoppingCartVM = new ShoppingCartViewModel()
            {
                Carts = viewModel,
                Order = new()
            };

            foreach (var cart in ShoppingCartVM.Carts)
            {
                cart.Price = cart.Product.SalePrice;
                ShoppingCartVM.Order.OrderTotal += (cart.Price * cart.Quantity);
            }

            return View(ShoppingCartVM);
        }
        #endregion

        #region Summary
        [HttpGet]
        public async Task<IActionResult> Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            IEnumerable<ShoppingCartModel> viewModel = new List<ShoppingCartModel>();

            using (var client = new HttpClient())
            {
                viewModel = await client.GetFromJsonAsync<IEnumerable<ShoppingCartModel>>(SD.localHost + $"ShoppingCarts?appUserId={claim.Value}&" + SD.ApiKey);
            }
            ShoppingCartVM = new ShoppingCartViewModel()
            {
                Carts = viewModel,
                Order = new()
            };

            AppUser appUser = new AppUser();
            appUser = _db.AppUsers.FirstOrDefault(x => x.Id == claim.Value);
            ShoppingCartVM.Order.FirstName = appUser.FirstName;
            ShoppingCartVM.Order.LastName = appUser.LastName;
            ShoppingCartVM.Order.PhoneNumber = appUser.PhoneNumber;
            ShoppingCartVM.Order.StreetAddress = appUser.StreetAddress;
            ShoppingCartVM.Order.City = appUser.City;
            ShoppingCartVM.Order.PostalCode = appUser.PostalCode;

            foreach (var cart in ShoppingCartVM.Carts)
            {
                cart.Price = cart.Product.SalePrice;
                ShoppingCartVM.Order.OrderTotal += (cart.Price * cart.Quantity);
            }
            return View(ShoppingCartVM);
        }
        [HttpPost]
        [ActionName("Summary")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SummaryPOST()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            IEnumerable<ShoppingCartModel> viewModel = new List<ShoppingCartModel>();

            using (var client = new HttpClient())
            {
                viewModel = await client.GetFromJsonAsync<IEnumerable<ShoppingCartModel>>(SD.localHost + $"ShoppingCarts?appUserId={claim.Value}&" + SD.ApiKey);
            }

            ShoppingCartVM.Carts = viewModel;
            ShoppingCartVM.Order.PaymentStatus = SD.PaymentStatusPending;
            ShoppingCartVM.Order.OrderStatus = SD.StatusPending;
            ShoppingCartVM.Order.OrderDate = DateTime.Now;
            ShoppingCartVM.Order.AppUserId = claim.Value;

            foreach (var cart in ShoppingCartVM.Carts)
            {
                cart.Price = cart.Product.SalePrice;
                ShoppingCartVM.Order.OrderTotal += (cart.Price * cart.Quantity);
            }

            string orderId;
            using (var client = new HttpClient())
            {
                var add = await client.PostAsJsonAsync(SD.localHost + "Orders?" + SD.ApiKey, ShoppingCartVM.Order);
                var result = add.Content.ReadAsStringAsync();
                orderId = result.Result;
            }
            ShoppingCartVM.Order.Id = int.Parse(orderId);

            foreach (var cart in ShoppingCartVM.Carts)
            {
                OrderDetail orderDetail = new()
                {
                    ProductId = cart.ProductId,
                    OrderId = ShoppingCartVM.Order.Id,
                    Price = cart.Price,
                    Quantity = cart.Quantity
                };
                using (var client = new HttpClient())
                {
                    var add = await client.PostAsJsonAsync(SD.localHost + "OrderDetails?" + SD.ApiKey, orderDetail);
                }
            }

            var domain = "https://localhost:44302/";
            var options = new SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                SuccessUrl = domain + $"cart/OrderConfirmation?id={int.Parse(orderId)}",
                CancelUrl = domain + $"cart/index",
            };

            foreach (var item in ShoppingCartVM.Carts)
            {
                var sessionLineItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.Price * 100),
                        Currency = "sek",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Product.Name
                        },
                    },
                    Quantity = item.Quantity,
                };
                options.LineItems.Add(sessionLineItem);
            }
            var service = new SessionService();
            Session session = service.Create(options);
            ShoppingCartVM.Order.SessionId = session.Id;
            ShoppingCartVM.Order.PaymentIntentId = session.PaymentIntentId;

            using (var client = new HttpClient())
            {
                var update = await client.PutAsJsonAsync(SD.localHost + $"Orders/{ShoppingCartVM.Order.Id}?" + SD.ApiKey, ShoppingCartVM.Order);
            }
   
            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);

        }
        #endregion

        #region OrderConfirmation
        public async Task<IActionResult> OrderConfirmation(int id)
        {          
            Order order = new Order();
            using (var client = new HttpClient())
            {
                order = await client.GetFromJsonAsync<Order>(SD.localHost + $"Orders/{id}?" + SD.ApiKey);
            }
            var service = new SessionService();
            Session session = service.Get(order.SessionId);

            if (session.PaymentStatus.ToLower() == "paid")
            {
                order.OrderStatus = SD.StatusApproved;
                order.PaymentStatus = SD.PaymentStatusApproved;

                using (var client = new HttpClient())
                {
                    var update = await client.PutAsJsonAsync(SD.localHost + $"Orders/{id}?" + SD.ApiKey, order);
                }
            }
            IEnumerable<ShoppingCartModel> shoppingCarts = new List<ShoppingCartModel>();
            using (var client = new HttpClient())
            {
                shoppingCarts = await client.GetFromJsonAsync<IEnumerable<ShoppingCartModel>>(SD.localHost + $"ShoppingCarts?appUserId={order.AppUserId}&" + SD.ApiKey);
                foreach (var c in shoppingCarts)
                {
                    var remove = await client.DeleteAsync(SD.localHost + $"ShoppingCarts/{c.Id}?" + SD.ApiKey);
                }
            }

            return View(id);
        }
        #endregion

        #region CartCount
        public async Task<IActionResult> Plus(int cartId)
        {
            using (var client = new HttpClient())
            {
                var cart = await client.GetFromJsonAsync<ShoppingCartModel>(SD.localHost + $"ShoppingCarts/{cartId}?" + SD.ApiKey);
                var increment = await client.PutAsJsonAsync(SD.localHost + $"ShoppingCarts/{cartId}?increment=1&" + SD.ApiKey, cart);
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Minus(int cartId)
        {
            using (var client = new HttpClient())
            {
                var cart = await client.GetFromJsonAsync<ShoppingCartModel>(SD.localHost + $"ShoppingCarts/{cartId}?" + SD.ApiKey);
                if (cart.Quantity <= 1)
                {
                    var remove = await client.DeleteAsync(SD.localHost + $"ShoppingCarts/{cartId}?" + SD.ApiKey);
                }
                else
                {
                    var decrement = await client.PutAsJsonAsync(SD.localHost + $"ShoppingCarts/{cartId}?decrement=1&" + SD.ApiKey, cart);
                }
            }
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Remove(int cartId)
        {
            using (var client = new HttpClient())
            {
                var result = await client.DeleteAsync(SD.localHost + $"ShoppingCarts/{cartId}?" + SD.ApiKey);
            }
            return RedirectToAction(nameof(Index));
        }
        #endregion
    }
}
