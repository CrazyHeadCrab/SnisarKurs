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
            switch (income_lvl)
            {
                case 'A': break;
                case 'B': break;
                case 'C':
                    MainMenuClient menu = new MainMenuClient(id);
                    menu.income_lvl = income_lvl;
                    menu.servation_client_page += servation_client_page;
                    menu.history_client_page += history_client_page;
                    menu.feedback_client_page += feedback_client_page;
                    menu.exit_client_page += exit_client_page;
                    Mainframe.Navigate(menu);
                    break;
            }


        }

        private void exit_client_page()
        {
            throw new NotImplementedException();
        }

        private void feedback_client_page()
        {
            throw new NotImplementedException();
        }

        private void history_client_page()
        {
            throw new NotImplementedException();
        }

        private void servation_client_page()
        {
            throw new NotImplementedException();
        }
    }
}
