using System.Collections.Generic;
using Xunit;
using static ShoppingCart.Core.Components.Enums;

namespace ShoppingCart.Core.Models.Tests
{
    public class CouponTests
    {
        public static List<object[]> CouponTestsData()
        {
            return new List<object[]>()
            {
                new object[] { 100, 20, DiscountType.Amount },
                new object[] { 50, 10, DiscountType.Rate }
            };
        }

        [Theory]
        [MemberData(nameof(CouponTestsData))]
        public void CouponTest_Constructor(decimal minAmount, decimal discount, DiscountType discountType)
        {
            Coupon coupon = new Coupon(minAmount, discount, discountType);
            Assert.Equal(minAmount, coupon.minAmount);
            Assert.Equal(discount, coupon.discount);
            Assert.Equal(discountType, coupon.discountType);
        }
    }
}