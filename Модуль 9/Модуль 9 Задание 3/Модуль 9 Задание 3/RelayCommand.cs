using System;
using System.Windows.Input;
namespace TaskManager
{
    // Вспомогательный класс для реализации команд в паттерне MVVM
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;
        // Конструктор команды
        public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }
        // Проверка возможности выполнения команды
        public bool CanExecute(object parameter) => _canExecute == null || _canExecute(parameter);
        // Выполнение команды
        public void Execute(object parameter) => _execute(parameter);
        // Событие изменения возможности выполнения
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
}