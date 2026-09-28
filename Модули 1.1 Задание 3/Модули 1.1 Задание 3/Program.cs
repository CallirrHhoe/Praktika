class Program
{
    static void Main()
    {
        Console.Write("Введите строку: ");
        string input = Console.ReadLine();
        char[] chars = input.ToCharArray();
        Array.Reverse(chars);
        string reversed = new string(chars);
        Console.WriteLine($"Обратная строка: {reversed}");
    }
}