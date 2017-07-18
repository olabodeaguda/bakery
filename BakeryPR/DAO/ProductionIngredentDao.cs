using BakeryPR.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Globalization;
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
            query = query + "'" + pi.createdBy + "'";
            query = query + ");";
            return query;
        }

        public List<ProductionIngredent> byProductionId(int productionId)
        {
            List<ProductionIngredent> lst = new List<ProductionIngredent>();
            using (SQLiteConnection conn = new SQLiteConnection(this.connectionString))
            {
                string query = " select ProductionIngredient.*,ingredent.unitCost,ingredent.ingredentName,measurementType.measureTypeName from ProductionIngredient ";
                query = query + " inner join ingredent on ingredent.id = ProductionIngredient.ingredentId ";
                query = query + " inner join measurementType on measurementType.id = ingredent.mTypeId ";
                query = query + " where ProductionIngredient.productionId = @pId ";
                conn.Open();
                DataSet dt = new DataSet();
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = query;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@pId", productionId);
                this.SQLiteAdaptor(dt, cmd);

                lst = dt.Tables[0].Rows.Cast<DataRow>().Select(x => new ProductionIngredent()
                {
                    id = int.Parse(x["id"].ToString()),
                    amount = double.Parse(x["amount"].ToString()),
                    createdBy = x["createdBy"].ToString(),
                    ingredentId = int.Parse(x["ingredentId"].ToString()),
                    ingredentName = x["ingredentName"].ToString(),
                    lastModifiiedBy = x.IsNull("lastModifiedBy") ? string.Empty : x["lastModifiiedBy"].ToString(),
                    dateCreated = DateTime.Parse(x["dateCreated"].ToString(), new CultureInfo("en-US", true)),
                    productionId = int.Parse(x["productionId"].ToString()),
                    measureType = x["measureTypeName"].ToString(),
                    unitCost = double.Parse(x["unitCost"].ToString())
                }).ToList();
            }

            return lst;
        }

    }
}
