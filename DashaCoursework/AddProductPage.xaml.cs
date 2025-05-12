using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace DashaCoursework
{
    public partial class AddProductPage : Page
    {
        private int receiptId;
        private int sellerId;

        public AddProductPage(int receiptId, int sellerId)
        {
            InitializeComponent();
            this.receiptId = receiptId;
            this.sellerId = sellerId;

            LoadProducts();
        }

        private void LoadProducts()
        {
            using (var context = new courseworkEntities())
            {
                var products = context.Product.ToList();
                ProductComboBox.ItemsSource = products;
                ProductComboBox.DisplayMemberPath = "Name";
                ProductComboBox.SelectedValuePath = "Id";
            }
        }

        private void BtnAddProduct_Click(object sender, RoutedEventArgs e)
        {
            if (ProductComboBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите товар.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!int.TryParse(QuantityTextBox.Text, out int quantity) || quantity <= 0)
            {
                MessageBox.Show("Введите корректное количество.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int productId = (int)ProductComboBox.SelectedValue;

            using (var context = new courseworkEntities())
            {
                var product = context.Product.FirstOrDefault(p => p.Id == productId);

                if (product == null)
                {
                    MessageBox.Show("Товар не найден.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (product.Count < quantity)
                {
                    MessageBox.Show("Недостаточно товара на складе.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                decimal saleSum = product.Price.GetValueOrDefault() * quantity;

                var sale = new Sale
                {
                    Product = productId,
                    Count = quantity,
                    Sale_date = DateTime.Now,
                    Sum = saleSum
                };

                context.Sale.Add(sale);
                context.SaveChanges();

                var saleOfReceipt = new Sale_of_receipt
                {
                    Id_Receipt = receiptId,
                    Id_Sale = sale.Id
                };

                context.Sale_of_receipt.Add(saleOfReceipt);

                product.Count -= quantity;

                context.SaveChanges();

                MessageBox.Show("Товар добавлен в чек.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.GoBack();
        }
    }
}
