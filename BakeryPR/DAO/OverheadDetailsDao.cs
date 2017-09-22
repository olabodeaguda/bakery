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
    public class OverheadDetailsDao : AbstractDao
    {
        public List<OverheadDetails> allSingle()
        {
            List<OverheadDetails> lst = new List<OverheadDetails>();
            using (SQLiteConnection conn = new SQLiteConnection(this.connectionString))
            {
                conn.Open();
                string query = "select * from overheadGrpDetails order by overheadGrpDetails.id desc; ";
                DataSet dt = new DataSet();
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = query;
                cmd.CommandType = CommandType.Text;
                this.SQLiteAdaptor(dt, cmd);

                lst = dt.Tables[0].Rows.Cast<DataRow>().Select(x => new OverheadDetails()
                {
                    id = int.Parse(x["id"].ToString()),
                    groupName = x["groupName"].ToString(),
                    quantity = double.Parse(x["quantity"].ToString())
                }).ToList();
            }

            return lst;
        }

        

        public int add(OverheadDetails values)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = "insert into overheadGrpDetails(groupName,quantity) values(@grpName,@quantity); SELECT last_insert_rowid();";
                cmd.Parameters.AddWithValue("@grpName", values.groupName);
                cmd.Parameters.AddWithValue("@quantity", values.quantity);
                cmd.CommandType = CommandType.Text;
                object obj = cmd.ExecuteScalar();
                int id = Convert.ToInt32(obj);
                return id;
            }
        }

        public bool update(OverheadDetails values)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = "update overheadGrpDetails set groupName = @grpName,quantity=@quantity where id=@id";
                cmd.Parameters.AddWithValue("@grpName", values.groupName);
                cmd.Parameters.AddWithValue("@quantity", values.quantity);
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
