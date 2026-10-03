using System;
class Author
{
    // свойства 
    public string Name { get; set; }
    public int BirthYear { get; set; }
    // конструктор 
    public Author(string name, int birthYear)
    {
        Name = name;
        BirthYear = birthYear;
    }
}
class Book
{
    public string Title { get; set; }
    public int ReleaseYear { get; set; }
    // Композиция объект Author находится внутри Book 
    public Author BookAuthor { get; set; }
    public Book(string title, int releaseYear, Author author)
    {
        Title = title;
        ReleaseYear = releaseYear;
        BookAuthor = author;
    }
    public void info()
    {
        Console.WriteLine($"Книга: \"{Title}\" ({ReleaseYear} г.), Автор: {BookAuthor.Name} ({BookAuthor.BirthYear} г.р.)");
    }
}
class Program
{
    static void Main()
    {
        Author author1 = new Author("Лев Толстой", 1828);
        Author author2 = new Author("Михаил Булгаков", 1891);
        Book book1 = new Book("Война и мир", 1869, author1);
        Book book2 = new Book("Мастер и Маргарита", 1967, author2);
        book1.info();
        book2.info();
    }
}