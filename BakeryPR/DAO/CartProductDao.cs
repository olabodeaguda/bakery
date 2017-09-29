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
    public class CartProductDao : AbstractDao
    {
        public string getInsertQuery(CartProductModel c)
        {
            string query = "insert into cartProduct(price,quantity,productId,cartId,costprice) values(";
            query = query + "'" + c.manualPrice + "',";
            query = query + "'" + c.quantity + "',";
            query = query + "'" + c.productId + "',";
            query = query + "'" + c.cartId + "',";
            query = query + "'" + c.costPrice + "'";
            query = query + " );";
            return query;
        }

        public List<CartProductModel> byCartId(int id)
        {
            List<CartProductModel> lst = new List<CartProductModel>();
            using (SQLiteConnection conn = new SQLiteConnection(this.connectionString))
            {
                conn.Open();
                DataSet dt = new DataSet();
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = "select * from cartProduct where cartId = @id";
                cmd.Parameters.AddWithValue("@id", id);
                cmd.CommandType = CommandType.Text;
                this.SQLiteAdaptor(dt, cmd);

                lst = dt.Tables[0].Rows.Cast<DataRow>().Select(x => new CartProductModel()
                {
                    id = int.Parse(x["id"].ToString()),
                    productId = int.Parse(x["productId"].ToString()),
                    cartId = int .Parse(x["cartId"].ToString()),
                    quantity = int.Parse(x["quantity"].ToString()),
                    price = double.Parse(x["price"].ToString())
                }).ToList();
            }

            return lst;
        }


    }
}
