using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Wpf
{
    public class ShapeManager
    {
        public Point CreateAndAddShape(
            Canvas canvas,
            MouseButtonEventArgs e,
            object colorSelectedItem,
            bool? isLineChecked,
            bool? isCircleChecked,
            bool? isSquareChecked)
        {
            Point startPoint = e.GetPosition(canvas);
            // Определяет цвет
            Brush strokeBrush = GetSelectedColor(colorSelectedItem);
            // Создает фигуру в зависимости от выбранного элемента управления
            Shape shape = BuildShape(startPoint, strokeBrush, isLineChecked, isCircleChecked, isSquareChecked);
            // Добавляет на холст, если фигура успешно создана
            if (shape != null)
            {
                canvas.Children.Add(shape);
            }
            return startPoint;
        }
        private Brush GetSelectedColor(object selectedItem)
        {
            if (selectedItem is ComboBoxItem item && item.Tag is string colorName)
            {
                try
                {
                    return (Brush)new BrushConverter().ConvertFromString(colorName);
                }
                catch
                {
                    // Возвращает цвет по умолчанию в случае ошибки парсинга
                    return Brushes.Black;
                }
            }
            return Brushes.Black;
        }
        private Shape BuildShape(Point startPoint, Brush strokeBrush, bool? isLine, bool? isCircle, bool? isSquare)
        {
            if (isLine == true)
            {
                return new Line
                {
                    X1 = startPoint.X,
                    Y1 = startPoint.Y,
                    X2 = startPoint.X + 100,
                    Y2 = startPoint.Y + 100,
                    Stroke = strokeBrush,
                    StrokeThickness = 3
                };
            }
            if (isCircle == true)
            {
                var ellipse = new Ellipse
                {
                    Width = 80,
                    Height = 80,
                    Stroke = strokeBrush,
                    StrokeThickness = 3,
                    Fill = Brushes.Transparent
                };
                Canvas.SetLeft(ellipse, startPoint.X);
                Canvas.SetTop(ellipse, startPoint.Y);
                return ellipse;
            }
            if (isSquare == true)
            {
                var rectangle = new Rectangle
                {
                    Width = 80,
                    Height = 80,
                    Stroke = strokeBrush,
                    StrokeThickness = 3,
                    Fill = Brushes.Transparent
                };
                Canvas.SetLeft(rectangle, startPoint.X);
                Canvas.SetTop(rectangle, startPoint.Y);
                return rectangle;
            }
            return null;
        }
    }
}