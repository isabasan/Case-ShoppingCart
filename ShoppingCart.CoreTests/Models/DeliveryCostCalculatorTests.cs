using Xunit;

namespace ShoppingCart.Core.Models.Tests
{
    public class DeliveryCostCalculatorTests
    {
        [Theory]
        [InlineData(2, 3, 2.99)]
        public void DeliveryCostCalculatorTest_Constructor(decimal costPerDelivery, decimal costPerProduct, decimal fixedCost)
        {
            DeliveryCostCalculator deliveryCostCalculator = new DeliveryCostCalculator(costPerDelivery, costPerProduct, fixedCost);
            Assert.Equal(costPerDelivery, deliveryCostCalculator.costPerDelivery);
            Assert.Equal(costPerProduct, deliveryCostCalculator.costPerProduct);
            Assert.Equal(fixedCost, deliveryCostCalculator.fixedCost);
        }

        [Fact()]
        public void CalculateForTest_ShouldReturnZero_WhenCartIsEmpty()
        {
            ShoppingCart cart = new ShoppingCart();

            DeliveryCostCalculator deliveryCostCalculator = new DeliveryCostCalculator(2, 3, (decimal)2.99);
            decimal calculatedDeliveryCost = deliveryCostCalculator.calculateFor(cart);

            Assert.True(calculatedDeliveryCost == 0);
        }

        [Theory]
        [InlineData(2, 3, 2.99, 10.99)]
        public void CalculateForTest_ShouldReturnExpectedCost_WhenFormulaIsApplied(decimal costPerDelivery, decimal costPerProduct, decimal fixedCost, decimal expectedCost)
        {
            Category category = new Category("food");

            Product apple = new Product("Apple", 100, category);
            Product almond = new Product("Almonds", 150, category);

            ShoppingCart cart = new ShoppingCart();
            cart.addItem(apple, 3);
            cart.addItem(almond, 1);

            DeliveryCostCalculator deliveryCostCalculator = new DeliveryCostCalculator(costPerDelivery, costPerProduct, fixedCost);
            decimal calculatedDeliveryCost = deliveryCostCalculator.calculateFor(cart);

            Assert.True(expectedCost == calculatedDeliveryCost);
        }
    }
}