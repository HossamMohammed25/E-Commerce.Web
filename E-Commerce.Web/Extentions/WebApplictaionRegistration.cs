using Domain.Contracts;
using E_Commerce.Web.CustomMiddlewares;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Text.Json;

namespace E_Commerce.Web.Extentions
{
    public static class WebApplictaionRegistration
    {
        public static async Task SeedDataBaseAsync(this WebApplication app)
        {
            using var Scoop = app.Services.CreateScope();
            var ObjectOfDataSeeding = Scoop.ServiceProvider.GetRequiredService<IDataSeeding>();
            await ObjectOfDataSeeding.DataSeedAsync();
            await ObjectOfDataSeeding.IdentityDataSeedAsync();
        }

        public static IApplicationBuilder UseCustomExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionHandleMiddleware>();

            return app;
        }
        public static IApplicationBuilder UseSwaggerMiddleware(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(option =>
            {
                option.ConfigObject= new ConfigObject()
                {
                    DisplayRequestDuration = true
                };
                option.DocumentTitle = "E-Commerce";
                option.JsonSerializerOptions = new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };
                option.DocExpansion(DocExpansion.None);
                option.EnableFilter();
               option.EnablePersistAuthorization();
            });
            return app;
        }


    }
}
