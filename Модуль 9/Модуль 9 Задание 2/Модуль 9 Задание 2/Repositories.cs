using System.Data;
namespace ProjectTaskApp
{
    public class ProjectRepository
    {
        Database db;
        public ProjectRepository(Database db) => this.db = db;
        public DataTable GetAll() => db.Run("SELECT Id AS Номер, Title AS Название FROM Projects");
        public void Add(string title) => db.Run("INSERT INTO Projects (Title) VALUES (@p0)", title);
        public void Update(int id, string title) => db.Run("UPDATE Projects SET Title = @p0 WHERE Id = @p1", title, id);
        public void Delete(int id) => db.Run("DELETE FROM Projects WHERE Id = @p0", id);
    }
    public class EmployeeRepository
    {
        Database db;
        public EmployeeRepository(Database db) => this.db = db;
        public DataTable GetAll() => db.Run("SELECT Id AS Номер, Name AS ФИО FROM Employees");
        public void Add(string name) => db.Run("INSERT INTO Employees (Name) VALUES (@p0)", name);
        public void Update(int id, string name) => db.Run("UPDATE Employees SET Name = @p0 WHERE Id = @p1", name, id);
        public void Delete(int id) => db.Run("DELETE FROM Employees WHERE Id = @p0", id);
    }
    public class TaskRepository
    {
        Database db;
        public TaskRepository(Database db) => this.db = db;
        public DataTable GetAll() => db.Run("SELECT T.Id AS Номер, T.Title AS Задача, P.Title AS Проект, IFNULL(E.Name, 'Не назначен') AS Исполнитель, T.Status AS Статус FROM Tasks T JOIN Projects P ON T.ProjectId = P.Id LEFT JOIN Employees E ON T.EmployeeId = E.Id");
        public void Add(string title, int projId, int empId, string status) => db.Run("INSERT INTO Tasks (Title, ProjectId, EmployeeId, Status) VALUES (@p0, @p1, @p2, @p3)", title, projId, empId, status);
        public void Update(int id, string title, string status) => db.Run("UPDATE Tasks SET Title = @p0, Status = @p1 WHERE Id = @p2", title, status, id);
        public void Delete(int id) => db.Run("DELETE FROM Tasks WHERE Id = @p0", id);
    }
}