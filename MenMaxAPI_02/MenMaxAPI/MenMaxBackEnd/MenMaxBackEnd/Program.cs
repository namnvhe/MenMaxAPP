using MenMaxBackEnd.Models;
using MenMaxBackEnd.Profiles;
using MenMaxBackEnd.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ✅ PHẢI THÊM DbContext TRƯỚC KHI BUILD
builder.Services.AddDbContext<MenMaxContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ✅ THÊM MEMORY CACHE CHO SESSION
builder.Services.AddMemoryCache();
builder.Services.AddDistributedMemoryCache();

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Add custom ModelMapper
builder.Services.AddScoped<ModelMapper>();

// Add session support - BÂY GIỜ SẼ HOẠT ĐỘNG
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// ✅ BUILD APP SAU KHI ĐĂNG KÝ TẤT CẢ SERVICES
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// ✅ THÊM UseSession() TRƯỚC UseAuthorization()
app.UseSession();
app.UseAuthorization();

app.MapControllers();

app.Run();
