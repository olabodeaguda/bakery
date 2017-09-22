using BakeryPR.Models;
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
    public class MeasureTypeDao:AbstractDao
    {
        public List<MeasurementType> all()
        {
            List<MeasurementType> lst = new List<MeasurementType>();
            using (SQLiteConnection conn = new SQLiteConnection(this.connectionString))
            {
                conn.Open();
                DataSet dt = new DataSet();
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = "select * from measurementType";
                cmd.CommandType = CommandType.Text;
                this.SQLiteAdaptor(dt, cmd);

                lst = dt.Tables[0].Rows.Cast<DataRow>().Select(x => new MeasurementType()
                {
                    id = int.Parse(x["id"].ToString()),
                    name = x["measureTypeName"].ToString()
                }).ToList();
            }

            return lst;
        }
    }
}
