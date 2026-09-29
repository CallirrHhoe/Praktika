class Program
{
    static void Main()
    {
        Console.Write("Введите размер массива K: ");
        int k = int.Parse(Console.ReadLine());
        string a = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя";
        string vowels = "аеёиоуыэюя";
        Random rnd = new Random();
        char[] b = new char[k];
        List<char> s = new List<char>();
        for (int i = 0; i < k; i++)
        {
            char ch = a[rnd.Next(a.Length)];
            b[i] = ch;
            // Если буквы нет в строке гласных, добавляем её в список согласных
            if (!vowels.Contains(ch))
                s.Add(ch);
        }
        char[] consonants = s.ToArray();
        Console.WriteLine("\nИсходный массив символов:  " + string.Join(" ", a));
        Console.WriteLine("Массив только с согласными: " + string.Join(" ", s));
    }
}