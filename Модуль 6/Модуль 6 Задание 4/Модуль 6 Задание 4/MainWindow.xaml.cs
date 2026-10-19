using System;
using System.Windows;
using System.Windows.Controls;
using FinanceApp.Database;
namespace FinanceApp
{
    public partial class MainWindow : Window
    {
        private readonly FinanceDb db = new FinanceDb();
        public MainWindow()
        {
            InitializeComponent();
            LoadData();
        }
        private void LoadData()
        {
            GridFinance.ItemsSource = db.GetAll();
        }
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            double.TryParse(TxtSum.Text, out double sum);

            var x = new Item
            {
                Type = (CmbType.SelectedItem as ComboBoxItem)?.Content.ToString(),
                Sum = sum,
                Cat = TxtCat.Text,
                Date = TxtDate.Text
            };

            db.Add(x);
            LoadData();
        }
        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (GridFinance.SelectedItem is Item item)
            {
                double.TryParse(TxtSum.Text, out double sum);

                item.Type = (CmbType.SelectedItem as ComboBoxItem)?.Content.ToString();
                item.Sum = sum;
                item.Cat = TxtCat.Text;
                item.Date = TxtDate.Text;

                db.Update(item);
                LoadData();
            }
        }
        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (GridFinance.SelectedItem is Item item)
            {
                db.Delete(item.Id);
                LoadData();
            }
        }
    }
}