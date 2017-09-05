using BakeryPR.Models;
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
    public class ProductionIngredentDao : AbstractDao
    {
        public IngredentDao ingrdentDao
        {
            get
            {
                return new IngredentDao();
            }
        }

        public String getProdString(ProductionIngredent pi)
        {
            String query = "";

            query = query + " insert into ProductionIngredient(ingredentId,productionId,amount,datecreated,createdBy) ";
            query = query + "values(";
            query = query + pi.ingredentId + ",";
            query = query + pi.productionId + ",";
            query = query + pi.amount + ",";
            query = query + "'" + DateTime.Now.ToString("yyyy-MM-dd") + "',";
            query = query + "'" + pi.createdBy + "'";
            query = query + ");";
            return query;
        }

        public bool byProductionIngredent(int productionId, int ingredentId)
        {
            using (SQLiteConnection conn = new SQLiteConnection(this.connectionString))
            {
                conn.Open();
                DataSet dt = new DataSet();
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = "select * from ProductionIngredient where productionId=@pid and ingredentId = @ingreId";
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@pid", productionId);
                cmd.Parameters.AddWithValue("@ingreId", ingredentId);
                this.SQLiteAdaptor(dt, cmd);

                var re = dt.Tables[0].Rows.Cast<DataRow>().Select(x => new ProductionIngredent()
                {
                    id = int.Parse(x["id"].ToString()),
                    amount = double.Parse(x["amount"].ToString()),
                }).FirstOrDefault();
                if (re != null)
                {
                    return true;
                }
            }

            return false;
        }

        public List<ProductionIngredent> byProductionId(int productionId)
        {
            List<ProductionIngredent> lst = new List<ProductionIngredent>();
            using (SQLiteConnection conn = new SQLiteConnection(this.connectionString))
            {
                string query = " select ProductionIngredient.*,ingredent.unitCost,ingredent.ingredentName,measurementType.measureTypeName from ProductionIngredient ";
                query = query + " inner join ingredent on ingredent.id = ProductionIngredient.ingredentId ";
                query = query + " inner join measurementType on measurementType.id = ingredent.mTypeId ";
                query = query + " where ProductionIngredient.productionId = @pId ";
                conn.Open();
                DataSet dt = new DataSet();
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = query;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@pId", productionId);
                this.SQLiteAdaptor(dt, cmd);

                foreach (var x in dt.Tables[0].Rows.Cast<DataRow>())
                {
                    ProductionIngredent pi = new ProductionIngredent();
                    pi.id = int.Parse(x["id"].ToString());
                    pi.measureType = x["measureTypeName"].ToString();
                    if (pi.measureType.ToLower() == "gram")
                    {
                        pi.amount = Math.Round(double.Parse(x["amount"].ToString()) /1000,2);
                        pi.measureType = "kg";
                    }
                    else
                    {
                        pi.amount = Math.Round(double.Parse(x["amount"].ToString()), 2);
                    }

                    pi.createdBy = x["createdBy"].ToString();
                    pi.ingredentId = int.Parse(x["ingredentId"].ToString());
                    pi.ingredentName = x["ingredentName"].ToString();
                    pi.lastModifiiedBy = x.IsNull("lastModifiedBy") ? string.Empty : x["lastModifiiedBy"].ToString();
                    pi.dateCreated = DateTime.Parse(x["dateCreated"].ToString(), new CultureInfo("en-US", true));
                    pi.productionId = int.Parse(x["productionId"].ToString());
                    
                    pi.unitCost = double.Parse(x["unitCost"].ToString());
                    lst.Add(pi);
                }

            }

            return lst;
        }

        public bool add(ProductionIngredent pi)
        {

            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = getProdString(pi);
                cmd.CommandType = CommandType.Text;
                int count = cmd.ExecuteNonQuery();
                if (count > 0)
                {
                    return true;
                }
            }

            return false;
        }

        public bool delete(int id)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = "delete from ProductionIngredient where id=@id";
                cmd.Parameters.AddWithValue("@id", id);
                cmd.CommandType = CommandType.Text;
                int count = cmd.ExecuteNonQuery();
                if (count > 0)
                {
                    return true;
                }
            }

            return false;
        }

        public string changeProdApprovalStatusQuery(String status, int productionId, string createdBy)
        {
            return $"update production set approval='{status}', approveBy='{createdBy}' where id='{productionId}';";
        }

        public bool changeProdApprovalStatus(String status, int productionId, string createdBy)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = "update production set approval=@approval, approveBy=@approveBy where id=@id";
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@approval", status);
                cmd.Parameters.AddWithValue("@approveBy", createdBy);
                cmd.Parameters.AddWithValue("@id", productionId);
                int count = cmd.ExecuteNonQuery();
                if (count > 0)
                {
                    return true;
                }
            }


            return false;
        }

        

        public void checkIngredentAvalabilityByProdId(int prodId)
        {
            List<ProductionIngredent> lst = byProductionId(prodId);
            if (lst.Count <= 0)
            {
                throw new Exception("No Ingredeint have been added to the production");
            }

            List<Ingredent> lstIngredent = ingrdentDao.all();

            foreach (var tm in lst)
            {
                var ng = lstIngredent.FirstOrDefault(x => x.id == tm.ingredentId);
                if (ng != null)
                {
                    if (ng.quantity < tm.amount)
                    {
                        throw new Exception(ng.ingredentName + " does not have enough quantity in stock");
                    }
                }
            }
        }

        public double sumtotalIngredientInKg(int pId)
        {
            var e = byProductionId(pId);
            return e.Sum(x => x.measureType.ToLower() == "gram" ? x.amount / 100 : x.amount);
        }

        public double sumtotalIngredientInGram(int pId)
        {
            var e = byProductionId(pId);
            return e.Sum(x => x.measureType.ToLower() == "kg" ? (x.amount * 100) : x.amount);
        }

        public bool Update(ProductionIngredent pi)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = "Update ProductionIngredient set ingredentId=@ingredient, amount=@amount where id=@id";
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@ingredient", pi.ingredentId);
                cmd.Parameters.AddWithValue("@amount", pi.amount);
                cmd.Parameters.AddWithValue("@id", pi.id);
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
