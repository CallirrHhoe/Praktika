using System.Collections.ObjectModel;
using System.Windows.Controls;
namespace Wpf
{
    // Модель данных задачи
    public class TaskItem
    {
        public string Title { get; set; }
        public bool IsCompleted { get; set; }
    }
    public class TaskManager
    {
        public ObservableCollection<TaskItem> Tasks { get; } = new ObservableCollection<TaskItem>();
        public bool AddTask(string rawText)
        {
            if (!string.IsNullOrWhiteSpace(rawText))
            {
                Tasks.Add(new TaskItem
                {
                    Title = rawText.Trim(),
                    IsCompleted = false
                });
                return true;
            }
            return false;
        }
        public void DeleteTask(object selectedItem)
        {
            if (selectedItem is TaskItem selectedTask)
            {
                Tasks.Remove(selectedTask);
            }
        }
        public void RefreshTasks(ItemsControl itemsControl)
        {
            itemsControl.Items.Refresh();
        }
    }
}