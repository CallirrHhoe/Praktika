using System.Windows;
using System.Windows.Controls;
using LibraryApp.Database;
namespace LibraryApp
{
    public partial class MainWindow : Window
    {
        private readonly LibraryDb db = new LibraryDb();

        public MainWindow()
        {
            InitializeComponent();
            Find();
        }
        private void Find()
        {
            GridBooks.ItemsSource = db.Search(TxtSearch.Text);
        }
        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            Find();
        }
        private void BtnRent_Click(object sender, RoutedEventArgs e)
        {
            if (GridBooks.SelectedItem is Book b && b.Available)
            {
                db.Rent(b.Id, false);
                Find();
            }
        }
        private void BtnReturn_Click(object sender, RoutedEventArgs e)
        {
            if (GridBooks.SelectedItem is Book b)
            {
                db.Rent(b.Id, true);
                Find();
            }
        }
    }
}