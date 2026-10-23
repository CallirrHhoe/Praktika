using System.Data;
using SQLitePCL;
using System.Windows;
using System.Windows.Controls;
namespace ProjectTaskApp
{
    public partial class MainWindow : Window
    {
        Database db = new Database("app.db");
        ProjectRepository projects;
        EmployeeRepository employees;
        TaskRepository tasks;
        public MainWindow()
        {
            Batteries.Init();
            InitializeComponent();
            projects = new ProjectRepository(db);
            employees = new EmployeeRepository(db);
            tasks = new TaskRepository(db);
            RefreshData();
        }
        void RefreshData()
        {
            DgProjects.ItemsSource = projects.GetAll().DefaultView;
            DgEmployees.ItemsSource = employees.GetAll().DefaultView;
            DgTasks.ItemsSource = tasks.GetAll().DefaultView;
            CmbTaskProj.ItemsSource = projects.GetAll().DefaultView;
            CmbTaskEmp.ItemsSource = employees.GetAll().DefaultView;
        }
        int GetSelectedId(DataGrid dg)
        {
            return dg.SelectedItem is DataRowView r ? System.Convert.ToInt32(r["Номер"]) : -1;
        }
        private void AddProj_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(TxtProj.Text))
            {
                projects.Add(TxtProj.Text.Trim());
                TxtProj.Clear();
                RefreshData();
            }
        }
        private void DelProj_Click(object sender, RoutedEventArgs e)
        {
            int id = GetSelectedId(DgProjects);
            if (id != -1)
            {
                projects.Delete(id);
                RefreshData();
            }
        }
        private void AddEmp_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(TxtEmp.Text))
            {
                employees.Add(TxtEmp.Text.Trim());
                TxtEmp.Clear();
                RefreshData();
            }
        }
        private void DelEmp_Click(object sender, RoutedEventArgs e)
        {
            int id = GetSelectedId(DgEmployees);
            if (id != -1)
            {
                employees.Delete(id);
                RefreshData();
            }
        }
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
        private void DelTask_Click(object sender, RoutedEventArgs e)
        {
            int id = GetSelectedId(DgTasks);
            if (id != -1)
            {
                tasks.Delete(id);
                RefreshData();
            }
        }
        private void Report_Click(object sender, RoutedEventArgs e)
        {
            var dt = tasks.GetAll();
            string report = "ОТЧЕТ О СТАТУСЕ ПРОЕКТОВ И ЗАДАЧ\n\n";
            foreach (DataRow row in dt.Rows)
            {
                report += $"Задача: {row["Задача"]} | Проект: {row["Проект"]} | Исполнитель: {row["Исполнитель"]} | Статус: {row["Статус"]}\n";
            }
            TxtReport.Text = report;
        }
    }
}