using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VeganStore.Models.Category;
using VeganStore.Models.Entities;
using VeganStore.Web.API.Data;
using VeganStore.Web.API.Filters;
using VeganStore.Web.API.Repository;

namespace VeganStore.Web.API.Controllers
{
    [ApiKeyAuth]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {       
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {           
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryModel>>> GetCategories()
        {
            var categories = new List<CategoryModel>();
            foreach (var item in await _categoryService.GetAllAsync())
            {
                categories.Add(new CategoryModel(item.Id,item.Name));
            }
            return categories;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryModel>> GetCategory(int id)
        {
            var categoryEntity = await _categoryService.GetByIdAsync(id);

            if (categoryEntity == null)
            {
                return NotFound();
            }

            return new CategoryModel(categoryEntity.Id, categoryEntity.Name);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory(int id, CategoryModel model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            var categoryEntity = await _categoryService.GetByIdAsync(model.Id);
            categoryEntity.Name = model.Name;
            
            try
            {
                await _categoryService.UpdateAsync(categoryEntity);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(id))
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
        public async Task<ActionResult<CategoryModel>> PostCategory(CategoryViewModel model)
        {
            if (await _categoryService.GetFirstWhereAsync(x => x.Name == model.Name) != null)
            {
                return Conflict();
            }

            var categoryEntity = new Category(model.Name);
            await _categoryService.AddAsync(categoryEntity);

            return new CategoryModel(categoryEntity.Id, categoryEntity.Name);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            await _categoryService.RemoveAsync(category);         
            return NoContent();
        }

        private bool CategoryExists(int id)
        {
            if(_categoryService.GetByIdAsync(id) != null)
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
