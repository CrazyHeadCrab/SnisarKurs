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
    class branch
    {
        public int id { get; set; }
        public string name { get; set; }
        public string town { get; set; }
        public string addres { get; set; }

        public static List<branch> branchpush()
        {
            List<branch> branches = new List<branch>();
            string strcon = ConfigurationManager.ConnectionStrings["defcon2"].ConnectionString;
            using (SqlConnection con = new SqlConnection(strcon))
            {
                con.Open();
                string cmd = "select branch_id, br_address, town_name from branch inner join town on branch.town_id = town.town_id";
                SqlCommand sqlCommand = new SqlCommand(cmd, con);
                SqlDataReader data = sqlCommand.ExecuteReader();
                while (data.Read())
                {
                    branch branch = new branch();
                    branch.id = data.GetInt32(0);
                    branch.addres = data.GetString(1);
                    branch.town = data.GetString(2);
                    branch.name = "XXX";
                    branches.Add(branch);
                }
                con.Close();
            }
            MessageBox.Show(branches.Count().ToString());
            MessageBox.Show(branches[0].addres);
            return branches;
        }

    }

}
