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
        public ClientAcc(Frame frame,int Id)
        {
            InitializeComponent();
            mainframe = frame;
            id = Id;
            accload = false;
            recordsActual = new List<record>();
            recordsHistory = new List<record>();
            records = record.recordpush(Id);
            foreach(record x in records)
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
        }

        private void accbut_Click(object sender, RoutedEventArgs e)
        {
            if (!accload)
            {
                client cli = client.client_inf(id);
                loginBox.Text = cli.logn;
                nameBox.Text = cli.name;
                surnameBox.Text = cli.surname;
                patroname.Text = cli.patronic;
                emailBox.Text = cli.email;
                phoneBox.Text = cli.phone;

            }
            ActBorder.Visibility = Visibility.Hidden;
            hisBorder.Visibility = Visibility.Hidden;
            PrivateInfBorder.Visibility = Visibility.Visible;
            accbut.IsEnabled = false;
            servbut.IsEnabled = true;
        }

        private void SaveChangeBut_Click(object sender, RoutedEventArgs e)
        {
            if (cheak_client_change())
            {

            }
            else
            {
                string strconection = ConfigurationManager.ConnectionStrings["defcon2"].ConnectionString;
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
                        string b = x.ToString();
                        MessageBox.Show(b);
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

        private bool cheak_client_change()
        {
            return false;
        }
    }
}
