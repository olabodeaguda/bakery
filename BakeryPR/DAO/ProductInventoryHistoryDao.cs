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
    public class ProductInventoryHistoryDao : AbstractDao
    {
        public string insertQuery(ProductInventoryHistory p)
        {
            string query = "insert into ProductInventoryHistory(productId,quantity,createdBy,createdDate,inventoryMode) ";
            query = query + "values('"+p.productId+"',"+p.quantity+",'"+p.createdBy+"','"+  DateTime.Now.ToString("yyyy-MM-dd") + "','"+p.inventoryMode+"'); ";

            return query;
        }

        public bool add(ProductInventoryHistory p)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                string query = "insert into ProductionInventoryHistory(productId,quantity,createdBy,createdDate,inventoryMode) ";
                query = query + "values(@productId,@quantity,@createdBy,@createdDate,@inventoryMode)";
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = query;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@productId", p.productId);
                cmd.Parameters.AddWithValue("@quantity", p.quantity);
                cmd.Parameters.AddWithValue("@createdBy", p.createdBy);
                cmd.Parameters.AddWithValue("@createdDate", p.createdDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@inventoryMode", p.inventoryMode);
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
