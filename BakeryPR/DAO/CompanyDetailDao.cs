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
    public class CompanyDetailDao : AbstractDao
    {
        public CompanyDetail All()
        {
            using (SQLiteConnection conn = new SQLiteConnection(this.connectionString))
            {
                conn.Open();
                DataSet dt = new DataSet();
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = "select * from CompanyDetail";
                cmd.CommandType = CommandType.Text;
                this.SQLiteAdaptor(dt, cmd);

                return dt.Tables[0].Rows.Cast<DataRow>().Select(x => new CompanyDetail()
                {
                    id = int.Parse(x["id"].ToString()),
                    businessAddress = x["businessAddress"].ToString(),
                    businessName = x["businessName"].ToString(),
                    businessRegNUmber = x["businessRegNUmber"].ToString(),
                    businessRegType = x["businessRegType"].ToString(),
                    contactEmail = x["contactEmail"].ToString(),
                    contactName = x["contactName"].ToString(),
                    contactPhoneNumber = x["contactPhoneNumber"].ToString(),
                    emailAddress = x["emailAddress"].ToString()
                }).FirstOrDefault();
            }
        }

        public bool Add(CompanyDetail companyDetail)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = "INSERT INTO CompanyDetail(businessAddress,businessName,businessRegNUmber,businessRegType,contactEmail,contactName,contactPhoneNumber,emailAddress) values(@businessAddress,@businessName,@businessRegNUmber,@businessRegType,@contactEmail,@contactName,@contactPhoneNumber,@emailAddress)";

                cmd.Parameters.AddWithValue("@businessAddress", companyDetail.businessAddress);
                cmd.Parameters.AddWithValue("@businessName", companyDetail.businessName);
                cmd.Parameters.AddWithValue("@businessRegNUmber", companyDetail.businessRegNUmber);
                cmd.Parameters.AddWithValue("@businessRegType", companyDetail.businessRegType);
                cmd.Parameters.AddWithValue("@contactEmail", companyDetail.contactEmail);
                cmd.Parameters.AddWithValue("@contactName", companyDetail.contactName);
                cmd.Parameters.AddWithValue("@contactPhoneNumber", companyDetail.contactPhoneNumber);
                cmd.Parameters.AddWithValue("@emailAddress", companyDetail.emailAddress);
                cmd.CommandType = CommandType.Text;

                int id = (int)cmd.ExecuteNonQuery();
                if (id <= 0)
                {
                    return false;
                }

                return true;
            }
        }
        
        public bool Update(CompanyDetail companyDetail)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = "UPDATE CompanyDetail SET emailAddress=@emailAddress,contactPhoneNumber=@contactPhoneNumber,contactEmail=@contactEmail,businessRegType=@businessRegType,businessRegNUmber=@businessRegNUmber,businessName=@businessName,businessAddress = @businessAddress WHERE id=@id";

                cmd.Parameters.AddWithValue("@businessAddress", companyDetail.businessAddress);
                cmd.Parameters.AddWithValue("@businessName", companyDetail.businessName);
                cmd.Parameters.AddWithValue("@businessRegNUmber", companyDetail.businessRegNUmber);
                cmd.Parameters.AddWithValue("@businessRegType", companyDetail.businessRegType);
                cmd.Parameters.AddWithValue("@contactEmail", companyDetail.contactEmail);
                cmd.Parameters.AddWithValue("@contactName", companyDetail.contactName);
                cmd.Parameters.AddWithValue("@contactPhoneNumber", companyDetail.contactPhoneNumber);
                cmd.Parameters.AddWithValue("@emailAddress", companyDetail.emailAddress);
                cmd.Parameters.AddWithValue("@id", companyDetail.id);

                cmd.CommandType = CommandType.Text;
                int id = (int)cmd.ExecuteNonQuery();
                if (id <= 0)
                {
                    return false;
                }

                return true;
            }
        }

        public string Title()
        {
            var r = All();
            if (r != null)
            {
                return r.businessName;
            }

            return "Bakery Software";
        }
    }
}
