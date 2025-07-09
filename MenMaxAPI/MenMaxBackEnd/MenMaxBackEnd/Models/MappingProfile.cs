// Profiles/MappingProfile.cs
using AutoMapper;
using MenMaxBackEnd.Models;

namespace MenMaxBackEnd.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Order mappings
            CreateMap<Order, OrderDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src =>
                    src.User != null ? src.User.UserName : null))
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems));

            CreateMap<OrderDto, Order>()
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.OrderItems, opt => opt.Ignore());

            // OrderItem mappings
            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src =>
                    src.Product != null ? src.Product.ProductName : null))
                .ForMember(dest => dest.ProductPrice, opt => opt.MapFrom(src =>
                    src.Product != null ? src.Product.Price : null))
                .ForMember(dest => dest.ProductImage, opt => opt.MapFrom(src =>
                    src.Product != null && src.Product.ProductImages != null && src.Product.ProductImages.Any()
                        ? src.Product.ProductImages.First().UrlImage
                        : null));

            CreateMap<OrderItemDto, OrderItem>()
                .ForMember(dest => dest.Product, opt => opt.Ignore())
                .ForMember(dest => dest.Order, opt => opt.Ignore());

            // User mappings
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.CartCount, opt => opt.MapFrom(src =>
                    src.Carts != null ? src.Carts.Count : 0))
                .ForMember(dest => dest.OrderCount, opt => opt.MapFrom(src =>
                    src.Orders != null ? src.Orders.Count : 0));

            CreateMap<UserDto, User>()
                .ForMember(dest => dest.Carts, opt => opt.Ignore())
                .ForMember(dest => dest.Orders, opt => opt.Ignore());

            // Product mappings
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src =>
                    src.Category != null ? src.Category.CategoryName : null))
                .ForMember(dest => dest.FirstImage, opt => opt.MapFrom(src =>
                    src.ProductImages != null && src.ProductImages.Any()
                        ? src.ProductImages.First().UrlImage
                        : null));

            CreateMap<ProductDto, Product>()
                .ForMember(dest => dest.Category, opt => opt.Ignore())
                .ForMember(dest => dest.ProductImages, opt => opt.Ignore())
                .ForMember(dest => dest.Carts, opt => opt.Ignore())
                .ForMember(dest => dest.OrderItems, opt => opt.Ignore());

            // Category mappings
            CreateMap<Category, CategoryDto>()
                .ForMember(dest => dest.ProductCount, opt => opt.MapFrom(src =>
                    src.Products != null ? src.Products.Count : 0));

            CreateMap<CategoryDto, Category>()
                .ForMember(dest => dest.Products, opt => opt.Ignore());

            // Cart mappings
            CreateMap<Cart, CartDto>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src =>
                    src.Product != null ? src.Product.ProductName : null))
                .ForMember(dest => dest.ProductPrice, opt => opt.MapFrom(src =>
                    src.Product != null ? src.Product.Price : null))
                .ForMember(dest => dest.ProductImage, opt => opt.MapFrom(src =>
                    src.Product != null && src.Product.ProductImages != null && src.Product.ProductImages.Any()
                        ? src.Product.ProductImages.First().UrlImage
                        : null))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src =>
                    src.User != null ? src.User.UserName : null));

            CreateMap<CartDto, Cart>()
                .ForMember(dest => dest.Product, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore());
        }
    }
}
