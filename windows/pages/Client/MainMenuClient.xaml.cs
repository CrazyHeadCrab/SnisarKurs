using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Configuration;
using System.Data.SqlClient;

namespace Barber.windows.pages
{
    /// <summary>
    /// Логика взаимодействия для MainMenuClient.xaml
    /// </summary>
    
    public partial class MainMenuClient : Page
    {
        public int ID;
        public char income_lvl = 'C';

        public delegate void Change_client_page();
        public event Change_client_page servation_client_page;
        public event Change_client_page history_client_page;
        public event Change_client_page feedback_client_page;
        public event Change_client_page exit_client_page;
        public MainMenuClient(int id)
        {
            InitializeComponent();
            ID = id;
            Headername();

        }

        private void Headername()
        {
            string strcon = ConfigurationManager.ConnectionStrings["defcon"].ConnectionString;
            using (SqlConnection sqlcon = new SqlConnection(strcon))
            {
                sqlcon.Open();
                string cmd = "select cl_name from client where client_id=@id";
                SqlCommand sqlCommand = new SqlCommand(cmd, sqlcon);
                sqlCommand.Parameters.AddWithValue("@id", ID.ToString());
                SqlDataReader sqlData = sqlCommand.ExecuteReader();
                if (sqlData.Read())
                {
                    header.Text = "Задраствуйте, " + sqlData.GetString(0);
                    header.HorizontalAlignment = HorizontalAlignment.Center;
                }
                sqlcon.Close();
            }
        }

        private void butreservation_Click(object sender, RoutedEventArgs e)
        {
            servation_client_page();
        }

        private void buthistory_Click(object sender, RoutedEventArgs e)
        {
            history_client_page();
        }

        private void butfeedback_Click(object sender, RoutedEventArgs e)
        {
            feedback_client_page();
        }

        private void butexit_Click(object sender, RoutedEventArgs e)
        {
            exit_client_page();
        }
    }
}
