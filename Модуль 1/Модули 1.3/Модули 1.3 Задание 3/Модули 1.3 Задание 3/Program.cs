class TProgram
{
    static void Main()
    {
        Console.Write("Введите размер матрицы N: ");
        int n = int.Parse(Console.ReadLine());
        Random rnd = new Random();
        int[][] matrix = new int[n][];
        //Заполнение матрицы случайными числами [-50, 50]
        Console.WriteLine("\nИсходная матрица:");
        for (int i = 0; i < n; i++)
        {
            matrix[i] = new int[n];
            for (int j = 0; j < n; j++) matrix[i][j] = rnd.Next(-50, 51);
            Console.WriteLine($"{string.Join("\t", matrix[i])}  | Сумма = {matrix[i].Sum()}");
        }
        // Сортировка строк по возрастанию их сумм 
        matrix = matrix.OrderBy(row => row.Sum()).ToArray();
        // Вывод отсортированной матрицы
        Console.WriteLine("\nУпорядоченная матрица (по возрастанию сумм строк):");
        foreach (var row in matrix)
        {
            Console.WriteLine($"{string.Join("\t", row)}  | Сумма = {row.Sum()}");
        }
    }
}