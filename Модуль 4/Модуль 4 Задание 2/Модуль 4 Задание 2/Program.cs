using System;
    public interface IProduct
    {
        decimal Cena();
        int Ostatok();
    }
    // Класс продукта питания
    public class FoodProduct : IProduct
    {
        //decimal - тип данных с точкой, позволяющий болле точно расчитывать значение
        private decimal price;
        private int stock;
        public FoodProduct(decimal price, int stock)
        {
            this.price = price;
            this.stock = stock; // Сохраняем цену и остаток
        }
        public decimal Cena() => price;
        public int Ostatok() => stock;
    }
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            // Создаем товар: цена 5.69, остаток 120 штук
            IProduct Chipsi = new FoodProduct(5.69m, 120);
            Console.WriteLine("Продукт");
            Console.WriteLine($"Цена товара: {Chipsi.Cena()} руб.");
            Console.WriteLine($"Остаток на складе: {Chipsi.Ostatok()} шт.");
            IProduct SOsiskaVteste = new FoodProduct(1.39m, 15);
            Console.WriteLine($"Цена товара: {SOsiskaVteste.Cena()} руб.");
            Console.WriteLine($"Остаток на складе: {SOsiskaVteste.Ostatok()} шт.");
            IProduct Voda = new FoodProduct(1.69m, 75);
            Console.WriteLine($"Цена товара: {Voda.Cena()} руб.");
            Console.WriteLine($"Остаток на складе: {Voda.Ostatok()} шт.");
            Console.ReadKey();
        }
    }