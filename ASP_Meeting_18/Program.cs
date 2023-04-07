using ASP_Meeting_18.AutoMapperProfiles;
using ASP_Meeting_18.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static System.Collections.Specialized.BitVector32;
using System.Configuration;
using ASP_Meeting_18.Infrastructure.ModelBinderProviders;
using Microsoft.AspNetCore.Routing.Constraints;
using ASP_Meeting_18.Models.RouteConstraints;
using ASP_Meeting_18.Models.Services;
using ASP_Meeting_18.Models.ClaimRequirements;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var configuration = builder.Configuration;
builder.Services.AddControllersWithViews(options =>
{
    options.ModelBinderProviders.Insert(0, new CartModelBinderProvider());
});
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminAndManagerOnly", policy => policy.Requirements.Add(new ClaimRequirement("Admin","Manager")));
});
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<ShopDBContext>();
string str = builder.Configuration.GetConnectionString("MyShopDB");
builder.Services.AddDbContext<ShopDBContext>(options=> options.UseSqlServer(str));
builder.Services.AddAutoMapper(typeof(UserProfile));
builder.Services.AddAuthentication().AddGoogle(options =>
{
    IConfigurationSection googlesection = configuration.GetSection("Authentication:Google");
    options.ClientId = googlesection.GetValue<string>("ClientId");
    options.ClientSecret = googlesection["ClientSecret"];
}).AddFacebook(fbOptions => {
    var section = configuration.GetSection("Authentication:Facebook");
    fbOptions.AppId = section.GetSection("AppId").Value;
    fbOptions.AppSecret = section.GetSection("AppSecret").Value;
}
);
builder.Services.AddSingleton<IAuthorizationHandler, ClaimHandler>();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
builder.Services.AddSingleton(new EmailService(builder.Configuration));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseAuthentication();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "CategoryPage",
    pattern: "{category?}/{page}",
    defaults: new { controller = "Home", action = "Index",page=1 },
    constraints: new { page = @"\d+" }
);
app.MapControllerRoute(
    name: "Page",
    pattern: "Page{page}",
    defaults: new { controller = "Home", action = "Index",page=1 },
    constraints: new { page = new MyIntRouteConstraint() }
);
app.MapControllerRoute(
    name: "Category",
    pattern: "{category=All}",
    defaults: new { controller = "Home", action = "Index",category="All"}
);


app.MapControllerRoute(
    name: "Admin",
    pattern: "/Admin/{controller}/{action}",
    defaults: new {controller="Home",action="Index"},
    constraints: new {controller=new HomeControllerRouteConstraint() }
);
app.MapControllerRoute(
    name: "Account",
    pattern: "/Account/{controller}/{action}",
    defaults: new { controller = "Home", action = "Index" }
);
app.MapControllerRoute(
    name: "Default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);
app.Run();
