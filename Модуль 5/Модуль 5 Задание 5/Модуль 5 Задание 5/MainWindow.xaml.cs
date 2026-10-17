using System.Windows;
using System.Windows.Controls;
namespace Wpf
{
    public partial class MainWindow : Window
    {
        private readonly CalculatorManager _calculatorManager;
        public MainWindow()
        {
            InitializeComponent();
            _calculatorManager = new CalculatorManager();
        }
        private void Number_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                string digit = button.Content.ToString();
                TxtDisplay.Text = _calculatorManager.InputDigit(digit, TxtDisplay.Text);
            }
        }
        private void Operator_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                _calculatorManager.SetOperator(button.Content.ToString(), TxtDisplay.Text);
            }
        }
        private void Equals_Click(object sender, RoutedEventArgs e)
        {
            var (displayText, isError, errorMessage) = _calculatorManager.Calculate(TxtDisplay.Text);

            if (isError)
            {
                MessageBox.Show(errorMessage, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            TxtDisplay.Text = displayText;
        }
        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            TxtDisplay.Text = _calculatorManager.Clear();
        }
    }
}