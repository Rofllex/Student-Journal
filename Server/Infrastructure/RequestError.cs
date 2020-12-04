using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Journal.Server.Infrastructure
{
    public class RequestError
    {
        public string Message { get; set; }
    }

    public class WrongParametersError : RequestError
    {
        public Dictionary<string,string> StringToParameterDictionary { get; private set; }

        public WrongParametersError(Dictionary<string, string> stringToParameterDictionary )
        {
            StringToParameterDictionary = stringToParameterDictionary;
        }
    }
}
