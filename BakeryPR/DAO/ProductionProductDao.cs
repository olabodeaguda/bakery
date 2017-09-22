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
                cmd.CommandText = "insert into productionProduct(productId,productionId,quantity,productWeight) " +
                    "values(@productId,@productionId,@quantity)";
                cmd.Parameters.AddWithValue("@productId", values.productId);
                cmd.Parameters.AddWithValue("@productionId", values.productionId);
                cmd.Parameters.AddWithValue("@quantity", values.quantity);
                cmd.Parameters.AddWithValue("@productWeight", values.weight);
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
                string query = "select productionProduct.*,product.descripton as productName,measurementType.measureTypeName,productionProduct.productWeight as weight  from productionProduct ";
                query += "inner join product on product.id = productionProduct.productId ";
                query += "inner join measurementType on measurementType.id = product.mTypeId where productionProduct.productionId = @productionId and productionProduct.ProductId = @productId ";

                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@productionId", prodId);
                cmd.Parameters.AddWithValue("@productId", productId);
                cmd.CommandType = CommandType.Text;
                this.SQLiteAdaptor(dt, cmd);

                //return dt.Tables[0].Rows.Cast<DataRow>().Select(x => new ProductionProduct()
                //{
                //    id = int.Parse(x["id"].ToString()),
                //    measureTypeName = x["measureTypeName"].ToString(),
                //    productId = int.Parse(x["productId"].ToString()),
                //    productionId = int.Parse(x["productionId"].ToString()),
                //    quantity = int.Parse(x["quantity"].ToString()),
                //    productName = x["productName"].ToString(),
                //    weight = int.Parse(x["weight"].ToString())
                //}).FirstOrDefault();
                var x = dt.Tables[0].Rows.Cast<DataRow>().FirstOrDefault();

                if (x != null)
                {
                    ProductionProduct pp = new ProductionProduct();
                    pp.id = int.Parse(x["id"].ToString());
                    pp.measureTypeName = x["measureTypeName"].ToString();
                    pp.productId = int.Parse(x["productId"].ToString());
                    pp.productionId = int.Parse(x["productionId"].ToString());
                    pp.quantity = int.Parse(x["quantity"].ToString());
                    pp.expectedQuantity = int.Parse(x["quantity"].ToString());
                    pp.productName = x["productName"].ToString();
                    pp.weight = double.Parse(x["weight"].ToString());
                    if (pp.measureTypeName.ToLower() == "gram")
                    {
                        pp.weight = pp.weight / 1000;
                        pp.measureTypeName = "kg";
                    }
                    pp.ingredientCost = double.Parse(x["ingredentCost"].ToString());
                    pp.overheadCost = double.Parse(x["overheadCost"].ToString());

                    return pp;
                }
                else
                {
                    return null;
                }
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
                string query = "select productionProduct.*,product.descripton as productName,measurementType.measureTypeName,productionProduct.productWeight as weight,product.costOfPackage  from productionProduct ";
                query += "inner join product on product.id = productionProduct.productId ";
                query += "inner join measurementType on measurementType.id = product.mTypeId where productionProduct.productionId = @productionId ";

                cmd.CommandText = query;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@productionId", productionId);
                this.SQLiteAdaptor(dt, cmd);

                foreach (var x in dt.Tables[0].Rows.Cast<DataRow>())
                {
                    ProductionProduct pp = new ProductionProduct();
                    pp.id = int.Parse(x["id"].ToString());
                    pp.measureTypeName = x["measureTypeName"].ToString();
                    pp.productId = int.Parse(x["productId"].ToString());
                    pp.productionId = int.Parse(x["productionId"].ToString());
                    pp.quantity = int.Parse(x["quantity"].ToString());
                    pp.expectedQuantity = int.Parse(x["quantity"].ToString());
                    pp.productName = x["productName"].ToString();
                    pp.weight = double.Parse(x["weight"].ToString());
                    //if (pp.measureTypeName.ToLower() == "gram")
                    //{
                    //    pp.weight = pp.weight / 1000;
                    //    pp.measureTypeName = "kg";
                    //}

                    pp.ingredientCost = double.Parse(x["ingredentCost"].ToString());
                    pp.overheadCost = double.Parse(x["overheadCost"].ToString());
                    pp.costOfPackage = double.Parse(x["costOfPackage"].ToString());
                    lst.Add(pp);
                }

            }

            return lst;
        }

        public double sumTotalProductIngram(List<ProductionProduct> e)
        {
            return e.Sum(x => x.measureTypeName.ToLower().Equals("kg") ? ((x.weight * x.quantity) / 1000) : (x.weight * x.quantity));
        }

        public double sumTotalProductInKg(List<ProductionProduct> e)
        {
            return e.Sum(x => x.measureTypeName.ToLower().Equals("gram") ? ((x.weight * x.quantity) * 1000) : (x.weight * x.quantity));
        }

        public string updateString(ProductionProduct pp)
        {
            return $"UPDATE productionProduct set overheadCost='{pp.overheadCost}',ingredentCost='{pp.ingredientCost}' where id='{pp.id}' ;";
        }

        public string updateString(ProductionProduct pp, int quantity, double weight)
        {
            return $"UPDATE productionProduct set productWeight='{weight}', quantity = '{quantity}',  overheadCost='{pp.overheadCost}',ingredentCost='{pp.ingredientCost}' where id='{pp.id}' ;";
        }

        public string insertString(ProductionProduct pp)
        {
            return $"insert into productionProduct(productId,productionId,quantity,overheadCost,ingredentCost,productWeight) " +
                $"values('{pp.productId}','{pp.productionId}','{pp.quantity}','{pp.overheadCost}','{pp.ingredientCost}','{pp.weight}');";
        }

        public string deleteProductionQuery(Production p)
        {
            return "delete from productionProduct where productionId = '" + p.id + "';";
        }
    }
}
