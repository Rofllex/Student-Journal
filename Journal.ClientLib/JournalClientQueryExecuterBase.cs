using System;
using System.Threading.Tasks;
using System.Collections.Generic;

using Newtonsoft.Json;

using Leaf.xNet;

#nullable enable

namespace Journal.ClientLib
{
    internal abstract class JournalClientQueryExecuterBase : IJournalClientQueryExecuter
    {
        public Task<T?> ExecuteQuery<T>( string controller, string method, ICollection<string>? args = null, bool useToken = true ) where T : class
        {
            return Task.Run( () =>
            {
                using ( HttpRequest request = CreateRequest( UriBase, useToken ) )
                {
                    string getArgumentsLine = CreateGetArgumentsLine( args );
                    if ( getArgumentsLine.Length > 0 )
                        getArgumentsLine = getArgumentsLine.Insert( 0, "?" );
                    HttpResponse response;
                    string relativeUrl = $"/api/{controller}/{method}{getArgumentsLine}";
                    try
                    {
                        response = request.Get( relativeUrl );
                    }
                    catch (HttpException ex) when (ex.Status == HttpExceptionStatus.ConnectFailure)
                    {
#if DEBUG
                        throw new ConnectFaillureException( new Uri( request.BaseAddress, relativeUrl ).ToString() );
#else
                        throw new ConnectFaillureException( request.BaseAddress.ToString() );

#endif
                    }

                    if ( response.IsOK )
                    {
                        string responseString = response.ToString();
                        if ( !string.IsNullOrWhiteSpace( responseString ) )
                            return JsonConvert.DeserializeObject<T>( responseString );
                        else
                            return null;
                    }
                    else
                        throw new WrongStatusCodeException( response.StatusCode );
                }
            } );
        }

        public Task<object?> ExecuteQuery( string controller, string method, ICollection<string>? args = null, bool useToken = true )
            => ExecuteQuery<object>( controller, method, args, useToken );

        public Task<T?> ExecutePostQuery<T>( string controller, string method, IDictionary<string, object>? args = null, bool useToken = true ) where T : class
        {
            return Task.Run<T?>( () =>
            {
                using ( HttpRequest request = CreateRequest( UriBase, useToken ) )
                {
                    HttpResponse response;
                    string url = $"/{controller}/{method}";
                    if ( args == null || args.Count == 0 )
                    {
                        response = request.Post( url );
                    }
                    else
                    {
                        StringContent postBody = new StringContent( JsonConvert.SerializeObject( args ) );
                        response = request.Post( url, postBody );
                    }

                    string responseString = response.ToString();
                    if ( response.IsOK )
                        return JsonConvert.DeserializeObject<T>( responseString );
                    else
                        throw new WrongStatusCodeException( response.StatusCode, response: responseString );
                }
            } );
        }

        public Task<object?> ExecutePostQuery( string controller, string method, IDictionary<string, object>? args = null, bool useToken = true )
            => ExecutePostQuery<object>( controller, method, args, useToken );



        protected abstract Uri UriBase { get; }

        protected abstract void UpdateToken();

        protected abstract HttpRequest CreateRequest( Uri uriBase, bool useToken = true );
        
        protected virtual string CreateGetArgumentsLine( ICollection<string>? getArguments )
        {
            string argumentsLine = string.Empty;
            if ( getArguments != null )
            {
                using ( IEnumerator<string> enumerator = getArguments.GetEnumerator() )
                {
                    if ( enumerator.MoveNext() )
                    {
                        argumentsLine = enumerator.Current;
                        while ( enumerator.MoveNext() )
                            argumentsLine += "&" + enumerator.Current;
                        return argumentsLine;
                    }
                    else
                        return argumentsLine;
                }
            }
            else
                return argumentsLine;
        }
    }
}
