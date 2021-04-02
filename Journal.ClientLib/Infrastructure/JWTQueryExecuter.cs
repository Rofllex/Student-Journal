﻿using System.Threading.Tasks;
using System.Collections.Generic;

using Newtonsoft.Json;
using System.Net.Http;
using System;
using Journal.Common.Models;
using System.Diagnostics;
using Newtonsoft.Json.Linq;

#nullable enable

namespace Journal.ClientLib.Infrastructure
{
    public class JWTQueryExecuter : IClientQueryExecuter
    {
        public JWTQueryExecuter(Uri uriBase, string? jwtToken)
        {
            JWTToken = jwtToken;
            _uriBase = uriBase ?? throw new ArgumentNullException(nameof(uriBase));
        }

        public string? JWTToken 
        { 
            get => _authorizedHttpClient.DefaultRequestHeaders.Authorization?.Parameter;
            set
            {
                if (value == null)
                    _authorizedHttpClient.DefaultRequestHeaders.Remove("Authorization");
                else
                    _authorizedHttpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", value);
            } 
        }

        public async Task<T> ExecuteGetQuery<T>(string controller, string method, IEnumerable<KeyValuePair<string, string>>? getArgs = null, bool useToken = true)
        {
            Uri uri = _CreateUri(_uriBase, controller, method, getArgs);
            HttpResponseMessage responseMessage = await _CatchSocketErrors(uri, _CreateClient(useToken), hc => hc.GetAsync(uri));
            if (responseMessage.IsSuccessStatusCode)
            {
                string? contentType = responseMessage.Content.Headers.ContentType?.MediaType ?? null;
                if (contentType == "application/json")
                {
                    string responseString = await responseMessage.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(responseString);
                }
                else
                {
                    throw new ExecuteQueryException($"Не удасться привести ответ к типу { typeof(T).FullName } т.к. тип ответа не соответствует \"application/json\" ContentType: \"{ contentType ?? "null" }\"", null);
                }
            }
            else
            {
                await _HandleErrorRequest(responseMessage);
                //  Остановка в этом месте не произойдет т.к. строка выше выбросит исключение
                throw new InvalidOperationException();
            }
        }

        public async Task ExecuteGetQuery(string controller, string method, IEnumerable<KeyValuePair<string, string>>? getArgs = null, bool useToken = true)
        {
            Uri uri = _CreateUri(_uriBase, controller, method, getArgs);
            HttpResponseMessage response = await _CatchSocketErrors(uri, _CreateClient(useToken), hc => hc.GetAsync(uri));
            if (!response.IsSuccessStatusCode)
            {
                await _HandleErrorRequest(response);
                throw new InvalidOperationException();
            }
        }

        public async Task<T> ExecutePostQuery<T>(string controller, string method, object? postBody, IEnumerable<KeyValuePair<string, string>>? getArgs = null, bool useToken = true)
        {
            Uri uri = _CreateUri(_uriBase, controller, method, getArgs);
            HttpContent content;
            if (postBody != null)
                content = new StringContent(JsonConvert.SerializeObject(postBody));
            else
                content = new StringContent(string.Empty);
            HttpResponseMessage response = await _CatchSocketErrors(uri, _CreateClient(useToken), hc => hc.PostAsync(uri, content));

            if (response.IsSuccessStatusCode)
            {
                string? contentType = response.Content.Headers.ContentType?.MediaType ?? null;
                if (contentType != null)
                {
                    return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
                }
                else
                    throw new ExecuteQueryException($"Не удасться привести ответ к типу { typeof(T).FullName } т.к. тип ответа не соответствует \"application/json\" ContentType: \"{ contentType ?? "null" }\"", null);
            }
            else
            {
                await _HandleErrorRequest(response);
                throw new InvalidOperationException();
            }
        }

        public async Task ExecutePostQuery(string controller, string method, object? postBody, IEnumerable<KeyValuePair<string, string>>? getArgs = null, bool useToken = true)
        {
            Uri uri = _CreateUri(_uriBase, controller, method, getArgs);
            HttpContent content;
            if (postBody != null)
                content = new StringContent(JsonConvert.SerializeObject(postBody));
            else
                content = new StringContent(string.Empty);
            HttpResponseMessage response = await _CatchSocketErrors(uri, _CreateClient(useToken), hc => hc.PostAsync(uri, content));
            
            if (!response.IsSuccessStatusCode)
            {
                await _HandleErrorRequest(response);
                throw new InvalidOperationException();
            }
        }


        private Uri _uriBase;
        private HttpClient _authorizedHttpClient = new HttpClient();
        private HttpClient _unauthorizedHttpClient = new HttpClient();

        private HttpClient _CreateClient(bool useToken)
        {
            if (useToken)
                return _authorizedHttpClient;
            else
                return _unauthorizedHttpClient;
        }

        private Uri _CreateUri(Uri uriBase, string controller, string method, IEnumerable<KeyValuePair<string,string>>? getArgs)
        {
            UriBuilder uriBuilder = new UriBuilder(uriBase);
            uriBuilder.Path = $"api/{controller}/{method}";

            if (getArgs != null)
            {
                using (IEnumerator<KeyValuePair<string, string>> enumerator = getArgs.GetEnumerator())
                {
                    if (enumerator.MoveNext())
                    {
                        KeyValuePair<string, string> current = enumerator.Current;
                        string argsLine = $"{current.Key}={current.Value}";
                        while (enumerator.MoveNext())
                        {
                            current = enumerator.Current;
                            argsLine += $"&{current.Key}={current.Value}";
                        }
                        uriBuilder.Query = argsLine;
                    }
                }
            }

            return uriBuilder.Uri;
        }        

        private async Task<HttpResponseMessage> _CatchSocketErrors(Uri uri, HttpClient httpClient, Func<HttpClient, Task<HttpResponseMessage>> func)
        {
            try
            {
                return await func(httpClient);
            }
            catch (Exception ex)
            {
                throw new ConnectFaillureException(uri, ex);
            }
        }
    
        private async Task _HandleErrorRequest(HttpResponseMessage response)
        {
            Debug.Assert(!response.IsSuccessStatusCode);
            string? contentType = response.Content.Headers.ContentType?.MediaType ?? null;
            if (contentType == "application/json")
            {
                string contentString = await response.Content.ReadAsStringAsync();
                RequestError requestError = JsonConvert.DeserializeObject<RequestError>(contentString);
                requestError.Throw();
                throw new InvalidOperationException();
            }
            else
                throw new ExecuteQueryException($"Ошибка при выполнении запроса. ContentType: \"{ (contentType ?? "null") }\". StatusCode: { (int)response.StatusCode}({response.ReasonPhrase}).", (int)response.StatusCode, null, null);
        }
    }

    
}
