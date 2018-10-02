using System.ComponentModel;

namespace ShoppingCart.Core.Components
{
    public class Enums
    {
        public enum DiscountType
        {
            [Description("Oran")]
            Rate = 1,
            [Description("Tutar")]
            Amount = 2
        }
    }
}
