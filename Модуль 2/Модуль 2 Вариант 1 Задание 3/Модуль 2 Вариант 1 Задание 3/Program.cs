// Абстрактный базовый класс для всех фигур 
using System;
abstract class GeometricShape
{
    // Абстрактные методы 
    public abstract double CalculateArea();
    public abstract double CalculatePerimeter();
}
// Класс Круг 
class Circle : GeometricShape
{
    // Радиус круга 
    public double Radus { get; set; }
    // Конструктор: принимаем радиус при создании объекта 
    public Circle(double radus) => Radus = radus;
    // Переопределяем вычисление площади: S = PI * R^2 
    public override double CalculateArea() => Math.PI * Radus * Radus;
    // Переопределяем вычисление периметра: P = 2 * PI * R 
    public override double CalculatePerimeter() => 2 * Math.PI * Radus;
}
// Класс Прямоугольник 
class Rectangle : GeometricShape
{
    // Ширина и высота 
    public double Shirina { get; set; }
    public double Vysota { get; set; }
    // Конструктор сохраняем ширину и высоту 
    public Rectangle(double shirina, double vysota)
    {
        Shirina = shirina;
        Vysota = vysota;
    }
    // Площадь прямоугольника: S = A * B 
    public override double CalculateArea() => Shirina * Vysota;
    // Периметр прямоугольника: P = 2 * (A + B) 
    public override double CalculatePerimeter() => 2 * (Shirina + Vysota);
}
// Класс Треугольник 
class Triangle : GeometricShape
{
    // Три стороны треугольника 
    public double StoronaA { get; set; }
    public double StoronaB { get; set; }
    public double StoronaC { get; set; }
    // Конструктор: сохраняем длины трёх сторон 
    public Triangle(double a, double b, double c)
    {
        StoronaA = a;
        StoronaB = b;
        StoronaC = c;
    }
    // Периметр треугольника: сумма всех сторон 
    public override double CalculatePerimeter() => StoronaA + StoronaB + StoronaC;
    // Площадь треугольника по формуле Герона 
    public override double CalculateArea()
    {
        // Полупериметр (p) 
        double p = CalculatePerimeter() / 2;
        return Math.Sqrt(p * (p - StoronaA) * (p - StoronaB) * (p - StoronaC));
    }
}
class Program
{
    static void Main()
    {
        // Создаём массив из разных фигур 
        GeometricShape[] figury = new GeometricShape[]
        {
            new Circle(4),            // Круг с радиусом 4 
            new Rectangle(3, 5),      // Прямоугольник 3x5 
            new Triangle(3, 4, 5)     // Треугольник со сторонами 3, 4, 5 
        };
        // Проходимся по каждой фигуре в массиве 
        foreach (var figura in figury)
        {
            //  подставляет имя класса 
            Console.WriteLine($"Фигура: {figura.GetType().Name}");
            Console.WriteLine($"  Площадь:   {figura.CalculateArea():F2}");
            Console.WriteLine($"  Периметр:  {figura.CalculatePerimeter():F2}\n");
        }
    }
}