class Program
{
    static void Main()
    {
        Random rnd = new Random();
        double[] array = new double[10];
        // Заполнение случайными вещественными числами от -10 до 10
        for (int i = 0; i < array.Length; i++)
            // Генерация целого числа от -10 до 10 и прибавление дробной части
            array[i] = rnd.Next(-10, 10) + rnd.NextDouble();
        Console.WriteLine("Исходный массив:");
        for (int i = 0; i < array.Length; i++)
            Console.WriteLine($"[Индекс {i}] = {array[i]:F2}");
        // Формирование массива по индексам
        int[] a = Enumerable.Range(0, array.Length)
            .OrderBy(i => array[i])
            .ToArray();
        Console.WriteLine("\nМассив индексов по возрастанию элементов:");
        Console.WriteLine(string.Join(" ", a));
    }
}