using Kafe.Application.Dtos.CategoryDtos;
using Kafe.Application.Dtos.ResponseDto;

namespace Kafe.Application.Services.Abstract
{

    public interface ICategoryServices

    
    {

        
        Task<ResponseDto<List<ResultCategoryDto>>> GetAllCategoriesAsync();
        Task<ResponseDto<DetailCategoryDto>> GetCategoryByIdAsync(int id);
        Task<ResponseDto<object>> AddCategoryAsync(CreateCategoryDto dto);
        Task <ResponseDto<object>>UpdateCategoryAsync(UpdateCategoryDto dto);
        Task <ResponseDto<object>>DeleteCategoryAsync(int id);

     
    }
}