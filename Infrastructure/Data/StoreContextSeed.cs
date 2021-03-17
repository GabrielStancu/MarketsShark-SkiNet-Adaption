using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;
using Core.Entities;
using System;
using Core.Entities.OrderAggregate;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data.WebScraper;

namespace Infrastructure.Data
{
    //seeds data in the database 
    public class StoreContextSeed
    {
        public static async Task SeedAsync(StoreContext context, ILoggerFactory loggerFactory)
        {
            try 
            {
                //product brands 
                if(!context.ProductBrands.Any())
                {
                    var brandsData = File.ReadAllText("../Infrastructure/Data/SeedData/brands.json");
                    var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);

                    foreach (var item in brands)
                    {
                        context.ProductBrands.Add(item);
                    }

                    await context.SaveChangesAsync();
                }          

                //product types 
                if(!context.ProductTypes.Any())
                {
                    var typesData = File.ReadAllText("../Infrastructure/Data/SeedData/types.json");
                    var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);

                    foreach (var item in types)
                    {
                        context.ProductTypes.Add(item);
                    }

                    await context.SaveChangesAsync();
                }               
                
                //products; if too many requests => use stored version of products 
                if(!context.Products.Any())
                {
                    List<Product> products;
                    try
                    {
                        ScrapeParser scrapeParser = new ScrapeParser();
                        products = scrapeParser.ParseProducts(true, true, false);
                    }
                    catch(Exception)
                    {
                        var productsData = File.ReadAllText("../Infrastructure/Data/SeedData/products.json");
                        products = JsonSerializer.Deserialize<List<Product>>(productsData);

                        var logger = loggerFactory.CreateLogger<StoreContextSeed>();
                        logger.LogError("Too many accesses.");
                    }
                    

                    foreach (var item in products)
                    {
                        context.Products.Add(item); 
                    }

                    await context.SaveChangesAsync();
                }
                
                if(!context.DeliveryMethods.Any())
                {
                    var dmData = File.ReadAllText("../Infrastructure/Data/SeedData/delivery.json");
                    var methods = JsonSerializer.Deserialize<List<DeliveryMethod>>(dmData);

                    foreach (var item in methods)
                    {
                        context.DeliveryMethods.Add(item);
                    }

                    await context.SaveChangesAsync();
                }
            }
            catch(Exception ex)
            {
                var logger = loggerFactory.CreateLogger<StoreContextSeed>();
                logger.LogError(ex.Message);
            }
        }
    }
}