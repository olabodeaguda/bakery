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
    public class ProductionOverheadDao : AbstractDao
    {
        public bool add(ProductionOverhead values)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = "insert into productionOverhead(overheadId,productionId,OverheadCount) " +
                    "values(@overheadId,@productionId,@OverheadCount)";
                cmd.Parameters.AddWithValue("@overheadId", values.overheadId);
                cmd.Parameters.AddWithValue("@productionId", values.productionId);
                cmd.Parameters.AddWithValue("@OverheadCount", values.overheadCount);
                cmd.CommandType = CommandType.Text;
                int count = cmd.ExecuteNonQuery();
                if (count > 0)
                {
                    return true;
                }
            }

            return false;
        }

        public List<ProductionOverhead> byproductionId(int prodId)
        {
            List<ProductionOverhead> lst = new List<ProductionOverhead>();
            using (SQLiteConnection conn = new SQLiteConnection(this.connectionString))
            {
                conn.Open();
                DataSet dt = new DataSet();
                SQLiteCommand cmd = new SQLiteCommand(conn);
                string query = "select productionOverhead.*,overheads.name as overheadName,measurementType.measureTypeName,overheads.unitCost  from productionOverhead ";
                query += "inner join overheads on overheads.id = productionOverhead.overheadId ";
                query += "inner join measurementType on measurementType.id = overheads.mTypeId where productionOverhead.productionId = @productionId";

                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@productionId", prodId);
                cmd.CommandType = CommandType.Text;
                this.SQLiteAdaptor(dt, cmd);

                lst = dt.Tables[0].Rows.Cast<DataRow>().Select(x => new ProductionOverhead()
                {
                    id = int.Parse(x["id"].ToString()),
                    mType = x["measureTypeName"].ToString(),
                    overheadId = int.Parse(x["overheadId"].ToString()),
                    productionId = int.Parse(x["productionId"].ToString()),
                    unitCost = double.Parse(x["unitCost"].ToString()),
                    overheadName = x["overheadName"].ToString(),
                    overheadCount = int.Parse(x["OverheadCount"].ToString())
                }).ToList();
            }

            return lst;
        }

        public List<ProductionOverhead> all()
        {
            List<ProductionOverhead> lst = new List<ProductionOverhead>();
            using (SQLiteConnection conn = new SQLiteConnection(this.connectionString))
            {
                conn.Open();
                DataSet dt = new DataSet();
                SQLiteCommand cmd = new SQLiteCommand(conn);
                string query = "select productionOverhead.*,overheads.name as overheadName,measurementType.measureTypeName,overheads.unitCost  from productionOverhead ";
                query += "inner join overheads on overheads.id = productionOverhead.overheadId ";
                query += "inner join measurementType on measurementType.id = overheads.mTypeId ";

                cmd.CommandText = query;
                cmd.CommandType = CommandType.Text;
                this.SQLiteAdaptor(dt, cmd);

                lst = dt.Tables[0].Rows.Cast<DataRow>().Select(x => new ProductionOverhead()
                {
                    id = int.Parse(x["id"].ToString()),
                    mType = x["measureTypeName"].ToString(),
                    overheadId = int.Parse(x["overheadId"].ToString()),
                    productionId = int.Parse(x["productionId"].ToString()),
                    unitCost = double.Parse(x["unitCost"].ToString()),
                    overheadName = x["overheadName"].ToString(),
                    overheadCount = int.Parse(x["OverheadCount"].ToString())
                }).ToList();
            }

            return lst;
        }

    }
}
