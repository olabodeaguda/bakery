using BakeryPR.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SQLite;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BakeryPR.DAO
{
    public class RecipeDao : AbstractDao
    {
        public List<Recipe> all()
        {
            List<Recipe> lst = new List<Recipe>();
            using (SQLiteConnection conn = new SQLiteConnection(this.connectionString))
            {
                conn.Open();
                DataSet dt = new DataSet();
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = "select recipe.*,ingredent.ingredentName, recipeIngredent.quantity from recipe left join  recipeIngredent on recipeIngredent.recipeId = recipe.id left join ingredent on ingredent.id = recipeIngredent.ingredentId";
                cmd.CommandType = CommandType.Text;
                this.SQLiteAdaptor(dt, cmd);

                IEnumerable<IGrouping<int, Recipe>> results = dt.Tables[0].Rows.Cast<DataRow>().Select(x => new Recipe()
                {
                    id = int.Parse(x["id"].ToString()),
                    title = x["title"].ToString(),
                    dateCreated = DateTime.Parse(x["dateCreated"].ToString(), new CultureInfo("en-US", true)),
                    lastUpdated = DateTime.Parse(x["lastUpdated"].ToString(), new CultureInfo("en-US", true))
                }).GroupBy(x => x.id);

                foreach (var p in results)
                {
                    Recipe pp = p.FirstOrDefault();
                    Recipe rec = new Recipe();
                    rec.id = p.Key;
                    rec.title = p.FirstOrDefault()?.title;

                    rec.recipeNos = p.Count();
                    rec.dateCreated = pp.dateCreated;
                    rec.lastUpdated = pp.lastUpdated;
                    lst.Add(rec);
                }

            }

            return lst;
        }

        public bool add(Recipe values)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = "insert into recipe(title,dateCreated,lastUpdated) " +
                    "values(@title,@dateCreated,@lastUpdated)";
                cmd.Parameters.AddWithValue("@title", values.title);
                cmd.Parameters.AddWithValue("@dateCreated", values.dateCreated.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@lastUpdated", values.dateCreated.ToString("yyyy-MM-dd"));
                cmd.CommandType = CommandType.Text;
                int count = cmd.ExecuteNonQuery();
                if (count > 0)
                {
                    return true;
                }
            }

            return false;
        }

        public bool update(Recipe values)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = "update recipe set title=@title,dateCreated=@dateCreated,lastUpdated=@lastUpdated where id = @id";
                cmd.Parameters.AddWithValue("@title", values.title);
                cmd.Parameters.AddWithValue("@dateCreated", values.dateCreated);
                cmd.Parameters.AddWithValue("@lastUpdated", values.lastUpdated);
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

    }
}
