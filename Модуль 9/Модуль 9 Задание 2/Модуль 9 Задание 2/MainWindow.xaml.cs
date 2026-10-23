using System.Data;
using SQLitePCL;
using System.Windows;
using System.Windows.Controls;
namespace ProjectTaskApp
{
    public partial class MainWindow : Window
    {
        // Инициализация базы данных и репозиториев
        Database db = new Database("app.db");
        ProjectRepository projects;
        EmployeeRepository employees;
        TaskRepository tasks;
        public MainWindow()
        {
            Batteries.Init(); // Инициализация SQLitePCL для загрузки нативных библиотек
            InitializeComponent();
            // Создание экземпляров репозиториев для работы с таблицами БД
            projects = new ProjectRepository(db);
            employees = new EmployeeRepository(db);
            tasks = new TaskRepository(db);
            RefreshData(); // Загрузка данных в интерфейс при запуске программы
        }
        // Метод для обновления всех таблиц и выпадающих списков на форме
        void RefreshData()
        {
            DgProjects.ItemsSource = projects.GetAll().DefaultView;
            DgEmployees.ItemsSource = employees.GetAll().DefaultView;
            DgTasks.ItemsSource = tasks.GetAll().DefaultView;
            CmbTaskProj.ItemsSource = projects.GetAll().DefaultView;
            CmbTaskEmp.ItemsSource = employees.GetAll().DefaultView;
        }
        // Получение Id выбранной строки в таблице (-1, если ничего не выбрано)
        int GetSelectedId(DataGrid dg)
        {
            return dg.SelectedItem is DataRowView r ? System.Convert.ToInt32(r["Номер"]) : -1;
        }
        // Обработчик кнопки добавления проекта
        private void AddProj_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(TxtProj.Text))
            {
                projects.Add(TxtProj.Text.Trim());
                TxtProj.Clear();
                RefreshData();
            }
        }
        // Обработчик кнопки удаления проекта
        private void DelProj_Click(object sender, RoutedEventArgs e)
        {
            int id = GetSelectedId(DgProjects);
            if (id != -1)
            {
                projects.Delete(id);
                RefreshData();
            }
        }
        // Обработчик кнопки добавления сотрудника
        private void AddEmp_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(TxtEmp.Text))
            {
                employees.Add(TxtEmp.Text.Trim());
                TxtEmp.Clear();
                RefreshData();
            }
        }
        // Обработчик кнопки удаления сотрудника
        private void DelEmp_Click(object sender, RoutedEventArgs e)
        {
            int id = GetSelectedId(DgEmployees);
            if (id != -1)
            {
                employees.Delete(id);
                RefreshData();
            }
        }
        // Обработчик кнопки добавления задачи
        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(TxtTask.Text) && CmbTaskProj.SelectedValue != null && CmbTaskEmp.SelectedValue != null)
            {
                string status = (CmbTaskStatus.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "Новая";
                tasks.Add(TxtTask.Text.Trim(), System.Convert.ToInt32(CmbTaskProj.SelectedValue), System.Convert.ToInt32(CmbTaskEmp.SelectedValue), status);
                TxtTask.Clear();
                RefreshData();
            }
        }
        // Обработчик кнопки удаления задачи
        private void DelTask_Click(object sender, RoutedEventArgs e)
        {
            int id = GetSelectedId(DgTasks);
            if (id != -1)
            {
                tasks.Delete(id);
                RefreshData();
            }
        }
        // Генерация текстового отчета по задачам и вывод в поле отчетов
        private void Report_Click(object sender, RoutedEventArgs e)
        {
            var dt = tasks.GetAll();
            string report = "=== ОТЧЕТ О СТАТУСЕ ПРОЕКТОВ И ЗАДАЧ ===\n\n";
            foreach (DataRow row in dt.Rows)
            {
                report += $"Задача: {row["Задача"]} | Проект: {row["Проект"]} | Исполнитель: {row["Исполнитель"]} | Статус: {row["Статус"]}\n";
            }
            TxtReport.Text = report;
        }
    }
}