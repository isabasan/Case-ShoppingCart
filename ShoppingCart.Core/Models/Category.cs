using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCart.Core.Models
{
    public class Category
    {
        public Category parent { get; set; }
        public string title { get; set; }
        public decimal deliveryCost { get; set; }

        public Category (string title)
        {
            this.title = title;
        }
    }
}
