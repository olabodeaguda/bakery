using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BakeryPR.Models
{
    public class ProductionReportModel
    {
        public DateTime productionDate { get; set; }
        public string productionTitle { get; set; }
        public string productionRecipe { get; set; }
        public string products { get; set; }
        public double quantityOfFlour { get; set; }
        public double quantityOfProduct { get; set; }
        public double bulkDoughWeight { get; set; }
        public double totalProductWeight { get; set; }
        public double totalIngredientCost { get; set; }
        public double totalPackageCost { get; set; }
        public double totalOverheadCost { get; set; }
        public double totalProductCost { get; set; }
    }
}
