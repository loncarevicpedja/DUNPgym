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
    /// Interaction logic for Page4.xaml
    /// </summary>
    public partial class Page4 : Page
    {
        Trening _trening = new Trening();
        public Page4()
        {
            InitializeComponent();
            this.DataContext = _trening;
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            List<Trening> rezultati = MainWindow.TrenutniKorisnik.ListRezervisaneTreninge(MainWindow.TrenutniKorisnik.Id);
            dataGrid.ItemsSource = rezultati;
        }

    }
}
