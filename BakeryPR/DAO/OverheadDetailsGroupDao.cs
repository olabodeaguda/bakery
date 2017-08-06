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
    public class OverheadDetailsGroupDao : AbstractDao
    {
        public OverheadDao overheadDao
        {
            get
            {
                return new OverheadDao();
            }
        }

        public string ProductionInsert(double measure, int prodId)
        {
            List<Overhead> allOverheads = overheadDao.all();

            if (allOverheads.Count > 0)
            {
                string query = "";
                foreach (var tm in allOverheads)
                {
                    query = query + "insert into productionOverhead(overheadId,productionId,overheadCount)values( ";
                    query = query + "'" + tm.id + "',";
                    query = query + "'" + prodId + "',";
                    double d = (measure * tm.unitCost) / tm.mTypeId;
                    query = query + "'" + d + "'";
                    query = query + ");";
                }
                return query;
            }

            return null;
        }

        public string ProductionInsert(int prodId)
        {
            return "delete from productionOverhead where productionId = '" + prodId + "';";
        }

        public string insertQuery(OverheadDetailsGroup values)
        {
            string query = "insert into overheadGrpDetailsExt(grpId,overheadId,quantity) values(";
            query = query + "'" + values.grpId + "',";
            query = query + "'" + values.overheadId + "',";
            query = query + "'" + values.quantity + "'";
            query = query + ");";
            return query;
        }

        //public

        public List<OverheadDetailsGroup> all()
        {
            List<OverheadDetailsGroup> lst = new List<OverheadDetailsGroup>();
            using (SQLiteConnection conn = new SQLiteConnection(this.connectionString))
            {
                conn.Open();
                string query = "select overheadGrpDetails.*,overheadGrpDetailsExt.quantity,overheads.name,overheads.unitCost,measurementType.measureTypeName from overheadGrpDetails ";
                query = query + "inner join overheadGrpDetailsExt on overheadGrpDetailsExt.grpId = overheadGrpDetails.id ";
                query = query + "inner join overheads on overheads.id = overheadGrpDetailsExt.overheadId ";
                query = query + "inner join measurementType on measurementType.id = overheads.mTypeId ";
                query = query + "order by overheadGrpDetails.groupName desc";
                DataSet dt = new DataSet();
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = query;
                cmd.CommandType = CommandType.Text;
                this.SQLiteAdaptor(dt, cmd);

                lst = dt.Tables[0].Rows.Cast<DataRow>().Select(x => new OverheadDetailsGroup()
                {
                    id = int.Parse(x["id"].ToString()),
                    groupName = x["groupName"].ToString(),
                    overheadId = int.Parse(x["overheadId"].ToString()),
                    unitCost = double.Parse(x["unitCost"].ToString()),
                    measureType = x["measureTypeName"].ToString(),
                    quantity = double.Parse(x["quantity"].ToString()),
                    overheadName = x["name"].ToString()
                }).ToList();
            }

            return lst;
        }

        public List<OverheadDetailsGroup> byGrpId(int grpId)
        {
            List<OverheadDetailsGroup> lst = new List<OverheadDetailsGroup>();
            using (SQLiteConnection conn = new SQLiteConnection(this.connectionString))
            {
                conn.Open();
                string query = "select overheadGrpDetails.*,overheadGrpDetailsExt.id as extId,overheadGrpDetailsExt.overheadId,overheadGrpDetailsExt.quantity as ExtQuantity,overheads.name,overheads.unitCost,measurementType.measureTypeName from overheadGrpDetails ";
                query = query + "inner join overheadGrpDetailsExt on overheadGrpDetailsExt.grpId = overheadGrpDetails.id ";
                query = query + "inner join overheads on overheads.id = overheadGrpDetailsExt.overheadId ";
                query = query + "inner join measurementType on measurementType.id = overheads.mTypeId where  overheadGrpDetailsExt.grpId=@grpId ";
                query = query + "order by overheadGrpDetails.groupName desc";
                DataSet dt = new DataSet();
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@grpId", grpId);
                cmd.CommandType = CommandType.Text;
                this.SQLiteAdaptor(dt, cmd);

                lst = dt.Tables[0].Rows.Cast<DataRow>().Select(x => new OverheadDetailsGroup()
                {
                    id = int.Parse(x["id"].ToString()),
                    groupName = x["groupName"].ToString(),
                    overheadId = int.Parse(x["overheadId"].ToString()),
                    //grpId = int.Parse(x["grpId"].ToString()),
                    unitCost = double.Parse(x["unitCost"].ToString()),
                    measureType = x["measureTypeName"].ToString(),
                    quantity = double.Parse(x["ExtQuantity"].ToString()),
                    overheadName = x["name"].ToString(),
                    extId = int.Parse(x["extId"].ToString())
                }).ToList();
            }

            return lst;
        }

        public bool add(OverheadDetailsGroup values)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = insertQuery(values); //"update overheadGrpDetailsExt grpId=@grpId,overheadId=@overheadId,quantity=@quantity where id=@id";
                cmd.Parameters.AddWithValue("@grpId", values.grpId);
                cmd.Parameters.AddWithValue("@overheadId", values.overheadId);
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

        public bool update(OverheadDetailsGroup values)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = "update overheadGrpDetailsExt set grpId=@grpId,overheadId=@overheadId,quantity=@quantity where id=@id";
                cmd.Parameters.AddWithValue("@grpId", values.id);
                cmd.Parameters.AddWithValue("@overheadId", values.overheadId);
                cmd.Parameters.AddWithValue("@quantity", values.quantity);
                cmd.Parameters.AddWithValue("@id", values.extId);
                cmd.CommandType = CommandType.Text;
                int count = cmd.ExecuteNonQuery();
                if (count > 0)
                {
                    return true;
                }
            }

            return false;
        }

        public List<OverheadDetailsGroup> byGrpId2(int grpId)
        {
            List<OverheadDetailsGroup> lst = new List<OverheadDetailsGroup>();
            using (SQLiteConnection conn = new SQLiteConnection(this.connectionString))
            {
                conn.Open();
                string query = "select overheadGrpDetailsExt.*,overheads.name as overheadName,measurementType.measureTypeName from overheadGrpDetailsExt ";
                query = query + "inner join overheads on overheads.id = overheadGrpDetailsExt.overheadId ";
                query = query + "inner join measurementType on measurementType.id = overheads.mTypeId  ";
                query = query + "where overheadGrpDetailsExt.grpId =@grpId ";
                query = query + "order by overheads.name";

                DataSet dt = new DataSet();
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = query;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@grpId", grpId);
                this.SQLiteAdaptor(dt, cmd);

                lst = dt.Tables[0].Rows.Cast<DataRow>().Select(x => new OverheadDetailsGroup()
                {
                    id = int.Parse(x["id"].ToString()),
                    grpId = int.Parse(x["grpId"].ToString()),
                    overheadId = int.Parse(x["overheadId"].ToString()),
                    quantity = double.Parse(x["quantity"].ToString()),
                    overheadName = x["overheadName"].ToString(),
                    measureType = x["measureTypeName"].ToString()
                }).ToList();
            }

            return lst;
        }
    }
}
