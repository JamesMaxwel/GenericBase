using GenericBase.Application.Helpers.Mappings;
using GenericBase.Application.Interfaces;
using GenericBase.Application.Interfaces.Common;
using GenericBase.Application.Services;
using GenericBase.Application.Services.Common;
using GenericBase.Infra.Data.Interfaces.Common;
using GenericBase.Infra.Data.Repositories.Common;
using GenericBase.Infra.Ioc.Extensions.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


namespace GenericBase.Infra.Ioc
{
    public static class ServiceLayerSetup
    {
        public static void AddServiceLayer(this IServiceCollection services, IConfiguration configuration)
        {
            //DataBase
            services.AddDataBase(configuration);

            //Cache
            services.AddMemoryCache();

            //Helpers           
            services.AddHttpContextAccessor();
            services.AddAutoMapper(typeof(MapProfile));


            //Repositories
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            //Services
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IPermissionService, PermissionService>();

            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IPaginatonService, PaginationService>();


            //Extension Services
            services.AddJwtSettings(configuration);
            services.AddEmailSettings(configuration);
            services.AddSwaggerConfigurations();
            services.AddAuthPolicies();


        }

        public static void AddBuildLayer(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<TokenRedirectMiddleware>();
            app.UseMiddleware<ErrorHandlerMiddleware>();
            app.UseAuthorization();

            if (env.IsDevelopment())
            {
                app.SeedDb();
            }

        }
    }
}
