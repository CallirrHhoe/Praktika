class Program
{
    static void Main()
    {
        Random rnd = new Random();
        int[] arr = new int[10];
        // Заполнение массива
        for (int i = 0; i < arr.Length; i++)
        {
            arr[i] = rnd.Next(-50, 50);
        }
        // Вывод массива
        Console.Write("Массив: ");
        for (int i = 0; i < arr.Length; i++)
        {
            Console.Write(arr[i] + " ");
        }
        Console.WriteLine();
        // Поиск индекса максимального элемента
        int maxIndex = 0;
        for (int i = 1; i < arr.Length; i++)
        {
            if (arr[i] > arr[maxIndex])
            {
                maxIndex = i;
            }
        }
        // Ввод числа и замена
        Console.Write("Число: ");
        int b = int.Parse(Console.ReadLine());
        arr[maxIndex] = b;
        // Вывод результата
        Console.Write("Итог: ");
        for (int i = 0; i < arr.Length; i++)
        {
            Console.Write(arr[i] + " ");
        }
        Console.WriteLine();
    }
}