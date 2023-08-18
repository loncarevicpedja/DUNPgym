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

namespace teretanaApp
{
    /// <summary>
    /// Interaction logic for AWindow.xaml
    /// </summary>
    public partial class AWindow : Window
    {
        public AWindow()
        {
            InitializeComponent();
        }

        private void createUser_Click(object sender, RoutedEventArgs e)
        {
            panel.Content = new CreUser();
        }

        private void createSchedule_Click(object sender, RoutedEventArgs e)
        {
            panel.Content = new Page1();
        }

        private void manageAcc_Click(object sender, RoutedEventArgs e)
        {
            panel.Content = new Page2();
        }

        private void finance_Click(object sender, RoutedEventArgs e)
        {
            panel.Content = new Page6();
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }
    }
}
