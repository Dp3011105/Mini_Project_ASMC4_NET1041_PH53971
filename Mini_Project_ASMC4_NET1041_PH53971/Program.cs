using Microsoft.EntityFrameworkCore;
using Mini_Project_ASMC4_NET1041_PH53971.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Đăng ký dịch vụ IHttpContextAccessor
builder.Services.AddHttpContextAccessor();


// Đăng ký các dịch vụ
builder.Services.AddDbContext<MiniProjectAsmc4Net1041Ph53971Context>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});


// Đăng ký dịch vụ Session
builder.Services.AddDistributedMemoryCache(); // Sử dụng bộ nhớ tạm cho Session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Thời gian timeout của Session
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});



var app = builder.Build();




// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseStaticFiles();

// Kích hoạt Session trước Authorization
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
