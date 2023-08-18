using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for Page2.xaml
    /// </summary>
    public partial class Page2 : Page
    {
        User _user = new User();

        public Page2()
        {
            InitializeComponent();
            this.DataContext = _user;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string imePrezime = _user.ImePrezime;
            string korisnickoIme = _user.Email;

            ObservableCollection<User> rezultatiPretrage = _user.Pretrazi(imePrezime, korisnickoIme);

            if (rezultatiPretrage.Count > 0)
            {
                dataGrid.ItemsSource = rezultatiPretrage;
            }
            else
            {
                MessageBox.Show("Nema podataka za unete parametre.");
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            _user.UpdateInDatabase();
        }
        
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem != null)
            {
                MessageBoxResult result = MessageBox.Show("Da li ste sigurni da želite da obrišete selektovanog korisnika?", "Potvrda brisanja", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    User selectedUser = (User)dataGrid.SelectedItem;
                    int selectedUserId = selectedUser.Id;
                    _user.DeleteUser(selectedUserId);
                }
            }
            else
            {
                MessageBox.Show("Nijedan korisnik nije selektovan.");
            }
        }
    }
}
