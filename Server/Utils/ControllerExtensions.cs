using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text;

namespace Journal.Server.Utils
{
    public static class ControllerExtensions
    {
        public static async Task<JToken> ReadJsonBody(this Controller controller, Encoding encoding = null, int bufferLength = 4096)
        {
            int dataLength = controller.Request.ContentLength.HasValue ? (int)controller.Request.ContentLength.Value : bufferLength;
            byte[] buffer = new byte[dataLength];
            int bytesRead = await controller.Request.Body.ReadAsync(buffer, 0, buffer.Length);
            if (encoding == null)
                encoding = Encoding.UTF8;
            return JsonConvert.DeserializeObject<JToken>(encoding.GetString(buffer, 0, bytesRead));
        }
    }
}
