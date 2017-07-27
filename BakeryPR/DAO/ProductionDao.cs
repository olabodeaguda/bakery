using BakeryPR.Models;
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
    public class ProductionDao : AbstractDao
    {
        public List<Production> all()
        {
            List<Production> lst = new List<Production>();
            using (SQLiteConnection conn = new SQLiteConnection(this.connectionString))
            {
                conn.Open();
                DataSet dt = new DataSet();
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = "select production.*,recipe.title as recipeTitle from production inner join recipe on recipe.id=production.recipeId order by production.title desc";
                cmd.CommandType = CommandType.Text;
                this.SQLiteAdaptor(dt, cmd);

                lst = dt.Tables[0].Rows.Cast<DataRow>().Select(x => new Production()
                {
                    id = int.Parse(x["id"].ToString()),
                    createdBY = x["createdBy"].ToString(),
                    title = x["title"].ToString(),
                    recipeTitle = x["recipeTitle"].ToString(),
                    quantity = String.IsNullOrEmpty(x["quantity"].ToString()) ? 0 : double.Parse(x["quantity"].ToString()),
                    recipeId = int.Parse(x["recipeId"].ToString()),
                    dateCreated = DateTime.Parse(x["dateCreated"].ToString(), new CultureInfo("en-US", true)),
                    lastUpdated = DateTime.Parse(x["lastUpdated"].ToString(), new CultureInfo("en-US", true)),
                    approval = x["approval"].ToString(),
                    approveBy = x["approveBy"].ToString()
                }).ToList();
            }

            return lst;
        }

        public List<Production> byStatus(string status)
        {
            List<Production> lst = new List<Production>();
            using (SQLiteConnection conn = new SQLiteConnection(this.connectionString))
            {
                conn.Open();
                DataSet dt = new DataSet();
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = "select production.*,recipe.title as recipeTitle from production inner join recipe on recipe.id=production.recipeId where production.approval=@approval order by production.title desc";
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@approval", status);
                this.SQLiteAdaptor(dt, cmd);

                lst = dt.Tables[0].Rows.Cast<DataRow>().Select(x => new Production()
                {
                    id = int.Parse(x["id"].ToString()),
                    createdBY = x["createdBy"].ToString(),
                    title = x["title"].ToString(),
                    recipeTitle = x["recipeTitle"].ToString(),
                    quantity = String.IsNullOrEmpty(x["quantity"].ToString()) ? 0 : double.Parse(x["quantity"].ToString()),
                    recipeId = int.Parse(x["recipeId"].ToString()),
                    dateCreated = DateTime.Parse(x["dateCreated"].ToString(), new CultureInfo("en-US", true)),
                    lastUpdated = DateTime.Parse(x["lastUpdated"].ToString(), new CultureInfo("en-US", true))
                }).ToList();
            }

            return lst;
        }

        public int add(Production values)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = "insert into production(title,dateCreated,lastUpdated,createdBy,recipeId,quantity,approval) " +
                    " values(@title,@dateCreated,@lastUpdated,@createdBy,@recipeId,@quantity,@approval); SELECT last_insert_rowid();";
                cmd.Parameters.AddWithValue("@title", values.title);
                cmd.Parameters.AddWithValue("@createdBy", values.createdBY);
                cmd.Parameters.AddWithValue("@recipeId", values.recipeId);
                cmd.Parameters.AddWithValue("@approval", values.approval);
                cmd.Parameters.AddWithValue("@quantity", values.quantity);
                cmd.Parameters.AddWithValue("@dateCreated", DateTime.Now.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@lastUpdated", DateTime.Now.ToString("yyyy-MM-dd"));

                cmd.CommandType = CommandType.Text;
                object obj = cmd.ExecuteScalar(); //cmd.ExecuteNonQuery();

                int l = Convert.ToInt32(obj);

                if (l < 1)
                {
                    throw new Exception("An error occur while trying to add production. Try again or contact administrator");
                }
                else
                {
                    return l;
                }
            }
        }

        public string deleteProductionQuery(Production p)
        {
            return "delete from ProductionIngredient where productionId = '" + p.id + "';";
        }

        public string updateProductionQuery(Production p)
        {
            string query = "Update production set quantity='" + p.quantity + "',";
            query = query + " quantity = '" + p.quantity + "',";
            query = query + " recipeId = '" + p.recipeId + "',";
            query = query + "lastUpdated = '" + p.dateCreated.ToString("yyyy-MM-dd") + "',";
            query = query + "title = '" + p.title + "'";
            query = query + " where id = '" + p.id + "';";

            return query;
        }

        public bool update(Production values)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = "update production set quantity=@quantity, recipeId = @recipeId,title=@title,lastUpdated=@lastUpdated where id=@id";
                cmd.Parameters.AddWithValue("@title", values.title);
                cmd.Parameters.AddWithValue("@recipeId", values.recipeId);
                cmd.Parameters.AddWithValue("@id", values.id);
                cmd.Parameters.AddWithValue("@quantity", values.quantity);
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

        public Production ProductionId(int prodId)
        {
            using (SQLiteConnection conn = new SQLiteConnection(this.connectionString))
            {
                conn.Open();
                DataSet dt = new DataSet();
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = "select production.*,recipe.title as recipeTitle from production inner join recipe on recipe.id=production.recipeId where production.id = @prodId order by production.title desc";
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@prodId", prodId);
                this.SQLiteAdaptor(dt, cmd);

                return dt.Tables[0].Rows.Cast<DataRow>().Select(x => new Production()
                {
                    id = int.Parse(x["id"].ToString()),
                    createdBY = x["createdBy"].ToString(),
                    title = x["title"].ToString(),
                    recipeTitle = x["recipeTitle"].ToString(),
                    quantity = String.IsNullOrEmpty(x["quantity"].ToString()) ? 0 : double.Parse(x["quantity"].ToString()),
                    recipeId = int.Parse(x["recipeId"].ToString()),
                    dateCreated = DateTime.Parse(x["dateCreated"].ToString(), new CultureInfo("en-US", true)),
                    lastUpdated = DateTime.Parse(x["lastUpdated"].ToString(), new CultureInfo("en-US", true))
                }).FirstOrDefault();
            }
        }

        public String updateApprovalStatusQuery(Production pr)
        {
            string query = "update production set approval='" + pr.approval + "' where id = '" + pr.id+"';";
            return query;
        }

        public bool exec(string query)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = query;
                cmd.CommandType = CommandType.Text;
                int obj = cmd.ExecuteNonQuery();

                if (obj < 1)
                {
                    throw new Exception("An error occur while trying to add production. Try again or contact administrator");
                }
                else
                {
                    return true;
                }
            }
        }


    }
}
