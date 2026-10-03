// Интерфейс, который будет реализовать метод Draw 
using System;
interface IDrawable
{
    void Draw();
}
class Circle : IDrawable
{
    public void Draw() => Console.WriteLine("Рисуем круг");
}
class Rectangle : IDrawable
{
    public void Draw() => Console.WriteLine("Рисуем прямоугольник");
}
class Triangle : IDrawable
{
    public void Draw() => Console.WriteLine("Рисуем треугольник");
}
class Program
{
    static void Main()
    {
        // Массив объектов, реализующих один и тот же интерфейс 
        IDrawable[] shapes = new IDrawable[]
        {
            new Circle(),
            new Rectangle(),
            new Triangle()
        };
        // Вызываем Draw у каждого объекта в цикле 
        foreach (var shape in shapes)
        {
            shape.Draw();
        }
    }
}