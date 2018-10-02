using Xunit;

namespace ShoppingCart.Core.Models.Tests
{
    public class CategoryTests
    {
        [Theory]
        [InlineData("food")]
        public void CategoryTest_Constructor(string title)
        {
            Category category = new Category(title);
            Assert.Equal(title, category.title);
        }
    }
}