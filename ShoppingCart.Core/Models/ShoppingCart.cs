using System.Collections.Generic;
using System.Linq;
using static ShoppingCart.Core.Components.Enums;

namespace ShoppingCart.Core.Models
{
    public class ShoppingCart
    {
        public List<ShoppingCartItem> items { get; set; }
        public List<Campaign> campaigns { get; set; }
        public Coupon coupon { get; set; }

        private decimal itemsAmount { get { return items.Sum(i => i.quantity * i.product.price); } }
        private decimal campaignDiscount { get; set; }
        private decimal couponDiscount { get; set; }

        public ShoppingCart()
        {
            items = new List<ShoppingCartItem>();
            campaigns = new List<Campaign>();
            coupon = new Coupon(0, 0, DiscountType.Amount);
        }

        public void addItem(Product product, int quantity)
        {
            if (quantity <= 0)
                return;

            if (items.Exists(i => i.product == product))
                items.Find(i => i.product == product).quantity += quantity;
            else
                items.Add(new ShoppingCartItem() { product = product, quantity = quantity });
        }

        public void applyDiscounts(List<Campaign> campaigns)
        {
            this.campaigns = campaigns;
        }

        public void applyCoupon(Coupon coupon)
        {
            this.coupon = coupon;
        }

        public decimal getCampaignDiscount()
        {
            if (items.Count == 0)
                return 0;

            List<CampaignDiscount> campaignDiscounts = new List<CampaignDiscount>();
            for (int i = 0; i < campaigns.Count; i++)
            {
                var campaignItems = items.FindAll(item => item.product.category == campaigns[i].category);
                if (campaignItems.Count > 0 && campaignItems.Sum(item => item.quantity) >= campaigns[i].quantity)
                {
                    var discountAmount = campaigns[i].discount;
                    if (campaigns[i].discountType == DiscountType.Rate)
                    {
                        decimal campaignItemsAmount = campaignItems.Sum(item => item.quantity * item.product.price);
                        discountAmount = campaignItemsAmount * campaigns[i].discount / 100;
                    }

                    var campaignDiscount = campaignDiscounts.Find(c => c.campaign.category == campaigns[i].category);
                    if (campaignDiscount == null)
                        campaignDiscounts.Add(new CampaignDiscount() { campaign = campaigns[i], appliedDiscount = discountAmount });
                    else
                    {
                        if (discountAmount > campaignDiscount.appliedDiscount)
                        {
                            campaignDiscount.campaign = campaigns[i];
                            campaignDiscount.appliedDiscount = discountAmount;
                        }
                    }
                }
            }
            campaignDiscount = campaignDiscounts.Sum(item => item.appliedDiscount);
            return campaignDiscount;
        }

        public decimal getCouponDiscount()
        {
            if (items.Count == 0 || coupon == null)
                return 0;

            couponDiscount = 0;
            var discountedAmount = itemsAmount - getCampaignDiscount();
            if (discountedAmount >= coupon.minAmount)
            {
                couponDiscount = coupon.discount;
                if (coupon.discountType == DiscountType.Rate)
                    couponDiscount = discountedAmount * couponDiscount / 100;
            }
            return couponDiscount;
        }

        public decimal getTotalAmountAfterDiscounts()
        {
            if (items.Count == 0)
                return 0;

            return itemsAmount - getCampaignDiscount() - getCouponDiscount();
        }

        public decimal getDeliveryCost(DeliveryCostCalculator deliveryCostCalculator)
        {
            return deliveryCostCalculator.calculateFor(this);
        }

        public string print() //Bu method ile beklenen outputu tam olarak anlayamadım malesef :(
        {
            if (items.Count == 0)
                return "Cart is empty";

            string message = "";
            var grouppedItems = items.GroupBy(item => item.product.category).Select(grp => new { category = grp.Key, items = grp.ToList() });
            foreach(var group in grouppedItems)
            {
                message += group.category.title + ":\n";
                foreach(var item in group.items)
                {
                    message += item.product.title + " - " + item.quantity + " - " + item.product.price.ToString() + " - " + (item.quantity * item.product.price).ToString() + "\n";
                }
                message += "\n";
            }

            decimal totalDiscount = 0;
            var campaignDiscount = getCampaignDiscount();
            if (campaignDiscount > 0)
            {
                totalDiscount += campaignDiscount;
                message += "Campaign Discount: " + campaignDiscount + "\n";
            }

            var couponDiscount = getCouponDiscount();
            if (couponDiscount > 0)
            {
                totalDiscount += couponDiscount;
                message += "Coupon Discount: " + couponDiscount + "\n";
            }

            if (totalDiscount > 0)
            {
                message += "Total Discount: " + totalDiscount;
            }

            return message;
        }
    }
}
