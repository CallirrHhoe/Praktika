using System;
// Нужен для List
using System.Collections.Generic;
class Program
{
    // Метод фильтрации принимает список и делегат Predicat
    // Predicate — это стандартный делегат C#, который принимает string и возвращает true или false
    // List<string> items — исходный список строк, который мы фильтруем
    static List<string> Filter(List<string> items, Predicate<string> uslovie)
    {
        List<string> result = new List<string>();

        foreach (var item in items)
        {
            // Проверяем каждую строку с помощью переданного делегата condition
            // item - если нету что добавлять
            if (uslovie(item))
            {
                result.Add(item); // Если проверка вернула true, добавляем в результат
            }
        }

        return result;
    }
    static void Main()
    {
        // Исходный список записей
        List<string> notes = new List<string>
        {
            "2026-10-01 Встреча по проекту",
            "2026-10-02 Покупка продуктов",
            "2026-11-05 Встреча с клиентом",
        };
        Console.WriteLine("1 — Фильтр по ключевому слову 'Встреча'");
        Console.WriteLine("2 — Фильтр по дате '2026-10'");
        Console.Write("Выберите вариант: ");
        string choice = Console.ReadLine();
        // Объявляем переменную-делегат
        Predicate<string> filter = null;
        // Присваиваем делегату условие фильтрации
        if (choice == "1")
            // Возвращает true, если строка содержит слово "Встреча"
            filter = text => text.Contains("Встреча");
        else if (choice == "2")
            // Возвращает true, если строка начинается с "2026-10"
            filter = text => text.StartsWith("2026-10");
        // Выполняем фильтрацию, если условие было выбрано
        if (filter != null)
        {
            // Передаем список и делегат в метод Filter
            var filtered = Filter(notes, filter);
            Console.WriteLine("\nРезультаты:");
            foreach (var item in filtered)
                Console.WriteLine($"- {item}");
        }
    }
}
