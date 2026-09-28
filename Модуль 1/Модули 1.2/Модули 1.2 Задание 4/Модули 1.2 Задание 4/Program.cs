class Program
{
    static void Main()
    {
        Console.Write("Введите K: ");
        int k = int.Parse(Console.ReadLine());
        Console.Write("Введите A: ");
        int a = int.Parse(Console.ReadLine());
        Console.Write("Введите B: ");
        int b = int.Parse(Console.ReadLine());
        Random rnd = new Random();
        int[] arr = new int[k];
        // Заполнение массива
        for (int i = 0; i < k; i++)
        {
            arr[i] = rnd.Next(a, b);
        }
        Console.WriteLine("Массив: " + string.Join(" ", arr));
        // Поиск индексов мин и макс
        int minI = 0, maxI = 0;
        for (int i = 1; i < k; i++)
        {
            if (arr[i] < arr[minI]) minI = i;
            if (arr[i] > arr[maxI]) maxI = i;
        }
        // Границы для вывода
        int start = Math.Min(minI, maxI);
        int end = Math.Max(minI, maxI);
        // Вывод элементов между ними
        Console.Write("Результат: ");
        for (int i = start; i <= end; i++)
        {
            Console.Write(arr[i] + " ");
        }
        Console.WriteLine();
    }
}