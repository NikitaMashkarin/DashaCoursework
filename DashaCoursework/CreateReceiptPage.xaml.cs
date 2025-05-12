using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace DashaCoursework
{
    public partial class CreateReceiptPage : Page
    {
        private int receiptId;
        private int sellerId;

        public CreateReceiptPage(int receiptId, int sellerId)
        {
            InitializeComponent();
            this.receiptId = receiptId;
            this.sellerId = sellerId;
        }

        private void BtnTasks_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddProductPage(receiptId, sellerId));
        }

        private void BtnRegisterTask_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new courseworkEntities())
            {
                var salesInReceipt = context.Sale_of_receipt
                                             .Where(sr => sr.Id_Receipt == receiptId)
                                             .Select(sr => sr.Id_Sale)
                                             .ToList();

                var sales = context.Sale
                                    .Where(s => salesInReceipt.Contains(s.Id))
                                    .ToList();

                decimal totalSum = sales.Sum(s => s.Sum ?? 0m);

                var receipt = context.Receipt.FirstOrDefault(r => r.Id == receiptId);
                if (receipt != null)
                {
                    receipt.Sum = totalSum;
                    context.SaveChanges();
                }

                MessageBox.Show("Продажа завершена!", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                Manager.MainFrame.Navigate(new SellerPage(sellerId));
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.GoBack();
        }
    }
}
