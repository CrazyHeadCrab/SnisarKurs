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
using Barber.Code;
using System.Data.SqlClient;
using System.Configuration;
using System.Data.SqlTypes;

namespace Barber.windows.pages.Client
{
    /// <summary>
    /// Логика взаимодействия для ClientAcc.xaml
    /// </summary>
    public partial class ClientAcc : Page
    {
        private int id;
        private Frame mainframe;
        private List<record> records;
        private List<record> recordsActual;
        private List<record> recordsHistory;
        private bool accload;
        public ClientAcc(Frame frame, int Id)
        {
            InitializeComponent();
            mainframe = frame;
            id = Id;
            accload = false;
            recordsActual = new List<record>();
            recordsHistory = new List<record>();
            records = record.recordpush(Id);
            foreach (record x in records)
            {
                //record rec = new record();
                //rec = x;
                if (x.date > DateTime.Now || (x.date == DateTime.Now && x.time > DateTime.Now.TimeOfDay))
                    recordsActual.Add(x);
                else recordsHistory.Add(x);
            }
            listactivserv.ItemsSource = recordsActual;
            listhisserv.ItemsSource = recordsHistory;


        }

        private void exitbut_Click(object sender, RoutedEventArgs e)
        {
            if (mainframe.CanGoBack)
            {
                mainframe.GoBack();
                mainframe.RemoveBackEntry();
            }
        }

        private void reviewbut_Click(object sender, RoutedEventArgs e)
        {
            record rec = (record)listhisserv.Items[listhisserv.Items.IndexOf(((Button)sender).DataContext)];
            Reviewpop.IsOpen = true;
            numpop.Text = rec.id.ToString();
            ordernumpop.Text = "Ваш Заказ #" + rec.id.ToString();
            maingrid.IsEnabled = false;
        }

        private void accbut_Click(object sender, RoutedEventArgs e)
        {
            client cli = client.client_inf(id);
            loginBox.Text = cli.logn;
            nameBox.Text = cli.name;
            surnameBox.Text = cli.surname;
            patroname.Text = cli.patronic;
            emailBox.Text = cli.email;
            phoneBox.Text = cli.phone;
            ActBorder.Visibility = Visibility.Hidden;
            hisBorder.Visibility = Visibility.Hidden;
            PrivateInfBorder.Visibility = Visibility.Visible;
            accbut.IsEnabled = false;
            servbut.IsEnabled = true;
            errchangeblock.Text = "";
        }

        private void SaveChangeBut_Click(object sender, RoutedEventArgs e)
        {

            {
                string strconection = ConfigurationManager.ConnectionStrings["defcon"].ConnectionString;
                using (SqlConnection sqlConnection = new SqlConnection(strconection))
                {
                    sqlConnection.Open();
                    string cmd = " declare @x int   exec dbo.update_client @id, @name, @sur, @patr, @email, @phone, @x output    select @x";
                    SqlCommand sqlCommand = new SqlCommand(cmd, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@id", id);
                    sqlCommand.Parameters.AddWithValue("@name", nameBox.Text);
                    sqlCommand.Parameters.AddWithValue("@sur", surnameBox.Text);

                    if (patroname.Text.Length > 0)
                        sqlCommand.Parameters.AddWithValue("@patr", patroname.Text);
                    else
                        sqlCommand.Parameters.AddWithValue("@patr", "null");
                    if (emailBox.Text.Length > 0)
                        sqlCommand.Parameters.AddWithValue("@email", emailBox.Text);
                    else
                        sqlCommand.Parameters.AddWithValue("@email", "null");
                    if (phoneBox.Text.Length > 0)
                        sqlCommand.Parameters.AddWithValue("@phone", phoneBox.Text);
                    else
                        sqlCommand.Parameters.AddWithValue("@phone", "null");
                    //SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                    object x = sqlCommand.ExecuteScalar();
                    if ((int)x == 1)
                    {
                        MessageBox.Show("ok");
                    }

                    else
                    {
                        if ((int)x == 2)
                        {
                            errchangeblock.Text = "Неправильный вид email";
                        }
                        else
                        {
                            errchangeblock.Text = "Неправильный вид телефона";
                        }

                    }
                }
            }
        }

        private void servbut_Click(object sender, RoutedEventArgs e)
        {
            ActBorder.Visibility = Visibility.Visible;
            hisBorder.Visibility = Visibility.Visible;
            PrivateInfBorder.Visibility = Visibility.Hidden;
            accbut.IsEnabled = true;
            servbut.IsEnabled = false;
        }



        private void Acc_date_KeyUp(object sender, KeyEventArgs e)
        {
            SaveChangeBut.IsEnabled = true;

        }

        private void endpuptrue_Click(object sender, RoutedEventArgs e)
        {
            string conect = ConfigurationManager.ConnectionStrings["defcon"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(conect))
            {
                connection.Open();
                string command = "exec dbo.add_review @rew, @rec_id, @star";
                SqlCommand sqlCommand = new SqlCommand(command, connection);
                sqlCommand.Parameters.AddWithValue("@rew", review.Text);
                sqlCommand.Parameters.AddWithValue("@rec_id", numpop.Text);
                sqlCommand.Parameters.AddWithValue("@star", star.val);
                if (sqlCommand.ExecuteNonQuery() == 1)
                {

                }
                connection.Close();
            }
            review.Text = "";
            Reviewpop.IsOpen = false;
            maingrid.IsEnabled = true;
        }

        private void endpupexit_Click(object sender, RoutedEventArgs e)
        {
            review.Text = "";
            Reviewpop.IsOpen = false;
            maingrid.IsEnabled = true;
        }
    }
}
