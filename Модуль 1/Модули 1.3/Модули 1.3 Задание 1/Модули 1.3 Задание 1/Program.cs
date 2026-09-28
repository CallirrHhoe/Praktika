class Program
{
    static void Main()
    {
        Console.Write("Введите числитель: ");
        int num = int.Parse(Console.ReadLine());
        Console.Write("Введите знаменатель: ");
        int k = int.Parse(Console.ReadLine());
        int p = GCD(num, k);
        Console.WriteLine($"Сокращенная дробь: {num / p} / {k / p}");
    }
    static int GCD(int a, int b)
    {
        while (b != 0)
        {
            int temp = b;
            b = a % b;
            a = temp;
        }
        return a;
    }
}