using System.Windows;
namespace Wpf
{
    public partial class MainWindow : Window
    {
        private readonly TaskManager _taskManager;
        public MainWindow()
        {
            InitializeComponent();

            _taskManager = new TaskManager();
            LbTasks.ItemsSource = _taskManager.Tasks;
        }
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (_taskManager.AddTask(TxtTask.Text))
            {
                TxtTask.Clear();
            }
        }
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            _taskManager.DeleteTask(LbTasks.SelectedItem);
        }
        private void TaskStateChanged(object sender, RoutedEventArgs e)
        {
            _taskManager.RefreshTasks(LbTasks);
        }
    }
}