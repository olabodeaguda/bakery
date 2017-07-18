using BakeryPR.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BakeryPR.DAO
{
    public class ProductionIngredentDao : AbstractDao
    {
        public String getProdString(ProductionIngredent pi)
        {
            String query = "";

            query = query + " insert into ProductionIngredient(ingredentId,productionId,amount,datecreated,createdBy) ";
            query = query + "values(";
            query = query + pi.ingredentId + ",";
            query = query + pi.productionId + ",";
            query = query + pi.amount + ",";
            query = query + "'" + DateTime.Now.ToString("yyyy-MM-dd") + "',";
            query = query + "'"+ pi.createdBy + "'";
            query = query + ");";
            return query;
        }

        public List<ProductionIngredent> byProductionId(int productionId)
        {
            List<ProductionIngredent> lst = new List<ProductionIngredent>();
            using (SQLiteConnection conn = new SQLiteConnection(this.connectionString))
            {
                conn.Open();
                DataSet dt = new DataSet();
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = "select production.*,recipe.title as recipeTitle from production inner join recipe on recipe.id=production.recipeId order by production.title desc";
                cmd.CommandType = CommandType.Text;
                this.SQLiteAdaptor(dt, cmd);

                lst = dt.Tables[0].Rows.Cast<DataRow>().Select(x => new ProductionIngredent()
                {
                    id = int.Parse(x["id"].ToString()),
                    createdBY = x["createdBy"].ToString(),
                    title = x["title"].ToString(),
                    recipeTitle = x["recipeTitle"].ToString(),
                    quantity = String.IsNullOrEmpty(x["quantity"].ToString()) ? 0 : double.Parse(x["quantity"].ToString()),
                    recipeId = int.Parse(x["recipeId"].ToString()),
                    dateCreated = DateTime.Parse(x["dateCreated"].ToString(), new CultureInfo("en-US", true)),
                    lastUpdated = DateTime.Parse(x["lastUpdated"].ToString(), new CultureInfo("en-US", true))
                }).ToList();
            }

            return lst;
        }

    }
}
