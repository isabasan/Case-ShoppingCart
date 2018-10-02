using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Core.Models
{
    public class CampaignDiscount
    {
        public Campaign campaign { get; set; }
        public decimal appliedDiscount { get; set; }
    }
}
