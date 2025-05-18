using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace DashaCoursework
{
    public partial class WriteOffProductPage : Page
    {
        public WriteOffProductPage()
        {
            InitializeComponent();
            LoadProducts();
        }

        private void LoadProducts()
        {
            using (var context = new courseworkEntities())
            {
                var products = context.Product
                                      .Where(p => p.Count > 0)
                                      .Select(p => new
                                      {
                                          ProductId = p.Id,
                                          ProductName = p.Name + " (Количество: " + p.Count + ")"
                                      })
                                      .ToList();

                ProductComboBox.ItemsSource = products;
                ProductComboBox.DisplayMemberPath = "ProductName";
                ProductComboBox.SelectedValuePath = "ProductId";
            }
        }

        private void BtnWriteOff_Click(object sender, RoutedEventArgs e)
        {
            if (ProductComboBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите товар для списания!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(QuantityTextBox.Text, out int quantity) || quantity <= 0)
            {
                MessageBox.Show("Введите корректное количество для списания!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
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
                    MessageBox.Show("Недостаточное количество товара для списания.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                product.Count -= quantity;

                try
                {
                    context.SaveChanges();
                    MessageBox.Show("Товар успешно списан!", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);

                    LoadProducts();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при списании товара: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.GoBack();
        }
    }
}
