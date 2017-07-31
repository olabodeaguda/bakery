using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BakeryPR.Models
{
    public class ExcelModel
    {
        private List<ProductionCostModel> _ProductionCost = new List<ProductionCostModel>();

        public List<ProductionCostModel> ProductionCost
        {
            get { return _ProductionCost; }
            set { _ProductionCost = value; }
        }

        private List<SalesRevenueModel> _SalesCost = new List<SalesRevenueModel>();

        public List<SalesRevenueModel> SalesCost
        {
            get { return _SalesCost; }
            set { _SalesCost = value; }
        }

        private List<Overhead> _OverrheadsCost = new List<Overhead>();

        public List<Overhead> OverrheadsCost
        {
            get { return _OverrheadsCost; }
            set
            {
                _OverrheadsCost = value;                
            }
        }
    }
}
