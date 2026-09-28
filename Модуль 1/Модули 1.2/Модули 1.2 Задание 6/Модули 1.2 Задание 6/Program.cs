class Program
{
    static void Main()
    {
        Random rnd = new Random();
        double[] array = new double[10];
        // Заполнение случайными вещественными числами от -10 до 10
        for (int i = 0; i < array.Length; i++)
            array[i] = rnd.NextDouble() * 20.0 - 10.0;
        Console.WriteLine("Исходный массив:");
        for (int i = 0; i < array.Length; i++)
            Console.WriteLine($"[Индекс {i}] = {array[i]:F2}");
        // Формирование массива индексов, отсортированных по значению элементов
        int[] a = Enumerable.Range(0, array.Length)
            .OrderBy(i => array[i])
            .ToArray();
        Console.WriteLine("\nМассив индексов по возрастанию элементов:");
        Console.WriteLine(string.Join(" ", a));
    }
}