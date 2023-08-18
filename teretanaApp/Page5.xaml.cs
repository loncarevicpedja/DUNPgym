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
    /// Interaction logic for Page5.xaml
    /// </summary>
    public partial class Page5 : Page
    {
        User _user = new User();
        Racun _racun = new Racun(); 
        public Page5()
        {
            InitializeComponent();
        }
        private void comboBoxPlacanje_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBoxPlacanje.SelectedIndex == 1)
            {
                textBoxBrojKartice.Visibility = Visibility.Visible;
                datePickerDatumVazenja.Visibility = Visibility.Visible;
                textBoxCCV.Visibility = Visibility.Visible;
                textBoxBrojKarticeTekst.Visibility = Visibility.Visible;
                datePickerDatumVazenjaTekst.Visibility = Visibility.Visible;
                textBoxCCVTekst.Visibility = Visibility.Visible;
            }
            else if (comboBoxPlacanje.SelectedIndex == 0)
            {
                textBoxBrojKartice.Visibility = Visibility.Collapsed;
                datePickerDatumVazenja.Visibility = Visibility.Collapsed;
                textBoxCCV.Visibility = Visibility.Collapsed;
                textBoxBrojKarticeTekst.Visibility = Visibility.Collapsed;
                datePickerDatumVazenjaTekst.Visibility = Visibility.Collapsed;
                textBoxCCVTekst.Visibility = Visibility.Collapsed;
            }
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            bool daLiVaziClanarina = _user.ProveriIstekClanarine(MainWindow.TrenutniKorisnik.Id);
            VazenjeClanarine.Text = daLiVaziClanarina ? "Vaša članarina i dalje važi." : "Vaša članarina je istekla. Molimo Vas da je produžite.";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DateTime? selectedDate = DatumProduzenja.SelectedDate;
            string datumProduzenjaString = selectedDate?.ToString("dd/MM/yyyy");
            var cena = _racun.IzracunavanjeCene(MainWindow.TrenutniKorisnik.Id, datumProduzenjaString);
            if(cena > 0)
            {

                _racun.EvidentirajPlacanje(datumProduzenjaString,MainWindow.TrenutniKorisnik.Id, comboBoxPlacanje.SelectedItem.ToString(), cena, textBoxBrojKartice.Text, datePickerDatumVazenja.Text, textBoxCCV.Text);
            }
            else
            {
                MessageBox.Show("Produženje članarine je moguće samo od sutrašnjeg datuma, pa na dalje!");
            }
        }
    }
}
