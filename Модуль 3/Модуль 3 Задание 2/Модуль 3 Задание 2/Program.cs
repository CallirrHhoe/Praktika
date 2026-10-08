using System;
class Notification
{
    // Определяем делегат и события для разного типа уведомлений
    public delegate void NotificationI(string message);
    public event NotificationI OnSms;
    public event NotificationI OnCall;
    public event NotificationI OnEmail;
    //? — операторпроверка на 0. Он проверяет, есть ли хотя бы один подписчик у события
    //Invoke — метод, который вызывает все методы-обработчики, подписанные на OnCall
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
}
