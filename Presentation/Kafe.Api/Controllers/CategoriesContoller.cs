using Kafe.Application.Dtos.CategoryDtos;
using Kafe.Application.Services.Abstract;
using KafeAPI.Application.Dtos.ReponseDtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Kafe.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryServices _categoryServices;

        public CategoriesController(ICategoryServices categoryServices)
        {
            _categoryServices = categoryServices;
        }
        // Endpointler buraya
        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _categoryServices.GetAllCategoriesAsync();
            if (!categories.Success)
            {
                if (categories.ErrorCodes == ErrorCodes.NotFound)
                {
                    return Ok(categories); // 200 döner
                }
                return BadRequest(categories); // 400 döner
                
            }
            return Ok(categories);
        }

        [HttpGet("{id}")] // api/categories/1 çakışma olmasın diye id ekledik
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var category = await _categoryServices.GetCategoryByIdAsync(id);
            if (!category.Success)
            {
                if (category.ErrorCodes == ErrorCodes.NotFound)
                {
                    return Ok(category); // 200 döner
                }
                return BadRequest(category); // 400 döner
                
            }
            return Ok(category);
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory(CreateCategoryDto dto)
        {
            var result = await _categoryServices.AddCategoryAsync(dto);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            
            return Ok(result);
        }

        [HttpPut] // Güncelleme işlemi için PUT kullanılır
        public async Task<IActionResult> UpdateCategory(UpdateCategoryDto dto)
        {   
            var result = await _categoryServices.UpdateCategoryAsync(dto);
            if (!result.Success)
            {
                if (result.ErrorCodes == ErrorCodes.NotFound)
                {
                    return Ok(result); // 200 döner
                }
                return BadRequest(result); // 400 döner
                
            }
                return Ok(result);
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteCategory(int id)
            {
                var result = await _categoryServices.DeleteCategoryAsync(id);
                
                if (!result.Success)
                {
                    if (result.ErrorCodes == ErrorCodes.NotFound)
                    {
                        return Ok(result);
                    }

                    return BadRequest(result);
                }

                return Ok(result);
            }

        

    }
}
