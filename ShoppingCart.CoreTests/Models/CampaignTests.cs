using ShoppingCart.Core.Models;
using System.Collections.Generic;
using Xunit;
using static ShoppingCart.Core.Components.Enums;

namespace ShoppingCart.Core.Models.Tests
{
    public class CampaignTests
    {
        public static List<object[]> CampaignTestData()
        {
            Category category = new Category("food");
            return new List<object[]>()
            {
                new object[] { category, 20, 3, DiscountType.Rate },
                new object[] { category, 50, 5, DiscountType.Rate },
                new object[] { category, 5, 5, DiscountType.Amount }
            };
        }

        [Theory]
        [MemberData(nameof(CampaignTestData))]
        public void CampaignTest_Constructor(Category category, decimal discount, int quantity, DiscountType discountType)
        {
            Campaign campaign = new Campaign(category, discount, quantity, discountType);
            Assert.True(campaign.category == category);
            Assert.True(campaign.discount == discount);
            Assert.True(campaign.quantity == quantity);
            Assert.True(campaign.discountType == discountType);
        }
    }
}