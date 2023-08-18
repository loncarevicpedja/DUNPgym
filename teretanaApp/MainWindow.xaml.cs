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
using System.Data;
using System.Data.SqlClient;

namespace teretanaApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static User TrenutniKorisnik { get; private set; }
        public MainWindow()
        {
            InitializeComponent();
            TrenutniKorisnik = null;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string email = txtEmail.Text;
            string lozinka = txtPassword.Text;

            User prijavljeniKorisnik = new User();
            User prijavKorins = prijavljeniKorisnik.Prijava(email, lozinka);

            if (prijavKorins != null)
            {
                MainWindow.TrenutniKorisnik = prijavKorins;

                if (prijavKorins.VrstaNaloga == "Korisnik")
                {
                    UWindow userWindow = new UWindow();
                    userWindow.Title = "Dobrodošao " + prijavKorins.ImePrezime;
                    userWindow.Show();
                    this.Close();
                }
                else if (prijavKorins.VrstaNaloga == "Admin")
                {
                    AWindow adminWindow = new AWindow();
                    adminWindow.Title = "Dobrodošao " + prijavKorins.ImePrezime;
                    adminWindow.Show();
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("E-adresa ili lozinka su neispravni");
            }
        }
    }
}
