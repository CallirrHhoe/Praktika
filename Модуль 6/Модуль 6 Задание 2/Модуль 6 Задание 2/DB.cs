using Microsoft.Data.Sqlite;
using System.Collections.Generic;
namespace TaskApp.Database
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Desc { get; set; }
        public string Status { get; set; }
        public string Date { get; set; }
    }
    public class TaskDb
    {
        private const string Path = "Data Source=tasks.db";
        public TaskDb()
        {
            using (var db = new SqliteConnection(Path))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandText = @"
                        CREATE TABLE IF NOT EXISTS Tasks (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            Title TEXT NOT NULL,
                            Desc TEXT,
                            Status TEXT,
                            Date TEXT
                        );";
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public List<TaskItem> GetAll()
        {
            var lst = new List<TaskItem>();
            using (var db = new SqliteConnection(Path))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, Title, Desc, Status, Date FROM Tasks";
                    using (var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            lst.Add(new TaskItem
                            {
                                Id = rdr.GetInt32(0),
                                Title = rdr.GetString(1),
                                Desc = rdr.IsDBNull(2) ? "" : rdr.GetString(2),
                                Status = rdr.IsDBNull(3) ? "" : rdr.GetString(3),
                                Date = rdr.IsDBNull(4) ? "" : rdr.GetString(4)
                            });
                        }
                    }
                }
            }
            return lst;
        }
        public void Add(TaskItem t)
        {
            using (var db = new SqliteConnection(Path))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandText = "INSERT INTO Tasks (Title, Desc, Status, Date) VALUES (@t, @d, @s, @dt)";
                    cmd.Parameters.AddWithValue("@t", t.Title);
                    cmd.Parameters.AddWithValue("@d", t.Desc ?? "");
                    cmd.Parameters.AddWithValue("@s", t.Status ?? "Новая");
                    cmd.Parameters.AddWithValue("@dt", t.Date ?? "");
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void Update(TaskItem t)
        {
            using (var db = new SqliteConnection(Path))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandText = "UPDATE Tasks SET Title = @t, Desc = @d, Status = @s, Date = @dt WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", t.Id);
                    cmd.Parameters.AddWithValue("@t", t.Title);
                    cmd.Parameters.AddWithValue("@d", t.Desc ?? "");
                    cmd.Parameters.AddWithValue("@s", t.Status ?? "");
                    cmd.Parameters.AddWithValue("@dt", t.Date ?? "");
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
                    cmd.CommandText = "DELETE FROM Tasks WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}