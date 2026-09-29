class Program
{
    static void Main()
    {
        Console.Write("Введите количество простых чисел K: ");
        int k = int.Parse(Console.ReadLine());
        int count = 0;
        int number = 2;
        while (count < k)
        {
            if (IsPrime(number))
            {
                Console.Write($"{number,6}");
                count++;
                // Вывод перевода строки каждые 10 чисел
                if (count % 10 == 0)
                    Console.WriteLine();
            }
            number++;
        }
        Console.WriteLine();
    }
    static bool IsPrime(int n)
    {
        if (n < 2) return false;
        for (int i = 2; i * i <= n; i++)
        {
            if (n % i == 0) return false;
        }
        return true;
    }
}
