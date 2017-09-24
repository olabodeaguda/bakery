using BakeryPR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BakeryPR.DAO
{
    public class ProductionReportDao : AbstractDao
    {
        public ProductionOverheadDao productionOverheadDao
        {
            get
            {
                return new ProductionOverheadDao();
            }
        }
        public ProductionDao productionDao
        {
            get
            {
                return new ProductionDao();
            }
        }

        public ProductionIngredentDao productionIngredentDao
        {
            get
            {
                return new ProductionIngredentDao();
            }
        }

        public ProductionProductDao productionProductDao
        {
            get
            {
                return new ProductionProductDao();
            }
        }

        public List<ProductionReportModel> ByDate(double startDate, double endDate)
        {
            List<ProductionReportModel> lst = new List<ProductionReportModel>();

            List<Production> lstProduction = productionDao.ByDateRange(startDate, endDate);

            foreach (var tm in lstProduction)
            {
                ProductionReportModel productionReportModel = new ProductionReportModel();
                productionReportModel.productionDate = new DateTime(tm.dateCreatedTimespan);
                productionReportModel.productionRecipe = tm.recipeTitle;
                productionReportModel.productionTitle = tm.title;
                productionReportModel.quantityOfFlour = tm.quantity;
                var totalingredient = productionIngredentDao.byProductionId(tm.id);
                productionReportModel.bulkDoughWeight = totalingredient.Sum(x => x.amount);
                productionReportModel.totalIngredientCost = Math.Round(totalingredient.Sum(x => (x.unitCost * x.amount)), 2);
                var productionoverhead = productionOverheadDao.byproductionId(tm.id);
                productionReportModel.totalOverheadCost = Math.Round(productionoverhead.Sum(x => x.overheadCount), 2);
                var productionProduct = productionProductDao.byproductionId(tm.id);
                productionReportModel.totalPackageCost = Math.Round(productionProduct.Sum(x => x.costOfPackage * x.quantity), 2);
                productionReportModel.totalProductWeight = Math.Round(productionProduct.Sum(x => x.weight * x.quantity), 2);
                productionReportModel.totalProductCost = productionReportModel.totalIngredientCost + productionReportModel.totalPackageCost + productionReportModel.totalOverheadCost;
                productionReportModel.products = ProductToString(productionProduct);
                lst.Add(productionReportModel);
            }

            return lst;
        }

        public List<ProductionReportModel> ByDate(double startDate)
        {
            List<ProductionReportModel> lst = new List<ProductionReportModel>();

            List<Production> lstProduction = productionDao.ByDateRange(startDate);

            foreach (var tm in lstProduction)
            {
                ProductionReportModel productionReportModel = new ProductionReportModel();
                productionReportModel.productionDate = new DateTime(tm.dateCreatedTimespan);
                productionReportModel.productionRecipe = tm.recipeTitle;
                productionReportModel.productionTitle = tm.title;
                productionReportModel.quantityOfFlour = tm.quantity;
                var totalingredient = productionIngredentDao.byProductionId(tm.id);
                productionReportModel.bulkDoughWeight = totalingredient.Sum(x => x.amount);
                productionReportModel.totalIngredientCost = Math.Round(totalingredient.Sum(x => (x.unitCost * x.amount)), 2);
                var productionoverhead = productionOverheadDao.byproductionId(tm.id);
                productionReportModel.totalOverheadCost = Math.Round(productionoverhead.Sum(x => x.overheadCount), 2);
                var productionProduct = productionProductDao.byproductionId(tm.id);
                productionReportModel.totalPackageCost = Math.Round(productionProduct.Sum(x => x.costOfPackage * x.quantity), 2);
                productionReportModel.totalProductWeight = Math.Round(productionProduct.Sum(x => x.weight), 2);
                productionReportModel.totalProductCost = productionReportModel.totalIngredientCost + productionReportModel.totalPackageCost + productionReportModel.totalOverheadCost;
                productionReportModel.products = ProductToString(productionProduct);
                lst.Add(productionReportModel);
            }

            return lst;
        }

        public String ProductToString(List<ProductionProduct> lst)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var tm in lst)
            {
                stringBuilder.Append($"{tm.productName} {tm.quantity}loave(s)\n");
            }

            return "";
        }
    }
}
