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
    public class ProductionProductDao : AbstractDao
    {

        public bool add(ProductionProduct values)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = "insert into productionProduct(productId,productionId,quantity) " +
                    "values(@productId,@productionId,@quantity)";
                cmd.Parameters.AddWithValue("@productId", values.productId);
                cmd.Parameters.AddWithValue("@productionId", values.productionId);
                cmd.Parameters.AddWithValue("@quantity", values.quantity);
                cmd.CommandType = CommandType.Text;
                int count = cmd.ExecuteNonQuery();
                if (count > 0)
                {
                    return true;
                }
            }

            return false;
        }

        public bool update(ProductionProduct values)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = "update productionProduct set productId=@productId,productionId=@productionId,quantity=@quantity where id = @id";
                cmd.Parameters.AddWithValue("@productId", values.productId);
                cmd.Parameters.AddWithValue("@productionId", values.productionId);
                cmd.Parameters.AddWithValue("@quantity", values.quantity);
                cmd.Parameters.AddWithValue("@id", values.id);
                cmd.CommandType = CommandType.Text;
                int count = cmd.ExecuteNonQuery();
                if (count > 0)
                {
                    return true;
                }
            }

            return false;
        }

        public ProductionProduct byProductionproductId(int prodId, int productId)
        {
            using (SQLiteConnection conn = new SQLiteConnection(this.connectionString))
            {
                conn.Open();
                DataSet dt = new DataSet();
                SQLiteCommand cmd = new SQLiteCommand(conn);
                string query = "select productionProduct.*,product.descripton as productName,measurementType.measureTypeName,product.weight  from productionProduct ";
                query += "inner join product on product.id = productionProduct.productId ";
                query += "inner join measurementType on measurementType.id = product.mTypeId where productionProduct.productionId = @productionId and productionProduct.ProductId = @productId ";

                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@productionId", prodId);
                cmd.Parameters.AddWithValue("@productId", productId);
                cmd.CommandType = CommandType.Text;
                this.SQLiteAdaptor(dt, cmd);

                return dt.Tables[0].Rows.Cast<DataRow>().Select(x => new ProductionProduct()
                {
                    id = int.Parse(x["id"].ToString()),
                    measureTypeName = x["measureTypeName"].ToString(),
                    productId = int.Parse(x["productId"].ToString()),
                    productionId = int.Parse(x["productionId"].ToString()),
                    quantity = int.Parse(x["quantity"].ToString()),
                    productName = x["productName"].ToString(),
                    weight = int.Parse(x["weight"].ToString())
                }).FirstOrDefault();
            }

        }

        public List<ProductionProduct> byproductionId(int productionId)
        {
            List<ProductionProduct> lst = new List<ProductionProduct>();
            using (SQLiteConnection conn = new SQLiteConnection(this.connectionString))
            {
                conn.Open();
                DataSet dt = new DataSet();
                SQLiteCommand cmd = new SQLiteCommand(conn);
                string query = "select productionProduct.*,product.descripton as productName,measurementType.measureTypeName,product.weight  from productionProduct ";
                query += "inner join product on product.id = productionProduct.productId ";
                query += "inner join measurementType on measurementType.id = product.mTypeId where productionProduct.productionId = @productionId ";

                cmd.CommandText = query;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@productionId", productionId);
                this.SQLiteAdaptor(dt, cmd);

                lst = dt.Tables[0].Rows.Cast<DataRow>().Select(x => new ProductionProduct()
                {
                    id = int.Parse(x["id"].ToString()),
                    measureTypeName = x["measureTypeName"].ToString(),
                    productId = int.Parse(x["productId"].ToString()),
                    productionId = int.Parse(x["productionId"].ToString()),
                    quantity = int.Parse(x["quantity"].ToString()),
                    expectedQuantity = int.Parse(x["quantity"].ToString()),
                    productName = x["productName"].ToString(),
                    weight = double.Parse(x["weight"].ToString()),
                    ingredientCost = double.Parse(x["ingredentCost"].ToString()),
                    overheadCost = double.Parse(x["overheadCost"].ToString())
                }).ToList();
            }

            return lst;
        }

        public double sumTotalProductIngram(List<ProductionProduct> e)
        {
            return e.Sum(x => x.measureTypeName.ToLower().Equals("kg") ? ((x.weight * x.quantity) * 100) : (x.weight * x.quantity));
        }

        public string updateString(ProductionProduct pp)
        {
            return $"UPDATE productionProduct set overheadCost='{pp.overheadCost}',ingredentCost='{pp.ingredientCost}' where id='{pp.id}' ;";
        }

        public string insertString(ProductionProduct pp)
        {
            return $"insert into productionProduct(productId,productionId,quantity,overheadCost,ingredentCost) " +
                $"values('{pp.productId}','{pp.productionId}','{pp.quantity}','{pp.overheadCost}','{pp.ingredientCost}');";
        }

    }
}
