using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace DashaCoursework
{
    public partial class ReturnPage : Page
    {
        private int receiptId;

        public ReturnPage(int receiptId)
        {
            InitializeComponent();
            this.receiptId = receiptId;
            LoadProducts();
        }

        private void LoadProducts()
        {
            using (var context = new courseworkEntities())
            {
                var products = from sale in context.Sale_of_receipt
                               join saleItem in context.Sale on sale.Id_Sale equals saleItem.Id
                               join product in context.Product on saleItem.Product equals product.Id
                               where sale.Id_Receipt == receiptId
                               select new
                               {
                                   ProductId = product.Id,
                                   ProductName = product.Name,
                                   SaleCount = saleItem.Count
                               };

                ProductComboBox.ItemsSource = products.ToList();
                ProductComboBox.DisplayMemberPath = "ProductName";
                ProductComboBox.SelectedValuePath = "ProductId";
            }
        }

        private void BtnReturnProduct_Click(object sender, RoutedEventArgs e)
        {
            if (ProductComboBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите товар для возврата!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(QuantityTextBox.Text, out int quantity) || quantity <= 0)
            {
                MessageBox.Show("Введите корректное количество для возврата!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int productId = (int)ProductComboBox.SelectedValue;

            using (var context = new courseworkEntities())
            {
                var saleItem = (from sale in context.Sale_of_receipt
                                join saleEntry in context.Sale on sale.Id_Sale equals saleEntry.Id
                                where sale.Id_Receipt == receiptId && saleEntry.Product == productId
                                select new { Sale = saleEntry, SaleOfReceipt = sale }).FirstOrDefault();

                if (saleItem == null)
                {
                    MessageBox.Show("Товар не найден в данном чеке.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (quantity > saleItem.Sale.Count)
                {
                    MessageBox.Show("Количество для возврата превышает количество проданных товаров.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                decimal productPrice = (decimal)saleItem.Sale.Product1.Price;
                decimal refundAmount = productPrice * quantity;

                saleItem.Sale.Count -= quantity;
                saleItem.Sale.Sum = saleItem.Sale.Count * productPrice;

                if (saleItem.Sale.Count == 0)
                {
                    context.Sale_of_receipt.Remove(saleItem.SaleOfReceipt);
                    context.Sale.Remove(saleItem.Sale);
                }

                var product = context.Product.FirstOrDefault(p => p.Id == productId);
                if (product != null)
                {
                    product.Count += quantity;
                }

                var receipt = context.Receipt.FirstOrDefault(r => r.Id == receiptId);
                if (receipt != null)
                {
                    receipt.Sum -= refundAmount;

                    if (receipt.Sum < 0)
                        receipt.Sum = 0;
                }

                try
                {
                    context.SaveChanges();
                    MessageBox.Show($"Возврат выполнен успешно! Возвращено: {refundAmount:C2}", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);

                    LoadProducts();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при возврате товара: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }



        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.GoBack();
        }
    }
}
