﻿using BakeryPR.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BakeryPR.DAO
{
    public class OverheadDao : AbstractDao
    {
        public string deleteProductionQuery(Production p)
        {
            return "delete from productionOverhead where productionId = '" + p.id + "';";
        }
        
        public List<Overhead> all()
        {
            List<Overhead> lst = new List<Overhead>();
            using (SQLiteConnection conn = new SQLiteConnection(this.connectionString))
            {
                conn.Open();
                DataSet dt = new DataSet();
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = "select * from overheads order by name";
                cmd.CommandType = CommandType.Text;
                this.SQLiteAdaptor(dt, cmd);

                lst = dt.Tables[0].Rows.Cast<DataRow>().Select(x => new Overhead()
                {
                    id = int.Parse(x["id"].ToString()),
                    name = x["name"].ToString(),
                    mTypeId = int.Parse(x["mTypeId"].ToString()),
                    unitCost = double.Parse(x["unitCost"].ToString()),
                     overheadType = x["overheadType"].ToString()
                }).ToList();
            }

            return lst;
        }

        public bool add(Overhead values)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = "insert into overheads(name,mTypeId,unitCost,overheadType) " +
                    "values(@name,@mTypeid,@unitCost,@overheadType)";
                cmd.Parameters.AddWithValue("@name", values.name);
                cmd.Parameters.AddWithValue("@unitCost", values.unitCost);
                cmd.Parameters.AddWithValue("@mTypeid", values.mTypeId);
                cmd.Parameters.AddWithValue("@overheadType", values.overheadType);
                cmd.CommandType = CommandType.Text;
                int count = cmd.ExecuteNonQuery();
                if (count > 0)
                {
                    return true;
                }
            }

            return false;
        }

        public bool update(Overhead values)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = "update overheads set overheadType=@overheadType, name = @name,unitCost=@unitCost,mTypeid = @mTypeid where id = @id";
                cmd.Parameters.AddWithValue("@name", values.name);
                cmd.Parameters.AddWithValue("@unitCost", values.unitCost);
                cmd.Parameters.AddWithValue("@id", values.id);
                cmd.Parameters.AddWithValue("@mTypeid", values.mTypeId);
                cmd.Parameters.AddWithValue("@overheadType", values.overheadType);
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
