using System.Collections.Generic;
using System.IO;
using System.Net;
using Core.Entities;
using Newtonsoft.Json;

namespace Infrastructure.Data.WebScraper
{
    public class ScrapeParser
    {
        private static List<Product> _products;
        public List<Product> ParseProducts(bool enableEmag, bool enablePcGarage, bool enableEvoMag)
        {
            _products = new List<Product>();

            //emag
            if (enableEmag)
            {
                using (WebClient client = new WebClient())
                {
                    EmagScraper emagScraper = (EmagScraper)new EmagScraper()
                        .WithBaseRegex(@"<img data-src=""(.*?)"" class=""lozad"".*? alt=""(.*?)""")
                        .WithPriceRegex(@"<p class=""product-new-price"">(.*?)<sup>(.*?)<\/sup> <span>Lei<\/span><\/p>")
                        .WithUrlRegex(@"<a href=""(.*?)"" rel=""nofollow"" class=""thumbnail-wrapper js-product-url"" data-zone=""thumbnail"">");

                    foreach ((string url, int typeId) in emagScraper.Urls)
                    {
                        emagScraper = (EmagScraper)emagScraper.WithData(client.DownloadString(url));
                        _products.AddRange(emagScraper.Scrape(typeId));
                    }
                }
            }
            
            if (enablePcGarage)
            {
                //pc garage
                using (WebClient client = new WebClient())
                {
                    PcGarageScraper pcGarageScraper = (PcGarageScraper)new PcGarageScraper()
                        .WithBaseRegex(@"<a href=""h(.*?)"" title=""(.*?)"".*?<source srcset=""(.*?)""")
                        .WithPriceRegex(@"<p class=""price"">(.*?) RON<\/p>");

                    foreach ((string url, int typeId) in pcGarageScraper.Urls)
                    {
                        pcGarageScraper = (PcGarageScraper)pcGarageScraper.WithData(client.DownloadString(url));
                        _products.AddRange(pcGarageScraper.Scrape(typeId));
                    }
                }
            }

            //evomag
            if (enableEvoMag)
            {
                using (WebClient client = new WebClient())
                {
                    EvoMagScraper evoMagScraper = (EvoMagScraper)new EvoMagScraper()
                        .WithBaseRegex(@"src=""https:\/\/static3.evomag\.ro(.*?)"" alt=""(.*?)""")
                        .WithPriceRegex(@"<span class=""real_price"">(.*?) Lei<\/span>")
                        .WithUrlRegex(@"<a class="""" title="".*?[\r\n]*"" href=""(.*?)"">");

                    foreach ((string url, int typeId) in evoMagScraper.Urls)
                    {
                        evoMagScraper = (EvoMagScraper)evoMagScraper.WithData(client.DownloadString(url));
                        _products.AddRange(evoMagScraper.Scrape(typeId));
                    }
                }
            }
            
            return _products;
        }

        private static void SerializeToFile()
        {
            string pathToTheFile = @"..\Infrastructure\Data\SeedData\products.json";
            string json = JsonConvert.SerializeObject(_products.ToArray(), Formatting.Indented);
            File.WriteAllText(pathToTheFile, string.Empty);
            File.WriteAllText(pathToTheFile, json);
        }
    }
}