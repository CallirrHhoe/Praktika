using System;
class TemperatureSensor
{
    // Объявляем событие и указываем на метод 
    public event Action<double> TemperatureChanged;
    private double currentTemperature;
    public void SetTemperature(double newTemp)
    {
        if (currentTemperature != newTemp)
        {
            currentTemperature = newTemp;
            Console.WriteLine($"\n[Датчик]: Температура изменилась на {currentTemperature}°C");
            // Вызываем событие что бы один класс передал свой метод другому классу 
            TemperatureChanged?.Invoke(currentTemperature);
        }
    }
}
class Thermostat
{
    // Метод-обработчик события 
    public void OnTemperatureChanged(double temperature)
    {
        if (temperature < 18.0)
        {
            Console.WriteLine("[Термостат]: Холодно! Включаем отопление.");
        }
        else if (temperature > 24.0)
        {
            Console.WriteLine("[Термостат]: Жарко! Выключаем отопление.");
        }
        else
        {
            Console.WriteLine("[Термостат]: Температура комфортная.");
        }
    }
}
class Program
{
    static void Main()
    {
        TemperatureSensor sensor = new TemperatureSensor();
        Thermostat thermostat = new Thermostat();
        // Подписываем термостат на событие датчика 
        sensor.TemperatureChanged += thermostat.OnTemperatureChanged;
        // Симулируем изменение температуры 
        sensor.SetTemperature(15.0); // Сработает включение 
        sensor.SetTemperature(21.0); // Комфортно 
        sensor.SetTemperature(26.0); // Выключение 
    }
}