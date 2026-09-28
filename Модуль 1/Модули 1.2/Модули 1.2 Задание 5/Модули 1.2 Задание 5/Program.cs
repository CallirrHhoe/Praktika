class Program
{
    static void Main()
    {
        Console.Write("Введите размер массива K: ");
        int k = int.Parse(Console.ReadLine());
        string alphabet = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя";
        string vowels = "аеёиоуыэюя";
        Random rnd = new Random();
        char[] original = new char[k];
        List<char> consonantsList = new List<char>();
        for (int i = 0; i < k; i++)
        {
            char ch = alphabet[rnd.Next(alphabet.Length)];
            original[i] = ch;
            // Если буквы нет в строке гласных, добавляем её в список согласных
            if (!vowels.Contains(ch))
                consonantsList.Add(ch);
        }
        char[] consonants = consonantsList.ToArray();
        Console.WriteLine("\nИсходный массив символов:  " + string.Join(" ", original));
        Console.WriteLine("Массив только с согласными: " + string.Join(" ", consonants));
    }
}