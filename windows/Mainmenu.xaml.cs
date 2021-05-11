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
using System.Windows.Shapes;
using Barber.windows.pages;
using Barber.Code;
using System.Collections;
using Barber.windows.pages.Client;

namespace Barber.windows
{
    /// <summary>
    /// Логика взаимодействия для Mainmenu.xaml
    /// </summary>
    public partial class Mainmenu : Window
    {
        public Mainmenu(int id, char income_lvl)
        {
            InitializeComponent();
            switch (income_lvl)
            {
                case 'A': break;
                case 'B': break;
                case 'C':
                    MainMenuClient menu = new MainMenuClient(id, Mainframe);
                   // menu.income_lvl = income_lvl;
                  
                    Mainframe.Navigate(menu);

                    break;
            }


        }

      
        private void butturn_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void butclose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        /*private void uni_butback(object sender, RoutedEventArgs e)
        {
            IEnumerable<Page> his = (IEnumerable<Page>)Mainframe.BackStack;
            Mainframe.Navigate(his.First());
        }
        */
    }
}
