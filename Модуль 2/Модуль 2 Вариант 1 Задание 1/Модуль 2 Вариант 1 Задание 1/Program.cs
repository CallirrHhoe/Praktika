using System;
class Student
{
    // поля доступные для чтения и записи 
    public string FirstName { get; set; }    // Имя 
    public string LastName { get; set; }     // Фамилия 
    public int Age { get; set; }             // Возраст 
    public double AverageGrade { get; set; } // Средний балл 
    // Конструктор: вызывается при создании нового объекта и заполняет его свойства 
    public Student(string firstName, string lastName, int age, double averageGrade)
    {
        FirstName = firstName;
        LastName = lastName;
        Age = age;
        AverageGrade = averageGrade;
    }
    // Метод для вывода информации о студенте  
    public void PrintInfo()
    {
        Console.WriteLine($"{LastName} {FirstName}, Возраст: {Age}, Средний балл: {AverageGrade:F1}");
    }
}
class Program
{
    static void Main()
    {
        // Создаём массив из трёх объектов класса Student 
        Student[] students = new Student[]
        {
            new Student("Артём", "Данилевич", 18, 7.9),
            new Student("Артём", "Политов", 17, 8),
            new Student("Евгений", "Манушко", 17, 7.7)
        };
        Console.WriteLine("Список студентов");
        // Пвызываем метод вывода для каждого студента 
        foreach (var student in students)
        {
            student.PrintInfo();
        }
    }
}