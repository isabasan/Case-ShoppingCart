using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCart.Core.Models
{
    public class ShoppingCartItem
    {
        public Product product { get; set; }
        public int quantity { get; set; }
    }
}
