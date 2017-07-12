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
        IngredentDao ingreDao = new IngredentDao();
        RecipeIngredentDao riDao = new RecipeIngredentDao();
        public List<Recipe> all()
        {
            List<Recipe> lst = new List<Recipe>();
            using (SQLiteConnection conn = new SQLiteConnection(this.connectionString))
            {
                conn.Open();
                DataSet dt = new DataSet();
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = "select * from recipe order by title";
                cmd.CommandType = CommandType.Text;
                this.SQLiteAdaptor(dt, cmd);

                List<Recipe> results = dt.Tables[0].Rows.Cast<DataRow>().Select(x => new Recipe()
                {
                    id = int.Parse(x["id"].ToString()),
                    title = x["title"].ToString(),
                    dateCreated = DateTime.Parse(x["dateCreated"].ToString(), new CultureInfo("en-US", true)),
                    lastUpdated = DateTime.Parse(x["lastUpdated"].ToString(), new CultureInfo("en-US", true)),
                    quantity = !String.IsNullOrEmpty(x["quantity"].ToString()) ? double.Parse(x["quantity"].ToString()) : 0
                }).ToList();

                foreach (var rec in results)
                {
                    rec.ingredent = new ObservableCollection<RecipeIngredents>(riDao.byRecipeId(rec.id));
                    lst.Add(rec);
                }

            }

            return lst;
        }

        public Recipe byId(int id)
        {
            Recipe results = null;
            using (SQLiteConnection conn = new SQLiteConnection(this.connectionString))
            {
                conn.Open();
                DataSet dt = new DataSet();
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = "select * from recipe where id = @id";
                cmd.Parameters.AddWithValue("@id", id);
                cmd.CommandType = CommandType.Text;
                this.SQLiteAdaptor(dt, cmd);

                results = dt.Tables[0].Rows.Cast<DataRow>().Select(x => new Recipe()
                {
                    id = int.Parse(x["id"].ToString()),
                    title = x["title"].ToString(),
                    dateCreated = DateTime.Parse(x["dateCreated"].ToString(), new CultureInfo("en-US", true)),
                    lastUpdated = DateTime.Parse(x["lastUpdated"].ToString(), new CultureInfo("en-US", true)),
                    quantity = !string.IsNullOrEmpty(x["quantity"].ToString()) ? double.Parse(x["quantity"].ToString()) : 0
                }).FirstOrDefault();

                if (results != null)
                {
                    results.ingredent = new ObservableCollection<RecipeIngredents>(riDao.byRecipeId(results.id));
                }
            }

            return results;
        }

        public bool add(Recipe values)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = "insert into recipe(title,dateCreated,lastUpdated,quantity) " +
                    "values(@title,@dateCreated,@lastUpdated,@quantity)";
                cmd.Parameters.AddWithValue("@title", values.title);
                cmd.Parameters.AddWithValue("@dateCreated", values.dateCreated.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@lastUpdated", values.dateCreated.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@quantity", values.quantity);
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
                cmd.CommandText = "update recipe set title=@title,lastUpdated=@lastUpdated,quantity=@quantity where id = @id";
                cmd.Parameters.AddWithValue("@title", values.title);
                cmd.Parameters.AddWithValue("@lastUpdated", values.lastUpdated);
                cmd.Parameters.AddWithValue("@id", values.id);
                cmd.Parameters.AddWithValue("@quantity", values.quantity);
                cmd.CommandType = CommandType.Text;
                int count = cmd.ExecuteNonQuery();
                if (count > 0)
                {
                    return true;
                }
            }

            return false;
        }

        public Error checkRecipeQuantity(int recipeId)
        {
            Error error = new Error() { success = true, errorMsg = "" };
            List<Ingredent> lst = ingreDao.all();
            List<RecipeIngredents> lstrecipeIngredent = riDao.byRecipeId(recipeId);
            foreach (var tm in lstrecipeIngredent)
            {
                var r = lst.FirstOrDefault(x => x.id == tm.ingredentId);
                if (r != null)
                {
                    if (tm.quantity > r.quantity)
                    {
                        //no enough material for
                        error = new Error() { success = false, errorMsg = r.ingredentName + " is out of stock for the selected recipe" };
                        break;
                    }
                }
            }
            return error;

        }

        public Error checkRecipeQuantity(int recipeId, double amount)
        {
            Error error = new Error() { success = true, errorMsg = "" };
            Recipe recipe = this.byId(recipeId);
            double ratio = 0;

            ratio = recipe.quantity == 0 ? 1 : (amount / recipe.quantity);

            List<Ingredent> lst = ingreDao.all();
            List<RecipeIngredents> lstrecipeIngredent = riDao.byRecipeId(recipeId);
            foreach (var tm in lstrecipeIngredent)
            {
                var r = lst.FirstOrDefault(x => x.id == tm.ingredentId);
                if (r != null)
                {
                    double comp = tm.quantity * ratio;
                    if (comp > r.quantity)
                    {
                        //no enough material for
                        error = new Error() { success = false, errorMsg = r.ingredentName + " is out of stock for the selected recipe" };
                        break;
                    }
                }
            }
            return error;

        }
    }
}