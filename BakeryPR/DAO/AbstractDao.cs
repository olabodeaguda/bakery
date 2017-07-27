using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BakeryPR.DAO
{
    public abstract class AbstractDao
    {
        protected string connectionString;
        public AbstractDao()
        {
            this.connectionString = ConfigurationManager.ConnectionStrings["defaultConnection"].ConnectionString;
        }

        protected bool execute(string query)
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

        protected int executeScalar(string query)
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();
                    SQLiteCommand cmd = new SQLiteCommand(conn);
                    cmd.CommandText = query;
                    cmd.CommandType = CommandType.Text;
                    return (int)cmd.ExecuteScalar();
                }
            }
            catch (Exception x)
            {
                throw new Exception(x.Message);
            }
        }
       
        protected void SQLiteAdaptor(DataSet dt, SQLiteCommand cmd)
        {
            using (SQLiteDataAdapter da = new SQLiteDataAdapter(cmd))
            {
                DataTable dt_mReport = new DataTable();
                da.Fill(dt_mReport);
                dt.Tables.Add(dt_mReport);
            }
        }
    }
}
