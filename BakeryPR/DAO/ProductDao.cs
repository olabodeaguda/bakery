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
    public class ProductDao : AbstractDao
    {
        public List<Product> all()
        {
            List<Product> lst = new List<Product>();
            using (SQLiteConnection conn = new SQLiteConnection(this.connectionString))
            {
                conn.Open();
                DataSet dt = new DataSet();
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = "select product.*,measurementType.measureTypeName from product inner join measurementType on product.mTypeId = measurementType.id order by product.descripton";
                cmd.CommandType = CommandType.Text;
                this.SQLiteAdaptor(dt, cmd);

                lst = dt.Tables[0].Rows.Cast<DataRow>().Select(x => new Product()
                {
                    id = int.Parse(x["id"].ToString()),
                    costOfPackage = double.Parse(x["costOfPackage"].ToString()),
                    descripton = x["descripton"].ToString(),
                    retailPrice = double.Parse(x["retailPrice"].ToString()),
                    weight = double.Parse(x["weight"].ToString()),
                    mTypeId = int.Parse(x["mTypeId"].ToString()),
                    wholeSales = double.Parse(x["wholeSales"].ToString()),
                    measureTypeName = x["measureTypeName"].ToString(),
                    name = x["name"].ToString(),
                    inventoryStore = String.IsNullOrEmpty(x["inventoryStore"].ToString()) ? 0 : int.Parse(x["inventoryStore"].ToString())
                }).ToList();
            }

            return lst;
        }

        public Product byId(int productId)
        {
            using (SQLiteConnection conn = new SQLiteConnection(this.connectionString))
            {
                conn.Open();
                DataSet dt = new DataSet();
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = "select product.*,measurementType.measureTypeName from product inner join measurementType on product.mTypeId = measurementType.id where product.id=@pId order by product.descripton";
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@pId", productId);
                this.SQLiteAdaptor(dt, cmd);

                var x = dt.Tables[0].Rows.Cast<DataRow>().FirstOrDefault();

                if (x != null)
                {
                    Product p = new Product();
                    p.id = int.Parse(x["id"].ToString());
                    p.costOfPackage = double.Parse(x["costOfPackage"].ToString());
                    p.descripton = x["descripton"].ToString();
                    p.retailPrice = double.Parse(x["retailPrice"].ToString());
                    p.weight = double.Parse(x["weight"].ToString());
                    p.mTypeId = int.Parse(x["mTypeId"].ToString());
                    p.wholeSales = double.Parse(x["wholeSales"].ToString());
                    p.measureTypeName = x["measureTypeName"].ToString();
                    if (p.measureTypeName.ToLower() == "gram")
                    {
                        p.weight = p.weight / 1000;
                        p.measureTypeName = "kg";
                    }
                   
                    p.name = x["name"].ToString();
                    p.inventoryStore = String.IsNullOrEmpty(x["inventoryStore"].ToString()) ? 0 : int.Parse(x["inventoryStore"].ToString());
                    p.isDiscount = string.IsNullOrEmpty(x["isDiscount"].ToString()) ? false : (int.Parse(x["isDiscount"].ToString()) == 1 ? true : false);
                    p.discount = string.IsNullOrEmpty(x["discount"].ToString()) ? 0.0 : (double.Parse(x["discount"].ToString()));

                    return p; 
                }
                else
                {
                    return null;
                }
            }

        }

        public bool add(Product values)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = "insert into product(weight,descripton,costOfPackage,retailPrice,wholeSales,mTypeId,name) " +
                    "values(@weight,@descripton,@costOfPackage,@retailPrice,@wholeSales,@mTypeId,@name)";
                cmd.Parameters.AddWithValue("@weight", values.weight);
                cmd.Parameters.AddWithValue("@descripton", values.descripton);
                cmd.Parameters.AddWithValue("@costOfPackage", values.costOfPackage);
                cmd.Parameters.AddWithValue("@retailPrice", values.retailPrice);
                cmd.Parameters.AddWithValue("@wholeSales", values.wholeSales);
                cmd.Parameters.AddWithValue("@mTypeId", values.mTypeId);
                cmd.Parameters.AddWithValue("@name", values.name);
                cmd.CommandType = CommandType.Text;
                int count = cmd.ExecuteNonQuery();
                if (count > 0)
                {
                    return true;
                }
            }

            return false;
        }

        public bool update(Product values)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = "update product set name=@name, retailPrice=@retailPrice,wholeSales=@wholeSales,mTypeId=@mTypeId, weight = @weight,descripton=@descripton,costOfPackage = @costOfPackage where id = @id";
                cmd.Parameters.AddWithValue("@weight", values.weight);
                cmd.Parameters.AddWithValue("@descripton", values.descripton);
                cmd.Parameters.AddWithValue("@costOfPackage", values.costOfPackage);
                cmd.Parameters.AddWithValue("@retailPrice", values.retailPrice);
                cmd.Parameters.AddWithValue("@wholeSales", values.wholeSales);
                cmd.Parameters.AddWithValue("@mTypeId", values.mTypeId);
                cmd.Parameters.AddWithValue("@name", values.name);
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


        public String updateStoreQuery(Product pr)
        {
            string query = "update product set inventoryStore=" + pr.inventoryStore + " where id = '" + pr.id + "' ;";
            return query;
        }

        public bool updateInventoryStore(Product values)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = "update product set inventoryStore=@inventoryStore where id = @id";
                cmd.Parameters.AddWithValue("@inventoryStore", values.inventoryStore);
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

    }
}
