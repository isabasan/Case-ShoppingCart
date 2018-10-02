using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCart.Core.Models
{
    public class Product
    {
        public string title { get; set; }
        public decimal price { get; set; }
        public Category category { get; set; }

        public Product(string title, decimal price, Category category)
        {
            this.title = title;
            this.price = price;
            this.category = category;
        }
    }
}
