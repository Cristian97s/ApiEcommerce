// Configuración de Mapster para el proyecto
using Mapster;
using ApiEcommerce.Models;
using ApiEcommerce.Models.Dtos;

namespace ApiEcommerce.Mapping;

public static class MapsterConfig
{
    public static void RegisterMappings()
    {
        TypeAdapterConfig<Category, CategoryDto>.NewConfig();
        TypeAdapterConfig<CreateCategoryDto, Category>.NewConfig();
        TypeAdapterConfig<Product, ProductDto>.NewConfig();
        TypeAdapterConfig<CreateProductDto, Product>.NewConfig();
        TypeAdapterConfig<UpdateProductDto, Product>.NewConfig();
        TypeAdapterConfig<User, UserDto>.NewConfig();
        TypeAdapterConfig<CreateUserDto, User>.NewConfig();
        TypeAdapterConfig<ApplicationUser, UserDataDto>.NewConfig();
        // Agrega más configuraciones según sea necesario
    }
}
