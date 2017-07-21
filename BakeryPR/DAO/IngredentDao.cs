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
    public class IngredentDao : AbstractDao
    {
        public List<Ingredent> all()
        {
            List<Ingredent> lst = new List<Ingredent>();
            using (SQLiteConnection conn = new SQLiteConnection(this.connectionString))
            {
                conn.Open();
                DataSet dt = new DataSet();
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = "select ingredent.*,measurementType.measureTypeName from ingredent inner join measurementType on ingredent.mTypeId = measurementType.id";
                cmd.CommandType = CommandType.Text;
                this.SQLiteAdaptor(dt, cmd);

                lst = dt.Tables[0].Rows.Cast<DataRow>().Select(x => new Ingredent()
                {
                    id = int.Parse(x["id"].ToString()),
                    ingredentName = x["ingredentName"].ToString(),
                    mTypeId = int.Parse(x["mTypeId"].ToString()),
                    quantity = double.Parse(x["quantity"].ToString()),//dao.ingredentQuantity(int.Parse(x["id"].ToString())),
                    unitCost = double.Parse(x["unitCost"].ToString()),
                    measureTypeName = x["measureTypeName"].ToString()
                }).ToList();
            }

            return lst;
        }

        public List<Ingredent> byId(int id)
        {
            List<Ingredent> lst = new List<Ingredent>();
            using (SQLiteConnection conn = new SQLiteConnection(this.connectionString))
            {
                conn.Open();
                DataSet dt = new DataSet();
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = "select ingredent.*,measurementType.measureTypeName from ingredent inner join measurementType on ingredent.mTypeId = measurementType.id where ingredent.id=@id";
                cmd.Parameters.AddWithValue("@id", id);
                cmd.CommandType = CommandType.Text;
                this.SQLiteAdaptor(dt, cmd);

                lst = dt.Tables[0].Rows.Cast<DataRow>().Select(x => new Ingredent()
                {
                    id = int.Parse(x["id"].ToString()),
                    ingredentName = x["ingredentName"].ToString(),
                    mTypeId = int.Parse(x["mTypeId"].ToString()),
                    quantity = double.Parse(x["quantity"].ToString()),//dao.ingredentQuantity(int.Parse(x["id"].ToString())),
                    unitCost = double.Parse(x["unitCost"].ToString()),
                    measureTypeName = x["measureTypeName"].ToString()
                }).ToList();
            }

            return lst;
        }

        public bool add(Ingredent values)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = "insert into ingredent(ingredentName,unitCost,quantity,mTypeid) " +
                    "values(@ingredentName,@unitCost,@quantity,@mTypeid)";
                cmd.Parameters.AddWithValue("@ingredentName", values.ingredentName);
                cmd.Parameters.AddWithValue("@unitCost", values.unitCost);
                cmd.Parameters.AddWithValue("@quantity", values.quantity);
                cmd.Parameters.AddWithValue("@mTypeid", values.mTypeId);
                cmd.CommandType = CommandType.Text;
                int count = cmd.ExecuteNonQuery();
                if (count > 0)
                {
                    return true;
                }
            }

            return false;
        }

        public bool update(Ingredent values)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = "update ingredent set ingredentName = @ingredentName,unitCost=@unitCost,mTypeid = @mTypeid where id = @id";
                cmd.Parameters.AddWithValue("@ingredentName", values.ingredentName);
                cmd.Parameters.AddWithValue("@unitCost", values.unitCost);
                cmd.Parameters.AddWithValue("@id", values.id);
                cmd.Parameters.AddWithValue("@mTypeid", values.mTypeId);
                cmd.CommandType = CommandType.Text;
                int count = cmd.ExecuteNonQuery();
                if (count > 0)
                {
                    return true;
                }
            }

            return false;
        }

        public bool updateIngredent(int ingredntId, double quantity)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = "update ingredent set quantity = @quantity where id = @id";
                cmd.Parameters.AddWithValue("@quantity", quantity);
                cmd.Parameters.AddWithValue("@id", ingredntId);
                cmd.CommandType = CommandType.Text;
                int count = cmd.ExecuteNonQuery();
                if (count > 0)
                {
                    return true;
                }
            }

            return false;
        }

        public string updateIngredientQuantityQuery(Production p, List<ProductionIngredent> lstOfPI)
        {
            string query = "";
            List<Ingredent> lst = all();
            foreach (var tm in lstOfPI)
            {
                var ini = lst.FirstOrDefault(x => x.id == tm.ingredentId);
                if (ini != null)
                {
                    double quantity = ini.quantity - tm.amount;
                    query = query + "update ingredent set quantity ='" + quantity + "' where id = '" + tm.ingredentId + "';";
                }
            }

            return query;
        }


        public bool delete(int id)
        {

            return false;
        }
    }
}