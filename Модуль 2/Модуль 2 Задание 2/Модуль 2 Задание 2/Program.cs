using System;
namespace Модуль_2_Задание_2
{
    internal class Program
    {
        static void Main(string[] args)
        {
        }
    }
}
class Shape
{
    // виртуальные методы, чтобы их можно было переопределить в дочерних классах 
    public virtual double Area() => 0;
    public virtual double Perimeter() => 0;
}
// Круг 
class Circle : Shape
{
    public double Radius { get; set; }
    public Circle(double radius)
    {
        Radius = radius;
    }
    //override переопределение метода 
    public override double Area() => Math.PI * Radius * Radius;
    public override double Perimeter() => 2 * Math.PI * Radius;
}
// Прямоугольник 
class Rectangle : Shape
{
    public double Width { get; set; }
    public double Height { get; set; }
    public Rectangle(double width, double height)
    {
        Width = width;
        Height = height;
    }
    public override double Area() => Width * Height;
    public override double Perimeter() => 2 * (Width + Height);
}
class Program
{
    static void Main()
    {
        // Создание фигуры 
        Shape circle = new Circle(5);
        Shape rectangle = new Rectangle(4, 6);
        // Вывод информации 
        Console.WriteLine($"Круг: Площадь = {circle.Area():F2}, Периметр = {circle.Perimeter():F2}");
        Console.WriteLine($"Прямоугольник: Площадь = {rectangle.Area()}, Периметр = {rectangle.Perimeter()}");
    }
}