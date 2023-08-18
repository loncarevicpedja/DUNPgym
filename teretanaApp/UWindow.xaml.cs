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
    /// Interaction logic for UWindow.xaml
    /// </summary>
    public partial class UWindow : Window
    {
        public UWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            panel.Content = new Page4();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            panel.Content = new Page3();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            panel.Content = new Page5();
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }
    }
}
