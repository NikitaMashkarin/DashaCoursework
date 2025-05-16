using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace DashaCoursework
{
    public partial class CreateProductPage : Page
    {
        public CreateProductPage()
        {
            InitializeComponent();
            LoadCategories();
        }

        private void LoadCategories()
        {
            using (var context = new courseworkEntities())
            {
                var categories = context.Category.Select(c => new
                {
                    c.Id,
                    c.Name
                }).ToList();

                CategoryComboBox.ItemsSource = categories;
                CategoryComboBox.DisplayMemberPath = "Name";
                CategoryComboBox.SelectedValuePath = "Id";
            }
        }

        private void BtnSingIn_Click(object sender, RoutedEventArgs e)
        {
            string productName = NameTxtBox.Text;
            string priceText = PriceTxtBox.Text;
            string countText = CountTxtBox.Text;
            var selectedCategory = CategoryComboBox.SelectedValue;

            if (string.IsNullOrWhiteSpace(productName) || string.IsNullOrWhiteSpace(priceText) ||
                string.IsNullOrWhiteSpace(countText) || selectedCategory == null)
            {
                MessageBox.Show("Заполните все поля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!decimal.TryParse(priceText, out decimal price) || price <= 0)
            {
                MessageBox.Show("Введите корректную цену!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(countText, out int count) || count < 0)
            {
                MessageBox.Show("Введите корректное количество!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using (var context = new courseworkEntities())
            {
                int categoryId = (int)selectedCategory;

                var newProduct = new Product
                {
                    Name = productName,
                    Price = price,
                    Count = count,
                    Category = categoryId
                };

                context.Product.Add(newProduct);

                try
                {
                    context.SaveChanges();
                    MessageBox.Show("Товар успешно добавлен!", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);

                    NameTxtBox.Clear();
                    PriceTxtBox.Clear();
                    CountTxtBox.Clear();
                    CategoryComboBox.SelectedIndex = -1;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при добавлении товара: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.GoBack();
        }
    }
}
