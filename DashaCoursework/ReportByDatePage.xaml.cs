using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;

namespace DashaCoursework
{
    public partial class ReportByDatePage : Page
    {
        public ReportByDatePage()
        {
            InitializeComponent();
        }

        private void BtnGenerateReport_Click(object sender, RoutedEventArgs e)
        {
            DateTime? startDate = StartDatePicker.SelectedDate;
            DateTime? endDate = EndDatePicker.SelectedDate;

            if (startDate == null || endDate == null)
            {
                MessageBox.Show("Выберите обе даты.");
                return;
            }

            if (startDate > endDate)
            {
                MessageBox.Show("Дата начала не может быть больше даты окончания.");
                return;
            }

            GenerateReport(startDate.Value, endDate.Value);
        }

        private void GenerateReport(DateTime startDate, DateTime endDate)
        {
            using (var context = new courseworkEntities())
            {
                var reportData = context.Sale
                    .Where(s => DbFunctions.TruncateTime(s.Sale_date) >= DbFunctions.TruncateTime(startDate)
                             && DbFunctions.TruncateTime(s.Sale_date) <= DbFunctions.TruncateTime(endDate))
                    .GroupBy(s => DbFunctions.TruncateTime(s.Sale_date))
                    .Select(g => new
                    {
                        Sale_date = g.Key,
                        TotalCount = g.Sum(s => s.Count ?? 0),
                        TotalSum = g.Sum(s => s.Sum ?? 0)
                    })
                    .OrderBy(r => r.Sale_date)
                    .ToList();

                ReportDataGrid.ItemsSource = reportData;
            }
        }

        private void BtnDownloadReport_Click(object sender, RoutedEventArgs e)
        {
            DateTime? startDate = StartDatePicker.SelectedDate;
            DateTime? endDate = EndDatePicker.SelectedDate;

            if (startDate == null || endDate == null)
            {
                MessageBox.Show("Выберите обе даты.");
                return;
            }

            if (startDate > endDate)
            {
                MessageBox.Show("Дата начала не может быть больше даты окончания.");
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Text file (*.txt)|*.txt",
                FileName = $"Отчет_{startDate.Value:yyyyMMdd}_{endDate.Value:yyyyMMdd}.txt"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                using (StreamWriter writer = new StreamWriter(saveFileDialog.FileName))
                {
                    writer.WriteLine($"Отчет за период: {startDate.Value:dd.MM.yyyy} - {endDate.Value:dd.MM.yyyy}");
                    writer.WriteLine("Дата\t\tТоваров продано\tСумма");

                    using (var context = new courseworkEntities())
                    {
                        var reportData = context.Sale
                            .Where(s => DbFunctions.TruncateTime(s.Sale_date) >= DbFunctions.TruncateTime(startDate)
                                     && DbFunctions.TruncateTime(s.Sale_date) <= DbFunctions.TruncateTime(endDate))
                            .GroupBy(s => DbFunctions.TruncateTime(s.Sale_date))
                            .Select(g => new
                            {
                                Sale_date = g.Key,
                                TotalCount = g.Sum(s => s.Count ?? 0),
                                TotalSum = g.Sum(s => s.Sum ?? 0)
                            })
                            .OrderBy(r => r.Sale_date)
                            .ToList();

                    foreach (var record in reportData)
                        {
                            writer.WriteLine($"{record.Sale_date:dd.MM.yyyy}\t{record.TotalCount}\t{record.TotalSum:C}");
                        }
                    }
                }

                MessageBox.Show("Отчет успешно сохранен.");
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.GoBack();
        }
    }
}
