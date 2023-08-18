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
    /// Interaction logic for Page3.xaml
    /// </summary>
    public partial class Page3 : Page
    {
        User _user = new User();
        Trening _trening = new Trening(); 
        public Page3()
        {
            InitializeComponent();
            this.DataContext = _trening;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string datum = _trening.DatumOdrzavanja;
            string vrstaTreninga = _trening.VrstaTreninga;

            Trening trening = new Trening();
            string[] arrVrstaTreninga = vrstaTreninga.Split(' ');
            vrstaTreninga = arrVrstaTreninga[arrVrstaTreninga.Length - 1];
            List<Trening> rezultati = trening.PretraziTreninge(datum, vrstaTreninga);

            dataGrid.ItemsSource = rezultati;    
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem != null)
            {
                Trening selectedTrening = (Trening)dataGrid.SelectedItem;
                int selectedTreningId = selectedTrening.Id;
                _user.RezervisiTrening(selectedTreningId, MainWindow.TrenutniKorisnik.Id);
            }
            else
            {
                MessageBox.Show("Nije izabran nijedan trening.");
            }
        }
    }
}
