using AutoMapper;
using WarehouseEFApp.DTOs;
using WarehouseEFApp.Models;

namespace WarehouseEFApp.Mappings;

/// <summary>
/// AutoMapper профіль дляMapping між моделями та DTOs
/// </summary>
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Category Mappings
        CreateMap<Category, CategoryReadDTO>();
        CreateMap<CategoryCreateUpdateDTO, Category>();

        // Product Mappings
        CreateMap<Product, ProductReadDTO>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : null));

        CreateMap<ProductCreateDTO, Product>();
        CreateMap<ProductUpdateDTO, Product>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}
