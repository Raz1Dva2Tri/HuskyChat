using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuskyChat
{
    public class Logger
    {
        private FileStream _file;
        private StreamWriter _writer;

        public Logger(string path)
        {
            _file = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            _writer = new StreamWriter(_file);

        }

        public Logger()
        {
            string path = Directory.GetCurrentDirectory() + "/log.txt";
            _file = new FileStream(path, FileMode.Append, FileAccess.Write); // открываем файл в режиме добавления
            _writer = new StreamWriter(_file);
  
            Console.WriteLine($"Лог ошибки находится: {path}");
        }
        public async Task LogErrorAsync(string message)
        {
            var now = DateTime.Now;
            var log = $"[ERROR] {now:G} - {message} \n";
            await _writer.WriteAsync(log);
            this.Dispose();

        }

        public void Dispose()
        {
            _writer.Flush(); // сбрасываем буферы на диск
            _writer.Dispose(); // освобождаем ресурсы
        }
    }
}
