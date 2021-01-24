using System;
using Newtonsoft.Json;
using System.IO;

namespace Journal.Server.Config
{
    /// <summary>
    /// Класс конфигурации базы данных.
    /// </summary>
    public class JournalConfiguration
    {
        /// <summary>
        /// Секция конфигурации БД.
        /// </summary>
        [JsonProperty("db")]
        public JournalDbConfiguration Database { get; private set; }

        [JsonProperty("pause")]
        public bool Pause { get; private set; } = false;

        public static JournalConfiguration Single { get; private set; }
        
        public static void FromJsonString(string jsonSerializedString)
        {
            Single = JsonConvert.DeserializeObject<JournalConfiguration>(jsonSerializedString);
        }

        /// <param name="configFilePath">Путь к файлу.</param>
        /// <exception cref="Exception">Если не удалось прочитать или спарсить файл.</exception>
        /// <exception cref="FileNotFoundException">Если файл не найден.</exception>"
        public static void FromFile(string configFilePath)
        {
            if (File.Exists(configFilePath))
            {
                try
                {
                    using (StreamReader reader = File.OpenText(configFilePath))
                    {
                        FromJsonString(reader.ReadToEnd());
                    }
                }
                catch (Exception e)
                {
                    throw new Exception("Не удалось прочитать файл. Нет доступа к файлу или он поврежден.", e);
                }
            }
            else
                throw new FileNotFoundException(configFilePath);
        }
    }

    /// <summary>
    /// Секция конфигурации базы данных.
    /// </summary>
    public class JournalDbConfiguration
    {
        /// <summary>
        /// Строка подключения к БД.
        /// </summary>
        [JsonProperty("connection_string")]
        public string ConnectionString { get; private set; }
    }
}
