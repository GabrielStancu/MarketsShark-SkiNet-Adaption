using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using Core.Entities;

namespace Infrastructure.Data.WebScraper
{
    //scraped the evomag website 
    public class EvoMagScraper : Scraper
    {
        public EvoMagScraper()
        {
            Urls = new List<(string url, int typeId)>
            {
                ("https://www.evomag.ro/portabile-laptopuri-notebook/", 1),
                ("https://www.evomag.ro/portabile-laptopuri-notebook/filtru/pagina:2", 1),
                ("https://www.evomag.ro/portabile-laptopuri-notebook/filtru/pagina:3", 1),
                ("https://www.evomag.ro/portabile-laptopuri-notebook/filtru/pagina:4", 1),
                ("https://www.evomag.ro/portabile-laptopuri-notebook/filtru/pagina:5", 1)
            };
        }
        public override List<Product> Scrape(int typeId)
        {
            var products = GetProductsDetails(typeId);
            GetProductsPrices(products);
            GetProductsUrls(products);

            return products;
        }

        protected override List<Product> GetProductsDetails(int typeId)
        {
            var products = new List<Product>();

            MatchCollection matchCollectionExt = Regex.Matches(this.Data, this.BaseRegex);

            foreach (Match match in matchCollectionExt)
            {
                string name = match.Groups[2].Value;
                string description = string.Empty;
                string pictureUrl = $"https://static3.evomag.ro{match.Groups[1].Value}";

                if (name.Contains('('))
                {
                    description = name.Substring(name.IndexOf('(') + 1);
                    description = description[0..^1];
                    name = name.Substring(0, name.IndexOf('('));
                }
                else
                {
                    description = name;
                }

                if (name.Contains("&quot"))
                {
                    name = name.Replace("&quot", inchesSign);
                    name = name.Replace(";", "");
                }
                if (description.Contains("&quot"))
                {
                    description = description.Replace("&quot", inchesSign);
                    description = description.Replace(";", "");
                }

                Product product = new Product()
                {
                    Name = name,
                    PictureUrl = pictureUrl,
                    Description = description,
                    ProductBrandId = GetProductBrand(name),
                    ProductTypeId = typeId
                };

                products.Add(product);
            }

            return products;
        }

        private new void GetProductsPrices(List<Product> products)
        {
            MatchCollection matchCollectionInt = Regex.Matches(this.Data, this.PriceRegex);

            for (int matchIndex = 0; matchIndex < matchCollectionInt.Count; matchIndex++)
            {
                Match match = matchCollectionInt[matchIndex];
                CultureInfo provider = new CultureInfo("ro-RO");
                decimal price = decimal.Parse(match.Groups[1].Value, provider);
                products[matchIndex].Price = price;
            }
        }

        private new void GetProductsUrls(List<Product> products)
        {
            MatchCollection matchCollectionUrl = Regex.Matches(this.Data, this.UrlRegex);

            for (int matchIndex = 0; matchIndex < matchCollectionUrl.Count; matchIndex++)
            {
                Match match = matchCollectionUrl[matchIndex];
                products[matchIndex].ProductUrl = $"https://www.evomag.ro/{match.Groups[1].Value}"; ;
            }
        }
    }
}