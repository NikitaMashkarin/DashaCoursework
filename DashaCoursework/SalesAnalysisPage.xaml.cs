using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace DashaCoursework
{
    public partial class SalesAnalysisPage : Page
    {
        public SalesAnalysisPage()
        {
            InitializeComponent();
            LoadSalesData("desc");
        }

        private void LoadSalesData(string sortOrder)
        {
            using (var context = new courseworkEntities())
            {
                var salesData = context.Sale
                    .GroupBy(s => s.Product1.Name)
                    .Select(g => new
                    {
                        ProductName = g.Key,
                        TotalCount = g.Sum(s => s.Count),
                        TotalSum = g.Sum(s => s.Sum)
                    });

                if (sortOrder == "asc")
                {
                    salesData = salesData.OrderBy(s => s.TotalCount);
                }
                else
                {
                    salesData = salesData.OrderByDescending(s => s.TotalCount);
                }

                SalesDataGrid.ItemsSource = salesData.ToList();
            }
        }

        private void BtnApplySort_Click(object sender, RoutedEventArgs e)
        {
            var selectedSort = (SortComboBox.SelectedItem as ComboBoxItem)?.Tag?.ToString();
            if (!string.IsNullOrEmpty(selectedSort))
            {
                LoadSalesData(selectedSort);
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
