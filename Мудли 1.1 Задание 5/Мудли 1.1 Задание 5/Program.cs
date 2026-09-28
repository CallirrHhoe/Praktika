class Program
{
    static void Main()
    {
        Console.Write("Введите число: ");
        int number = int.Parse(Console.ReadLine());
        bool a = true;
        if (number <= 1)
        {
            a = false;
        }
        else
        {
            for (int i = 2; i * i <= number; i++)
            {
                if (number % i == 0)
                {
                    a = false;
                    break;
                }
            }
        }
        if (a)
        {
            Console.WriteLine($"Число {number} — простое.");
        }
        else
        {
            Console.WriteLine($"Число {number} — не является простым.");
        }
    }
}