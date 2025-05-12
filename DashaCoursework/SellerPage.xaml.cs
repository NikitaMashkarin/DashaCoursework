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
    public partial class SellerPage : Page
    {
        private int sellerId;
        private int? receiptId = null;

        public SellerPage(int sellerId)
        {
            InitializeComponent();
            this.sellerId = sellerId;
        }

        private void BtnTasks_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new courseworkEntities())
            {
                var receipt = new Receipt
                {
                    Date_create = DateTime.Now,
                    User = sellerId,
                    Sum = 0
                };
                context.Receipt.Add(receipt);
                context.SaveChanges();

                Manager.MainFrame.Navigate(new CreateReceiptPage(receipt.Id, sellerId));
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AuthPage());
        }

        

        private void BtnRegisterTask_Click(object sender, RoutedEventArgs e)
        {
            if (receiptId == null)
            {
                MessageBox.Show("Введите корректный номер чека!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Manager.MainFrame.Navigate(new ReturnPage((int)receiptId));
        }

        private void ReceiptIdTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            if (int.TryParse(textBox.Text, out int parsedId))
            {
                receiptId = parsedId;
                textBox.Background = new SolidColorBrush(Colors.White);
            }
            else
            {
                receiptId = null;
                textBox.Background = new SolidColorBrush(Colors.LightCoral);
            }
        }

    }
}
