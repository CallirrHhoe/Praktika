using System.Windows;
using System.Windows.Controls;
using Sotrudnik.Database;
namespace Sotrudnik
{
    public partial class MainWindow : Window
    {
        private readonly SotrudnikiDB db = new SotrudnikiDB();

        public MainWindow()
        {
            InitializeComponent();
            LoadData();
        }
        private void LoadData()
        {
            GridSotrudniki.ItemsSource = db.GetAll();
        }
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            // Переменная с полями
            var emp = new Sotrudniki
            {
                Name = TxtName.Text,
                Post = TxtPost.Text,
                Dept = TxtDept.Text,
                Date = TxtDate.Text
            };
            db.Add(emp);
            LoadData();
        }
        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (GridSotrudniki.SelectedItem is Sotrudniki emp)
            {
                emp.Name = TxtName.Text;
                emp.Post = TxtPost.Text;
                emp.Dept = TxtDept.Text;
                emp.Date = TxtDate.Text;
                db.Update(emp);
                LoadData();
            }
        }
        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (GridSotrudniki.SelectedItem is Sotrudniki emp)
            {
                db.Delete(emp.Id);
                LoadData();
            }
        }
    }
}