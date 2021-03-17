using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using Core.Entities;

namespace Infrastructure.Data.WebScraper
{
    //scrapes the pc garage website 
    public class PcGarageScraper : Scraper
    {
        public PcGarageScraper()
        {
            Urls = new List<(string url, int typeId)>
            {
                ("https://www.pcgarage.ro/notebook-laptop/filtre/tip-notebook-categorie-gaming/", 1),
                ("https://www.pcgarage.ro/notebook-laptop/pagina2/filtre/tip-notebook-categorie-gaming/", 1),
                ("https://www.pcgarage.ro/notebook-laptop/pagina3/filtre/tip-notebook-categorie-gaming/", 1),
                ("https://www.pcgarage.ro/notebook-laptop/pagina4/filtre/tip-notebook-categorie-gaming/", 1),
                ("https://www.pcgarage.ro/notebook-laptop/pagina5/filtre/tip-notebook-categorie-gaming/", 1),
                ("https://www.pcgarage.ro/notebook-laptop/pagina6/filtre/tip-notebook-categorie-gaming/", 1),
                ("https://www.pcgarage.ro/notebook-laptop/pagina7/filtre/tip-notebook-categorie-gaming/", 1),
                ("https://www.pcgarage.ro/notebook-laptop/pagina8/filtre/tip-notebook-categorie-gaming/", 1),
                ("https://www.pcgarage.ro/notebook-laptop/pagina9/filtre/tip-notebook-categorie-gaming/", 1),
                ("https://www.pcgarage.ro/sisteme-pc-garage/", 2),
                ("https://www.pcgarage.ro/sisteme-pc-garage/pagina2/", 2),
                ("https://www.pcgarage.ro/sisteme-pc-garage/pagina3/", 2),
                ("https://www.pcgarage.ro/sisteme-pc-garage/pagina4/", 2)
            };
        }
        public override List<Product> Scrape(int typeId)
        {
            var products = GetProductsDetails(typeId);
            var giftProducts = GetGiftProducts(products);

            foreach (var giftProduct in giftProducts)
            {
                products.Remove(giftProduct);
            }

            return products;
        }

        protected override List<Product> GetProductsDetails(int typeId)
        {
            var products = new List<Product>();

            MatchCollection matchCollectionExt = Regex.Matches(this.Data, this.BaseRegex);

            foreach (Match match in matchCollectionExt)
            {
                string productUrl = $"h{match.Groups[1].Value}";
                string name = match.Groups[2].Value;
                string description = string.Empty;
                string pictureUrl = match.Groups[3].Value;

                if (name.Contains('(') && name.Contains(',') && name.IndexOf('(') < name.IndexOf(','))
                {
                    description = name.Substring(name.IndexOf('(') + 1);
                    description = description[0..^1];
                    name = name.Substring(0, name.IndexOf('('));
                }
                else if (name.Contains(','))
                {
                    description = name.Substring(name.IndexOf(',') + 1);
                    name = name.Substring(0, name.IndexOf(','));
                }
                else
                {
                    description = name;
                }

                if (name.Contains("&#039;&#039"))
                {
                    name = name.Replace("&#039;&#039", inchesSign);
                    name = name.Replace("&#039;&#039", "");
                }
                if (description.Contains("&#039;&#039"))
                {
                    description = description.Replace("&#039;&#039", inchesSign);
                    description = description.Replace("&#039;&#039", "");
                }

                Product product = new Product()
                {
                    Name = name,
                    PictureUrl = pictureUrl,
                    Description = description,
                    ProductBrandId = GetProductBrand(name),
                    ProductTypeId = typeId,
                    ProductUrl = productUrl
                };

                products.Add(product);
            }

            return products;
        }

        //some collected items are gifts, remove them 
        private List<Product> GetGiftProducts(List<Product> products)
        {
            List<Product> giftProducts = new List<Product>();
            MatchCollection matchCollectionInt = Regex.Matches(this.Data, this.PriceRegex);
            for (int matchIndex = 0; matchIndex < matchCollectionInt.Count; matchIndex++)
            {
                Match match = matchCollectionInt[matchIndex];
                CultureInfo provider = new CultureInfo("ro-RO");

                if (products[matchIndex].Name.ToUpper().Contains(("cadou").ToUpper()) ||
                    products[matchIndex].Name.ToUpper().Contains(("bonus").ToUpper()))
                {
                    giftProducts.Add(products[matchIndex]);
                }
                else
                {
                    decimal price = decimal.Parse(match.Groups[1].Value, provider);
                    products[matchIndex].Price = price;

                    if (price == 0)
                    {
                        giftProducts.Add(products[matchIndex]);
                    }
                }
            }

            return giftProducts;
        }
    }
}