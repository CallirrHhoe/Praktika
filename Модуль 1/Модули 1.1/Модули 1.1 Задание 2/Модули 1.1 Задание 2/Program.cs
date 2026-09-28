class Program
{
    static void Main()
    {
        Console.Write("Введите первое число: ");
        int a = int.Parse(Console.ReadLine());
        Console.Write("Введите второе число: ");
        int b = int.Parse(Console.ReadLine());
        int sum = a + b;
        Console.WriteLine($"Сумма: {sum}");
    }
}