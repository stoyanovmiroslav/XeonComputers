using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using XeonComputers.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using XeonComputers.Middlewares;
using XeonComputers.Models;
using System.Text;
using Microsoft.Extensions.WebEncoders;
using System.Text.Unicode;
using System.Text.Encodings.Web;
using XeonComputers.Services.Contracts;
using XeonComputers.Services;
using Microsoft.Extensions.DependencyInjection.Extensions;
using AutoMapper;
using XeonComputers.MappingConfiguration;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace XeonComputers
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<XeonDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<XeonUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
            })
                .AddDefaultTokenProviders()
                .AddDefaultUI()
                .AddEntityFrameworkStores<XeonDbContext>();

            services.AddSession(options =>
            {
                options.Cookie.HttpOnly = true;
                options.IdleTimeout = new TimeSpan(0, 4, 0, 0);
            });

            services.AddAutoMapper(cfg => {
                cfg.AddProfile<XeonComputersProfile>();
            });
            
            services.AddScoped<IChildCategoriesService, ChildCategoriesService>();
            services.AddScoped<IParentCategoriesService, ParentCategoriesService>();
            services.AddScoped<IImagesService, ImagesService>();
            services.AddScoped<IProductsService, ProductsService>();
            services.AddScoped<IPaymentsService, PaymentsService>();
            services.AddScoped<IShoppingCartsService, ShoppingCartsService>();
            services.AddScoped<IUsersService, UsersService>();
            services.AddScoped<IAdressesService, AddressesService>();
            services.AddScoped<IOrdersService, OrdersService>();
            services.AddScoped<IFavoritesService, FavoritesService>();
            services.AddScoped<IPartnerRequestsService, PartnerRequestsService>();
            services.AddScoped<IUserRequestsService, UserRequestsService>();
            services.AddScoped<ISuppliersService, SuppliersService>();
            services.AddScoped<IViewRender, ViewRender>();

            services.AddSingleton<IEmailSender, EmailSender>();

            services.AddAuthentication().AddFacebook(facebookOptions =>
            {
                facebookOptions.AppId = Configuration["Authentication:Facebook:AppId"];
                facebookOptions.AppSecret = Configuration["Authentication:Facebook:AppSecret"];
            });

            services.AddAuthentication().AddGoogle(googleOptions =>
            {
                googleOptions.ClientId = Configuration["Authentication:Google:ClientId"];
                googleOptions.ClientSecret = Configuration["Authentication:Google:ClientSecret"];
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseSession();

            app.UseAuthentication();

            app.UseSeedDataMiddleware();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "areaRoute",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
