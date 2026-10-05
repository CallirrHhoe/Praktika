using System;
class Program
{
    // Объявляем делегат, который принимает массив целых чисел
    delegate void SortDelegate(int[] array);
    // Пузырьковая сортировка
    // Сравнивает соседние элементы и проталкивает максимальные в конец
    static void BubbleSort(int[] arr)
    {
        for (int i = 0; i < arr.Length - 1; i++)
        {
            for (int j = 0; j < arr.Length - i - 1; j++)
            {
                // Если текущий элемент больше следующего — меняем их местами
                if (arr[j] > arr[j + 1])
                {
                    int temp = arr[j];
                    arr[j] = arr[j + 1];
                    arr[j + 1] = temp;
                }
            }
        }
    }
    // Сортировка выбором 
    // Ищет наименьший элемент в неотсортированной части и ставит его в начало
    static void SelectionSort(int[] arr)
    {
        for (int i = 0; i < arr.Length - 1; i++)
        {
            int minIndex = i; // Предполагаем, что текущий элемент минимальный

            for (int j = i + 1; j < arr.Length; j++)
            {
                if (arr[j] < arr[minIndex])
                    minIndex = j; // Нашли элемент еще меньше
            }
            // Меняем найденный минимальный элемент с текущим
            int temp = arr[i];
            arr[i] = arr[minIndex];
            arr[minIndex] = temp;
        }
    }
    // Сортировка вставками 
    // Берет элементы по одному и вставляет их на нужную позицию в левую часть
    static void InsertionSort(int[] arr)
    {
        for (int i = 1; i < arr.Length; i++)
        {
            int key = arr[i]; // Запоминаем текущий элемент
            int j = i - 1;
            // Сдвигаем элементы, которые больше key, вправо
            while (j >= 0 && arr[j] > key)
            {
                arr[j + 1] = arr[j];
                j--;
            }
            arr[j + 1] = key; // Ставим key на его правильное место
        }
    }
    // Быстрая сортировка 
    // Встроенный метод C#
    static void QuickSort(int[] arr)
    {
        Array.Sort(arr); // Использует алгоритм Introsort 
    }
    // Сортировка подсчётом 
    // Считает количество повторений каждого числа и перезаписывает массив
    static void CountingSort(int[] arr)
    {
        if (arr.Length == 0) return;
        // Ищем максимальный элемент в массиве
        int max = arr[0];
        for (int i = 1; i < arr.Length; i++)
            if (arr[i] > max) max = arr[i];
        // Массив для подсчета частоты каждого числа
        int[] count = new int[max + 1];
        foreach (int num in arr)
            count[num]++;
        // Заполняем исходный массив по порядку
        int index = 0;
        for (int i = 0; i <= max; i++)
        {
            while (count[i] > 0)
            {
                arr[index++] = i;
                count[i]--;
            }
        }
    }
    // BogoSort (Обезьянья сортировка)
    // Случайно перемешивает элементы, пока массив случайно не отсортируется
    static void BogoSort(int[] arr)
    {
        Random rnd = new Random();
        int attempts = 0;
        int maxAttempts = 100000; // Ограничение попыток
        // Вспомогательная локальная функция проверки отсортированности
        bool IsSorted(int[] a)
        {
            for (int i = 0; i < a.Length - 1; i++)
                if (a[i] > a[i + 1]) return false;
            return true;
        }
        // Перемешиваем, пока массив не отсортируется или не кончатся попытки
        while (!IsSorted(arr) && attempts < maxAttempts)
        {
            attempts++;
            // Случайное перемешивание
            for (int i = 0; i < arr.Length; i++)
            {
                int r = rnd.Next(i, arr.Length);
                int temp = arr[i];
                arr[i] = arr[r];
                arr[r] = temp;
            }
        }
        if (IsSorted(arr))
            Console.WriteLine($"\n[Успех BogoSort] Массив случайно отсортировался за {attempts} попыток!");
        else
            Console.WriteLine($"\n[BogoSort сдался] Сделано {maxAttempts} попыток перемешивания, но удача не улыбнулась.");
    }
    static void Main()
    {
        // Бесконечный цикл
        while (true)
        {
            // Каждую итерацию восстанавливаем неотсортированный массив чисел
            int[] numbers = { 42, 12, 88, 3, 67, 21, 95, 14, 5, 50, 31, 9, 73, 2 };
            Console.WriteLine("Исходный массив (" + numbers.Length + " элементов):");
            Console.WriteLine(string.Join(", ", numbers));
            Console.WriteLine("\nВыберите метод сортировки:");
            Console.WriteLine("1 — Пузырьковая сортировка (Bubble Sort)");
            Console.WriteLine("2 — Сортировка выбором (Selection Sort)");
            Console.WriteLine("3 — Сортировка вставками (Insertion Sort)");
            Console.WriteLine("4 — Быстрая сортировка (QuickSort / Array.Sort)");
            Console.WriteLine("5 — Сортировка подсчётом (Counting Sort)");
            Console.WriteLine("6 — BogoSort (Обезьянья сортировка / Рандом)");
            Console.WriteLine("0 — Выход из программы");
            Console.Write("\nВведите номер пункта: ");
            string choice = Console.ReadLine();
            // Проверка на выход из цикла
            if (choice == "0")
            {
                Console.WriteLine("Выход из программы...");
                break;
            }
            // Переменная делегата
            SortDelegate sorter = null;
            // Связываем выбранный метод с делегатом
            if (choice == "1") sorter = BubbleSort;
            else if (choice == "2") sorter = SelectionSort;
            else if (choice == "3") sorter = InsertionSort;
            else if (choice == "4") sorter = QuickSort;
            else if (choice == "5") sorter = CountingSort;
            else if (choice == "6") sorter = BogoSort;
            // Если выбор сделан правильно
            if (sorter != null)
            {
                // Вызываем выбранную сортировку через делегат
                sorter(numbers);
                Console.WriteLine("\nОтсортированный массив:");
                Console.WriteLine(string.Join(", ", numbers));
            }
            else
            {
                Console.WriteLine("\nОшибка: Неверный выбор! Попробуйте снова.");
            }
        }
    }
}