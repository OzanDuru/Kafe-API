
using AutoMapper;
using Kafe.Application.Dtos.CategoryDtos;
using Kafe.Application.Dtos.MenuItemDtos;
using Kafe.Domain.Entities;

namespace Kafe.Application.Mapping
{
    public class GeneralMapping : Profile
    {
        // CreateMap<Source, Destination>(); -> Example mapping configuration
        // CreateMap <Destination , Source>(); -> Example reverse mapping configuration
        // CreateMap<Source, Destination>().ReverseMap(); -> Example bidirectional mapping configuration

        public GeneralMapping()
        {
            CreateMap<Category, CreateCategoryDto>().ReverseMap();
            CreateMap<Category, UpdateCategoryDto>().ReverseMap();
            CreateMap<Category, ResultCategoryDto>().ReverseMap();
            CreateMap<Category, DetailCategoryDto>().ReverseMap();

            CreateMap<MenuItem, CreateMenuItemDto>().ReverseMap();
            CreateMap<MenuItem, UpdateMenuItemDto>().ReverseMap();
            CreateMap<MenuItem, ResultMenuItemDto>().ReverseMap();
            CreateMap<MenuItem, DetailMenuItemDto>().ReverseMap();





        }
    }
}