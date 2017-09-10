using BakeryPR.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BakeryPR.DAO
{
    public class LicenseDao : AbstractDao
    {
        public bool UpdateLoadCount()
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(conn);

                cmd.CommandText = "update licensekey set loadCount = ((select loadCount from licensekey)+1)";

                cmd.CommandType = CommandType.Text;
                int count = cmd.ExecuteNonQuery();
                if (count > 0)
                {
                    return true;
                }
            }

            return false;
        }

        public bool ResetLoadCount()
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(conn);

                cmd.CommandText = "update licensekey set loadCount = 0";

                cmd.CommandType = CommandType.Text;
                int count = cmd.ExecuteNonQuery();
                if (count > 0)
                {
                    return true;
                }
            }

            return false;
        }

        public LicenseModel get()
        {
            using (SQLiteConnection conn = new SQLiteConnection(this.connectionString))
            {
                conn.Open();
                DataSet dt = new DataSet();
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = "select * from licenseKey";
                cmd.CommandType = CommandType.Text;
                this.SQLiteAdaptor(dt, cmd);

                return dt.Tables[0].Rows.Cast<DataRow>().Select(x => new LicenseModel()
                {
                    id = int.Parse(x["id"].ToString()),
                    value = x["value"].ToString(),
                    loadCount = int.Parse(x["loadCount"].ToString())
                }).FirstOrDefault();
            }
        }

        public LicenseModel getFull()
        {
            using (SQLiteConnection conn = new SQLiteConnection(this.connectionString))
            {
                conn.Open();
                DataSet dt = new DataSet();
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = "select * from licenseKey";
                cmd.CommandType = CommandType.Text;
                this.SQLiteAdaptor(dt, cmd);

                LicenseModel ls = dt.Tables[0].Rows.Cast<DataRow>().Select(x => new LicenseModel()
                {
                    id = int.Parse(x["id"].ToString()),
                    value = x["value"].ToString(),
                    loadCount = int.Parse(x["loadCount"].ToString())
                }).FirstOrDefault();
                if (ls != null)
                {
                    byte[] byteValue = Convert.FromBase64String(ls.value);
                    String valueString = Encoding.UTF8.GetString(byteValue);
                    LicenseModel m = JsonConvert.DeserializeObject<LicenseModel>(valueString);
                    m.id = ls.id;
                    return m;
                }

                return null;
            }
        }

        public bool Update(LicenseModel licenseModel)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = "UPDATE licenseKey SET loadCount = '0' WHERE id = @id";
                cmd.Parameters.AddWithValue("@id", licenseModel.id);
                cmd.CommandType = CommandType.Text;
                int id = (int)cmd.ExecuteNonQuery();
                if (id <= 0)
                {
                    return false;
                }

                return true;
            }
        }

        public bool UpdateComplete(LicenseModel licenseModel)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = "UPDATE licenseKey SET value=@value, loadCount = '0' WHERE id = @id";
                String value = Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(licenseModel)));
                cmd.Parameters.AddWithValue("@value", value);
                cmd.Parameters.AddWithValue("@id", licenseModel.id);
                cmd.CommandType = CommandType.Text;
                int id = (int)cmd.ExecuteNonQuery();
                if (id <= 0)
                {
                    return false;
                }

                return true;
            }
        }

        public bool Add(LicenseModel licenseModel)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = "INSERT INTO licenseKey(value) values(@value)";

                String value = Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(licenseModel)));

                cmd.Parameters.AddWithValue("@value", value);
                cmd.CommandType = CommandType.Text;

                int id = (int)cmd.ExecuteNonQuery();
                if (id <= 0)
                {
                    return false;
                }

                return true;
            }
        }
    }
}
