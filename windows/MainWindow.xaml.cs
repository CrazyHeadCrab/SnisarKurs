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

namespace Barber
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new Uri("/windows/pages/enterance.xaml",UriKind.RelativeOrAbsolute));
          
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
                MainFrame.Navigate(new Uri("/windows/pages/regist.xaml", UriKind.RelativeOrAbsolute));
                // MainFrame.Navigate(new Uri("pack://application:,,,/Barber;component/windows/pages/regist.xaml", UriKind.RelativeOrAbsolute));
                /*regist regist = new regist();
                regist.regisrate += txtlink_MouseLeftButtonUp;
                MainFrame.Navigate(regist);*/
                headerblock.Text = "Регистрация";
                txtlink.Text = "Вход";
            }
            else
            {
                
                MainFrame.Navigate(new Uri("/windows/pages/enterance.xaml", UriKind.RelativeOrAbsolute));
                headerblock.Text = "Вход";
                txtlink.Text = "Регистрация";
            }
        }
    }
}
