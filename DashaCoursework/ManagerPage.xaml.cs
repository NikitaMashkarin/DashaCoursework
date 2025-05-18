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

namespace DashaCoursework
{
    /// <summary>
    /// Логика взаимодействия для ManagerPage.xaml
    /// </summary>
    public partial class ManagerPage : Page
    {
        private int id;
        public ManagerPage(int id)
        {
            InitializeComponent();
            this.id = id;
        }

        private void Btn1_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new CreateProductPage());
        }

        private void Btn2_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new WriteOffProductPage());
        }

        private void Btn3_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new SalesAnalysisPage());
        }

        private void Btn4_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new StockControlPage());
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.GoBack();
        }
    }
}
