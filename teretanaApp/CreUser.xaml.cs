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

namespace teretanaApp
{
    /// <summary>
    /// Interaction logic for CreUser.xaml
    /// </summary>
    public partial class CreUser : Page
    {
        User _user = new User();
           
        public CreUser()
        {
            InitializeComponent();
             this.DataContext = _user;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
           _user.UpisUBazu(potvrdaLozinke.Text, datumClanarine.SelectedDate.ToString());
        }
    }
}
