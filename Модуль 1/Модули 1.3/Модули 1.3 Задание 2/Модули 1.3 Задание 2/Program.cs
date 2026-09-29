class Program
{
    static void Main()
    {
        Console.Write("Введите пороговую сумму: ");
        int maxSum = int.Parse(Console.ReadLine());
        Random rnd = new Random();
        List<int> list = new List<int>();
        int a = 0;
        while (true)
        {
            int val = rnd.Next(1, 10); 
            if (a + val > maxSum) break;
            list.Add(val);
            a += val;
        }
        int[] arr = list.ToArray();
        Console.WriteLine("Сгенерированный массив: " + string.Join(" ", arr));
        Console.WriteLine($"Сумма элементов: {a} (порог: {maxSum})");
    }
}
