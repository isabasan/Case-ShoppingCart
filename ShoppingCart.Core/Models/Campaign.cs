using System;
using System.Collections.Generic;
using System.Text;
using static ShoppingCart.Core.Components.Enums;

namespace ShoppingCart.Core.Models
{
    public class Campaign
    {
        public Category category { get; set; }
        public decimal discount { get; set; }
        public int quantity { get; set; }
        public DiscountType discountType { get; set; }

        public Campaign(Category category, decimal discount, int quantity, DiscountType discountType)
        {
            this.category = category;
            this.discount = discount;
            this.quantity = quantity;
            this.discountType = discountType;
        }
    }
}
