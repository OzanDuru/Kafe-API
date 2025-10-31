
using AutoMapper;
using Kafe.Application.Dtos.CategoryDtos;
using Kafe.Application.Dtos.ResponseDto;
using Kafe.Application.Interfaces;
using Kafe.Application.Services.Abstract;
using Kafe.Domain.Entities;
using KafeAPI.Application.Dtos.ReponseDtos;

namespace Kafe.Application.Services.Concrete
{
    public class CategoryServices : ICategoryServices
    {
        private readonly IGenericRepository<Category> _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryServices(IGenericRepository<Category> categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper= mapper;
        }

        public async Task<ResponseDto<List<ResultCategoryDto>>> GetAllCategoriesAsync()
        {
            try
            {
                var categories = await _categoryRepository.GetAllAsync();
                if (categories is null || !categories.Any())
                {
                    return new ResponseDto<List<ResultCategoryDto>>
                        {
                            Success = false,
                            Message = "No categories found",
                            ErrorCodes = ErrorCodes.NotFound

                        };
                }
                var result = _mapper.Map<List<ResultCategoryDto>>(categories);
                    return new ResponseDto<List<ResultCategoryDto>>
                        {
                            Success = true,
                            Message = "Categories retrieved successfully",
                            Data = result
                        };
                  

                
            }
            catch (Exception ex)
            {
                return new ResponseDto<List<ResultCategoryDto>>
                {
                    Success = false,
                    Message = ex.Message,
                    ErrorCodes = ErrorCodes.Exception
                };

            }
            
        }
         public async Task<ResponseDto<DetailCategoryDto>> GetCategoryByIdAsync(int id)
        {
            try
            {
                var categories = await _categoryRepository.GetByIdAsync(id);
                if (categories is null)
                {
                    return new ResponseDto<DetailCategoryDto>
                    {
                        Success = false,
                        Message = "Category not found",
                        ErrorCodes = ErrorCodes.NotFound
                    };
                }
                var result = _mapper.Map<DetailCategoryDto>(categories);
                return new ResponseDto<DetailCategoryDto>
                {
                    Success = true,
                    Message = "Category retrieved successfully",
                    Data = result
                };
               
                
            }
            catch (Exception ex)
            {
                return new ResponseDto<DetailCategoryDto>
                {
                    Success = false,
                    Message = ex.Message,
                    ErrorCodes = ErrorCodes.Exception
                };
                
            }
           
            
            
        }


        public async Task <ResponseDto<object>> AddCategoryAsync(CreateCategoryDto dto)
        {
            try
            {
                var category = _mapper.Map<Category>(dto);
                await _categoryRepository.AddAsync(category); // ben zaten category oluşturdum bana gelen dto yu category e map ettim ve bunu ekledim
                return new ResponseDto<object>
                {
                    Success = true,
                    Data = null,
                    Message = "Category added successfully"
                };
            }
            catch (System.Exception)
            {

                return new ResponseDto<object>
                {
                    Success = false,
                    Message = "An error occurred while adding the category",
                    ErrorCodes = ErrorCodes.Exception
                };
            }


            
        }

        public async Task <ResponseDto<object>> DeleteCategoryAsync(int id)

        {



            try
            {

                var category = _categoryRepository.GetByIdAsync(id).Result;
                if (category is null)
                {
                    return new ResponseDto<object>
                    {
                        Success = false,
                        Message = "Category not found",
                        ErrorCodes = ErrorCodes.NotFound
                    };

                }
                await _categoryRepository.DeleteAsync(category);
                return new ResponseDto<object>
                {
                    Success = true,
                    Message = "Category deleted successfully"
                };
            }
            catch (Exception ex)
            {

                return new ResponseDto<object>
                {
                    Success = false,
                    Message = ex.Message,
                    ErrorCodes = ErrorCodes.Exception
                };
            }
        }



       

        public async  Task<ResponseDto<object>> UpdateCategoryAsync(UpdateCategoryDto dto)
        {
            try
            {
                var categorydb = await  _categoryRepository.GetByIdAsync(dto.Id);
                if(categorydb is null)
                {
                    return new ResponseDto<object>
                    {
                        Success = false,
                        Message = "Category not found",
                        ErrorCodes = ErrorCodes.NotFound
                    };
                }
                var category = _mapper.Map(dto, categorydb); // dto yu categorydb ye map et yani güncelle 
                await _categoryRepository.UpdateAsync(category);
                return new ResponseDto<object>
                {
                    Success = true,
                    Data = null,
                    Message = "Category updated successfully"
                };
                
            }
            catch (Exception ex)
            {

                return new ResponseDto<object>
                {
                    Success = false,
                    Message = ex.Message,
                    ErrorCodes = ErrorCodes.Exception
                };
            }
            
        }
    }
}
    