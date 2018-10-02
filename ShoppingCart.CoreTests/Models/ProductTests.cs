using System.Collections.Generic;
using Xunit;

namespace ShoppingCart.Core.Models.Tests
{
    public class ProductTests
    {
        public static List<object[]> ProductTestData()
        {
            return new List<object[]>()
            {
                new object[] { "Apple", 100.0, new Category("food") }
            };
        }
        
        [Theory]
        [MemberData(nameof(ProductTestData))]
        public void ProductTest_Constructor(string title, decimal price, Category category)
        {
            Product product = new Product(title, price, category);
            Assert.Equal(title, product.title);
            Assert.Equal(price, product.price);
            Assert.Equal(category.title, product.category.title);
        }
    }
}