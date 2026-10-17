using System.Windows;
using System.Windows.Input;
namespace Wpf
{
    public partial class MainWindow : Window
    {
        private Point _startPoint;
        private readonly ShapeManager _shapeManager;

        public MainWindow()
        {
            InitializeComponent();
            _shapeManager = new ShapeManager();
        }
        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                // Вызывает логику создания фигуры из отдельного класса
                _startPoint = _shapeManager.CreateAndAddShape(
                    DrawingCanvas,
                    // объект с ссылками
                    e,
                    CbColor.SelectedItem,
                    RbLine.IsChecked,
                    RbCircle.IsChecked,
                    RbSquare.IsChecked
                );
            }
        }
        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            DrawingCanvas.Children.Clear();
        }
    }
}