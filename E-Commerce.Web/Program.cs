
using Domain.Contracts;
using E_Commerce.Web.CustomMiddlewares;
using E_Commerce.Web.Extentions;
using E_Commerce.Web.Factories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Data;
using Persistence.Repositories;
using Service;
using ServiceAbstraction;
using Shared.ErrorModels;

namespace E_Commerce.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Services Container 
            builder.Services.AddControllers()
              .AddApplicationPart(typeof(Presentation.Controllers.ProductsController).Assembly);
            //.AddApplicationPart(typeof(Presentation.Controllers.BasketController).Assembly);
            builder.Services.AddCors(Option =>
            {
                Option.AddPolicy("AllowAll", Builder =>
                {
                    Builder.AllowAnyHeader();
                    Builder.AllowAnyMethod();
                    Builder.AllowAnyOrigin();
                    
                });
            });
            builder.Services.AddSwaggerServices();
            builder.Services.AddInfraStructureServices(builder.Configuration);
            builder.Services.AddApplicationService();
            builder.Services.AddWebApplicationServices();
            builder.Services.AddJWTServices(builder.Configuration);

            #endregion

            var app = builder.Build();
           await app.SeedDataBaseAsync();
            // Configure the HTTP request pipeline.
            app.UseCustomExceptionMiddleware();
            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerMiddleware();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors("AllowAll");
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
