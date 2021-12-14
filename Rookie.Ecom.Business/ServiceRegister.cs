using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using Rookie.Ecom.Business.Interfaces;
using Rookie.Ecom.Business.Interfaces.Id4;
using Rookie.Ecom.Business.Security.Handler;
using Rookie.Ecom.Business.Services;
using Rookie.Ecom.DataAccessor;
using System;
using System.Reflection;

namespace Rookie.Ecom.Business
{
    public static class ServiceRegister
    {
        public static void AddBusinessLayer(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddDataAccessorLayer(configuration);
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<IUnitService, UnitService>();
            services.AddTransient<IBrandService, BrandService>();
            services.AddTransient<IProductImageService, ProductImageService>();
            services.AddTransient<ICloudinaryService, CloudinaryService>();
            services.AddTransient<IHomeService, HomeService>();
            services.AddTransient<IOrderService, OrderService>();
            services.AddTransient<IHelper, Helper>();
            services.AddSingleton<IAuthorizationHandler, AdminRoleHandler>();

            services
                .AddRefitClient<IUserProvider>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://localhost:5000"));

            //services.AddTransient<IPictureService, Pi>();


        }
    }
}
