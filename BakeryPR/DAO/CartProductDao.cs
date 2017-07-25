using BakeryPR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BakeryPR.DAO
{
    public class CartProductDao
    {
        public string getInsertQuery(CartProductModel c)
        {
            string query = "insert into cartProduct(price,quantity,productId,cartId) values(";
            query = query + "'" + c.price + "',";
            query = query + "'" + c.quantity + "',";
            query = query + "'" + c.productId + "',";
            query = query + "'" + c.cartId + "'";
            query = query + " );";
            return query;
        }

        public string updateProductInventory(int productId)
        {
            string query = "insert into cartProduct(price,quantity,productId,cartId) values(";
            //query = query + "'" + c.price + "',";
            //query = query + "'" + c.quantity + "',";
            //query = query + "'" + c.productId + "',";
            //query = query + "'" + c.cartId + "'";
            //query = query + " );";
            return query;
        }


    }
}
