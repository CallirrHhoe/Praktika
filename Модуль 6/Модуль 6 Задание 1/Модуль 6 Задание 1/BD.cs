using Microsoft.Data.Sqlite;
using System.Collections.Generic;
namespace Sotrudnik.Database
{
    public class Sotrudniki
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Post { get; set; }
        public string Dept { get; set; }
        public string Date { get; set; }
    }

    public class SotrudnikiDB
    {
        private const string Path = "Data Source=Sotrudniki.db";

        public SotrudnikiDB()
        {
            using (var db = new SqliteConnection(Path))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandText = @"
                        CREATE TABLE IF NOT EXISTS Sotrudniki (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            Name TEXT NOT NULL,
                            Post TEXT,
                            Dept TEXT,
                            Date TEXT
                        );";
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public List<Sotrudniki> GetAll()
        {
            var lst = new List<Sotrudniki>();
            using (var db = new SqliteConnection(Path))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, Name, Post, Dept, Date FROM Sotrudniki";
                    using (var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            lst.Add(new Sotrudniki
                            {
                                Id = rdr.GetInt32(0),
                                Name = rdr.GetString(1),
                                Post = rdr.IsDBNull(2) ? "" : rdr.GetString(2),
                                Dept = rdr.IsDBNull(3) ? "" : rdr.GetString(3),
                                Date = rdr.IsDBNull(4) ? "" : rdr.GetString(4)
                            });
                        }
                    }
                }
            }
            return lst;
        }
        public void Add(Sotrudniki e)
        {
            using (var db = new SqliteConnection(Path))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandText = "INSERT INTO Sotrudniki (Name, Post, Dept, Date) VALUES (@n, @p, @d, @dt)";
                    cmd.Parameters.AddWithValue("@n", e.Name);
                    cmd.Parameters.AddWithValue("@p", e.Post ?? "");
                    cmd.Parameters.AddWithValue("@d", e.Dept ?? "");
                    cmd.Parameters.AddWithValue("@dt", e.Date ?? "");
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void Update(Sotrudniki e)
        {
            using (var db = new SqliteConnection(Path))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandText = "UPDATE Sotrudniki SET Name = @n, Post = @p, Dept = @d, Date = @dt WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", e.Id);
                    cmd.Parameters.AddWithValue("@n", e.Name);
                    cmd.Parameters.AddWithValue("@p", e.Post ?? "");
                    cmd.Parameters.AddWithValue("@d", e.Dept ?? "");
                    cmd.Parameters.AddWithValue("@dt", e.Date ?? "");
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
                    cmd.CommandText = "DELETE FROM Sotrudniki WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}