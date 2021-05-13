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
    /// Логика взаимодействия для Page1.xaml
    /// </summary>
    public partial class enterance : Page
    {
        public delegate void beentrance(int id, char income_lvl);
        public event beentrance beentranceevent;
        public enterance()
        {
            InitializeComponent();
            
        }

        private void InBut_Click(object sender, RoutedEventArgs e)
        {
            string conect = ConfigurationManager.ConnectionStrings["defcon2"].ConnectionString;
            using(SqlConnection connection = new SqlConnection(conect))
            {
                connection.Open();
                string command = "exec dbo.login_cheaks @log, @pas";
                SqlCommand sqlCommand = new SqlCommand(command, connection);
                sqlCommand.Parameters.AddWithValue("@log", loginBox.Text);
                sqlCommand.Parameters.AddWithValue("@pas", PasBox.Password);
                SqlDataReader sqlData = sqlCommand.ExecuteReader();
                if (sqlData.Read())
                {
                    switch (sqlData.GetString(3))
                    {
                        case "A":
                            
                            MessageBox.Show("begin A");
                            break;
                        case "B":
                        
                            MessageBox.Show("begin B");
                            break;
                        case "C":
                        
                            MessageBox.Show("begin C");
                            beentranceevent(sqlData.GetInt32(2), 'C');
                            break;
                    }
                }
                else
                {
                    errblock.Visibility = Visibility.Visible;
                }


            }
        }
    }
}
