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
                    title = "Manage Ingredient";
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
                    title = "Ingredient Movement Report";
                    return new ProductionIngredientHistory();
                case "ProductInventory":
                    title = "Product Movement Report";
                    return new ProductinventoryHistory();
                case "grpOverhead":
                    title = "Group Overhead";
                    return new GroupOverhead();
                case "company":
                    title = "About Company";
                    return new CompanyRegistration();
                case "saleshistory":
                    title = "Transaction History";
                    return new SalesHistory();
                case "productionReport":
                    title = "Production Report";
                    return new ProductionReport();
                default:
                    title = "Error";
                    return new ErrorView();
            }
        }
    }
}
