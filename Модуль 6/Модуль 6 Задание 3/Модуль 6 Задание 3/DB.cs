using Microsoft.Data.Sqlite;
using System.Collections.Generic;
namespace LibraryApp.Database
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public bool Available { get; set; }
    }
    public class LibraryDb
    {
        private const string Path = "Data Source=library.db";
        public LibraryDb()
        {
            using (var db = new SqliteConnection(Path))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandText = @"
                        CREATE TABLE IF NOT EXISTS Books (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            Title TEXT NOT NULL,
                            Author TEXT NOT NULL,
                            Available INTEGER DEFAULT 1
                        );";
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public List<Book> Search(string q)
        {
            var lst = new List<Book>();
            using (var db = new SqliteConnection(Path))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, Title, Author, Available FROM Books WHERE Title LIKE @q OR Author LIKE @q";
                    cmd.Parameters.AddWithValue("@q", $"%{q}%");
                    using (var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            lst.Add(new Book
                            {
                                Id = rdr.GetInt32(0),
                                Title = rdr.GetString(1),
                                Author = rdr.GetString(2),
                                Available = rdr.GetInt32(3) == 1
                            });
                        }
                    }
                }
            }
            return lst;
        }
        public void Rent(int id, bool status)
        {
            using (var db = new SqliteConnection(Path))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandText = "UPDATE Books SET Available = @s WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@s", status ? 1 : 0);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}