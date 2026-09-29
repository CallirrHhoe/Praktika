class Program
{
    static void Main()
    {
        Console.Write("Введите размер массива K: ");
        int k = int.Parse(Console.ReadLine());
        string alphabet = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя";
        string sogl = "аеёиоуыэюя";
        Random rnd = new Random();
        char[] bl = new char[k];
        List<char> a = new List<char>();
        for (int i = 0; i < k; i++)
        {
            char ch = alphabet[rnd.Next(alphabet.Length)];
            // изначальный массив
            b[i] = ch;
            // подстрока в строке
            if (!sogl.Contains(ch))
                a.Add(ch);
        }
        char[] a = a.ToArray();
        Console.WriteLine("\nИсходный массив символов:  " + string.Join(" ", b));
        Console.WriteLine("Массив только с согласными: " + string.Join(" ", a));
    }
}
