using BakeryPR.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BakeryPR.Utilities
{
    public class NavigateUtils
    {
        public static object getNav(string pms)
        {
            switch (pms)
            {
                case "dashboard":
                    return new UCDashboard();
                case "ingredient":
                    return new IngredentView();
                case "mAnalysis":
                    return new UCDashboard();
                case "product":
                    return new ProductView();
                case "overhead":
                    return new OverheadView();
                case "production":
                    return new Production();
                case "sales":
                    return new UCDashboard();
                case "reports":
                    return new UCDashboard();
                case "recipe":
                    return new RecipeIngredentView();
                default:
                    return new ErrorView();
            }
        }
    }
}
