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
    public class UserDao : AbstractDao
    {
        public List<Profile> all()
        {
            List<Profile> lst = new List<Profile>();
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                DataSet dt = new DataSet();
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = "select * from profile";
                cmd.CommandType = CommandType.Text;
                this.SQLiteAdaptor(dt, cmd);

                lst = dt.Tables[0].Rows.Cast<DataRow>().Select(x => new Profile()
                {
                    id = int.Parse(x["id"].ToString()),
                    othername = x["othername"].ToString(),
                    pwd = x["pwd"].ToString(),
                    status = x["status"].ToString(),
                    surname = x["surname"].ToString(),
                    username = x["username"].ToString()
                }).ToList();
            }

            return lst;
        }

        public bool add(Profile values)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(conn);

                cmd.CommandText = "insert into profile(username,surname,othername,status,pwd) " +
                    "values(@username,@surname,@othername,@status,@pwd)";

                cmd.Parameters.AddWithValue("@username", values.username);
                cmd.Parameters.AddWithValue("@surname", values.surname);
                cmd.Parameters.AddWithValue("@othername", values.othername);
                cmd.Parameters.AddWithValue("@status", values.status);
                cmd.Parameters.AddWithValue("@pwd", values.pwd);

                cmd.CommandType = CommandType.Text;
                int count = cmd.ExecuteNonQuery();
                if (count > 0)
                {
                    return true;
                }
            }

            return false;
        }

        public bool ChangePwd(Profile values)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = " update profile pwd=@pwd where id=@id";
                cmd.Parameters.AddWithValue("@username", values.username);
                cmd.Parameters.AddWithValue("@pwd", values.pwd);
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

        public bool Update(Profile values)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(conn);

                cmd.CommandText = "insert into profile set username = @username,surname=@surname,othername=@othername,status=@status,pwd=@pwd where id=@id";

                cmd.Parameters.AddWithValue("@username", values.username);
                cmd.Parameters.AddWithValue("@surname", values.surname);
                cmd.Parameters.AddWithValue("@othername", values.othername);
                cmd.Parameters.AddWithValue("@status", values.status);
                cmd.Parameters.AddWithValue("@pwd", values.pwd);
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
