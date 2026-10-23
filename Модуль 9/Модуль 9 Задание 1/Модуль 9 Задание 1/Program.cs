using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Globalization;
// Класс, описывающий модель данных студента
public class Student
{
    public string Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int Age { get; set; }
    public string Group { get; set; }
    public double Gpa { get; set; }
}
// Класс, управляющий бизнес-логикой и сохранением данных в файл
public class StudentManager
{
    private readonly string filePath = "students.txt";
    public List<Student> Students { get; private set; } = new List<Student>();
    // Конструктор инициализирует менеджер и загружает данные из файла
    public StudentManager()
    {
        LoadFromFile();
    }
    // Метод загрузки списка студентов
    public void LoadFromFile()
    {
        if (File.Exists(filePath))
        {
            try
            {
                Students.Clear();
                string[] lines = File.ReadAllLines(filePath);
                for (int i = 0; i < lines.Length; i++)
                {
                    string[] parts = lines[i].Split(';');
                    if (parts.Length == 6)
                    {
                        Student s = new Student();
                        s.Id = parts[0];
                        s.FirstName = parts[1];
                        s.LastName = parts[2];
                        s.Age = int.Parse(parts[3]);
                        s.Group = parts[4];
                        s.Gpa = double.Parse(parts[5], CultureInfo.InvariantCulture);
                        Students.Add(s);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка загрузки: " + ex.Message);
            }
        }
    }
    // Метод сохранения списка студентов
    public void SaveToFile()
    {
        try
        {
            List<string> lines = new List<string>();
            for (int i = 0; i < Students.Count; i++)
            {
                Student s = Students[i];
                string line = s.Id + ";" + s.FirstName + ";" + s.LastName + ";" + s.Age + ";" + s.Group + ";" + s.Gpa.ToString(CultureInfo.InvariantCulture);
                lines.Add(line);
            }
            File.WriteAllLines(filePath, lines);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка сохранения: " + ex.Message);
        }
    }
    // Метод добавления нового студента с проверкой уникальности ID
    public bool AddStudent(Student student)
    {
        for (int i = 0; i < Students.Count; i++)
        {
            if (Students[i].Id == student.Id) return false;
        }
        Students.Add(student);
        SaveToFile();
        return true;
    }
    // Метод редактирования существующего студента
    public bool EditStudent(string id, string firstName, string lastName, int? age, string group, double? gpa)
    {
        Student student = null;
        for (int i = 0; i < Students.Count; i++)
        {
            if (Students[i].Id == id)
            {
                student = Students[i];
                break;
            }
        }
        if (student == null) return false;
        if (!string.IsNullOrWhiteSpace(firstName)) student.FirstName = firstName;
        if (!string.IsNullOrWhiteSpace(lastName)) student.LastName = lastName;
        if (age.HasValue) student.Age = age.Value;
        if (!string.IsNullOrWhiteSpace(group)) student.Group = group;
        if (gpa.HasValue) student.Gpa = gpa.Value;
        SaveToFile();
        return true;
    }
    // Метод удаления студента по ID
    public bool DeleteStudent(string id)
    {
        Student student = null;
        for (int i = 0; i < Students.Count; i++)
        {
            if (Students[i].Id == id)
            {
                student = Students[i];
                break;
            }
        }
        if (student == null) return false;
        Students.Remove(student);
        SaveToFile();
        return true;
    }
    // Метод поиска студентов по тексту
    public List<Student> Search(string query)
    {
        query = query.ToLower();
        List<Student> result = new List<Student>();
        for (int i = 0; i < Students.Count; i++)
        {
            Student s = Students[i];
            if (s.FirstName.ToLower().Contains(query) || s.LastName.ToLower().Contains(query) || s.Group.ToLower().Contains(query))
            {
                result.Add(s);
            }
        }
        return result;
    }
    // Метод сортировки студентов по выбранному критерию
    public List<Student> Sort(int choice)
    {
        if (choice == 1) return Students.OrderBy(s => s.LastName).ToList();
        if (choice == 2) return Students.OrderBy(s => s.Age).ToList();
        if (choice == 3) return Students.OrderByDescending(s => s.Gpa).ToList();
        return Students;
    }
}
// Класс пользовательского консольного интерфейса
public class ConsoleUI
{
    private StudentManager manager = new StudentManager();
    // Главный цикл работы консольного меню
    public void Run()
    {
        bool running = true;
        while (running)
        {
            Console.WriteLine("\nМеню управления студентами");
            Console.WriteLine("1. Показать всех студентов");
            Console.WriteLine("2. Добавить студента");
            Console.WriteLine("3. Редактировать студента");
            Console.WriteLine("4. Удалить студента");
            Console.WriteLine("5. Поиск студента");
            Console.WriteLine("6. Сортировка студентов");
            Console.WriteLine("7. Выход");
            Console.Write("Выберите пункт меню: ");
            string choice = Console.ReadLine();
            if (choice == "1") Display(manager.Students);
            else if (choice == "2") Add();
            else if (choice == "3") Edit();
            else if (choice == "4") Delete();
            else if (choice == "5") Search();
            else if (choice == "6") Sort();
            else if (choice == "7") running = false;
            else Console.WriteLine("Неверный выбор.");
        }
    }
    // Вспомогательный метод для вывода списка в виде таблицы
    private void Display(List<Student> list)
    {
        if (list.Count == 0)
        {
            Console.WriteLine("Список пуст.");
            return;
        }
        Console.WriteLine("ID    | Фамилия         | Имя          | Возраст  | Группа     | Ср. балл");
        Console.WriteLine(new string('-', 70));
        for (int i = 0; i < list.Count; i++)
        {
            Student s = list[i];
            Console.WriteLine($"{s.Id,-5} | {s.LastName,-15} | {s.FirstName,-12} | {s.Age,-8} | {s.Group,-10} | {s.Gpa,-8:F2}");
        }
    }
    // Обработка добавления студента
    private void Add()
    {
        Console.Write("ID: "); string id = Console.ReadLine();
        Console.Write("Имя: "); string fName = Console.ReadLine();
        Console.Write("Фамилия: "); string lName = Console.ReadLine();
        Console.Write("Возраст: "); int age;
        if (!int.TryParse(Console.ReadLine(), out age)) { Console.WriteLine("Ошибка возраста"); return; }
        Console.Write("Группа: "); string group = Console.ReadLine();
        Console.Write("Ср. балл: "); double gpa;
        string gpaInput = Console.ReadLine();
        if (!double.TryParse(gpaInput, NumberStyles.Any, CultureInfo.InvariantCulture, out gpa) && !double.TryParse(gpaInput, out gpa)) { Console.WriteLine("Ошибка балла"); return; }
        Student student = new Student { Id = id, FirstName = fName, LastName = lName, Age = age, Group = group, Gpa = gpa };
        if (manager.AddStudent(student)) Console.WriteLine("Добавлено!");
        else Console.WriteLine("ID уже существует!");
    }
    // Обработка редактирования студента
    private void Edit()
    {
        Console.Write("ID студента для редактирования: "); string id = Console.ReadLine();
        Console.Write("Новое имя: "); string fName = Console.ReadLine();
        Console.Write("Новая фамилия: "); string lName = Console.ReadLine();
        Console.Write("Новый возраст: "); int a;
        int? age = int.TryParse(Console.ReadLine(), out a) ? (int?)a : null;
        Console.Write("Новая группа: "); string group = Console.ReadLine();
        Console.Write("Новый балл: "); double g;
        string gpaInput = Console.ReadLine();
        double? gpa = double.TryParse(gpaInput, NumberStyles.Any, CultureInfo.InvariantCulture, out g) || double.TryParse(gpaInput, out g) ? (double?)g : null;
        if (manager.EditStudent(id, fName, lName, age, group, gpa)) Console.WriteLine("Обновлено!");
        else Console.WriteLine("Не найдено!");
    }
    // Обработка удаления студента
    private void Delete()
    {
        Console.Write("ID для удаления: "); string id = Console.ReadLine();
        if (manager.DeleteStudent(id)) Console.WriteLine("Удалено!");
        else Console.WriteLine("Не найдено!");
    }
    // Обработка поиска
    private void Search()
    {
        Console.Write("Текст поиска: "); string q = Console.ReadLine();
        Display(manager.Search(q));
    }
    // Обработка сортировки
    private void Sort()
    {
        Console.WriteLine("1. Фамилия, 2. Возраст, 3. Ср. балл");
        int c;
        if (int.TryParse(Console.ReadLine(), out c)) Display(manager.Sort(c));
    }
}
class Program
{
    static void Main(string[] args)
    {
        ConsoleUI ui = new ConsoleUI();
        ui.Run();
    }
}