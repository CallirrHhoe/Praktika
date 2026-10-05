using System;
    // Интерфейс для геометрических фигур
    public interface IFigure
    {
        double GetArea();      // Метод для вычисления площади
        double GetPerimeter(); // Метод для вычисления периметра
    }
    // Класс Круг
    public class Circle : IFigure
    {
        private double radius; // Личное поле для хранения радиуса
        public Circle(double radius)
        {
            this.radius = radius; // Сохраняем переданный радиус в память объекта
        }
        public double GetArea() => Math.PI * radius * radius;
        public double GetPerimeter() => 2 * Math.PI * radius;
    }
    // Класс Прямоугольник
    public class Rectangle : IFigure
    {
        private double width;  // Ширина
        private double height; // Высота
        public Rectangle(double width, double height)
        {
            this.width = width;   // Сохраняем ширину
            this.height = height; // Сохраняем высоту
        }
        public double GetArea() => width * height;
        public double GetPerimeter() => 2 * (width + height);
    }
    // Класс Треугольник
    public class Triangle : IFigure
    {
        private double a, b, c; // Три стороны треугольника
        public Triangle(double a, double b, double c)
        {
            this.a = a;
            this.b = b;
            this.c = c; // Сохраняем стороны в память объекта
        }
        public double GetPerimeter() => a + b + c;
        public double GetArea()
        {
            // Вычисляем площадь по формуле Герона через полупериметр
            double p = GetPerimeter() / 2;
            return Math.Sqrt(p * (p - a) * (p - b) * (p - c));
        }
    }
    class Program
    {
        static void Main()
        {
            // Создаем круг с радиусом 5
            IFigure circle = new Circle(5);
            Console.WriteLine("Круг (радиус: 5):");
            Console.WriteLine($"  Площадь: {circle.GetArea():F2}");
            Console.WriteLine($"  Периметр: {circle.GetPerimeter():F2}\n");
            // Создаем прямоугольник со сторонами 4 и 6
            IFigure rectangle = new Rectangle(4, 6);
            Console.WriteLine("Прямоугольник (стороны: 4 и 6):");
            Console.WriteLine($"  Площадь: {rectangle.GetArea()}");
            Console.WriteLine($"  Периметр: {rectangle.GetPerimeter()}\n");
            // Создаем треугольник со сторонами 3, 4 и 5
            IFigure triangle = new Triangle(3, 4, 5);
            Console.WriteLine("Треугольник (стороны: 3, 4, 5):");
            Console.WriteLine($"  Площадь: {triangle.GetArea():F2}");
            Console.WriteLine($"  Периметр: {triangle.GetPerimeter()}\n");
            // Остановка консоли перед закрытием
            Console.WriteLine("Нажмите любую клавишу для выхода");
            Console.ReadKey();
        }
  }