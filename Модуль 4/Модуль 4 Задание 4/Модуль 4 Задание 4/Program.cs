using System;
    public interface IKniga
    {
        bool ProveritDostupnost();
        void VydatKnigu();
    }
    // Художественная книга
    public class HudozhestvennayaKniga : IKniga
    {
        private string nazvanie;
        private bool dostupna = true;
        public HudozhestvennayaKniga(string nazvanie) => this.nazvanie = nazvanie;
        public bool ProveritDostupnost() => dostupna;
        public void VydatKnigu()
        {
            if (dostupna) 
                { 
                    dostupna = false; 
                    Console.WriteLine($"Художественная книга '{nazvanie}' выдана.");
                }
            else
                { 
                    Console.WriteLine($"Художественная книга '{nazvanie}'уже была выдана.");
                }
        }
    }
    // Научная книга
    public class NauchnayaKniga : IKniga
    {
        private string nazvanie;
        private bool dostupna = true;
        public NauchnayaKniga(string nazvanie) => this.nazvanie = nazvanie;
        public bool ProveritDostupnost() => dostupna;
        public void VydatKnigu()
        {
            if (dostupna) 
                { 
                    dostupna = false; 
                    Console.WriteLine($"Научная книга '{nazvanie}' выдана.");
                }
            else 
                {  
                    Console.WriteLine($"Научная книга'{nazvanie}' уже была выдана."); 
                }
        }
    }
    // Детская книга
    public class DetskayaKniga : IKniga
    {
        private string nazvanie;
        private bool dostupna = true;
        public DetskayaKniga(string nazvanie) => this.nazvanie = nazvanie;
        public bool ProveritDostupnost() => dostupna;
        public void VydatKnigu()
        {
            if (dostupna) 
                {  
            dostupna = false; 
            Console.WriteLine($"Детская книга '{nazvanie}' выдана."); 
                }
            else 
                {  
            Console.WriteLine($"Детская книга '{nazvanie}' уже выдана."); 
                }
        }
    }
    class Program
    {
        static void Main()
        {
            IKniga b1 = new HudozhestvennayaKniga("Мастер и Маргарита");
            IKniga b2 = new NauchnayaKniga("Квантовая физика");
            IKniga b3 = new DetskayaKniga("Незнайка в солнечном городе");
            b1.VydatKnigu();
            b2.VydatKnigu();
            b3.VydatKnigu();
            Console.ReadKey();
        }
    }