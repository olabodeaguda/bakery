using BakeryPR.Models;
using BakeryPR.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BakeryPR.DAO
{
    public class InventoryHistoryDao : AbstractDao
    {
        IngredentDao dao = new IngredentDao();

        public List<InventoryHistory> byId(int ingredentId)
        {
            List<InventoryHistory> lst = new List<InventoryHistory>();
            using (SQLiteConnection conn = new SQLiteConnection(this.connectionString))
            {
                conn.Open();
                string query = " select inventoryHistory.*,ingredent.ingredentName, measurementType.measureTypeName from inventoryHistory ";
                query = query + "inner join ingredent on ingredent.id = inventoryHistory.ingredentId ";
                query = query + "inner join measurementType on measurementType.id = ingredent.mTypeId ";
                query = query + "where ingredentId =@ingredentId ";
                query = query + " order by inventoryHistory.id desc ";

                DataSet dt = new DataSet();
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = query;// "select * from inventoryHistory where ingredentId =@ingredentId";
                cmd.Parameters.AddWithValue("ingredentId", ingredentId);
                cmd.CommandType = CommandType.Text;
                this.SQLiteAdaptor(dt, cmd);

                lst = dt.Tables[0].Rows.Cast<DataRow>().Select(x => new InventoryHistory()
                {
                    id = int.Parse(x["id"].ToString()),
                    addedBy = x["addedBy"].ToString(),
                    ingredentId = int.Parse(x["ingredentId"].ToString()),
                    amount = double.Parse(x["amount"].ToString()),
                    inventoryMode = x["inventoryMode"].ToString(),
                    dateCreated = DateTime.ParseExact(x["dateCreated"].ToString(), "dd-MM-yyyy", CultureInfo.InvariantCulture)
                }).ToList();
            }

            return lst;
        }

        public bool add(InventoryHistory values)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = "insert into inventoryHistory(ingredentId,amount,dateCreated,addedBy,inventoryMode) " +
                    "values(@ingredentId,@amount,@dateCreated,@addedBy,@inventoryMode)";
                cmd.Parameters.AddWithValue("@ingredentId", values.ingredentId);
                cmd.Parameters.AddWithValue("@amount", values.newQuantity);
                cmd.Parameters.AddWithValue("@dateCreated", DateTime.Now.ToString("dd-MM-yyyy"));
                cmd.Parameters.AddWithValue("@addedBy", values.addedBy);
                cmd.Parameters.AddWithValue("@inventoryMode", values.inventoryMode);
                cmd.CommandType = CommandType.Text;
                int count = cmd.ExecuteNonQuery();
                if (count > 0)
                {
                    //ingredentQuantity(values.ingredentId)
                    double unitCost = WeightAverageCostUtil.calculate(values.amount, values.oldUnitCost,
                        values.newQuantity, values.newUnitCost);
                    dao.updateIngredent(values.ingredentId, (values.newQuantity+values.amount), unitCost);
                    return true;                   
                }
            }

            return false;
        }

        public double ingredentQuantity(int id)
        {
            List<InventoryHistory> lts = byId(id);
            double amount = 0.0;
            foreach (var tm in lts)
            {
                if (tm.inventoryMode == InventoryMode.ADD.ToString())
                {
                    amount += tm.amount;
                }
                else if (tm.inventoryMode == InventoryMode.MINUS.ToString())
                {
                    amount -= tm.amount;
                }
            }

            return amount;
        }


        
    }
}
