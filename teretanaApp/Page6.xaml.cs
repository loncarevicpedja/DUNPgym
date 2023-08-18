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
    /// Interaction logic for Page6.xaml
    /// </summary>
    public partial class Page6 : Page
    {
        public Page6()
        {
            InitializeComponent();
            Uplata _uplata = new Uplata(); 
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DateTime? pocetakPerioda = datePickerPocetak.SelectedDate;
            DateTime? krajPerioda = datePickerKraj.SelectedDate;

            if (!pocetakPerioda.HasValue || !krajPerioda.HasValue)
            {
                MessageBox.Show("Molimo unesite početak i kraj perioda.");
                return;
            }

            if (pocetakPerioda > krajPerioda)
            {
                MessageBox.Show("Početak perioda ne može biti veći od kraja perioda.");
                return;
            }

            List<Uplata> uplate = Uplata.DohvatiUplate(pocetakPerioda.Value, krajPerioda.Value);

            if (uplate.Count > 0)
            {
                decimal ukupno = 0;
                foreach (var uplata in uplate)
                {
                    ukupno += uplata.Cena;
                }
                ukupnoTextBox.Text = ukupno.ToString();
                dataGrid.ItemsSource = uplate;
            }
            else
            {
                MessageBox.Show("Nema izvršenih transakcija u tom periodu.");
            }
        }

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
