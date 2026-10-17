namespace Wpf
{
    public class CalculatorManager
    {
        private double _result = 0;
        private string _selectedOperator = string.Empty;
        private bool _isNewEntry = true;
        public string InputDigit(string digit, string Text)
        {
            if (_isNewEntry)
            {
                _isNewEntry = false;
                return digit;
            }
            else
            {
                if (Text == "0")
                    return digit;
                else
                    return Text + digit;
            }
        }
        public void SetOperator(string op, string Text)
        {
            if (double.TryParse(Text, out double value))
            {
                _result = value;
            }
            _selectedOperator = op;
            _isNewEntry = true;
        }
        public (string displayText, bool isError, string errorMessage) Calculate(string Text)
        {
            if (double.TryParse(Text, out double secondValue))
            {
                switch (_selectedOperator)
                {
                    case "+":
                        _result += secondValue;
                        break;
                    case "-":
                        _result -= secondValue;
                        break;
                    case "*":
                        _result *= secondValue;
                        break;
                    case "/":
                        if (secondValue != 0)
                        {
                            _result /= secondValue;
                        }
                        else
                        {
                            Clear();
                            return ("0", true, "Деление на ноль невозможно!");
                        }
                        break;
                }
                _isNewEntry = true;
                return (_result.ToString(), false, string.Empty);
            }

            return (Text, false, string.Empty);
        }
        public string Clear()
        {
            _result = 0;
            _selectedOperator = string.Empty;
            _isNewEntry = true;
            return "0";
        }
    }
}