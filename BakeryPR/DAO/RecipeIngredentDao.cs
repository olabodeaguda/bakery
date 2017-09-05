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
    public class RecipeIngredentDao : AbstractDao
    {
        public List<RecipeIngredents> all()
        {
            List<RecipeIngredents> lst = new List<RecipeIngredents>();
            using (SQLiteConnection conn = new SQLiteConnection(this.connectionString))
            {
                conn.Open();
                string query = "select recipeIngredent.*,ingredent.unitCost,ingredent.ingredentName,measurementType.measureTypeName from recipeIngredent inner join ingredent on ingredent.id=recipeIngredent.ingredentId ";
                query = query + " inner join measurementType on measurementType.id = ingredent.mTypeId";

                DataSet dt = new DataSet();
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = query;// "select * from recipeIngredent";
                cmd.CommandType = CommandType.Text;
                this.SQLiteAdaptor(dt, cmd);

                //lst = dt.Tables[0].Rows.Cast<DataRow>().Select(x => new RecipeIngredents()
                //{
                //    id = int.Parse(x["id"].ToString()),
                //    ingredentId = int.Parse(x["ingredentId"].ToString()),
                //    quantity = x.Field<double>("quantity"),
                //    recipeId = x.Field<int>("recipeId")
                //}).ToList();


                foreach (var x in dt.Tables[0].Rows.Cast<DataRow>())
                {
                    RecipeIngredents ri = new RecipeIngredents();
                    ri.id = int.Parse(x["id"].ToString());
                    ri.ingredentId = int.Parse(x["ingredentId"].ToString());
                    ri.recipeId = int.Parse(x["recipeId"].ToString());
                    ri.mType = x["measureTypeName"].ToString();
                    ri.quantity = double.Parse(x["quantity"].ToString());

                    if (ri.mType.ToLower() == "gram")
                    {
                        ri.quantity = Math.Round(ri.quantity / 1000, 2);
                        ri.mType = "kg";
                    }

                    ri.unitCost = double.Parse(x["unitCost"].ToString());
                    ri.ingredentName = x["ingredentName"].ToString();
                    lst.Add(ri);
                }
            }

            return lst;
        }

        public bool add(RecipeIngredents values)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = "insert into recipeIngredent(quantity,recipeId,ingredentId) " +
                    "values(@quantity,@recipeId,@ingredentId)";
                cmd.Parameters.AddWithValue("@quantity", values.quantity);
                cmd.Parameters.AddWithValue("@recipeId", values.recipeId);
                cmd.Parameters.AddWithValue("@ingredentId", values.ingredentId);
                cmd.CommandType = CommandType.Text;
                int count = cmd.ExecuteNonQuery();
                if (count > 0)
                {
                    return true;
                }
            }

            return false;
        }

        public bool Update(RecipeIngredents values)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = "update recipeIngredent set quantity = @quantity ,recipeId=@recipeId,ingredentId=@ingredentId where id=@id";
                cmd.Parameters.AddWithValue("@quantity", values.quantity);
                cmd.Parameters.AddWithValue("@recipeId", values.recipeId);
                cmd.Parameters.AddWithValue("@ingredentId", values.ingredentId);
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

        public List<RecipeIngredents> byRecipeId(int recipeId)
        {
            List<RecipeIngredents> lst = new List<RecipeIngredents>();
            using (SQLiteConnection conn = new SQLiteConnection(this.connectionString))
            {
                conn.Open();
                DataSet dt = new DataSet();
                SQLiteCommand cmd = new SQLiteCommand(conn);
                string query = "select recipeIngredent.*,ingredent.unitCost,ingredent.ingredentName,measurementType.measureTypeName from recipeIngredent inner join ingredent on ingredent.id=recipeIngredent.ingredentId ";
                query = query + " inner join measurementType on measurementType.id = ingredent.mTypeId where recipeId = @recipeId";
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@recipeId", recipeId);
                cmd.CommandType = CommandType.Text;
                this.SQLiteAdaptor(dt, cmd);

                foreach (var x in dt.Tables[0].Rows.Cast<DataRow>())
                {
                    RecipeIngredents ri = new RecipeIngredents();
                    ri.id = int.Parse(x["id"].ToString());
                    ri.ingredentId = int.Parse(x["ingredentId"].ToString());
                    ri.recipeId = int.Parse(x["recipeId"].ToString());
                    ri.mType = x["measureTypeName"].ToString();
                    ri.quantity = double.Parse(x["quantity"].ToString());

                    if (ri.mType.ToLower() == "gram")
                    {
                        ri.quantity = Math.Round(ri.quantity / 1000, 2);
                        ri.mType = "kg";
                    }

                    ri.unitCost = double.Parse(x["unitCost"].ToString());
                    ri.ingredentName = x["ingredentName"].ToString();
                    lst.Add(ri);
                }
            }

            return lst;
        }

        public RecipeIngredents byRecipeIdIngredent(int recipeId, int ingredentId)
        {
            using (SQLiteConnection conn = new SQLiteConnection(this.connectionString))
            {
                conn.Open();
                DataSet dt = new DataSet();
                SQLiteCommand cmd = new SQLiteCommand(conn);
                string query = "select recipeIngredent.*,ingredent.unitCost,ingredent.ingredentName,measurementType.measureTypeName from recipeIngredent";
                query = query + " inner join ingredent on ingredent.id=recipeIngredent.ingredentId ";
                query = query + " inner join measurementType on measurementType.id = ingredent.mTypeId where recipeId = @recipeId and ingredentId = @ingredentId ";

                // string query = "select * from recipeIngredent where recipeId=@recipeId and ingredentId = @ingredentId ";
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@recipeId", recipeId);
                cmd.Parameters.AddWithValue("@ingredentId", ingredentId);
                cmd.CommandType = CommandType.Text;
                this.SQLiteAdaptor(dt, cmd);

                //return dt.Tables[0].Rows.Cast<DataRow>().Select(x => new RecipeIngredents()
                //{
                //    id = int.Parse(x["id"].ToString()),
                //    ingredentId = int.Parse(x["ingredentId"].ToString()),
                //    quantity = double.Parse(x["quantity"].ToString()),
                //    recipeId = int.Parse(x["recipeId"].ToString())
                //}).FirstOrDefault();

                var x = dt.Tables[0].Rows.Cast<DataRow>().FirstOrDefault();

                if (x != null)
                {
                    RecipeIngredents ri = new RecipeIngredents();
                    ri.id = int.Parse(x["id"].ToString());
                    ri.ingredentId = int.Parse(x["ingredentId"].ToString());
                    ri.recipeId = int.Parse(x["recipeId"].ToString());
                    ri.mType = x["measureTypeName"].ToString();
                    ri.quantity = double.Parse(x["quantity"].ToString());

                    if (ri.mType.ToLower() == "gram")
                    {
                        ri.quantity = Math.Round(ri.quantity / 1000, 2);
                        ri.mType = "kg";
                    }

                    ri.unitCost = double.Parse(x["unitCost"].ToString());
                    ri.ingredentName = x["ingredentName"].ToString();
                    return ri; 
                }
                return null;
            }
        }

    }
}
