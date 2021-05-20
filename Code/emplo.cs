using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;
using System.Windows;


namespace Barber.Code
{
    class emplo
    {
        public int id { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public string patronic { get; set; }

        public string post_name { get; set; }
        public int post_id { get; set; }

        public static List<emplo> emplopush( int branch_id, int service_id)
        {
            List<emplo> emplos = new List<emplo>();
            string strcon = ConfigurationManager.ConnectionStrings["defcon"].ConnectionString;
            using (SqlConnection con = new SqlConnection(strcon))
            {
                con.Open();
                string cmd = "select employer_id, emp_name, emp_surname, emp_patronic, post_name, employer.post_id from employer inner join branch on branch.branch_id = employer.branch_id inner join dbo.all_srvice_post("+ service_id +") on employer.post_id = all_srvice_post.post_id inner join post on post.post_id = employer.post_id where employer.branch_id = " + branch_id;
                SqlCommand sqlCommand = new SqlCommand(cmd, con);
                SqlDataReader data = sqlCommand.ExecuteReader();
                while (data.Read())
                {
                    emplo empl = new emplo();
                    empl.id = data.GetInt32(0);
                    empl.name = data.GetString(1);
                    empl.surname = data.GetString(2);
                    empl.patronic = data.GetString(3);
                    empl.post_name = data.GetString(4);
                    empl.post_id = data.GetInt32(5);
                    emplos.Add(empl);
                }
                con.Close();
            }
            return emplos;
        }
    }

    public static  string x(string x)
    {
        string b =  x.Replace('\'', '\"');
        b = b.Replace('_', '-');
        b = b.Replace('%', '\0');
        b.
        
        return b;

    }
}
