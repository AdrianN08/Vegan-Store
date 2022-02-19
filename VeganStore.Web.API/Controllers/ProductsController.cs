using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VeganStore.Models.Entities;
using VeganStore.Models.Product;
using VeganStore.Web.API.Filters;
using VeganStore.Web.API.Repository;

namespace VeganStore.Web.API.Controllers
{
    [ApiKeyAuth]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ISubCategoryService _subCategoryService;

        public ProductsController(IProductService productService, ISubCategoryService subCategoryService)
        {
            _productService = productService;
            _subCategoryService = subCategoryService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductViewModel>>> GetProducts(string category = null, string subcategory = null)
        {
            var products = new List<ProductViewModel>();
            IEnumerable<Product> items;
            if (category == null && subcategory == null)
            {
                items = await _productService.GetAllAsync();
            }
            else
            {
                items = await _productService.GetByCategoriesAsync(_category: category, _subcategory: subcategory);           
            }
            foreach (var i in items)
            {
                products.Add(new ProductViewModel(i.Id, i.Name, i.ArticleNumber, i.RegularPrice, i.SalePrice, i.SubCategory.Name, i.SubCategory.Category.Name));
            }
            return products;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductModel>> GetProduct(int id)
        {
            var product = await _productService.GetByIdAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return new ProductModel(product.Id, product.Name, product.ArticleNumber, product.RegularPrice, product.SalePrice, product.SubCategory.Name);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, ProductModel model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            var subCategory = await _subCategoryService.GetFirstWhereAsync(x => x.Name == model.SubCategoryName);
            var productEntity = await _productService.GetByIdAsync(id);
            productEntity.Name = model.Name;
            productEntity.ArticleNumber = model.ArticleNumber;
            productEntity.RegularPrice = model.RegularPrice;
            productEntity.SalePrice = model.SalePrice;
            productEntity.SubCategoryId = subCategory.Id;

            try
            {
                await _productService.UpdateAsync(productEntity);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<ProductModel>> PostProduct(ProductCreateModel model)
        {
            var subCategory = await _subCategoryService.GetFirstWhereAsync(x => x.Name == model.SubCategoryName);
            var productEntity = new Product(model.Name, model.ArticleNumber, model.RegularPrice, model.SalePrice, subCategory.Id);
            await _productService.AddAsync(productEntity);

            return new ProductModel(productEntity.Id, productEntity.Name, productEntity.ArticleNumber, productEntity.RegularPrice, productEntity.SalePrice, productEntity.SubCategory.Name);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            await _productService.RemoveAsync(product);
            return NoContent();
        }

        private bool ProductExists(int id)
        {
            if (_productService.GetByIdAsync(id) != null)
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
