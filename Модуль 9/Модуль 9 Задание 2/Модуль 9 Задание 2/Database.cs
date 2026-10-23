using System;
using System.Data;
using System.IO;
using Microsoft.Data.Sqlite;
namespace ProjectTaskApp
{
    public class Database
    {
        string connStr;
        public Database(string dbName)
        {
            connStr = "Data Source=" + Path.Combine(AppContext.BaseDirectory, dbName);
            Run("CREATE TABLE IF NOT EXISTS Projects (Id INTEGER PRIMARY KEY AUTOINCREMENT, Title TEXT NOT NULL);");
            Run("CREATE TABLE IF NOT EXISTS Employees (Id INTEGER PRIMARY KEY AUTOINCREMENT, Name TEXT NOT NULL);");
            Run("CREATE TABLE IF NOT EXISTS Tasks (Id INTEGER PRIMARY KEY AUTOINCREMENT, Title TEXT NOT NULL, ProjectId INTEGER NOT NULL, EmployeeId INTEGER, Status TEXT, FOREIGN KEY(ProjectId) REFERENCES Projects(Id) ON DELETE CASCADE, FOREIGN KEY(EmployeeId) REFERENCES Employees(Id) ON DELETE SET NULL);");
        }
        public DataTable Run(string sql, params object[] args)
        {
            using (var conn = new SqliteConnection(connStr))
            {
                conn.Open();
                using (var pragmaCmd = new SqliteCommand("PRAGMA foreign_keys = ON", conn))
                {
                    pragmaCmd.ExecuteNonQuery();
                }
                using (var cmd = new SqliteCommand(sql, conn))
                {
                    for (int i = 0; i < args.Length; i++)
                    {
                        cmd.Parameters.AddWithValue("@p" + i, args[i] ?? DBNull.Value);
                    }
                    using (var reader = cmd.ExecuteReader())
                    {
                        var dt = new DataTable();
                        if (reader.FieldCount > 0)
                        {
                            dt.Load(reader);
                        }
                        return dt;
                    }
                }
            }
        }
    }
}