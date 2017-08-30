using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BakeryPR.Utilities
{
    public class WeightAverageCostUtil
    {
        public static double calculate(double oldQuantity,double oldUnitCost,double newQuantity,double newUnitCost)
        {
            double tCost = (oldQuantity * oldUnitCost) + (newQuantity*newUnitCost);
            double tQuantity = oldQuantity + newQuantity;

            return Math.Round((tCost / tQuantity),2);
        }

        public static double ProductIngredentUnitCost(double productWeigt, double totalRecipeCost,double totalDougt)
        {
            return (productWeigt * totalRecipeCost) / totalDougt;
        }

        public static double ProductOverheadUnitCost(double productWeigt, double totaloverhead, double totalDougt)
        {
            return (productWeigt * totaloverhead) / totalDougt;
        }
    }
}
