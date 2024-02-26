using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuskyChat
{
    public class Configuration
    {
        public string IPAddress { get; set; }
        public int SendMessagePort { get; set; }
        public int ReceiveMessagePort { get; set; }
    }


    public class ConfigurationReader
    {


        public Configuration ReadConfiguration(string filePath)
        {

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("File not found");
            }

            string json = File.ReadAllText(filePath);   //////Directory.GetCurrentDirectory() + "/" + 
            return JsonConvert.DeserializeObject<Configuration>(json);
        }
    }
}