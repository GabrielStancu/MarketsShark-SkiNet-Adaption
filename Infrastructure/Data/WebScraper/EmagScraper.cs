using System.Collections.Generic;
using System.Text.RegularExpressions;
using Core.Entities;

namespace Infrastructure.Data.WebScraper
{
    //scraped the emag website 
    public class EmagScraper : Scraper
    {
        public EmagScraper()
        {
            //the urls we extract information from 
            Urls = new List<(string url, int typeId)>
            {
                ("https://www.emag.ro/laptopuri/c?ref=hp_menu_quick-nav_1_1&type=category", 1),
                ("https://www.emag.ro/laptopuri/p2/c", 1),
                ("https://www.emag.ro/laptopuri/p3/c", 1),
                ("https://www.emag.ro/laptopuri/p4/c", 1),
                ("https://www.emag.ro/laptopuri/p5/c", 1),
                ("https://www.emag.ro/laptopuri/p6/c", 1),
                ("https://www.emag.ro/laptopuri/p7/c", 1),
                ("https://www.emag.ro/laptopuri/p8/c", 1),
                ("https://www.emag.ro/laptopuri/p9/c", 1),
                ("https://www.emag.ro/desktop-pc/c?ref=hp_menu_quick-nav_23_1&type=category", 2),
                ("https://www.emag.ro/desktop-pc/p2/c", 2),
                ("https://www.emag.ro/desktop-pc/p3/c", 2),
                ("https://www.emag.ro/desktop-pc/p4/c", 2),
                ("https://www.emag.ro/desktop-pc/p5/c", 2),
                ("https://www.emag.ro/desktop-pc/p6/c", 2),
                ("https://www.emag.ro/desktop-pc/p7/c", 2)
            };
        }

        //sets the regex pattern for the price
        public new Scraper WithPriceRegex(string regex)
        {
            this.PriceRegex = regex;
            return this;
        }

        //sets the regex pattern for the url of the product

        public new Scraper WithUrlRegex(string regex)
        {
            this.UrlRegex = regex;
            return this;
        }

        public override List<Product> Scrape(int typeId)
        {
            var products = GetProductsDetails(typeId);
            GetProductsUrls(products);
            GetProductsPrices(products);

            return products;
        }

        //gets all product details apart from the url and price

        protected override List<Product> GetProductsDetails(int typeId)
        {
            List<Product> products = new List<Product>();

            MatchCollection matchCollectionExt = Regex.Matches(this.Data, this.BaseRegex);

            foreach (Match match in matchCollectionExt)
            {
                string pictureUrl = match.Groups[1].Value;
                string name = match.Groups[2].Value;
                string description = string.Empty;

                if (name.Contains(','))
                {
                    description = name.Substring(name.IndexOf(',') + 1);
                    name = name.Substring(0, name.IndexOf(','));
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

        //gets the prices of the products in a separate format
        private new void GetProductsPrices(List<Product> products)
        {
            MatchCollection matchCollectionInt = Regex.Matches(Data, PriceRegex);

            for (int matchIndex = 0; matchIndex < matchCollectionInt.Count; matchIndex++)
            {
                Match match = matchCollectionInt[matchIndex];
                string intPart = match.Groups[1].Value;
                string decPart = match.Groups[2].Value;
                string priceString = $"{intPart[0]}{intPart.Substring(intPart.IndexOf(';') + 1)}.{decPart}";

                decimal price = decimal.Parse(priceString);
                products[matchIndex].Price = price;
            }
        }

        //the urls separately 
        private new void GetProductsUrls(List<Product> products)
        {
            MatchCollection matchCollectionUrl = Regex.Matches(this.Data, this.UrlRegex);

            for (int matchIndex = 0; matchIndex < matchCollectionUrl.Count; matchIndex++)
            {
                Match match = matchCollectionUrl[matchIndex];
                products[matchIndex].ProductUrl = match.Groups[1].Value;
            }
        }
    }
}