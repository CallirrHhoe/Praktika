using System.Windows;
using System.Windows.Controls;
using TaskApp.Database;
namespace TaskApp
{
    public partial class MainWindow : Window
    {
        private readonly TaskDb db = new TaskDb();
        public MainWindow()
        {
            InitializeComponent();
            LoadData();
        }
        private void LoadData()
        {
            GridTasks.ItemsSource = db.GetAll();
        }
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            var t = new TaskItem
            {
                Title = TxtTitle.Text,
                Desc = TxtDesc.Text,
                // Достать текст выбранного элемента из выпадающего списка
                Status = (CmbStatus.SelectedItem as ComboBoxItem)?.Content.ToString(),
                Date = TxtDate.Text
            };
            db.Add(t);
            LoadData();
        }
        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (GridTasks.SelectedItem is TaskItem item)
            {
                item.Title = TxtTitle.Text;
                item.Desc = TxtDesc.Text;
                item.Status = (CmbStatus.SelectedItem as ComboBoxItem)?.Content.ToString();
                item.Date = TxtDate.Text;
                db.Update(item);
                LoadData();
            }
        }
        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (GridTasks.SelectedItem is TaskItem item)
            {
                db.Delete(item.Id);
                LoadData();
            }
        }
    }
}