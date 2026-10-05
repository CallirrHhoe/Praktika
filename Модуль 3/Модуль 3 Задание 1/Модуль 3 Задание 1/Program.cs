using System;
class Shape
{
    // Виртуальный метод, который будет переопределен в дочерних классах
    public virtual double GetArea() => 0;
}
class Circle : Shape
{
    public double Radius { get; set; }
    public Circle(double r) => Radius = r;
    // Метод расчёта площади круга
    public override double GetArea() => Math.PI * Radius * Radius;
}
class Rectangle : Shape
{
    public double Width { get; set; }
    public double Height { get; set; }
    public Rectangle(double w, double h) { Width = w; Height = h; }
    // Метод расчёта площади прямоугольника
    public override double GetArea() => Width * Height;
}
class Triangle : Shape
{
    public double storona { get; set; }
    public double Height { get; set; }
    public Triangle(double a, double h) { storona = a; Height = h; }
    // Метод расчёта площади треугольника
    public override double GetArea() => 0.5 * storona * Height;
}
class Program
{
    // Объявляем делегата он может ссылаться на любой метод, который ничего не принимает и возвращает double
    delegate double Delegate();
        static void Main()
    {
        // Создаем объекты фигур
        Shape circle = new Circle(5);
        Shape rect = new Rectangle(4, 6);
        Shape triangle = new Triangle(3, 8);
        // Привязываем делегат к методу объекта circle
        Delegate calculateArea = circle.GetArea;
        // Вызываем делегат, вызывается circle.GetArea()
        Console.WriteLine($"Площадь круга: {calculateArea()}");
        // Перенаправляем тот же делегат на метод объекта rect
        calculateArea = rect.GetArea;
        Console.WriteLine($"Площадь прямоугольника: {calculateArea()}");
        // Перенаправляем делегат на метод объекта triangle
        calculateArea = triangle.GetArea;
        Console.WriteLine($"Площадь треугольника: {calculateArea()}");
    }
}