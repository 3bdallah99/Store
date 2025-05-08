using Domain.Contracts;
using Domain.Models.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Persistence;
using Persistence.Identity;
using Services;
using Shared;
using Shared.ErrorModels;
using Store.Api.Middlewares;
using System.Text;

namespace Store.Api.Extentions
{
    public static class Extentions
    {
        public static IServiceCollection RegisterAllServices(this IServiceCollection services , IConfiguration configuration)
        {
            services.AddBuiltInServices();
            services.AddSwaggerServices();
            services.AddInfrastructureService(configuration);

            services.AddIdentityServices();
            services.AddApplicationServices(configuration);
            services.ConfigureServices();
            services.ConfigureJwtService(configuration);

            services.AddCors(config =>
            {
                config.AddPolicy("MyPolicy", option =>
                {
                    option.AllowAnyHeader();
                    option.AllowAnyMethod();
                    option.WithOrigins("http://localhost:4200");
                });
            });

            return services;
        }
        private static IServiceCollection AddBuiltInServices(this IServiceCollection services )
        {
            services.AddControllers();
        
            return services;
        }
        private static IServiceCollection ConfigureJwtService(this IServiceCollection services ,IConfiguration configuration)
        {
            var JwtOptions = configuration.GetSection("JwtOptions").Get<JwtOptions>();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,

                    ValidIssuer = JwtOptions.Issure,
                    ValidAudience = JwtOptions.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtOptions.SecretKey)),
                };
            });
            return services;
        }
        private static IServiceCollection AddIdentityServices(this IServiceCollection services )
        {
            services.AddIdentity<AppUser, IdentityRole>()
                    .AddEntityFrameworkStores<StoreIdentityDbContext>()
                ;
        
            return services;
        }
        private static IServiceCollection AddSwaggerServices(this IServiceCollection services )
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            return services;
        }
        private static IServiceCollection ConfigureServices(this IServiceCollection services )
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.Configure<ApiBehaviorOptions>(config =>
            {
                config.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    var errors = actionContext.ModelState.Where(m => m.Value.Errors.Any())
                                 .Select(m => new ValidationError()
                                 {
                                     Field = m.Key,
                                     Errors = m.Value.Errors.Select(errors => errors.ErrorMessage)
                                 });
                    var response = new ValidationErrorResponse()
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(response);
                };

            });
            return services;
        }
        public static async Task<WebApplication> ConfigureMiddleWares(this WebApplication app)
        {
            await app.InitializeDatabaseAsync();

            app.UseMiddleware<GlobalErrorHandlingMiddleware>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseStaticFiles();
            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors("MyPolicy");
            app.MapControllers();
            return app;
        }
        public static async Task<WebApplication> InitializeDatabaseAsync(this WebApplication app)
        {
            #region Seeding
            using var scope = app.Services.CreateScope();
            var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
            await dbInitializer.InitializeAsync();
            await dbInitializer.InitializeIdentityAsync();
            #endregion
            return app;
        }

    }
}
