using System;
    public interface IRisunok
    {
        void Liniya(int x1, int y1, int x2, int y2);
        void Krug(int x, int y, int radiys);
        void PryamoUgol(int x, int y, int shirina, int vysota);
    }
    // Класс для работы с холстом
    public class Holst : IRisunok
    {
        private string nazvanieHolsta;
        public Holst(string nazvanieHolsta)
        {
            this.nazvanieHolsta = nazvanieHolsta; // Сохраняем переданное название
        }
        public void Liniya(int x1, int y1, int x2, int y2)
        {
            Console.WriteLine($"На холсте \"{nazvanieHolsta}\" рисуем линию из ({x1}, {y1}) в ({x2}, {y2})");
        }
        public void Krug(int x, int y, int radiys)
        {
            Console.WriteLine($"На холсте \"{nazvanieHolsta}\" рисуем круг с центром в ({x}, {y}) и радиусом {radiys}");
        }
        public void PryamoUgol(int x, int y, int shirina, int vysota)
        {
            Console.WriteLine($"На холсте \"{nazvanieHolsta}\" рисуем прямоугольник в ({x}, {y}), ширина: {shirina}, высота: {vysota}");
        }
    }
    class Program
    {
        static void Main()
        {
            // Создаем холст
            IRisunok moyHolst = new Holst("Рабочий Холст");
            Console.WriteLine("Приложения для рисования на холсте");
            // Рисуем фигуры с помощью методов интерфейса
            moyHolst.Liniya(0, 0, 10, 10);
            moyHolst.Krug(5, 5, 3);
            moyHolst.PryamoUgol(2, 3, 6, 4);
            Console.WriteLine("Нажмите любую клавишу для выхода");
            Console.ReadKey();
        }
    }