using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
namespace TaskManager
{
    // Перечисление приоритетов задачи
    public enum TaskPriority
    {
        Низкий,
        Средний,
        Высокий
    }
    // Класс, представляющий отдельную задачу
    public class TaskItem : INotifyPropertyChanged
    {
        private string _title = string.Empty;
        private string _description = string.Empty;
        private TaskPriority _priority = TaskPriority.Средний;
        private DateTime _dueDate = DateTime.Now.AddDays(1);
        private int _progress;
        private bool _isCompleted;
        // Название задачи
        public string Title
        {
            get => _title;
            set { _title = value; OnPropertyChanged(); }
        }
        // Описание задачи
        public string Description
        {
            get => _description;
            set { _description = value; OnPropertyChanged(); }
        }
        // Приоритет задачи
        public TaskPriority Priority
        {
            get => _priority;
            set { _priority = value; OnPropertyChanged(); }
        }
        // Срок выполнения задачи
        public DateTime DueDate
        {
            get => _dueDate;
            set { _dueDate = value; OnPropertyChanged(); }
        }
        // Прогресс выполнения
        public int Progress
        {
            get => _progress;
            set
            {
                _progress = Math.Max(0, Math.Min(100, value));
                OnPropertyChanged();
                if (_progress == 100 && !IsCompleted)
                    IsCompleted = true;
                else if (_progress < 100 && IsCompleted)
                    _isCompleted = false;
            }
        }
        // Статус завершенности задачи
        public bool IsCompleted
        {
            get => _isCompleted;
            set
            {
                _isCompleted = value;
                OnPropertyChanged();
                if (_isCompleted && _progress != 100)
                    Progress = 100;
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}