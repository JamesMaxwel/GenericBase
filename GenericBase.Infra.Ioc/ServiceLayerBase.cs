using GenericBase.Application.Constants;
using GenericBase.Application.Helpers.Options;
using GenericBase.Infra.Data.DataContext;
using GenericBase.Infra.Ioc.Extensions.Authentication.PolicyRequirements;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace GenericBase.Infra.Ioc
{
    internal static class ServiceLayerBase
    {
        internal static void AddDataBase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<MyDbContext>(options =>
            {
                options.UseSqlite(configuration.GetConnectionString("Sqlite"),
                    assembly => assembly.MigrationsAssembly(typeof(MyDbContext).Assembly.FullName));
            });
        }
        internal static void AddJwtSettings(this IServiceCollection services, IConfiguration configuration)
        {
            var _jwtOptions = JwtSettings.FromConfiguration(configuration);

            services.Configure<JwtSettings>(options =>
            {
                options.Issuer = _jwtOptions.Issuer;
                options.Audience = _jwtOptions.Audience;
                options.SecretKey = _jwtOptions.SecretKey;
                options.AccessTokenExpiration = _jwtOptions.AccessTokenExpiration;
                options.RefreshTokenExpiration = _jwtOptions.RefreshTokenExpiration;
                options.SymmetricSecurityKey = _jwtOptions.SymmetricSecurityKey;
                options.SigningCredentials = _jwtOptions.SigningCredentials;

            });

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = _jwtOptions.Issuer,

                ValidateAudience = true,
                ValidAudience = _jwtOptions.Audience,

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _jwtOptions.SymmetricSecurityKey,

                RequireExpirationTime = true,
                ValidateLifetime = true,

                ClockSkew = TimeSpan.Zero
            };

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = tokenValidationParameters;
            });

        }
        internal static void AddSwaggerConfigurations(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.EnableAnnotations();
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Description =
                        "JWT Authorization header using the Bearer scheme. " +
                        "Example: \"Authorization: Bearer {token}\"",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey

                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme()
                        {
                            Reference = new OpenApiReference()
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
                c.CustomSchemaIds(type => type.Name.ToUpper().EndsWith("DTO") ? type.Name[..^3] : type.Name);
            });
        }
        internal static void AddAuthPolicies(this IServiceCollection services)
        {
            services.AddSingleton<IAuthorizationHandler, BusinessTimeHandler>();
            services.AddAuthorization(options =>
            {
                //Set [Authorize] in all controllers
                options.FallbackPolicy = new AuthorizationPolicyBuilder()
                   .RequireAuthenticatedUser()
                   .Build();

                options.AddPolicy(PolicieTypes.BusinessTime, policy =>
                    policy.Requirements.Add(new BusinessTimeRequirement()));

            });
        }
        internal static void AddEmailSettings(this IServiceCollection services, IConfiguration configuration)
        {
            var _emailOptions = EmailSettings.FromConfiguration(configuration);

            services.Configure<EmailSettings>(options =>
            {
                options.Host = _emailOptions.Host;
                options.Port = _emailOptions.Port;
                options.EmailAddress = _emailOptions.EmailAddress;
                options.Password = _emailOptions.Password;
            });
        }
        public static void SeedDb(this IApplicationBuilder app)
        {
            ArgumentNullException.ThrowIfNull(app, nameof(app));

            using var scope = app.ApplicationServices.CreateScope();
            var services = scope.ServiceProvider;

            var context = services.GetRequiredService<MyDbContext>();
            var logger = services.GetRequiredService<ILogger<DbInitializer>>();
            DbInitializer.Initialize(context, logger);
        }
    }
}
