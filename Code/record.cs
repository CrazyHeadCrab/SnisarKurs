using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;

namespace Barber.Code
{
    class record
    {
        public int id { get; set; }

        public int id_empl { get; set; }

        public int id_client { get; set; }
        public int id_branch { get; set; }
        public int id_serv { get; set; }
        public TimeSpan time { get; set; }
        public DateTime date { get; set; }
        public string name_serv { get; set; }
        public string serv_cost { get; set; }
        public string name_branch { get; set; }
        public string branch_addres { get; set; }
        public string name_empl { get; set; }
        public string surname_empl { get; set; }
        public string post_name { get; set; }

        public static List<record> recordpush(int client_id)
        {
            List<record> records = new List<record>();
            string strcon = ConfigurationManager.ConnectionStrings["defcon"].ConnectionString;
            using (SqlConnection con = new SqlConnection(strcon))
            {
                con.Open();
                string cmd = "select* from  all_client_record(" + client_id +")";
                SqlCommand sqlCommand = new SqlCommand(cmd, con);
                SqlDataReader data = sqlCommand.ExecuteReader();
                while (data.Read())
                {
                    record recor = new record();
                    recor.id = data.GetInt32(0);
                    recor.id_empl = data.GetInt32(1);
                    recor.id_client = data.GetInt32(2);
                    recor.id_branch = data.GetInt32(3);
                    recor.id_serv = data.GetInt32(4);
                    recor.time = data.GetTimeSpan(5);
                    recor.date = data.GetDateTime(6);
                    recor.name_serv = data.GetString(7);
                    recor.serv_cost = data.GetDecimal(8).ToString();
                   // recor.name_branch = data.GetString(9);
                    recor.branch_addres = data.GetString(14) +" "+ data.GetString(10);
                    recor.name_empl = data.GetString(12) +" " + data.GetString(11);
                    //recor.surname_empl = data.GetString(12);
                    recor.post_name = data.GetString(13);
                    records.Add(recor);
                }
                con.Close();
            }
            return records;
        }
    }
}
