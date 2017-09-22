using BakeryPR.Models;
using BakeryPR.Utilities;
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
    public class CartDao : AbstractDao
    {
        public string getInsert(CartModel cart)
        {
            string query = "insert into cart(customerName,dateCreated,createdBy,cartStatus,salesType,createdTimeSpan) values(";
            query = query + "'" + cart.customerName + "',";
            query = query + "'" + DateTime.Now.ToString("yyyy-MM-dd") + "',";
            query = query + "'" + cart.createdBy + "',";
            query = query + "'" + cart.cartStatus + "',";
            query = query + "'" + cart.salesType + "',";
            query = query + "'" + DateTime.Now.Ticks + "'";
            query = query + " );";
            return "";
        }

        public List<CartModel> byDate(string date)
        {
            List<CartModel> lst = new List<CartModel>();
            using (SQLiteConnection conn = new SQLiteConnection(this.connectionString))
            {
                conn.Open();
                DataSet dt = new DataSet();
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = "select * from cart where dateCreated = @date";
                cmd.Parameters.AddWithValue("@date", date);
                cmd.CommandType = CommandType.Text;
                this.SQLiteAdaptor(dt, cmd);

                lst = dt.Tables[0].Rows.Cast<DataRow>().Select(x => new CartModel()
                {
                    id = int.Parse(x["id"].ToString()),
                    cartStatus = x["cartStatus"].ToString(),
                    customerName = x["customerName"].ToString(),
                    dateCreated = DateTime.ParseExact(x["dateCreated"].ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture),
                    createdBy = x["createdBy"].ToString(),
                    isRetails = x["salesType"].ToString() == SalesType.RETAIL.ToString() ? true : false,
                    isWholesales = x["salesType"].ToString() == SalesType.WHOLESALES.ToString() ? true : false
                }).ToList();
            }

            return lst;
        }

        public List<CartModel> byDaily(string date)
        {
            List<CartModel> lst = new List<CartModel>();
            using (SQLiteConnection conn = new SQLiteConnection(this.connectionString))
            {
                string query = "select cart.*,cartProduct.quantity,cartProduct.price,product.name as pName,product.retailPrice,product.wholeSales from cart ";
                query = query + "inner join cartProduct on cartProduct.cartId = cart.id ";
                query = query + "inner join product on product.id = cartProduct.productId where cart.dateCreated = @date order by cart.id desc";
                conn.Open();
                DataSet dt = new DataSet();
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@date", date);
                cmd.CommandType = CommandType.Text;
                this.SQLiteAdaptor(dt, cmd);

                lst = dt.Tables[0].Rows.Cast<DataRow>().Select(x => new CartModel()
                {
                    id = int.Parse(x["id"].ToString()),
                    cartStatus = x["cartStatus"].ToString(),
                    customerName = x["customerName"].ToString(),
                    dateCreated = DateTime.ParseExact(x["dateCreated"].ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture),
                    createdBy = x["createdBy"].ToString(),
                    isRetails = x["salesType"].ToString() == SalesType.RETAIL.ToString() ? true : false,
                    isWholesales = x["salesType"].ToString() == SalesType.WHOLESALES.ToString() ? true : false,
                    pName = x["pName"].ToString(),
                    price = double.Parse(x["price"].ToString()),
                    quantity = int.Parse(x["quantity"].ToString()),
                    retailPrice = double.Parse(x["retailPrice"].ToString()),
                    wholeSales = double.Parse(x["wholeSales"].ToString())
                }).ToList();
            }

            return lst;
        }

        public int add(CartModel values)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = "insert into cart(customerName,dateCreated,createdBy,cartStatus,salesType,createdTimeSpan) values(" +
                    "@customerName,@dateCreated,@createdBy,@cartStatus,@salesType,@createdTimeSpan); SELECT last_insert_rowid();";
                cmd.Parameters.AddWithValue("@customerName", values.customerName);
                cmd.Parameters.AddWithValue("@dateCreated", DateTime.Now.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@createdBy", values.createdBy);
                cmd.Parameters.AddWithValue("@cartStatus", values.cartStatus);
                cmd.Parameters.AddWithValue("@salesType", values.isRetails ? SalesType.RETAIL.ToString() : SalesType.WHOLESALES.ToString());
                cmd.Parameters.AddWithValue("@createdTimeSpan", DateTime.Now.Ticks);
                cmd.CommandType = CommandType.Text;

                object obj = cmd.ExecuteScalar();
                int id = Convert.ToInt32(obj);

                if (id <= 0)
                {
                    throw new Exception("Cart was not created successfully. Please try again or contact administrator");
                }

                return id;
            }

        }
        public string updateQuery(CartModel cart)
        {
            return "UPDATE cart set cartStatus = '" + cart.cartStatus + "' where id='" + cart.id + "';";
        }

        public bool UpdateStatus(CartModel values)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = "UPDATE cart set cartStatus = @cartStatus where id=@id";
                cmd.Parameters.AddWithValue("@cartStatus", values.cartStatus);
                cmd.Parameters.AddWithValue("@id", values.id);
                cmd.CommandType = CommandType.Text;
                int id = (int)cmd.ExecuteNonQuery();
                if (id <= 0)
                {
                    return false;
                }

                return true;
            }

        }

        public bool executeQuery(string query)
        {
            bool result = false;

            try
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
                        result = true;
                    }
                }
            }
            catch (Exception x)
            {
                throw new Exception(x.Message);
            }

            return result;
        }

        public List<CartModel> GetCart(long dateTick, string customername, string productname)
        {
            List<CartModel> lst = new List<CartModel>();
            using (SQLiteConnection conn = new SQLiteConnection(this.connectionString))
            {
                conn.Open();
                String t1 = "createdTimeSpan >= '" + dateTick + "'";
                String t2 = "createdTimeSpan < '0'";
                string query = $" select cart.*,cartProduct.productId,cartProduct.price,cartProduct.quantity,product.name as ProductName from cart  ";
                query = query + $" inner join cartProduct on cartProduct.cartId = cart.id ";
                query = query + $" inner join product on product.id = cartProduct.productId ";
                query = query + $" where product.name like '{(productname == null ? "" : "%" + productname + "%")}' " +
                    $"or cart.customerName like '{(customername == null ? "" : "%" + customername + "%")}' " +
                    $"or {(dateTick == 0 ? t2 : t1)}";

                DataSet dt = new DataSet();
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = query;
                cmd.CommandType = CommandType.Text;
                this.SQLiteAdaptor(dt, cmd);

                foreach (var tm in dt.Tables[0].Rows.Cast<DataRow>().GroupBy(x => x["id"]))
                {
                    foreach (var t in tm)
                    {
                        CartModel cartModel = new CartModel();
                        cartModel.id = int.Parse(tm.Key.ToString());
                        DataRow dRow = t;
                        cartModel.customerName = t["customerName"].ToString();
                        cartModel.cartStatus = t["cartStatus"].ToString();
                        cartModel.createdBy = t["createdBy"].ToString();
                        cartModel.dateCreated = DateTime.Parse(t["dateCreated"].ToString(), new CultureInfo("en-US", true));
                        cartModel.pName = t["ProductName"].ToString();
                        cartModel.price = double.Parse(t["price"].ToString());
                        cartModel.quantity = int.Parse(t["quantity"].ToString());
                        cartModel.salesType = t["salesType"].ToString();
                        lst.Add(cartModel);
                    }
                }
            }

            return lst;
        }
    }
}
