using Microsoft.Data.Sqlite;
using System.Collections.Generic;
namespace FinanceApp.Database
{
    public class Item
    {
        public int Id { get; set; }
        public string Type { get; set; }  // Доход или Расход
        public double Sum { get; set; }
        public string Cat { get; set; }   // Категория
        public string Date { get; set; }
    }
    public class FinanceDb
    {
        private const string Path = "Data Source=finance.db";

        public FinanceDb()
        {
            using (var db = new SqliteConnection(Path))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandText = @"
                        CREATE TABLE IF NOT EXISTS Finance (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            Type TEXT NOT NULL,
                            Sum REAL NOT NULL,
                            Cat TEXT,
                            Date TEXT
                        );";
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public List<Item> GetAll()
        {
            var lst = new List<Item>();
            using (var db = new SqliteConnection(Path))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, Type, Sum, Cat, Date FROM Finance";
                    using (var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            lst.Add(new Item
                            {
                                Id = rdr.GetInt32(0),
                                Type = rdr.GetString(1),
                                Sum = rdr.GetDouble(2),
                                Cat = rdr.IsDBNull(3) ? "" : rdr.GetString(3),
                                Date = rdr.IsDBNull(4) ? "" : rdr.GetString(4)
                            });
                        }
                    }
                }
            }
            return lst;
        }
        public void Add(Item x)
        {
            using (var db = new SqliteConnection(Path))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandText = "INSERT INTO Finance (Type, Sum, Cat, Date) VALUES (@t, @s, @c, @d)";
                    cmd.Parameters.AddWithValue("@t", x.Type ?? "Расход");
                    cmd.Parameters.AddWithValue("@s", x.Sum);
                    cmd.Parameters.AddWithValue("@c", x.Cat ?? "");
                    cmd.Parameters.AddWithValue("@d", x.Date ?? "");
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void Update(Item x)
        {
            using (var db = new SqliteConnection(Path))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandText = "UPDATE Finance SET Type = @t, Sum = @s, Cat = @c, Date = @d WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", x.Id);
                    cmd.Parameters.AddWithValue("@t", x.Type ?? "Расход");
                    cmd.Parameters.AddWithValue("@s", x.Sum);
                    cmd.Parameters.AddWithValue("@c", x.Cat ?? "");
                    cmd.Parameters.AddWithValue("@d", x.Date ?? "");
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void Delete(int id)
        {
            using (var db = new SqliteConnection(Path))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM Finance WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}