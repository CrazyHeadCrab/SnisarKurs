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
            MainMenuClient menu = new MainMenuClient(id);
            menu.income_lvl = income_lvl;
            Mainframe.Navigate(menu);
        }
    }
}
