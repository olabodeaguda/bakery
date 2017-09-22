using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BakeryPR.Models
{
    public class SalesRevenueModel
    {
        public int ProoductId { get; set; }
        public string ProductName { get; set; }
        public int QuantityProduced { get; set; }
        public double UnitPrice { get; set; }
        public double totalPrice { get; set; }
        public string ProductStatus { get; set; }
    }
}
