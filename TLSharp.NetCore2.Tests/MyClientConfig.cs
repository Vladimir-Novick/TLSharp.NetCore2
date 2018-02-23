using Newtonsoft.Json;
using System;
using System.IO;

namespace TLSharp.Tests
{
    public class MyClientConfig
    {

        private static MyClientConfigData ConfigData = null;

        public static MyClientConfigData GetConfigData {
            get
            {
                if (ConfigData == null)
                {
                    GetConfiguration();
                }
                return ConfigData;
            }


             }

        public static  void GetConfiguration()
        {
            String configFile = "config.json";

            String pathToTheFile = Directory.GetCurrentDirectory()  + Path.DirectorySeparatorChar +  configFile;

            ConfigData = new MyClientConfigData();
            using (StreamReader file = File.OpenText(pathToTheFile))
            {
                JsonSerializer serializer = new JsonSerializer();
                ConfigData = (MyClientConfigData)serializer.Deserialize(file, typeof(MyClientConfigData));
            }

        }

    }
}
