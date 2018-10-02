using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;
using static ShoppingCart.Core.Components.Enums;

namespace ShoppingCart.Core.Models.Tests
{
    public class ShoppingCartTests
    {
        private readonly ITestOutputHelper output;

        public ShoppingCartTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact()]
        public void ShoppingCartTest_Constructor()
        {
            ShoppingCart cart = new ShoppingCart();
            Assert.True(cart.items != null);
            Assert.True(cart.items.Count == 0);
        }

        [Fact()]
        public void AddItemTest_ShouldNotAddItem_WhenQuantityIsLessThenOrEqualToZero()
        {
            Category food = new Category("food");

            Product apple = new Product("Apple", 100, food);
            Product almond = new Product("Almonds", 150, food);

            ShoppingCart cart = new ShoppingCart();
            cart.addItem(apple, 0);
            cart.addItem(almond, -1);

            Assert.True(cart.items.Count == 0);
        }

        [Fact()]
        public void AddItemTest_ShouldAddItem_WhenQuantityIsGreaterZero()
        {
            Category food = new Category("food");

            Product apple = new Product("Apple", 100, food);
            Product almond = new Product("Almonds", 150, food);

            ShoppingCart cart = new ShoppingCart();
            cart.addItem(apple, 3);
            cart.addItem(almond, 1);

            Assert.True(cart.items.Count == 2);
            Assert.True(cart.items.Exists(item => item.product.title == "Apple"));
            Assert.True(cart.items.Exists(item => item.product.title == "Almonds"));
            Assert.True(cart.items.Find(item => item.product.title == "Apple").quantity == 3);
            Assert.True(cart.items.Find(item => item.product.title == "Almonds").quantity == 1);
        }

        [Fact()]
        public void ApplyDiscountsTest_ShouldApplyCampaign()
        {
            ShoppingCart cart = new ShoppingCart();

            Category category = new Category("food");
            Campaign campaign1 = new Campaign(category, 20, 3, DiscountType.Rate);
            Campaign campaign2 = new Campaign(category, 50, 5, DiscountType.Rate);
            Campaign campaign3 = new Campaign(category, 5, 5, DiscountType.Amount);

            List<Campaign> campaigns = new List<Campaign>()
            {
                campaign1,
                campaign2,
                campaign3
            };

            cart.applyDiscounts(campaigns);
            Assert.True(cart.campaigns.Count == 3);

            Campaign campaign4 = new Campaign(category, 5, 5, DiscountType.Amount);
            cart.applyDiscounts(new List<Campaign>() { campaign4 });
            Assert.True(cart.campaigns.Count == 1);
        }

        [Fact()]
        public void ApplyDiscountsTest_ShouldNotAddCampaign_WhenCalledMultipleTimes()
        {
            ShoppingCart cart = new ShoppingCart();

            Category category = new Category("food");
            Campaign campaign1 = new Campaign(category, 20, 3, DiscountType.Rate);
            Campaign campaign2 = new Campaign(category, 50, 5, DiscountType.Rate);
            Campaign campaign3 = new Campaign(category, 5, 5, DiscountType.Amount);

            List<Campaign> campaigns = new List<Campaign>()
            {
                campaign1,
                campaign2,
                campaign3
            };

            cart.applyDiscounts(campaigns);
            cart.applyDiscounts(campaigns);
            Assert.True(cart.campaigns.Count == 3);
        }

        [Theory]
        [InlineData(100, 10, DiscountType.Rate)]
        public void ApplyCouponTest_ShouldApplyCoupon(decimal minAmount, decimal discount, DiscountType discountType)
        {
            ShoppingCart cart = new ShoppingCart();

            Coupon coupon = new Coupon(minAmount, discount, discountType);
            cart.applyCoupon(coupon);

            Assert.True(cart.coupon.minAmount == minAmount);
            Assert.True(cart.coupon.discount == discount);
            Assert.True(cart.coupon.discountType == discountType);
        }

        [Fact()]
        public void GetCampaignDiscountTest_ShouldApplyMaximumAmountOfDiscout_WhenMultipleCampaignAppliedAndRightCampaignHasDiscountTypeRate()
        {
            Category category = new Category("food");

            Product apple = new Product("Apple", 100, category);
            Product almond = new Product("Almonds", 150, category);

            ShoppingCart cart = new ShoppingCart();
            cart.addItem(apple, 3);
            cart.addItem(almond, 1);

            Campaign campaign1 = new Campaign(category, 20, 3, DiscountType.Rate);
            Campaign campaign2 = new Campaign(category, 50, 5, DiscountType.Rate);
            Campaign campaign3 = new Campaign(category, 5, 5, DiscountType.Amount);

            List<Campaign> campaigns = new List<Campaign>()
            {
                campaign1,
                campaign2,
                campaign3
            };

            cart.applyDiscounts(campaigns);
            var campaignDiscount = cart.getCampaignDiscount();
            Assert.True(campaignDiscount == 90);
        }

        [Fact()]
        public void GetCampaignDiscountTest_ShouldApplyMaximumAmountOfDiscout_WhenMultipleCampaignAppliedAndRightCampaignHasDiscountTypeAmount()
        {
            Category category = new Category("food");

            Product apple = new Product("Apple", 100, category);
            Product almond = new Product("Almonds", 150, category);

            ShoppingCart cart = new ShoppingCart();
            cart.addItem(apple, 3);
            cart.addItem(almond, 1);

            Campaign campaign1 = new Campaign(category, 1, 3, DiscountType.Rate);
            Campaign campaign2 = new Campaign(category, 2, 3, DiscountType.Rate);
            Campaign campaign3 = new Campaign(category, 50, 3, DiscountType.Amount);

            List<Campaign> campaigns = new List<Campaign>()
            {
                campaign1,
                campaign2,
                campaign3
            };

            cart.applyDiscounts(campaigns);
            var campaignDiscount = cart.getCampaignDiscount();
            Assert.True(campaignDiscount == 50);
        }

        [Fact()]
        public void GetCampaignDiscountTest_ShouldApplyMultipleCampaign_WhenCartMeetMultipleCampaignRequirements()
        {
            Category category = new Category("food");

            Product apple = new Product("Apple", 100, category);
            Product almond = new Product("Almonds", 150, category);

            ShoppingCart cart = new ShoppingCart();
            cart.addItem(apple, 3);
            cart.addItem(almond, 1);

            Category category2 = new Category("other");
            Product banana = new Product("Banana", 100, category2);
            cart.addItem(banana, 1);

            Campaign campaign1 = new Campaign(category, 20, 3, DiscountType.Rate);
            Campaign campaign2 = new Campaign(category, 50, 5, DiscountType.Rate);
            Campaign campaign3 = new Campaign(category, 5, 5, DiscountType.Amount);
            Campaign campaign4 = new Campaign(category2, 50, 1, DiscountType.Rate);

            List<Campaign> campaigns = new List<Campaign>()
            {
                campaign1,
                campaign2,
                campaign3,
                campaign4
            };

            cart.applyDiscounts(campaigns);
            var campaignDiscount = cart.getCampaignDiscount();
            Assert.True(campaignDiscount == 140);
        }

        [Fact()]
        public void GetCampaignDiscountTest_ShouldNotApplyCampaign_WhenCartDoesNotMeetCampaignRequirements()
        {
            Category category = new Category("food");

            Product apple = new Product("Apple", 100, category);
            Product almond = new Product("Almonds", 150, category);

            ShoppingCart cart = new ShoppingCart();
            cart.addItem(apple, 3);
            cart.addItem(almond, 1);

            Campaign campaign1 = new Campaign(category, 1, 999, DiscountType.Rate);
            Campaign campaign2 = new Campaign(category, 2, 999, DiscountType.Rate);
            Campaign campaign3 = new Campaign(category, 50, 999, DiscountType.Amount);

            List<Campaign> campaigns = new List<Campaign>()
            {
                campaign1,
                campaign2,
                campaign3
            };

            cart.applyDiscounts(campaigns);
            var campaignDiscount = cart.getCampaignDiscount();
            Assert.True(campaignDiscount == 0);
        }

        [Fact()]
        public void GetCampaignDiscountTest_ShouldReturnZero_WhenCartIsEmpty()
        {
            ShoppingCart cart = new ShoppingCart();

            Category category = new Category("food");
            Campaign campaign1 = new Campaign(category, 1, 999, DiscountType.Rate);
            Campaign campaign2 = new Campaign(category, 2, 999, DiscountType.Rate);
            Campaign campaign3 = new Campaign(category, 50, 999, DiscountType.Amount);

            List<Campaign> campaigns = new List<Campaign>()
            {
                campaign1,
                campaign2,
                campaign3
            };

            cart.applyDiscounts(campaigns);
            var campaignDiscount = cart.getCampaignDiscount();
            Assert.True(campaignDiscount == 0);
        }

        [Fact()]
        public void GetCampaignDiscountTest_ShouldReturnZero_WhenNoCampaignsApplied()
        {
            ShoppingCart cart = new ShoppingCart();

            var campaignDiscount = cart.getCampaignDiscount();

            Assert.True(campaignDiscount == 0);
        }

        [Theory]
        [InlineData(100, 10, DiscountType.Rate, 45)]
        [InlineData(100, 10, DiscountType.Amount, 10)]
        public void GetCouponDiscountTest_ShouldApplyCoupon_WhenCartMeetsCouponRequirements(decimal minAmount, decimal discount, DiscountType discountType, decimal expectedDiscount)
        {
            Category category = new Category("food");

            Product apple = new Product("Apple", 100, category);
            Product almond = new Product("Almonds", 150, category);

            ShoppingCart cart = new ShoppingCart();
            cart.addItem(apple, 3);
            cart.addItem(almond, 1); //cart amount 450

            Coupon coupon = new Coupon(minAmount, discount, discountType);
            cart.applyCoupon(coupon);

            var calculatedCouponDiscount = cart.getCouponDiscount();
            Assert.True(calculatedCouponDiscount == expectedDiscount);
        }

        [Fact()]
        public void GetCouponDiscountTest_ShouldNotApplyCoupon_WhenCartDoesNotMeetCouonRequirements()
        {
            Category category = new Category("food");

            Product apple = new Product("Apple", 100, category);
            Product almond = new Product("Almonds", 150, category);

            ShoppingCart cart = new ShoppingCart();
            cart.addItem(apple, 3);
            cart.addItem(almond, 1);

            Coupon coupon = new Coupon(1000, 20, DiscountType.Amount);
            cart.applyCoupon(coupon);

            var couponDiscount = cart.getCouponDiscount();
            Assert.True(couponDiscount == 0);
        }

        [Fact()]
        public void GetCouponDiscountTest_ShouldReturnZero_WhenCartIsEmpty()
        {
            ShoppingCart cart = new ShoppingCart();

            Coupon coupon = new Coupon(100, 20, DiscountType.Amount);
            cart.applyCoupon(coupon);

            var couponDiscount = cart.getCouponDiscount();
            Assert.True(couponDiscount == 0);
        }

        [Fact()]
        public void GetCouponDiscountTest_ShouldReturnZero_WhenNoCouponApplied()
        {
            ShoppingCart cart = new ShoppingCart();

            var couponDiscount = cart.getCouponDiscount();
            Assert.True(couponDiscount == 0);
        }

        [Theory]
        [InlineData(2, 3, 2.99, 10.99)]
        public void GetDeliveryCostTest_ShouldReturnExpectedDeliveryCost_WhenRequiredDataProvided(decimal costPerDelivery, decimal costPerProduct, decimal fixedCost, decimal expectedCost)
        {
            Category category = new Category("food");

            Product apple = new Product("Apple", 100, category);
            Product almond = new Product("Almonds", 150, category);

            ShoppingCart cart = new ShoppingCart();
            cart.addItem(apple, 3);
            cart.addItem(almond, 1);

            DeliveryCostCalculator deliveryCostCalculator = new DeliveryCostCalculator(costPerDelivery, costPerProduct, fixedCost);
            decimal calculatedDeliveryCost = cart.getDeliveryCost(deliveryCostCalculator);

            Assert.True(calculatedDeliveryCost == expectedCost);
        }

        [Theory]
        [InlineData(2, 3, 2.99)]
        public void GetDeliveryCostTest_ShouldReturnZero_WhenCartIsEmpty(decimal costPerDelivery, decimal costPerProduct, decimal fixedCost)
        {
            ShoppingCart cart = new ShoppingCart();

            DeliveryCostCalculator deliveryCostCalculator = new DeliveryCostCalculator(costPerDelivery, costPerProduct, fixedCost);
            decimal calculatedDeliveryCost = cart.getDeliveryCost(deliveryCostCalculator);

            Assert.True(calculatedDeliveryCost == 0);
        }

        [Fact()]
        public void GetTotalAmountAfterDiscountsTest_ShouldReturnZero_WhenCartIsEmpty()
        {
            ShoppingCart cart = new ShoppingCart();
            Assert.True(cart.getTotalAmountAfterDiscounts() == 0);
        }

        [Fact()]
        public void GetTotalAmountAfterDiscountsTest_ShouldReturnItemsAmount_WhenNoDiscountApplied()
        {
            Category category = new Category("food");

            Product apple = new Product("Apple", 100, category);
            Product almond = new Product("Almonds", 150, category);

            ShoppingCart cart = new ShoppingCart();
            cart.addItem(apple, 3);
            cart.addItem(almond, 1); //cart amount 450

            Assert.True(cart.getTotalAmountAfterDiscounts() == 450);
        }

        [Fact()]
        public void GetTotalAmountAfterDiscountsTest_ShouldReturnItemsAmount_WhenDiscountsAppliedButNoMatch()
        {
            Category category = new Category("food");

            Product apple = new Product("Apple", 100, category);
            Product almond = new Product("Almonds", 150, category);

            ShoppingCart cart = new ShoppingCart();
            cart.addItem(apple, 3);
            cart.addItem(almond, 1); //cart amount 450

            Campaign campaign1 = new Campaign(category, 20, 1000, DiscountType.Rate);
            List<Campaign> campaigns = new List<Campaign>() { campaign1 };
            cart.applyDiscounts(campaigns);

            Coupon coupon = new Coupon(1000, 100, DiscountType.Rate);
            cart.applyCoupon(coupon);

            Assert.True(cart.getTotalAmountAfterDiscounts() == 450);
        }

        [Fact()]
        public void GetTotalAmountAfterDiscountsTest_ShouldReturnExpectedAmount_WhenOnlyCampaignsAppliedAndSingleMatch()
        {
            Category category = new Category("food");

            Product apple = new Product("Apple", 100, category);
            Product almond = new Product("Almonds", 150, category);

            ShoppingCart cart = new ShoppingCart();
            cart.addItem(apple, 3);
            cart.addItem(almond, 1); //cart amount 450

            Campaign campaign1 = new Campaign(category, 20, 3, DiscountType.Rate);
            Campaign campaign2 = new Campaign(category, 50, 5, DiscountType.Rate);
            Campaign campaign3 = new Campaign(category, 5, 5, DiscountType.Amount);

            List<Campaign> campaigns = new List<Campaign>()
            {
                campaign1,
                campaign2,
                campaign3
            };

            cart.applyDiscounts(campaigns);
            Assert.True(cart.getTotalAmountAfterDiscounts() == 360);
        }

        [Fact()]
        public void GetTotalAmountAfterDiscountsTest_ShouldReturnExpectedAmount_WhenOnlyCampaignsAppliedAndMultipleMatch()
        {
            Category category = new Category("food");

            Product apple = new Product("Apple", 100, category);
            Product almond = new Product("Almonds", 150, category);

            ShoppingCart cart = new ShoppingCart();
            cart.addItem(apple, 3);
            cart.addItem(almond, 1); //cart amount 450

            Category category2 = new Category("other");
            Product banana = new Product("Banana", 100, category2);
            cart.addItem(banana, 1);

            Campaign campaign1 = new Campaign(category, 20, 3, DiscountType.Rate);
            Campaign campaign2 = new Campaign(category, 50, 5, DiscountType.Rate);
            Campaign campaign3 = new Campaign(category, 5, 5, DiscountType.Amount);
            Campaign campaign4 = new Campaign(category2, 50, 1, DiscountType.Rate);

            List<Campaign> campaigns = new List<Campaign>()
            {
                campaign1,
                campaign2,
                campaign3,
                campaign4
            };

            cart.applyDiscounts(campaigns);
            Assert.True(cart.getTotalAmountAfterDiscounts() == 410);
        }

        [Fact()]
        public void GetTotalAmountAfterDiscountsTest_ShouldReturnExpectedAmount_WhenOnlyCouponsAppliedAndMatch()
        {
            Category category = new Category("food");

            Product apple = new Product("Apple", 100, category);
            Product almond = new Product("Almonds", 150, category);

            ShoppingCart cart = new ShoppingCart();
            cart.addItem(apple, 3);
            cart.addItem(almond, 1); //cart amount 450

            Coupon coupon = new Coupon(100, 100, DiscountType.Amount);
            cart.applyCoupon(coupon);

            Assert.True(cart.getTotalAmountAfterDiscounts() == 350);
        }

        [Fact()]
        public void GetTotalAmountAfterDiscountsTest_ShouldReturnExpectedAmount_WhenCampaignsAndCouponAppliedAndMultipleMatch()
        {
            Category category = new Category("food");

            Product apple = new Product("Apple", 100, category);
            Product almond = new Product("Almonds", 150, category);

            ShoppingCart cart = new ShoppingCart();
            cart.addItem(apple, 3);
            cart.addItem(almond, 1); //cart amount 450

            Category category2 = new Category("other");
            Product banana = new Product("Banana", 100, category2);
            cart.addItem(banana, 1); //cart amount 550

            Campaign campaign1 = new Campaign(category, 20, 3, DiscountType.Rate);
            Campaign campaign2 = new Campaign(category, 50, 5, DiscountType.Rate);
            Campaign campaign3 = new Campaign(category, 5, 5, DiscountType.Amount);
            Campaign campaign4 = new Campaign(category2, 50, 1, DiscountType.Rate);

            List<Campaign> campaigns = new List<Campaign>()
            {
                campaign1,
                campaign2,
                campaign3,
                campaign4
            };
            cart.applyDiscounts(campaigns);

            Coupon coupon = new Coupon(100, 100, DiscountType.Amount);
            cart.applyCoupon(coupon);

            Assert.True(cart.getTotalAmountAfterDiscounts() == 310);
        }

        [Fact()]
        public void PrintTest()
        {
            Category category = new Category("food");
            Product apple = new Product("Apple", 100, category);
            Product almond = new Product("Almonds", 150, category);
            
            Category category2 = new Category("other");
            Product banana = new Product("Banana", 100, category2);

            ShoppingCart cart = new ShoppingCart();
            cart.addItem(apple, 3);
            cart.addItem(banana, 1);
            cart.addItem(almond, 1);

            Campaign campaign1 = new Campaign(category, 20, 3, DiscountType.Rate);
            Campaign campaign2 = new Campaign(category, 50, 5, DiscountType.Rate);
            Campaign campaign3 = new Campaign(category, 5, 5, DiscountType.Amount);
            Campaign campaign4 = new Campaign(category2, 50, 1, DiscountType.Rate);

            List<Campaign> campaigns = new List<Campaign>()
            {
                campaign1,
                campaign2,
                campaign3,
                campaign4
            };
            cart.applyDiscounts(campaigns);

            Coupon coupon = new Coupon(100, 100, DiscountType.Amount);
            cart.applyCoupon(coupon);

            var message = cart.print();
            output.WriteLine(message);

            Assert.True(!string.IsNullOrEmpty(message), "You can see the output in test output");
        }
    }
}