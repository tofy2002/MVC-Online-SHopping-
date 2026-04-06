using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
//using MvcProject.Data;
using OnlineShopping;
using OnlineShopping.Data;
using OnlineShopping.Repo;

namespace MvcProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
          //  builder.Services.AddAuthentication("Cookies")
          //.AddCookie("Cookies", options =>
          //{
          //    options.LoginPath = "/Account/Login";
          //});
            builder.Services.AddAuthorization();
            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();
            builder.Services.AddDbContext<ShoppingContext>(options =>
     options.UseSqlServer(
         builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddIdentity<Customer, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
            })
                .AddEntityFrameworkStores<ShoppingContext>()
                .AddDefaultTokenProviders();
            builder.Services.AddScoped<IUnitofWork, UnitofWork>();
            var app = builder.Build();


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }
    }
}
