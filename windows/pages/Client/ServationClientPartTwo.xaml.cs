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
using System.Collections.ObjectModel;

using System.Data.SqlClient;
using System.Configuration;

namespace Barber.windows.pages.Client
{
    /// <summary>
    /// Логика взаимодействия для ServationClientPartTwo.xaml
    /// </summary>
    public partial class ServationClientPartTwo : Page
    {
        private Frame mainframe;
        private int ID;
        private branch branchID;
        private List<emplo> emploes;
        private ObservableCollection<bookingtime> bookingtimes;
        private ObservableCollection<service> services;
        public ServationClientPartTwo(Frame frame, int id, object branchid)
        {
            InitializeComponent();
            mainframe = frame;
            ID = id;
            branchID =(branch) branchid;
            services = service.servicepush(branchID.id);
            listservices.ItemsSource = services;
            calendar.DisplayDateStart = DateTime.Today;
            calendar.SelectedDatesChanged += Calendar_SelectedDatesChanged;
            calendar.SelectedDate = null;
        }

        private void Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            timepup.IsOpen = false;
            timepup.IsOpen = true;
            emplo empl = (emplo)listemplo.SelectedItem;
            if (bookingtimes != null) bookingtimes.Clear();
            bookingtimes = bookingtime.bookingtimepush(empl.id, (DateTime)calendar.SelectedDate);
            listtime.ItemsSource = bookingtimes;
        }

        private void listservices_Selected(object sender, RoutedEventArgs e)
        {
            service serv = (service)listservices.SelectedItem;
            if (serv != null)
            {
                timepup.IsOpen = false;
                listemplo.SelectedItem = null;
                listtime.SelectedItem = null;
                if (emploes != null) emploes.Clear();
                emploes = emplo.emplopush(branchID.id, serv.id);
                listemplo.ItemsSource = emploes;
                listemplo.Visibility = Visibility;
                calendar.Visibility = Visibility.Hidden;

            }



        }

        private void butback_Click(object sender, RoutedEventArgs e)
        {
            if (mainframe.CanGoBack)
            {
                mainframe.GoBack();
                mainframe.RemoveBackEntry();
            }
        }

        private void butnext_Click(object sender, RoutedEventArgs e)
        {

            if(listtime.SelectedItem != null)
            {
                endpupadd.Text = "Адресс: г." + branchID.town +", " + branchID.addres;
                endpupserv.Text = "Услуга:" + ((service)listservices.SelectedItem).name + ", Стоимость:" + "Услуга:" + ((service)listservices.SelectedItem).cost + " руб.";
                endpupempl.Text = "Сотрудник:" + ((emplo)listemplo.SelectedItem).surname + " " + ((emplo)listemplo.SelectedItem).name;
                endpupdata.Text = ((bookingtime)listtime.SelectedItem).date.ToShortDateString() + " " + ((bookingtime)listtime.SelectedItem).time.ToString();
                maingrid.IsEnabled = false;
                endpup.IsOpen = true;
            }
        }

        private void butpopclose_Click(object sender, RoutedEventArgs e)
        {
            timepup.IsOpen = false;
            listtime.SelectedItem = null;
        }

        private void listemplo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            calendar.Visibility = Visibility.Visible;
            listtime.SelectedItem = null;
            timepup.IsOpen = false;
        }

        private void endpupexit_Click(object sender, RoutedEventArgs e)
        {
            maingrid.IsEnabled = true;
            endpup.IsOpen = false;
        }

        private void endpuptrue_Click(object sender, RoutedEventArgs e)
        {
            string d = ((bookingtime)listtime.SelectedItem).date.Year.ToString() + "-" + ((bookingtime)listtime.SelectedItem).date.Month.ToString() + "-" + ((bookingtime)listtime.SelectedItem).date.Day.ToString();
                string strcon = ConfigurationManager.ConnectionStrings["defcon2"].ConnectionString;
                using (SqlConnection con = new SqlConnection(strcon))
                {
                    con.Open();
                    string cmd = "exec dbo.order_creation  "+ ID + ","+ ((emplo)listemplo.SelectedItem).id + "," + branchID.id + ",'" + ((bookingtime)listtime.SelectedItem).time + "'," + ((service)listservices.SelectedItem).id + ",'" + d + "'";
                    SqlCommand sqlCommand = new SqlCommand(cmd, con);

                    if (sqlCommand.ExecuteNonQuery() == 1)
                    {
                        
                        while (mainframe.CanGoBack)
                        {
                            mainframe.GoBack();
                            mainframe.RemoveBackEntry();
                        }
                }
                    
                    con.Close();
                }
                
        }

        private void listtime_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listtime.SelectedItem == null)
                butnext.IsEnabled = false;
            else butnext.IsEnabled = true;
        }


    }
}
