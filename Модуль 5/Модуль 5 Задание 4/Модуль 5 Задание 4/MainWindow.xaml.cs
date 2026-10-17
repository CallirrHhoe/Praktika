using System.Windows;
namespace Wpf
{
    public partial class MainWindow : Window
    {
        private readonly ImageManager _imageManager;
        public MainWindow()
        {
            InitializeComponent();
            _imageManager = new ImageManager();
        }
        private void SelectImage_Click(object sender, RoutedEventArgs e)
        {
            var bitmap = _imageManager.LoadImage();

            if (bitmap != null)
            {
                ImgViewer.Source = bitmap;
                ZoomSlider.Value = 1.0; // Сброс масштаба
            }
        }
    }
}