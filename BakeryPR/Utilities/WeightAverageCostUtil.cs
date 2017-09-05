using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BakeryPR.Utilities
{
    public class WeightAverageCostUtil
    {
        public static double calculate(double oldQuantity, double oldUnitCost, double newQuantity, double newUnitCost)
        {
            double tCost = (oldQuantity * oldUnitCost) + (newQuantity * newUnitCost);
            double tQuantity = oldQuantity + newQuantity;

            double result = Math.Round((tCost / tQuantity), 2);
            return double.IsNaN(result) || double.IsInfinity(result) ? 0 : result;
        }

        public static double ProductIngredentUnitCost(double productWeigt, double totalRecipeCost, double totalDougt)
        {
            double result = (productWeigt * totalRecipeCost) / totalDougt;
            return double.IsNaN(result) || double.IsInfinity(result) ? 0 : result; ;
        }

        public static double ProductOverheadUnitCost(double productWeigt, double totaloverhead, double totalDougt)
        {
            double result = (productWeigt * totaloverhead) / totalDougt;
            return double.IsNaN(result) || double.IsInfinity(result) ? 0 : result;
        }
    }
}
