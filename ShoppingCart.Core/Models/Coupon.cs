using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ShoppingCart.Core.Components.Enums;

namespace ShoppingCart.Core.Models
{
    public class Coupon
    {
        public decimal minAmount { get; set; }
        public decimal discount { get; set; }
        public DiscountType discountType { get; set; }

        public Coupon(decimal minAmount, decimal discount, DiscountType discountType)
        {
            this.minAmount = minAmount;
            this.discount = discount;
            this.discountType = discountType;
        }
    }
}
