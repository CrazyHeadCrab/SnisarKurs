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
    public partial class regist : Page
    {
        public delegate void trueregist(object sender, MouseButtonEventArgs e);
        public event trueregist regisrate;

        public regist()
        {
            InitializeComponent();

        }

        private void regbut_Click(object sender, RoutedEventArgs e)
        {
            if (cheak_input_date())
            {

            }
            else
            {
                string strconection = ConfigurationManager.ConnectionStrings["defcon2"].ConnectionString;
                using (SqlConnection sqlConnection = new SqlConnection(strconection))
                {
                    sqlConnection.Open();
                    string cmd = "exec dbo.regist_client @log, @pas, @name, @sur, @patr, @email, @phone";
                    SqlCommand sqlCommand = new SqlCommand(cmd, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@log", loginBox.Text);
                    sqlCommand.Parameters.AddWithValue("@pas", PasBox.Password);
                    sqlCommand.Parameters.AddWithValue("@name", nameBox.Text);
                    sqlCommand.Parameters.AddWithValue("@sur", surnameBox.Text);
                    
                    if (patroname.Text.Length > 0) 
                        sqlCommand.Parameters.AddWithValue("@patr", patroname.Text);
                    else
                        sqlCommand.Parameters.AddWithValue("@patr", "null");
                    if (emailBox.Text.Length>0)
                        sqlCommand.Parameters.AddWithValue("@email", emailBox.Text);
                    else
                        sqlCommand.Parameters.AddWithValue("@email", "null");
                    if (emailBox.Text.Length > 0)
                        sqlCommand.Parameters.AddWithValue("@phone", phoneBox.Text);
                    else
                        sqlCommand.Parameters.AddWithValue("@phone", "null");
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                    if (sqlCommand.ExecuteNonQuery() == 2)
                    {
                        MessageBox.Show("ok");
                        regisrate.Invoke(this, null);
                    }
                        
                    else MessageBox.Show("not ok");

                }
            }
        }

       

        private bool cheak_input_date()
        {
            return false;
        }
    }
}
