using System.Windows;
namespace Wpf
{
    public partial class MainWindow : Window
    {
        private readonly FileManager _fileManager;

        public MainWindow()
        {
            InitializeComponent();
            _fileManager = new FileManager();
        }
        private void Open_Click(object sender, RoutedEventArgs e)
        {
            string fileContent = _fileManager.OpenFile();
            if (fileContent != null)
            {
                TxtEditor.Text = fileContent;
            }
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            bool isSaved = _fileManager.SaveFile(TxtEditor.Text);

            if (isSaved)
            {
                MessageBox.Show("Файл успешно сохранен!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}