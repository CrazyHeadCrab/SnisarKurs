using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;
using System.Windows;
using System.Collections.ObjectModel;

namespace Barber.Code
{
    class service
    {
        public int id { get; set; }
        public string name { get; set; }

        public string cost { get; set; }

        public static ObservableCollection<service> servicepush( int branch_id)
        {
            ObservableCollection<service> services = new ObservableCollection<service>();
            string strcon = ConfigurationManager.ConnectionStrings["defcon"].ConnectionString;
            using (SqlConnection con = new SqlConnection(strcon))
            {
                con.Open();
                string cmd = " select srvice.srvice_id, srvice_name, srvice_cost from srvice inner join dbo.all_branch_srvice("+ branch_id + ") on all_branch_srvice.srvice_id = srvice.srvice_id" ;
                SqlCommand sqlCommand = new SqlCommand(cmd, con);
                SqlDataReader data = sqlCommand.ExecuteReader();
                while (data.Read())
                {
                    service srvices = new service();
                    srvices.id = data.GetInt32(0);
                    srvices.name = data.GetString(1);
                    srvices.cost = data.GetDecimal(2).ToString();
                    services.Add(srvices);
                }
                con.Close();
            }
           // MessageBox.Show(branches.Count().ToString());
           // MessageBox.Show(branches[0].addres);
            return services;
        }
    }
}
