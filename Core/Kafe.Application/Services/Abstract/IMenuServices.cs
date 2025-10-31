using Kafe.Application.Dtos.MenuItemDtos;
namespace Kafe.Application.Services.Abstract
{
    public interface IMenuServices
    {
        Task<List<ResultMenuItemDto>> GetAllMenusAsync();
        Task<DetailMenuItemDto> GetMenuByIdAsync(int id);
        Task AddMenuAsync(CreateMenuItemDto dto);
        Task UpdateMenuAsync(UpdateMenuItemDto dto);
        Task DeleteMenuAsync(int id);
        
    }
}