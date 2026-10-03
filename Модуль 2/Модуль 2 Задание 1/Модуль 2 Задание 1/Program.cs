using System;
class Person
{
    // приватные поля 
    private string name;
    private int age;
    private string address;
    // проверяет и записывает новое значение в поле 
    public void SetName(string name) => this.name = name;
    public void SetAge(int age) => this.age = age;
    public void SetAddress(string address) => this.address = address;
    // читает и возвращает значение из поля 
    public string GetName() => name;
    public int GetAge() => age;
    public string GetAddress() => address;
    public void PrintInfo()
    {
        Console.WriteLine($"Имя: {name}, Возраст: {age}, Адрес: {address}");
    }
}
class Program
{
    static void Main()
    {
        Person person1 = new Person();
        person1.SetName("Иван");
        person1.SetAge(20);
        person1.SetAddress("ул. Ленина, д. 10");
        Person person2 = new Person();
        person2.SetName("Анна");
        person2.SetAge(22);
        person2.SetAddress("пр. Мира, д. 5");
        person1.PrintInfo();
        person2.PrintInfo();
    }
}