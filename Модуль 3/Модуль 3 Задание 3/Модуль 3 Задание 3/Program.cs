using System;
class Program
{
    // Объявляем делегат, который представляет собой шаблон метода выполнения задачи
    // Принимает строку название задачи и ничего не возвращает
    delegate void Zadanie(string taskName);
    // Вариант №1: отправка уведомления
    static void SendNotification(string task)
    {
        Console.WriteLine($"Уведомление: задача \"{task}\" готова!");
    }
    // Вариант №2: логирование в журнал
    static void LogToFile(string task)
    {
        Console.WriteLine($"Журнал: запись о задаче \"{task}\" добавлена в лог.");
    }
    static void Main()
    {
        // Запрашиваем у пользователя имя задачи
        Console.Write("Введите название задачи: ");
        string task = Console.ReadLine();
        // Предлагаем выбор действия
        Console.WriteLine("Выберите действие:");
        Console.WriteLine("1 — Отправить уведомление");
        Console.WriteLine("2 — Записать в журнал");
        string choice = Console.ReadLine();
        // Переменная делегата
        Zadanie action = null;
        // В зависимости от выбора пользователя записываем в делегат нужный метод
        if (choice == "1")
            action = SendNotification; // Ссылка на метод SendNotification
        else if (choice == "2")
            action = LogToFile;        // Ссылка на метод LogToFile
        // Проверяем, был ли выбран правильный пункт, и запускаем делегат
        if (action != null)
            action(task); // Динамический вызов выбранного метода
        else
            Console.WriteLine("Неверный выбор.");
    }
}