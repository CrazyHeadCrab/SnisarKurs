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
        public ClientAcc(Frame frame,int Id)
        {
            InitializeComponent();
            mainframe = frame;
            id = Id;
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

        }

        private void SaveChangeBut_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
