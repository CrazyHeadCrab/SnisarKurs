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
    /// Логика взаимодействия для ServationClient.xaml
    /// </summary>
    public partial class ServationClient : Page
    {
        private Frame mainframe;
        private int ID;
        List<branch> Branches;
        public ServationClient( Frame frame, int id)
        {
            InitializeComponent();
            mainframe = frame;
            ID = id;
    //        Branches = new List<branch>();
            Branches = (branch.branchpush());
            listbranch.ItemsSource = Branches; 
            
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
            branch selectbranch = (branch)listbranch.SelectedItem;
            if (selectbranch != null)
            {
                ServationClientPartTwo servationclientparttwo = new ServationClientPartTwo(mainframe, ID, selectbranch);
                mainframe.Navigate(servationclientparttwo);
            }
        }
    }
}
