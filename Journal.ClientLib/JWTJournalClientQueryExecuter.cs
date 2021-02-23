using System;

using Leaf.xNet;

#nullable enable

namespace Journal.ClientLib
{
    /// <summary>
    ///     Клиент с передачей токена аутентификации JWT.
    /// </summary>
    internal class JWTJournalClientQueryExecuter : JournalClientQueryExecuterBase
    {
        public string JwtToken { get; set; } = string.Empty;


        public JWTJournalClientQueryExecuter(Uri uriBase)
        {
            _uriBase = uriBase ?? throw new ArgumentNullException(nameof(uriBase));
        }

        protected override Uri UriBase => _uriBase;

        protected override void UpdateToken()
        {
            
        }

        protected override HttpRequest CreateRequest( Uri uriBase, bool useToken )
        {
            HttpRequest request = new HttpRequest( uriBase );
            request.UserAgent = Http.ChromeUserAgent();
            if ( useToken )
                request.AddHeader( "Authorization", "Bearer " + JwtToken );
            return request;
        }

        private Uri _uriBase;
    }
}
