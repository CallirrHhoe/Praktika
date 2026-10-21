using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
namespace FileManagerApp
{
    public partial class MainWindow : Window
    {
        private readonly Manager _manager = new Manager();
        private string _leftPath = @"C:\";
        private string _rightPath = @"C:\";
        private readonly Stack<string> _leftBackHistory = new Stack<string>();
        private readonly Stack<string> _leftForwardHistory = new Stack<string>();
        private readonly Stack<string> _rightBackHistory = new Stack<string>();
        private readonly Stack<string> _rightForwardHistory = new Stack<string>();
        private string _activeSide = "Left";
        private string _sortColumn = "Name";
        private bool _ascending = true;
        private string _copiedPath = null;
        private bool _isCopiedDirectory = false;
        private bool _isMoveOperation = false;
        public MainWindow()
        {
            InitializeComponent();
            InitDrives();
            LoadLeft(_leftPath, false);
            LoadRight(_rightPath, false);
        }
        private void InitDrives()
        {
            var drives = DriveInfo.GetDrives();
            LeftDriveCombo.ItemsSource = drives;
            LeftDriveCombo.DisplayMemberPath = "Name";
            LeftDriveCombo.SelectedIndex = 0;
            RightDriveCombo.ItemsSource = drives;
            RightDriveCombo.DisplayMemberPath = "Name";
            RightDriveCombo.SelectedIndex = 0;
        }
        private void LeftDriveCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LeftDriveCombo.SelectedItem is DriveInfo drive)
            {
                NavigateLeft(drive.Name);
            }
        }
        private void RightDriveCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RightDriveCombo.SelectedItem is DriveInfo drive)
            {
                NavigateRight(drive.Name);
            }
        }
        private void LeftListView_GotFocus(object sender, RoutedEventArgs e)
        {
            _activeSide = "Left";
        }
        private void RightListView_GotFocus(object sender, RoutedEventArgs e)
        {
            _activeSide = "Right";
        }
        private void NavigateLeft(string newPath)
        {
            if (_leftPath != newPath)
            {
                _leftBackHistory.Push(_leftPath);
                _leftForwardHistory.Clear();
            }
            LoadLeft(newPath, true);
        }
        private void NavigateRight(string newPath)
        {
            if (_rightPath != newPath)
            {
                _rightBackHistory.Push(_rightPath);
                _rightForwardHistory.Clear();
            }
            LoadRight(newPath, true);
        }
        private void LeftBack_Click(object sender, RoutedEventArgs e)
        {
            if (_leftBackHistory.Count > 0)
            {
                _leftForwardHistory.Push(_leftPath);
                string prev = _leftBackHistory.Pop();
                LoadLeft(prev, false);
            }
        }
        private void LeftForward_Click(object sender, RoutedEventArgs e)
        {
            if (_leftForwardHistory.Count > 0)
            {
                _leftBackHistory.Push(_leftPath);
                string next = _leftForwardHistory.Pop();
                LoadLeft(next, false);
            }
        }
        private void RightBack_Click(object sender, RoutedEventArgs e)
        {
            if (_rightBackHistory.Count > 0)
            {
                _rightForwardHistory.Push(_rightPath);
                string prev = _rightBackHistory.Pop();
                LoadRight(prev, false);
            }
        }
        private void RightForward_Click(object sender, RoutedEventArgs e)
        {
            if (_rightForwardHistory.Count > 0)
            {
                _rightBackHistory.Push(_rightPath);
                string next = _rightForwardHistory.Pop();
                LoadRight(next, false);
            }
        }
        private void LoadLeft(string path, bool pushHistory)
        {
            try
            {
                if (pushHistory && _leftPath != path)
                {
                    _leftBackHistory.Push(_leftPath);
                    _leftForwardHistory.Clear();
                }
                _leftPath = path;
                LeftPathBox.Text = _leftPath;
                LeftListView.ItemsSource = _manager.GetContents(_leftPath, _sortColumn, _ascending);
            }
            catch { }
        }
        private void LoadRight(string path, bool pushHistory)
        {
            try
            {
                if (pushHistory && _rightPath != path)
                {
                    _rightBackHistory.Push(_rightPath);
                    _rightForwardHistory.Clear();
                }
                _rightPath = path;
                RightPathBox.Text = _rightPath;
                RightListView.ItemsSource = _manager.GetContents(_rightPath, _sortColumn, _ascending);
            }
            catch { }
        }
        private void LeftListView_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            _activeSide = "Left";
            var item = LeftListView.SelectedItem as Item;
            if (item != null && item.IsDirectory)
            {
                NavigateLeft(item.FullPath);
            }
        }
        private void RightListView_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            _activeSide = "Right";
            var item = RightListView.SelectedItem as Item;
            if (item != null && item.IsDirectory)
            {
                NavigateRight(item.FullPath);
            }
        }
        private void GridViewColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            var header = e.OriginalSource as GridViewColumnHeader;
            if (header != null && header.Column != null)
            {
                string headerText = header.Column.Header.ToString();
                string newSort = "Name";
                if (headerText == "Размер") newSort = "Size";
                if (headerText == "Дата") newSort = "Date";
                if (_sortColumn == newSort)
                {
                    _ascending = !_ascending;
                }
                else
                {
                    _sortColumn = newSort;
                    _ascending = true;
                }
                LoadLeft(_leftPath, false);
                LoadRight(_rightPath, false);
            }
        }
        private void Copy_Click(object sender, RoutedEventArgs e)
        {
            Item selected = _activeSide == "Left" ? LeftListView.SelectedItem as Item : RightListView.SelectedItem as Item;
            if (selected != null)
            {
                _copiedPath = selected.FullPath;
                _isCopiedDirectory = selected.IsDirectory;
                _isMoveOperation = false;
                MessageBox.Show($"Скопировано: {selected.Name}", "Буфер", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private void Move_Click(object sender, RoutedEventArgs e)
        {
            Item selected = _activeSide == "Left" ? LeftListView.SelectedItem as Item : RightListView.SelectedItem as Item;
            if (selected != null)
            {
                _copiedPath = selected.FullPath;
                _isCopiedDirectory = selected.IsDirectory;
                _isMoveOperation = true;
                MessageBox.Show($"Вырезано: {selected.Name}", "Буфер", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private void Paste_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_copiedPath))
            {
                MessageBox.Show("Буфер обмена пуст!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            string destDir = _activeSide == "Left" ? _leftPath : _rightPath;
            try
            {
                if (_isMoveOperation)
                {
                    _manager.Move(_copiedPath, destDir, _isCopiedDirectory);
                    _copiedPath = null;
                }
                else
                {
                    _manager.Copy(_copiedPath, destDir, _isCopiedDirectory);
                }
                LoadLeft(_leftPath, false);
                LoadRight(_rightPath, false);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            Item selected = _activeSide == "Left" ? LeftListView.SelectedItem as Item : RightListView.SelectedItem as Item;
            if (selected != null)
            {
                var res = MessageBox.Show($"Удалить {selected.Name}?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (res == MessageBoxResult.Yes)
                {
                    _manager.Delete(selected.FullPath, selected.IsDirectory);
                    LoadLeft(_leftPath, false);
                    LoadRight(_rightPath, false);
                }
            }
        }
    }
}