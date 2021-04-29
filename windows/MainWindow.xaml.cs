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
using Barber.windows.pages;
using Barber.windows;

namespace Barber
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        regist registpg;
        enterance enterancepg;
        public MainWindow()
        {
            InitializeComponent();
            enterancepg = new enterance();
            enterancepg.beentranceevent += enterencebe;
            MainFrame.Navigate(enterancepg);


        }

        private void Label_SizeChanged(object sender, SizeChangedEventArgs e)
        {

        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void butturn_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void butclose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void txtlink_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (headerblock.Text.Contains("Вход"))
            {
                if (registpg == null)
                {
                    registpg = new regist();
                    registpg.regisrate += txtlink_MouseLeftButtonUp;
                }
                MainFrame.Navigate(registpg);
                headerblock.Text = "Регистрация";
                txtlink.Text = "Вход";
            }
            else
            {
                MainFrame.Navigate(enterancepg);
                headerblock.Text = "Вход";
                txtlink.Text = "Регистрация";
            }
        }

        private void enterencebe(int id, char income_lvl)
        {
            Mainmenu mainmenu = new Mainmenu(id, income_lvl);
            mainmenu.Show();
            this.Close();
        }
    }
}
