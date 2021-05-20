using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;

namespace Barber.Code
{
    class client
    {
        public int id { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public string patronic { get; set; }

        public string email { get; set; }
        public string phone { get; set; }

        public string logn { get; set; }
        public string pass { get; set; }

        public static client client_inf(int client_id)
        {
            client clie = new client();
            string strcon = ConfigurationManager.ConnectionStrings["defcon"].ConnectionString;
            using (SqlConnection con = new SqlConnection(strcon))
            {
                con.Open();
                string cmd = "select * from client_inf where client_id = " + client_id;
                SqlCommand sqlCommand = new SqlCommand(cmd, con);
                SqlDataReader data = sqlCommand.ExecuteReader();
                if (data.Read())
                {
                    clie.id = data.GetInt32(0);
                    clie.name = data.GetString(1);
                    clie.surname = data.GetString(2);
                    if (!data.IsDBNull(3))
                        clie.patronic = data.GetString(3);
                    if (!data.IsDBNull(4))
                        clie.email = data.GetString(4);
                    if (!data.IsDBNull(5))
                        clie.phone = data.GetString(5);
                    clie.logn = data.GetString(6);
                }
                con.Close();
            }
            return clie;
        }
    }
}
