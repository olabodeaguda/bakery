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
    public class ProductionOverheadDao: AbstractDao
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

    }
}
