class Program
{
    static void Main()
    {
        Random random = new Random();
        int[] numbers = new int[15];
        Console.WriteLine("Сгенерированный массив:");
        for (int i = 0; i < numbers.Length; i++)
        {
            numbers[i] = random.Next(-50, 51);
            Console.Write(numbers[i] + " ");
        }
        Console.WriteLine();
        double sum = 0;
        int count = 0;
        foreach (int number in numbers)
        {
            if (number > 0)
            {
                sum += number;
                count++;
            }
        }
        if (count > 0)
        {
            double a = sum / count;
            Console.WriteLine($"Среднее значение положительных чисел: {a}");
        }
        else
        {
            Console.WriteLine("Положительных чисел в массиве нет.");
        }
    }
}
