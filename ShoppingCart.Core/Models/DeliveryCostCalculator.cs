using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShoppingCart.Core.Models
{
    public class DeliveryCostCalculator
    {
        public decimal costPerDelivery { get; set; }
        public decimal costPerProduct { get; set; }
        public decimal fixedCost { get; set; }

        public DeliveryCostCalculator(decimal costPerDelivery, decimal costPerProduct, decimal fixedCost)
        {
            this.costPerDelivery = costPerDelivery;
            this.costPerProduct = costPerProduct;
            this.fixedCost = fixedCost;
        }

        public decimal calculateFor(ShoppingCart shoppingCart)
        {
            if (shoppingCart.items.Count == 0)
                return 0;

            decimal deliveryCost = 0;

            int numberOfDeliveries = shoppingCart.items.GroupBy(item => item.product.category).Count();
            int numberOfProducts = shoppingCart.items.Count;
            deliveryCost = (costPerDelivery * numberOfDeliveries) + (costPerProduct * numberOfProducts) + fixedCost;

            return deliveryCost;
        }
    }
}
