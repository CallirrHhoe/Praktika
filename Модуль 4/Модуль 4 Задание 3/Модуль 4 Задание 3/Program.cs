using System;
    // Интерфейс Студент
    public interface IStudent
    {
        double Bbal();
        int KKurs();
    }
    //Студент 1 курса
    public class Pervokursnik : IStudent
    {
        private double srednibal;
        public Pervokursnik(double srednibal) => this.srednibal = srednibal;
        public double Bbal() => srednibal;
        public int KKurs() => 1;
    }
    // Студент 3 курса
    public class Tretiekursnik : IStudent
    {
        private double srednibal;
        public Tretiekursnik(double srednibal) => this.srednibal = srednibal;
        public double Bbal() => srednibal;
        public int KKurs() => 3;
    }
    // Студент 2 курса
    public class VtoraKursnik : IStudent
    {
        private double srednibal;
        public VtoraKursnik(double srednibal) => this.srednibal = srednibal;
        public double Bbal() => srednibal;
        public int KKurs() => 2;
    }
    class Program
    {
        static void Main()
        {
            IStudent student1 = new Pervokursnik(7.9);
            IStudent student2 = new VtoraKursnik(8.4); 
            IStudent student3 = new Tretiekursnik(8.0);
            Console.WriteLine($" Курс = {student1.KKurs()}, Средний бал = {student1.Bbal()}");
            Console.WriteLine($" Курс = {student3.KKurs()}, Средний бал = {student3.Bbal()}");
            Console.WriteLine($" Курс = {student2.KKurs()}, Средний бал = {student2.Bbal()}");
            Console.ReadKey();
        }
    }