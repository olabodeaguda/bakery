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
    public class ProductInventoryHistoryDao : AbstractDao
    {
        public List<ProductInventoryHistory> byId(int id)
        {
            List<ProductInventoryHistory> lst = new List<ProductInventoryHistory>();
            using (SQLiteConnection conn = new SQLiteConnection(this.connectionString))
            {
                string query = "select ProductInventoryHistory.*,product.name from ProductInventoryHistory ";
                query = query + "inner join product on product.id = ProductInventoryHistory.productId ";
                query = query + " where ProductInventoryHistory.productId = @id ";
                query = query + "order by ProductInventoryHistory.id desc ";

                conn.Open();
                DataSet dt = new DataSet();
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@id", id);
                cmd.CommandType = CommandType.Text;
                this.SQLiteAdaptor(dt, cmd);

                lst = dt.Tables[0].Rows.Cast<DataRow>().Select(x => new ProductInventoryHistory()
                {
                    id = int.Parse(x["id"].ToString()),
                    productName = x["name"].ToString(),
                    productId = int.Parse(x["productId"].ToString()),
                    quantity = int.Parse(x["quantity"].ToString()),
                    createdBy = x["createdBy"].ToString(),
                    createdDate = DateTime.ParseExact(x["createdDate"].ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture),
                    dateCreatedTimespan = double.Parse(x["dateCreatedTimespan"].ToString())
                }).ToList();
            }

            return lst;
        }

        public string insertQuery(ProductInventoryHistory p)
        {
            string query = "insert into ProductInventoryHistory(productId,quantity,createdBy,createdDate,inventoryMode,dateCreatedTimespan) ";
            query = query + "values('" + p.productId + "'," + p.quantity + ",'" + p.createdBy + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + p.inventoryMode + "','" + DateTime.Now.Ticks + "'); ";

            return query;
        }

        public bool add(ProductInventoryHistory p)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                string query = "insert into ProductionInventoryHistory(productId,quantity,createdBy,createdDate,inventoryMode,dateCreatedTimespan) ";
                query = query + "values(@productId,@quantity,@createdBy,@createdDate,@inventoryMode,@dateCreatedTimespan)";
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = query;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@productId", p.productId);
                cmd.Parameters.AddWithValue("@quantity", p.quantity);
                cmd.Parameters.AddWithValue("@createdBy", p.createdBy);
                cmd.Parameters.AddWithValue("@createdDate", p.createdDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@inventoryMode", p.inventoryMode);
                cmd.Parameters.AddWithValue("@dateCreatedTimespan", DateTime.Now.Ticks);
                int count = cmd.ExecuteNonQuery();
                if (count > 0)
                {
                    return true;
                }
            }

            return false;
        }

        public bool add(String query)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = query;
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
