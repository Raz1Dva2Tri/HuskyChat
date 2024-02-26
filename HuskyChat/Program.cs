using HuskyChat;
using System.Net;
using System.Net.Sockets;
using System.Text;

string configFilePath = "config.json"; // путь к файлу конфигурации
ConfigurationReader configReader = new ConfigurationReader();
Configuration config = configReader.ReadConfiguration(configFilePath);

if (config != null)
{
    Console.WriteLine($"Config.json - настройка ip, ports.");
    Console.WriteLine($"IP Address: {config.IPAddress}");
    Console.WriteLine($"Send Message Port: {config.SendMessagePort}");
    Console.WriteLine($"Receive Message Port: {config.ReceiveMessagePort}");
}
//// Сделать проверку сеанса
IPAddress localAddress = IPAddress.Parse(config.IPAddress);  ///127.0.0.1
Console.Write("Введите свое имя: ");
string? username = Console.ReadLine();
///Console.Write("Введите порт для приема сообщений: ");
if (!int.TryParse(config.SendMessagePort.ToString(), out var localPort)) return;
///Console.Write("Введите порт для отправки сообщений: ");
if (!int.TryParse(config.ReceiveMessagePort.ToString(), out var remotePort)) return;
Console.WriteLine();

try
{
    ///////////запуск приложения////////////

    // запускаем получение сообщений
    Task.Run(ReceiveMessageAsync);
    // запускаем ввод и отправку сообщений
    await SendMessageAsync();


}
catch (Exception ex)
{

    new Logger().LogErrorAsync(ex.Message).Wait();
    throw;
}



// отправка сообщений в группу
 async Task SendMessageAsync()
{




    try
    {


        using Socket sender = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        Console.WriteLine("Для отправки сообщений введите сообщение и нажмите Enter[Еслихотите     выйти из чата отправьте пустое сообщение.]");
        // отправляем сообщения
        while (true)
        {
            var message = Console.ReadLine(); // сообщение для отправки
                                              // если введена пустая строка, выходим из циклаи     завершаем ввод сообщений
            if (string.IsNullOrWhiteSpace(message))
            {
                Console.WriteLine("Вы уверены, что хотите покинуть чат?[Да/Нет]");
                string da = Console.ReadLine();
                if (da == "Да")
                {
                    break;
                }
                continue;
            }

            Console.SetCursorPosition(0, Console.CursorTop - 1);
            Console.Write("\r" + new string(' ', Console.BufferWidth) + "\r");
            Console.WriteLine($"[{DateTime.Now}]" + $" {username}: " + message);


            // иначе добавляем к сообщению имя пользователя
            message = $"{username}: {message}";
            byte[] data = Encoding.UTF8.GetBytes(message);
            // и отправляем на 127.0.0.1:remotePort
            await sender.SendToAsync(data, new IPEndPoint(localAddress, remotePort));
        }
    }
        catch 
    {
        throw;
    }

    
}
// отправка сообщений
async Task ReceiveMessageAsync()
{
    byte[] data = new byte[65535]; // буфер для получаемых данных
    // сокет для прослушки сообщений
    using Socket receiver = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
    // запускаем получение сообщений по адресу 127.0.0.1:localPort
    receiver.Bind(new IPEndPoint(localAddress, localPort));
    while (true)
    {
        // получаем данные в массив data
        var result = await receiver.ReceiveFromAsync(data, new IPEndPoint(IPAddress.Any, 0));
        var message = Encoding.UTF8.GetString(data, 0, result.ReceivedBytes);
        // выводим сообщение

        Console.WriteLine($"[{DateTime.Now}]" + " " + message);
    }
}

