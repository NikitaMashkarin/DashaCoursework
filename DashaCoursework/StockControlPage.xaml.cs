using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace DashaCoursework
{
    public partial class StockControlPage : Page
    {
        public StockControlPage()
        {
            InitializeComponent();
            LoadCategories();
            LoadStockData();
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

                categories.Insert(0, new { Id = 0, Name = "Все" });

                CategoryComboBox.ItemsSource = categories;
                CategoryComboBox.DisplayMemberPath = "Name";
                CategoryComboBox.SelectedValuePath = "Id";
                CategoryComboBox.SelectedIndex = 0;
            }
        }

        private void LoadStockData(int? categoryId = null)
        {
            using (var context = new courseworkEntities())
            {
                var products = from product in context.Product
                               join category in context.Category on product.Category equals category.Id into catGroup
                               from cat in catGroup.DefaultIfEmpty()
                               select new
                               {
                                   product.Id,
                                   product.Name,
                                   product.Count,
                                   CategoryName = cat != null ? cat.Name : "Без категории"
                               };

                if (categoryId.HasValue && categoryId.Value != 0)
                {
                    products = products.Where(p => p.CategoryName == CategoryComboBox.Text);
                }

                StockDataGrid.ItemsSource = products.ToList();
            }
        }

        private void BtnApplyFilter_Click(object sender, RoutedEventArgs e)
        {
            int selectedCategoryId = (int)CategoryComboBox.SelectedValue;
            LoadStockData(selectedCategoryId);
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.GoBack();
        }
    }
}
