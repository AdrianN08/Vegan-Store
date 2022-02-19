using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VeganStore.Models.Entities;
using VeganStore.Models.Product;
using VeganStore.Models.ShoppingCart;
using VeganStore.Web.API.Filters;
using VeganStore.Web.API.Repository;

namespace VeganStore.Web.API.Controllers
{
    [ApiKeyAuth]
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartsController : ControllerBase
    {
        private readonly IShoppingCartService _shoppingCartService;

        public ShoppingCartsController(IShoppingCartService shoppingCartService)
        {
            _shoppingCartService = shoppingCartService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShoppingCartModel>>> GetShoppingCarts(string appUserId = null)
        {
            var shoppingCarts = new List<ShoppingCartModel>();
            IEnumerable<ShoppingCart> items = new List<ShoppingCart>();
            if (appUserId != null)
            {
                items = await _shoppingCartService.GetAllWhereAsync(x => x.AppUserId == appUserId);
            }
            else
            {
                items = await _shoppingCartService.GetAllAsync();
            }
            foreach (var item in items)
            {
                shoppingCarts.Add(new ShoppingCartModel(item.Id, item.ProductId, item.Quantity, item.AppUserId, new ProductCartViewModel(item.Product.Name, item.Product.ArticleNumber, item.Product.SalePrice)));
            }

            return shoppingCarts;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ShoppingCartModel>> GetShoppingCart(int id)
        {
            var shoppingCart = await _shoppingCartService.GetByIdAsync(id);

            if (shoppingCart == null)
            {
                return NotFound();
            }

            return new ShoppingCartModel(shoppingCart.Id, shoppingCart.ProductId, shoppingCart.Quantity, shoppingCart.AppUserId, new ProductCartViewModel(shoppingCart.Product.Name, shoppingCart.Product.ArticleNumber, shoppingCart.Product.SalePrice));
        }

        [HttpGet("GetCurrent/{appUserId}&{productId}")]
        public async Task<ActionResult<ShoppingCartModel>> GetShoppingCartExists(string appUserId, int productId)
        {
            var shoppingCart = await _shoppingCartService.GetFirstWhereAsync(x => x.AppUserId == appUserId && x.ProductId == productId);

            if (shoppingCart == null)
            {
                string result = null;
                return Ok(result);
            }
            else
            {
                return new ShoppingCartModel(shoppingCart.Id, shoppingCart.ProductId, shoppingCart.Quantity, shoppingCart.AppUserId, new ProductCartViewModel(shoppingCart.Product.Name, shoppingCart.Product.ArticleNumber, shoppingCart.Product.SalePrice));
            }

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutShoppingCart(int id, ShoppingCartModel model, int increment = 0, int decrement = 0)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }
            var shoppingCart = await _shoppingCartService.GetByIdAsync(id);

            if (increment > 0 || decrement > 0)
            {
                if (increment > 0)
                {
                    await _shoppingCartService.IncrementCount(shoppingCart, increment);
                }
                if (decrement > 0)
                {
                    await _shoppingCartService.DecrementCount(shoppingCart, decrement);
                }
            }
            else
            {
                try
                {
                    await _shoppingCartService.UpdateAsync(shoppingCart);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShoppingCartExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<ShoppingCartModel>> PostShoppingCart(ShoppingCartModel model)
        {
            var shoppingCart = new ShoppingCart(model.ProductId, model.Quantity, model.AppUserId);
            await _shoppingCartService.AddAsync(shoppingCart);
            var shoppinCartAdded = await _shoppingCartService.GetFirstWhereAsync(x => x.ProductId == model.ProductId && x.AppUserId == model.AppUserId);

            return new ShoppingCartModel(shoppinCartAdded.Id, model.ProductId, model.Quantity, model.AppUserId, new ProductCartViewModel(shoppinCartAdded.Product.Name, shoppinCartAdded.Product.ArticleNumber, shoppinCartAdded.Product.SalePrice));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShoppingCart(int id)
        {
            var shoppingCart = await _shoppingCartService.GetByIdAsync(id);
            if (shoppingCart == null)
            {
                return NotFound();
            }
            await _shoppingCartService.RemoveAsync(shoppingCart);

            return NoContent();
        }

        private bool ShoppingCartExists(int id)
        {
            if (_shoppingCartService.GetByIdAsync(id) != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
