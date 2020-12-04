using System.Collections.Generic;

namespace Journal.Common.Models
{
    /// <summary>
    /// Ошибка запроса связанная с неправильной передачей параметров
    /// </summary>
    public class WrongParametersError : RequestError
    {
        public Dictionary<string, string> StringToParameterDictionary { get; private set; }

        public WrongParametersError(string message, Dictionary<string, string> stringToParameterDictionary) : base (message)
        {
            StringToParameterDictionary = stringToParameterDictionary;
        }

        public WrongParametersError(Dictionary<string,string> stringToParametersDictionary) : this ("Неверные аргументы", stringToParametersDictionary)
        {
        }
    }
}
