using System.IO;
using System.Windows;
using System.Windows.Controls;
namespace FileManagerApp
{
    // Главное окно
    public partial class MainWindow : Window
    {
        private readonly Manager _manager;
        private string _currentPath;
        public MainWindow()
        {
            InitializeComponent();
            _manager = new Manager();
            LoadDrives();
        }
        // Загрузка дисков в дерево
        private void LoadDrives()
        {
            FoldersTreeView.Items.Clear();
            foreach (var drive in DriveInfo.GetDrives())
            {
                var item = new TreeViewItem { Header = drive.Name, Tag = drive.Name };
                item.Items.Add(null);
                item.Expanded += Folder_Expanded;
                FoldersTreeView.Items.Add(item);
            }
        }
        // Подгрузка подпапок в дереве
        private void Folder_Expanded(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is TreeViewItem item && item.Items.Count == 1 && item.Items[0] == null)
            {
                item.Items.Clear();
                try
                {
                    var dirInfo = new DirectoryInfo(item.Tag.ToString());
                    foreach (var subDir in dirInfo.GetDirectories())
                    {
                        var subItem = new TreeViewItem { Header = subDir.Name, Tag = subDir.FullName };
                        subItem.Items.Add(null);
                        subItem.Expanded += Folder_Expanded;
                        item.Items.Add(subItem);
                    }
                }
                catch { }
            }
        }
        // Событие выбора папки в дереве
        private void Selest(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (FoldersTreeView.SelectedItem is TreeViewItem item && item.Tag != null)
            {
                _currentPath = item.Tag.ToString();
                CurrentPathTextBox.Text = _currentPath;
                RefreshFilesList();
            }
        }
        // Обновить список файлов
        private void RefreshFilesList()
        {
            if (!string.IsNullOrEmpty(_currentPath))
            {
                FilesListView.ItemsSource = _manager.GetDirectoryContents(_currentPath);
            }
        }
        // Двойной клик по элементу
        private void MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (FilesListView.SelectedItem is Item selectedItem && selectedItem.IsDirectory)
            {
                _currentPath = selectedItem.FullPath;
                CurrentPathTextBox.Text = _currentPath;
                RefreshFilesList();
            }
        }
        // Кнопка копирования
        private void Copy_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Функция копирования требует указания пути назначения.");
        }
        // Кнопка перемещения
        private void Move_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Функция перемещения требует указания пути назначения.");
        }
        // Кнопка удаления
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (FilesListView.SelectedItem is Item selectedItem)
            {
                var result = MessageBox.Show($"Удалить {selectedItem.Name}?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    _manager.DeleteItem(selectedItem.FullPath, selectedItem.IsDirectory);
                    RefreshFilesList();
                }
            }
        }
    }
}