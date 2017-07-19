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
    public class RoleDao : AbstractDao
    {
        public List<Role> all()
        {
            List<Role> lst = new List<Role>();
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                DataSet dt = new DataSet();
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = "select * from role order by name";
                cmd.CommandType = CommandType.Text;
                this.SQLiteAdaptor(dt, cmd);

                lst = dt.Tables[0].Rows.Cast<DataRow>().Select(x => new Role()
                {
                    id = int.Parse(x["id"].ToString()),
                    name = x["name"].ToString()
                }).ToList();
            }

            return lst;
        }

        public bool assignRoleTouser(UserRole ur)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = "insert into userRole(userId,roleId) values(@userId,@roleId)";

                cmd.Parameters.AddWithValue("@userId", ur.userId);
                cmd.Parameters.AddWithValue("@roleId", ur.roleId);

                cmd.CommandType = CommandType.Text;
                int count = cmd.ExecuteNonQuery();
                if (count > 0)
                {
                    return true;
                }
            }

            return false;
        }

        public bool UpdateUserRole(UserRole ur)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = "update userRole set roleId = @roleId where id=@id";
                cmd.Parameters.AddWithValue("@roleId", ur.roleId);
                cmd.Parameters.AddWithValue("@id", ur.id);
                cmd.CommandType = CommandType.Text;
                int count = cmd.ExecuteNonQuery();
                if (count > 0)
                {
                    return true;
                }
            }

            return false;
        }

        public List<Role> byProfileId(int profileId)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                DataSet dt = new DataSet();
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = "select Role.id,Role.name,UserRole.userId from UserRole inner join Role on Role.id = userRole.roleId where userRole.userId= @id";
                cmd.Parameters.AddWithValue("@id", profileId);
                cmd.CommandType = CommandType.Text;
                this.SQLiteAdaptor(dt, cmd);

                return dt.Tables[0].Rows.Cast<DataRow>().Select(x => new Role()
                {
                    id = int.Parse(x["id"].ToString()),
                    name = x["name"].ToString(),
                    userId = int.Parse(x["userId"].ToString())
                }).ToList();
            }
        }

        public bool deletebyProfileId(int userId, int roleId)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                DataSet dt = new DataSet();
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = " delete from UserRole where userId=@userId and roleId=@roleId";
                cmd.Parameters.AddWithValue("@userId", userId);
                cmd.Parameters.AddWithValue("@roleId", roleId);
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
