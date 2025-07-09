using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MenMaxBackEnd.Models;

public partial class MenMaxContext : DbContext
{
    public MenMaxContext()
    {
    }

    public MenMaxContext(DbContextOptions<MenMaxContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderItem> OrderItems { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductImage> ProductImages { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var ConnectionString = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetConnectionString("DefaultConnection");
            optionsBuilder.UseSqlServer(ConnectionString);
        }

    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__cart__3213E83F00E26DD5");

            entity.ToTable("cart");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Count).HasColumnName("count");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.UserId)
                .HasMaxLength(255)
                .HasColumnName("user_id");

            entity.HasOne(d => d.Product).WithMany(p => p.Carts)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK3d704slv66tw6x5hmbm6p2x3u");

            entity.HasOne(d => d.User).WithMany(p => p.Carts)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FKl70asp4l4w0jmbm1tqyofho4o");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__category__3213E83F6DE660D4");

            entity.ToTable("category");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CategoryImage)
                .HasMaxLength(1111)
                .HasColumnName("category_Image");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(1111)
                .HasColumnName("category_name");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__order__3213E83F1C745D15");

            entity.ToTable("order");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Address)
                .HasMaxLength(1111)
                .HasColumnName("address");
            entity.Property(e => e.BookingDate).HasColumnName("booking_date");
            entity.Property(e => e.Country)
                .HasMaxLength(1111)
                .HasColumnName("country");
            entity.Property(e => e.Email)
                .HasMaxLength(1111)
                .HasColumnName("email");
            entity.Property(e => e.Fullname)
                .HasMaxLength(1111)
                .HasColumnName("fullname");
            entity.Property(e => e.Note)
                .HasMaxLength(1111)
                .HasColumnName("note");
            entity.Property(e => e.PaymentMethod)
                .HasMaxLength(1111)
                .HasColumnName("payment_method");
            entity.Property(e => e.Phone)
                .HasMaxLength(1111)
                .HasColumnName("phone");
            entity.Property(e => e.Status)
                .HasMaxLength(1111)
                .HasColumnName("status");
            entity.Property(e => e.Total).HasColumnName("total");
            entity.Property(e => e.UserId)
                .HasMaxLength(255)
                .HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FKcpl0mjoeqhxvgeeeq5piwpd3i");
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__order_it__3213E83F07D7ECD6");

            entity.ToTable("order_item");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Count).HasColumnName("count");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FKs234mi6jususbx4b37k44cipy");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK551losx9j75ss5d6bfsqvijna");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__product__3213E83F34D29D05");

            entity.ToTable("product");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.IsSelling).HasColumnName("is_selling");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.ProductName)
                .HasMaxLength(1111)
                .HasColumnName("product_name");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.Sold).HasColumnName("sold");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK1mtsbur82frn64de7balymq9s");
        });

        modelBuilder.Entity<ProductImage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__product___3213E83FE299DA62");

            entity.ToTable("product_image");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.UrlImage)
                .HasMaxLength(1111)
                .HasColumnName("url_image");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductImages)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK6oo0cvcdtb6qmwsga468uuukk");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__user__3213E83F05C18ED2");

            entity.ToTable("user");

            entity.Property(e => e.Id)
                .HasMaxLength(255)
                .HasColumnName("id");
            entity.Property(e => e.Avatar)
                .HasMaxLength(1111)
                .HasColumnName("avatar");
            entity.Property(e => e.Email)
                .HasMaxLength(1111)
                .HasColumnName("email");
            entity.Property(e => e.LoginType)
                .HasMaxLength(1111)
                .HasColumnName("login_type");
            entity.Property(e => e.Password)
                .HasMaxLength(1111)
                .HasColumnName("password");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(1111)
                .HasColumnName("phone_number");
            entity.Property(e => e.Role)
                .HasMaxLength(1111)
                .HasColumnName("role");
            entity.Property(e => e.UserName)
                .HasMaxLength(1111)
                .HasColumnName("user_name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
