using System;
class Notification
<<<<<<< HEAD
{
=======
{ 
>>>>>>> ef9e02fe654f64d0f94ee8bfdaac3f01a06dcbfe
    // Определяем делегат и события для разного типа уведомлений
    public delegate void NotificationI(string message);
    public event NotificationI OnSms;
    public event NotificationI OnCall;
    public event NotificationI OnEmail;
    //? — операторпроверка на 0. Он проверяет, есть ли хотя бы один подписчик у события
<<<<<<< HEAD
    //Invoke — метод, который поочередно вызывает все методы-обработчики, подписанные на OnCall
=======
    //Invoke — метод, который вызывает все методы-обработчики, подписанные на OnCall
>>>>>>> ef9e02fe654f64d0f94ee8bfdaac3f01a06dcbfe
    public void SendSms(string text) => OnSms?.Invoke(text);
    public void MakeCall(string number) => OnCall?.Invoke(number);
    public void SendEmail(string mail) => OnEmail?.Invoke(mail);
}
class Program
{
    static void Main()
    {
        Notification notifier = new Notification();
        // Подписываем обработчики на события
        // += — оператор подписки
        notifier.OnSms += msg => Console.WriteLine($"[SMS] Отправлено сообщение: {msg}");
        notifier.OnCall += num => Console.WriteLine($"[Звонок] Набор номера: {num}");
        notifier.OnEmail += text => Console.WriteLine($"[Email] Письмо отправлено: {text}");
        // Вызываем события
        notifier.SendSms("Привет! Вышло новове видео по программированию.");
        notifier.MakeCall("+375 (29) 000-11-22");
        notifier.SendEmail("Подтвердите регистрацию на сайте.");
    }
