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
                DataSet dt = new DataSet();
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = "select * from recipeIngredent";
                cmd.CommandType = CommandType.Text;
                this.SQLiteAdaptor(dt, cmd);

                lst = dt.Tables[0].Rows.Cast<DataRow>().Select(x => new RecipeIngredents()
                {
                    id = int.Parse(x["id"].ToString()),
                    ingredentId = int.Parse(x["ingredentId"].ToString()),
                    quantity = x.Field<double>("quantity"),
                    recipeId = x.Field<int>("recipeId")
                }).ToList();
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

                lst = dt.Tables[0].Rows.Cast<DataRow>().Select(x => new RecipeIngredents()
                {
                    id = int.Parse(x["id"].ToString()),
                    ingredentId = int.Parse(x["ingredentId"].ToString()),
                    quantity = double.Parse(x["quantity"].ToString()),
                    recipeId = int.Parse(x["recipeId"].ToString()),
                    mType = x["measureTypeName"].ToString(),
                    unitCost = double.Parse(x["unitCost"].ToString()),
                    ingredentName = x["ingredentName"].ToString()
                }).ToList();
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
                string query = "select * from recipeIngredent where recipeId=@recipeId and ingredentId = @ingredentId ";
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@recipeId", recipeId);
                cmd.Parameters.AddWithValue("@ingredentId", ingredentId);
                cmd.CommandType = CommandType.Text;
                this.SQLiteAdaptor(dt, cmd);

                return dt.Tables[0].Rows.Cast<DataRow>().Select(x => new RecipeIngredents()
                {
                    id = int.Parse(x["id"].ToString()),
                    ingredentId = int.Parse(x["ingredentId"].ToString()),
                    quantity = double.Parse(x["quantity"].ToString()),
                    recipeId = int.Parse(x["recipeId"].ToString())
                }).FirstOrDefault();
            }
        }

    }
}
