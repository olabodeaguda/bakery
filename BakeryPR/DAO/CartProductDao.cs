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
            string query = "insert into cartProduct(price,quantity,productId,cartId,costprice) values(";
            query = query + "'" + c.manualPrice + "',";
            query = query + "'" + c.quantity + "',";
            query = query + "'" + c.productId + "',";
            query = query + "'" + c.cartId + "',";
            query = query + "'" + c.costPrice + "'";
            query = query + " );";
            return query;
        }

    }
}
