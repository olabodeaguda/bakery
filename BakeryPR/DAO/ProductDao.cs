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
    public class ProductDao:AbstractDao
    {
        public List<Product> all()
        {
            List<Product> lst = new List<Product>();
            using (SQLiteConnection conn = new SQLiteConnection(this.connectionString))
            {
                conn.Open();
                DataSet dt = new DataSet();
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = "select product.*,measurementType.measureTypeName from product inner join measurementType on product.mTypeId = measurementType.id";
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
                    measureTypeName = x["measureTypeName"].ToString()
                }).ToList();
            }

            return lst;
        }

        public bool add(Product values)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = "insert into product(weight,descripton,costOfPackage,retailPrice,wholeSales,mTypeId) " +
                    "values(@weight,@descripton,@costOfPackage,@retailPrice,@wholeSales,@mTypeId)";
                cmd.Parameters.AddWithValue("@weight", values.weight);
                cmd.Parameters.AddWithValue("@descripton", values.descripton);
                cmd.Parameters.AddWithValue("@costOfPackage", values.costOfPackage);
                cmd.Parameters.AddWithValue("@retailPrice", values.retailPrice);
                cmd.Parameters.AddWithValue("@wholeSales", values.wholeSales);
                cmd.Parameters.AddWithValue("@mTypeId", values.mTypeId);
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
                cmd.CommandText = "update product set retailPrice=@retailPrice,wholeSales=@wholeSales,mTypeId=@mTypeId, weight = @weight,descripton=@descripton,costOfPackage = @costOfPackage where id = @id";
                cmd.Parameters.AddWithValue("@weight", values.weight);
                cmd.Parameters.AddWithValue("@descripton", values.descripton);
                cmd.Parameters.AddWithValue("@costOfPackage", values.costOfPackage);
                cmd.Parameters.AddWithValue("@retailPrice", values.retailPrice);
                cmd.Parameters.AddWithValue("@wholeSales", values.wholeSales);
                cmd.Parameters.AddWithValue("@mTypeId", values.mTypeId);
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
