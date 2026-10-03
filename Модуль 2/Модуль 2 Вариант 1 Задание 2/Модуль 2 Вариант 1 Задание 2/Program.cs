// Структура для хранения информации об одном поезде 
using System;
using System.Linq;
struct Train
{
    public string Punkt { get; set; }
    public int nomer { get; set; }
    public TimeSpan pribitie { get; set; }

    public void PrintInfo()
    {
        Console.WriteLine($"Поезд №{nomer} | Пункт: {Punkt} | Отправление: {pribitie:hh\\:mm}");
    }
}
class Program
{
    static void Main()
    {
        // Создаем массив на 3 поезда 
        Train[] poezda = new Train[3];
        Console.WriteLine("Ввод данных о 3 поездах");
        for (int i = 0; i < 3; i++)
        {
            Console.WriteLine($"\nПоезд #{i + 1}:");
            Console.Write("Пункт назначения: ");
            string punkt = Console.ReadLine();
            Console.Write("Номер поезда: ");
            int nomer = int.Parse(Console.ReadLine());
            TimeSpan vremya;
            while (true)
            {
                Console.Write("Время отправления: ");
                string vvodVremeni = Console.ReadLine();
                // Проверяем формат ЧЧ:ММ (от 00:00 до 23:59) 
                if (TimeSpan.TryParseExact(vvodVremeni, @"hh\:mm", null, out vremya))
                {
                    break; //  
                }
                Console.WriteLine("Ошибка! Введите время в 24-часовом формате ЧЧ:ММ (от 00:00 до 23:59).");
            }
            // Записываем собранные данные в массив 
            poezda[i] = new Train
            {
                Punkt = punkt,
                nomer = nomer,
                pribitie = vremya
            };
        }
        // Сортируем поезда по их номерам  
        Array.Sort(poezda, (a, b) => a.nomer.CompareTo(b.nomer));
        Console.WriteLine("\nПоезда, отсортированные по номерам");
        foreach (var p in poezda)
        {
            p.PrintInfo();
        }
        // Поиск поезда по номеру 
        Console.Write("\nВведите номер поезда для поиска: ");
        int iskomyyNomer = int.Parse(Console.ReadLine());
        bool nasheli = false;
        foreach (var p in poezda)
        {
            if (p.nomer == iskomyyNomer)
            {
                Console.WriteLine("Найден поезд:");
                p.PrintInfo();
                nasheli = true;
                break;
            }
        }
        if (!nasheli)
        {
            Console.WriteLine("Поезд с таким номером не найден.");
        }
        // Сортируем по пункту назначения, а если они одинаковые, то по времени 
        poezda = poezda
            .OrderBy(p => p.Punkt)
            .ThenBy(p => p.pribitie)
            .ToArray();
        Console.WriteLine("\nПоезда, отсортированные по назначению и времени");
        foreach (var p in poezda)
        {
            p.PrintInfo();
        }
    }
}