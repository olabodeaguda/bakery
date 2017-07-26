using BakeryPR.Models;
using BakeryPR.Utilities;
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
        public Profile validateCredentials(LoginModel lm)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                DataSet dt = new DataSet();
                SQLiteCommand cmd = new SQLiteCommand(conn);

                String query = "select profile.*,Role.name as roleName from profile ";
                query = query + "left join userRole on userRole.userId = profile.id ";
                query = query + "left join Role on Role.id = userRole.roleId ";
                query = query + "where username = @username and profile.pwd = @pwd";

                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@username", lm.username);
                cmd.Parameters.AddWithValue("@pwd", lm.pwd);
                cmd.CommandType = CommandType.Text;
                this.SQLiteAdaptor(dt, cmd);

                return dt.Tables[0].Rows.Cast<DataRow>().Select(x => new Profile()
                {
                    id = int.Parse(x["id"].ToString()),
                    othername = x["othername"].ToString(),
                    pwd = x["pwd"].ToString(),
                    status = x["status"].ToString(),
                    surname = x["surname"].ToString(),
                    username = x["username"].ToString(),
                    roleName = x["roleName"].ToString()
                }).FirstOrDefault();
            }
        }

        public Profile byProfile(Profile profile)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                String query = "select profile.*,Role.name as roleName from profile ";
                query = query + "left join userRole on userRole.userId = profile.id ";
                query = query + "left join Role on Role.id = userRole.roleId ";
                query = query + "where username = @username and profile.id = @id";

                conn.Open();
                DataSet dt = new DataSet();
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = query;// "select * from profile where username = @username and id != @id";
                cmd.Parameters.AddWithValue("@username", profile.username);
                cmd.Parameters.AddWithValue("@id", profile.id);
                cmd.CommandType = CommandType.Text;
                this.SQLiteAdaptor(dt, cmd);

                return dt.Tables[0].Rows.Cast<DataRow>().Select(x => new Profile()
                {
                    id = int.Parse(x["id"].ToString()),
                    othername = x["othername"].ToString(),
                    pwd = x["pwd"].ToString(),
                    status = x["status"].ToString(),
                    surname = x["surname"].ToString(),
                    username = x["username"].ToString(),
                    roleName = x["roleName"].ToString()
                }).FirstOrDefault();
            }
        }

        public Profile byUsername(string username)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                DataSet dt = new DataSet();
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = "select * from profile where username = @username";
                cmd.Parameters.AddWithValue("@username", username);
                cmd.CommandType = CommandType.Text;
                this.SQLiteAdaptor(dt, cmd);

                return dt.Tables[0].Rows.Cast<DataRow>().Select(x => new Profile()
                {
                    id = int.Parse(x["id"].ToString()),
                    othername = x["othername"].ToString(),
                    pwd = x["pwd"].ToString(),
                    status = x["status"].ToString(),
                    surname = x["surname"].ToString(),
                    username = x["username"].ToString()
                }).FirstOrDefault();
            }
        }

        public List<Profile> all()
        {
            List<Profile> lst = new List<Profile>();
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                DataSet dt = new DataSet();
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = "select * from profile order by id";
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

        public bool ChangePwd(ChangePass values)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = " update profile set pwd=@pwd where id=@id";
                cmd.Parameters.AddWithValue("@pwd", values.newPassword);
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

                cmd.CommandText = "update profile set username = @username,surname=@surname,othername=@othername,status=@status where id=@id";

                cmd.Parameters.AddWithValue("@username", values.username);
                cmd.Parameters.AddWithValue("@surname", values.surname);
                cmd.Parameters.AddWithValue("@othername", values.othername);
                cmd.Parameters.AddWithValue("@status", values.status);
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

        public bool UpdateStatus(Profile values)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(conn);

                cmd.CommandText = "update profile set status=@status where id=@id";
                cmd.Parameters.AddWithValue("@status", values.status);
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



        public List<DropdownModel> lstUserStatus()
        {
            return Enum.GetNames(typeof(UserSatus)).Select(x => new DropdownModel() { valuesId = x, value = x }).ToList();
        }
    }
}
