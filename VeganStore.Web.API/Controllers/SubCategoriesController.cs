using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VeganStore.Models.Entities;
using VeganStore.Models.SubCategory;
using VeganStore.Web.API.Data;
using VeganStore.Web.API.Filters;
using VeganStore.Web.API.Repository;

namespace VeganStore.Web.API.Controllers
{
    [ApiKeyAuth]
    [Route("api/[controller]")]
    [ApiController]
    public class SubCategoriesController : ControllerBase
    {       
        private readonly ISubCategoryService _subCategoryService;
        private readonly ICategoryService _categoryService;

        public SubCategoriesController(ISubCategoryService subCategoryService, ICategoryService categoryService)
        {
            _subCategoryService = subCategoryService;
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SubCategoryModel>>> GetSubCategories(string category = null)
        {
            var subCategories = new List<SubCategoryModel>();
            IEnumerable<SubCategory> items;
            if (category == null)
            {
                items = await _subCategoryService.GetAllAsync();
            }
            else
            {
                items = await _subCategoryService.GetByCategoriesAsync(_category: category);
            }
            foreach (var i in items)
            {
                subCategories.Add(new SubCategoryModel(i.Id,i.Name, i.Category.Name));
            }
            return subCategories;
        }

        [HttpGet("GetByName/{name}")]
        public async Task<ActionResult<SubCategoryModel>> GetSubCategoryByName(string name)
        {
            var subCategoryEntity = await _subCategoryService.GetFirstWhereAsync(x=> x.Name == name);

            if (subCategoryEntity == null)
            {
                return NotFound();
            }

            return new SubCategoryModel(subCategoryEntity.Id, subCategoryEntity.Name, subCategoryEntity.Category.Name);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SubCategoryModel>> GetSubCategoryById(int id)
        {
            var subCategoryEntity = await _subCategoryService.GetByIdAsync(id);

            if (subCategoryEntity == null)
            {
                return NotFound();
            }

            return new SubCategoryModel(subCategoryEntity.Id, subCategoryEntity.Name, subCategoryEntity.Category.Name);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutSubCategory(int id, SubCategoryModel model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }
            var category = await _categoryService.GetFirstWhereAsync(x => x.Name == model.CategoryName);
            var subCategoryEntity = await _subCategoryService.GetByIdAsync(id);
            subCategoryEntity.Name = model.Name;
            subCategoryEntity.CategoryId = category.Id;

            try
            {
                await _subCategoryService.UpdateAsync(subCategoryEntity);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubCategoryExists(id))
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
        public async Task<ActionResult<SubCategoryModel>> PostSubCategory(SubCategoryViewModel model)
        {
            if (await _subCategoryService.GetFirstWhereAsync(x => x.Name == model.Name) != null)
            {
                return Conflict();
            }
            var category = await _categoryService.GetFirstWhereAsync(x => x.Name == model.CategoryName);
            var subCategoryEntity = new SubCategory(model.Name, category.Id);
            await _subCategoryService.AddAsync(subCategoryEntity);

            return new SubCategoryModel(subCategoryEntity.Id, subCategoryEntity.Name, subCategoryEntity.Category.Name);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubCategory(int id)
        {
            var subCategory = await _subCategoryService.GetByIdAsync(id);
            if (subCategory == null)
            {
                return NotFound();
            }

            await _subCategoryService.RemoveAsync(subCategory);
            return NoContent();
        }

        private bool SubCategoryExists(int id)
        {
            if (_subCategoryService.GetByIdAsync(id) != null)
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
