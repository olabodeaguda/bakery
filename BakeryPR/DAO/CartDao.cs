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
    public class CartDao : AbstractDao
    {
        public string getInsert(CartModel cart)
        {
            string query = "insert into cart(customerName,dateCreated,createdBy,cartStatus,salesType) values(";
            query = query + "'"+cart.customerName+"',";
            query = query + "'"+DateTime.Now.ToString("yyyy-MM-dd")+"',";
            query = query + "'"+cart.createdBy+"',";
            query = query + "'"+cart.cartStatus+"',";
            query = query + "'"+cart.salesType+"',";
            query = query + " );";
            return "";
        }

        public bool add(CartModel values)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = "insert into cart(customerName,dateCreated,createdBy,cartStatus,salesType) values(" +
                    "@customerName,@dateCreated,@createdBy,@cartStatus,@salesType)";
                cmd.Parameters.AddWithValue("@customerName", values.customerName);
                cmd.Parameters.AddWithValue("@dateCreated", DateTime.Now.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@createdBy", values.createdBy);
                cmd.Parameters.AddWithValue("@cartStatus", values.cartStatus);
                cmd.Parameters.AddWithValue("@salesType", values.salesType);
                cmd.CommandType = CommandType.Text;
                int count = (int)cmd.ExecuteScalar();
                if (count > 0)
                {
                    return true;
                }
            }

            return false;
        }




    }
}
