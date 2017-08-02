using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BakeryPR.Models
{
    public class ProductionCostModel
    {
        public int ingredentId { get; set; }
        public string ingredentName { get; set; }
        public double quantity { get; set; }
        public double UnitCost { get; set; }
        public double totalCost { get; set; }
    }
}
