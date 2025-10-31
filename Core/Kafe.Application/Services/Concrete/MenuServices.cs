namespace Kafe.Application.Services.Concrete
{
    using AutoMapper;
    using Kafe.Application.Dtos.MenuItemDtos;
    using Kafe.Application.Interfaces;
    using Kafe.Application.Services.Abstract;
    using Kafe.Domain.Entities;

    public class MenuServices : IMenuServices
    {
        private readonly IGenericRepository<MenuItem> _menuRepository;
        private readonly IMapper _mapper;

        public MenuServices(IGenericRepository<MenuItem> menuRepository, IMapper mapper)
        {
            _menuRepository = menuRepository;
            _mapper = mapper;
        }

        public async Task AddMenuAsync(CreateMenuItemDto dto)
        {
            var menuItem = _mapper.Map<MenuItem>(dto); // CreateMenuItemDto'dan MenuItem entity'sine dönüştürme mapping işlemi reverse map ile de yapılabilir o şekilde ayarlamıştın GeneralMapping.cs'de
            await _menuRepository.AddAsync(menuItem); // Veritabanına ekleme
        }

        public async Task DeleteMenuAsync(int id)
        {
            var menuItem = await _menuRepository.GetByIdAsync(id);
            await _menuRepository.DeleteAsync(menuItem);
        }

        public async Task<List<ResultMenuItemDto>> GetAllMenusAsync()
        {
            var menuItems = _menuRepository.GetAllAsync().Result;
            var result = _mapper.Map<List<ResultMenuItemDto>>(menuItems);
            return result;
        }

        public async Task<DetailMenuItemDto> GetMenuByIdAsync(int id)
        {
            var menuItem = await _menuRepository.GetByIdAsync(id);
            var result = _mapper.Map<DetailMenuItemDto>(menuItem);
            return result; 
        }

        public async Task UpdateMenuAsync(UpdateMenuItemDto dto)
        {
            var existingMenuItem = await _menuRepository.GetByIdAsync(dto.Id);
            var newmenuItem = _mapper.Map(dto,existingMenuItem); // dto'dan gelen verilerle existingMenuItem'ı güncelle
            await _menuRepository.UpdateAsync(newmenuItem); // Veritabanında güncelleme 
        }
    }
} 