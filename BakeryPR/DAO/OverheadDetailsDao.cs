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
        public List<OverheadDetails> all()
        {
            List<OverheadDetails> lst = new List<OverheadDetails>();
            using (SQLiteConnection conn = new SQLiteConnection(this.connectionString))
            {
                conn.Open();
                string query = "select overheadGrpDetails.*,overheadGrpDetailsExt.overheadId,overheadGrpDetailsExt.quantity from overheadGrpDetails ";
                query = query + "inner join overheadGrpDetailsExt where overheadGrpDetailsExt.grpId = overheadGrpDetails.id ";
                query = query + "order by overheadGrpDetails.groupName desc;";
                DataSet dt = new DataSet();
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = query;
                cmd.CommandType = CommandType.Text;
                this.SQLiteAdaptor(dt, cmd);

                lst = dt.Tables[0].Rows.Cast<DataRow>().Select(x => new OverheadDetails()
                {
                    id = int.Parse(x["id"].ToString()),
                    groupName = x["groupName"].ToString(),
                    overheadId = int.Parse(x["overheadId"].ToString()),
                    overheadQuantity = double.Parse(x["quantity"].ToString())
                }).ToList();
            }

            return lst;
        }

        public bool add(OverheadDetails values)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = "insert into overheadGrpDetails(groupName) values(@grpName)";
                cmd.Parameters.AddWithValue("@grpName", values.groupName);
                cmd.CommandType = CommandType.Text;
                int count = cmd.ExecuteNonQuery();
                if (count > 0)
                {
                    return true;
                }
            }

            return false;
        }

        public bool update(OverheadDetails values)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = "update overheadGrpDetails groupName = @grpName where id=@id";
                cmd.Parameters.AddWithValue("@grpName", values.groupName);
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
