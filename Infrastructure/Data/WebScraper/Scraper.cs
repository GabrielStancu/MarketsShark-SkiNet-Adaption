using System;
using System.Collections.Generic;
using System.Linq;
using Core.Entities;

namespace Infrastructure.Data.WebScraper
{
    //base scraper in the builder design pattern 
    public abstract class Scraper
    {
        protected const string inchesSign = "''";

        public string Data { get; protected set; }
        public string BaseRegex { get; protected set; }
        public string PriceRegex { get; protected set; }
        public string UrlRegex { get; protected set; }
        public List<(string Url, int typeId)> Urls { get; set; }

        public Scraper WithData(string data)
        {
            this.Data = data;
            return this;
        }

        public Scraper WithBaseRegex(string regex)
        {
            this.BaseRegex = regex;
            return this;
        }

        public Scraper WithPriceRegex(string regex)
        {
            this.PriceRegex = regex;
            return this;
        }

        public Scraper WithUrlRegex(string regex)
        {
            this.UrlRegex = regex;
            return this;
        }

        public abstract List<Product> Scrape(int typeId);

        protected abstract List<Product> GetProductsDetails(int typeId);

        protected void GetProductsPrices(List<Product> products)
        {
            foreach(var product in products)
            {
                product.Price = 0;
            }
        }

        protected void GetProductsUrls(List<Product> products)
        {
            foreach (var product in products)
            {
                product.ProductUrl = String.Empty;
            }
        }

        protected int GetProductBrand(string name)
        {
            List<(int Id, string Name)> brands = new List<(int Id, string Name)>()
            {
                (1, "ASUS"),
                (2, "HP"),
                (3, "Lenovo"),
                (4, "Apple"),
                (5, "Dell"),
                (6, "Acer"),
                (7, "MSI"),
                (8, "Microsoft"),
                (9, "Huawei"),
                (10, "Alienware"),
                (11, "Razer"),
                (12, "Toshiba"),
                (13, "Fujitsu"),
                (14, "Others")
            };

            try
            {
                int id = brands.First(b => name.ToUpper().Contains(b.Name.ToUpper())).Id;
                return id;
            } 
            catch (Exception)
            {
                return brands.Count;
            }
        }
    }
}