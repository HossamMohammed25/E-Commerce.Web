using Domain.Contracts;
using Domain.Models.IdentityModule;
using Domain.Models.OrderModule;
using Domain.Models.ProductModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using Persistence.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Persistence
{
    public class DataSeeding(StoreDbContext _dbContext,UserManager<ApplicationUser> _userManager,RoleManager<IdentityRole> _roleManager,StoreIdentityDbContext _identityDbContext) : IDataSeeding
    {
        public async Task DataSeedAsync()
        {
            try
            {
                var PendingMigrations = await _dbContext.Database.GetPendingMigrationsAsync();
                if (PendingMigrations.Any())
                {
                 await _dbContext.Database.MigrateAsync();
                }

                if (!_dbContext.Set<ProductBrand>().Any())
                {
                    var ProductBrandData = File.OpenRead(@"..\InfraStructure\Persistence\Data\DataSeed\brands.json");
                    var ProductBrand = await JsonSerializer.DeserializeAsync<List<ProductBrand>>(ProductBrandData);
                    if (ProductBrand is not null && ProductBrand.Any())
                      await _dbContext.ProductBrands.AddRangeAsync(ProductBrand);
                }
                if (!_dbContext.Set<ProductType>().Any())
                {
                    var ProductTypesData = File.OpenRead(@"..\InfraStructure\Persistence\Data\DataSeed\types.json");
                    var ProductTypes = await JsonSerializer.DeserializeAsync<List<ProductType>>(ProductTypesData);
                    if (ProductTypes is not null && ProductTypes.Any())
                       await _dbContext.ProductTypes.AddRangeAsync(ProductTypes);
                }
                if (!_dbContext.Set <Product>().Any())
                {
                    var ProductData = File.OpenRead(@"..\InfraStructure\Persistence\Data\DataSeed\products.json");
                    var Products = await JsonSerializer.DeserializeAsync<List<Product>>(ProductData);
                    if (Products is not null && Products.Any())
                      await  _dbContext.Products.AddRangeAsync(Products);
                }
                if (!_dbContext.Set <DeliveryMethod>().Any())
                {
                    var DeliveryMethodData = File.OpenRead(@"..\InfraStructure\Persistence\Data\DataSeed\delivery.json");
                    var DeliveryMethods = await JsonSerializer.DeserializeAsync<List<DeliveryMethod>>(DeliveryMethodData);
                    if (DeliveryMethods is not null && DeliveryMethods.Any())
                      await  _dbContext.Set<DeliveryMethod>().AddRangeAsync(DeliveryMethods);
                }
               await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // TODO
            }
        }

        public async Task IdentityDataSeedAsync()
        {
            try
            {
                if (!_roleManager.Roles.Any())
                {
                    await _roleManager.CreateAsync(new IdentityRole("Admin"));
                    await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
                }
                if (!_userManager.Users.Any())
                {
                    var User01 = new ApplicationUser()
                    {
                        Email = "Hossam@gamil.com",
                        DisplayName = "Hossam Mohamed",
                        PhoneNumber = "01125449654",
                        UserName = "HossamMohamed"

                    };
                    var User02 = new ApplicationUser()
                    {
                        Email = "Mohamed@gamil.com",
                        DisplayName = "Mohamed Saad",
                        PhoneNumber = "01125449654",
                        UserName = "MohamedSaad"

                    };
                    await _userManager.CreateAsync(User01, "P@ssw0rd");
                    await _userManager.CreateAsync(User02, "P@ssw0rd");

                    await _userManager.AddToRoleAsync(User01, "Admin");
                    await _userManager.AddToRoleAsync(User01, "SuperAdmin");
                }
                await _identityDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {

            }
        }
    }
}
