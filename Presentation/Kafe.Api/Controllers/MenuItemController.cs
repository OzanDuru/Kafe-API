using Kafe.Application.Dtos.MenuItemDtos;
using Kafe.Application.Services.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Kafe.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuItemController : ControllerBase
    {
        private readonly IMenuServices _menuItemServices;
        public MenuItemController(IMenuServices menuItemServices)
        {
            _menuItemServices = menuItemServices;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllMenuItems()
        {
            var menuItems = await _menuItemServices.GetAllMenusAsync();
            return Ok(menuItems);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMenuItemById(int id)
        {
            var menuItem = await _menuItemServices.GetMenuByIdAsync(id);
            return Ok(menuItem);
        }

        [HttpPost]
        public async Task<IActionResult> AddMenuItem(CreateMenuItemDto dto)
        {
            await _menuItemServices.AddMenuAsync(dto);
            return Ok("Menü öğesi eklendi");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateMenuItem(UpdateMenuItemDto dto)
        {
            await _menuItemServices.UpdateMenuAsync(dto);
            return Ok("Menü öğesi güncellendi");
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteMenuItem(int id)
        {
            await _menuItemServices.DeleteMenuAsync(id);
            return Ok("Menü öğesi silindi");
        }
        
    }
}