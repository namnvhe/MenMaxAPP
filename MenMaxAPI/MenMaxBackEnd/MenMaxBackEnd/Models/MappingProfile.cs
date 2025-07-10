using AutoMapper;
using MenMaxBackEnd.Models;

namespace MenMaxBackEnd.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Type converters
            CreateMap<DateOnly, DateTime>().ConvertUsing(src => src.ToDateTime(TimeOnly.MinValue));
            CreateMap<DateTime, DateOnly>().ConvertUsing(src => DateOnly.FromDateTime(src));

            // User mappings
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.LoginType, opt => opt.MapFrom(src => src.LoginType))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber));

            CreateMap<UserDto, User>()
                .ForMember(dest => dest.LoginType, opt => opt.MapFrom(src => src.LoginType))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.Orders, opt => opt.Ignore())
                .ForMember(dest => dest.Carts, opt => opt.Ignore());

            // ProductImage mappings
            CreateMap<ProductImage, ProductImageDto>()
                .ForMember(dest => dest.UrlImage, opt => opt.MapFrom(src => src.UrlImage));

            CreateMap<ProductImageDto, ProductImage>()
                .ForMember(dest => dest.UrlImage, opt => opt.MapFrom(src => src.UrlImage))
                .ForMember(dest => dest.Product, opt => opt.Ignore());

            // ✅ SỬA Product mappings - Loại bỏ CartDto để tránh vòng lặp
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.ProductName))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
                .ForMember(dest => dest.IsSelling, opt => opt.MapFrom(src => src.IsSelling))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.ProductImages, opt => opt.MapFrom(src => src.ProductImages))
                .ForMember(dest => dest.CartDto, opt => opt.Ignore()); // ← IGNORE để tránh vòng lặp

            CreateMap<ProductDto, Product>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.ProductName))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
                .ForMember(dest => dest.IsSelling, opt => opt.MapFrom(src => src.IsSelling))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.Category, opt => opt.Ignore())
                .ForMember(dest => dest.ProductImages, opt => opt.Ignore())
                .ForMember(dest => dest.OrderItems, opt => opt.Ignore())
                .ForMember(dest => dest.Carts, opt => opt.Ignore());

            // Order mappings
            CreateMap<Order, OrderDto>()
                .ForMember(dest => dest.BookingDate, opt => opt.MapFrom(src => src.BookingDate))
                .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => src.PaymentMethod))
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems))
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User));

            CreateMap<OrderDto, Order>()
                .ForMember(dest => dest.BookingDate, opt => opt.MapFrom(src => src.BookingDate))
                .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => src.PaymentMethod))
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.OrderItems, opt => opt.Ignore());

            // OrderItem mappings
            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Product));

            CreateMap<OrderItemDto, OrderItem>()
                .ForMember(dest => dest.Product, opt => opt.Ignore())
                .ForMember(dest => dest.Order, opt => opt.Ignore());

            // Category mappings
            CreateMap<Category, CategoryDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.CategoryName))
                .ForMember(dest => dest.CategoryImage, opt => opt.MapFrom(src => src.CategoryImage))
                .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Products));

            CreateMap<CategoryDto, Category>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.CategoryName))
                .ForMember(dest => dest.CategoryImage, opt => opt.MapFrom(src => src.CategoryImage))
                .ForMember(dest => dest.Products, opt => opt.Ignore());

            // ✅ SỬA Cart mappings - Chỉ map những field cần thiết
            CreateMap<Cart, CartDto>()
                .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Product));

            CreateMap<CartDto, Cart>()
                .ForMember(dest => dest.Product, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore());
        }
    }
}
