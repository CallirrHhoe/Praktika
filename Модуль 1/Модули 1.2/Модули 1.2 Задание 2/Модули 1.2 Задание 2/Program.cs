class Program
{
    static void Main()
    {
        var a = new Random();
        int[] arr = Enumerable.Range(0, 10).Select(_ => a.Next(-50, 50)).ToArray();
        Console.WriteLine("Массив: " + string.Join(" ", arr));
        Console.Write("Число: ");
        int b = int.Parse(Console.ReadLine());
        arr[Array.IndexOf(arr, arr.Max())] = b;
        Console.WriteLine("Итог: " + string.Join(" ", arr));
    }
}