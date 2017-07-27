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
        public static object getNav(string pms,out string title)
        {
            switch (pms)
            {
                case "dashboard":
                    title = "Dashboard";
                    return new UCDashboard();
                case "ingredient":
                    title = "Manage Ingredent";
                    return new IngredentView();
                case "mAnalysis":
                    title = "Dashboard";
                    return new UCDashboard();
                case "product":
                    title = "Manage Product";
                    return new ProductView();
                case "overhead":
                    title = "Manage Overhead";
                    return new OverheadView();
                case "production":
                    title = "Manage Production";
                    return new ProductionView();
                case "sales":
                    title = "Manage Sales";
                    return new SalesView();
                case "user":
                    title = "User Management";
                    return new UserManagement();
                case "reports":
                    title = "Manage Report";
                    return new UCDashboard();
                case "recipe":
                    title = "Manage Recipe";
                    return new RecipeIngredentView();
                case "IngredientInventory":
                    title = "Ingredient Inventory History";
                    return new ProductionIngredientHistory();
                case "ProductInventory":
                    title = "Ingredient Inventory History";
                    return new ProductinventoryHistory();
                default:
                    title = "Error";
                    return new ErrorView();
            }
        }
    }
}
