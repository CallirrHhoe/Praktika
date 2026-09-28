class Program
{
    static void Main()
    {
        Console.Write("Введите размер массива N: ");
        int n = int.Parse(Console.ReadLine());
        double[] array = new double[n];
        for (int i = 0; i < n; i++)
        {
            Console.Write($"Элемент [{i}]: ");
            array[i] = double.Parse(Console.ReadLine());
        }
        // Поиск максимального по модулю элемента
        double maxAbs = Math.Abs(array[0]);
        for (int i = 1; i < n; i++)
        {
            if (Math.Abs(array[i]) > maxAbs)
                maxAbs = Math.Abs(array[i]);
        }
        if (maxAbs == 0)
        {
            Console.WriteLine("Все элементы равны 0. Деление на ноль невозможно.");
            return;
        }
        Console.WriteLine("\nНормированный массив:");
        for (int i = 0; i < n; i++)
        {
            array[i] /= maxAbs;
            Console.Write($"{array[i]:F2} ");
        }
        Console.WriteLine();
    }
}