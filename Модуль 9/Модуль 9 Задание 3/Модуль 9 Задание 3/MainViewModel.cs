using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
namespace TaskManager
{
    // Главная модель представления для управления списком задач
    public class MainViewModel : INotifyPropertyChanged
    {
        private TaskItem _selectedTask;
        private readonly DispatcherTimer _notificationTimer;
        // Коллекция задач для отображения в интерфейсе
        public ObservableCollection<TaskItem> Tasks { get; set; }
        // Выбранная в данный момент задача
        public TaskItem SelectedTask
        {
            get => _selectedTask;
            set { _selectedTask = value; OnPropertyChanged(); }
        }
        // Список приоритетов для привязки к ComboBox
        public Array Priorities => Enum.GetValues(typeof(TaskPriority));
        // Команда добавления новой задачи
        public ICommand AddTaskCommand { get; set; }
        // Команда удаления выбранной задачи
        public ICommand DeleteTaskCommand { get; set; }
        // Конструктор модели представления
        public MainViewModel()
        {
            Tasks = new ObservableCollection<TaskItem>
            {
                new TaskItem { Title = "Подготовить отчет", Description = "Завершить квартальный отчет", Priority = TaskPriority.Высокий, DueDate = DateTime.Now.AddHours(2), Progress = 50 },
                new TaskItem { Title = "Купить продукты", Description = "Молоко, хлеб, сыр", Priority = TaskPriority.Низкий, DueDate = DateTime.Now.AddDays(1), Progress = 0 }
            };
            AddTaskCommand = new RelayCommand(AddTask);
            DeleteTaskCommand = new RelayCommand(DeleteTask, p => SelectedTask != null);
            // Инициализация таймера для проверки дедлайнов
            _notificationTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(30)
            };
            _notificationTimer.Tick += CheckDeadlines;
            _notificationTimer.Start();
        }
        // Логика добавления задачи
        private void AddTask(object parameter)
        {
            var newTask = new TaskItem
            {
                Title = "Новая задача",
                Description = "Описание задачи...",
                DueDate = DateTime.Now.AddDays(1)
            };
            Tasks.Add(newTask);
            SelectedTask = newTask;
        }
        // Логика удаления задачи
        private void DeleteTask(object parameter)
        {
            if (SelectedTask != null)
            {
                Tasks.Remove(SelectedTask);
                SelectedTask = null;
            }
        }
        // Проверка приближающихся сроков выполнения задач
        private void CheckDeadlines(object sender, EventArgs e)
        {
            var urgentTasks = Tasks.Where(t => !t.IsCompleted &&
                                               t.DueDate > DateTime.Now &&
                                               t.DueDate <= DateTime.Now.AddHours(1)).ToList();
            foreach (var task in urgentTasks)
            {
                MessageBox.Show($"Напоминание: До окончания задачи \"{task.Title}\" осталось меньше часа!\nСрок: {task.DueDate:HH:mm}",
                                "Приближается дедлайн",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}