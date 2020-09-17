using KIRTStudentJournal.NetLib.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Threading.Tasks;


namespace KIRTStudentJournal.NetLib.Extensions
{
    public static class HttpContentExtensions
    {
        /// <summary>
        /// Прочитать как Json.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static async Task<JToken> ReadAsJson(this HttpContent message)
        {
            string str = await message.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<JToken>(str);
        }
        
        /// <summary>
        /// Прочитать как Json и привести к типу<typeparamref name="T"/>
        /// </summary>
        public static async Task<T> ReadAsJson<T>(this HttpContent message)
        {
            string str = await message.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(str);
        }

        /// <summary>
        /// Прочитать как Json и если <see cref="Error.IsError(JToken)"/> вызвать <see cref="Error.Throw"/>
        /// </summary>
        public static async Task<JToken> ReadAsJsonAndThrowIfError(this HttpContent message)
        {
            string str = await message.ReadAsStringAsync();
            JToken token = JsonConvert.DeserializeObject<JToken>(str);
            if (!Error.IsError(token))
                return token;
            else 
                token.ToObject<Error>().Throw();
            return default; // до этой строки никогда не дойдет
        }

        /// <summary>
        /// Прочитать как Json и если <see cref="Error.IsError(JToken)"/> вызвать <see cref="Error.Throw"/>
        /// Если ошибок нет то привести к <typeparamref name="T"/>
        /// </summary>
        public static async Task<T> ReadAsJsonAndThrowIfError<T>(this HttpContent message)
        {
            string str = await message.ReadAsStringAsync();
            JToken token = JsonConvert.DeserializeObject<JToken>(str);
            if (!Error.IsError(token))
                return token.ToObject<T>();
            else
                token.ToObject<Error>().Throw();
            return default; // до этой строки никогда не дойдет
        }
    }
}
