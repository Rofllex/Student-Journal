using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

using Journal.Common.Entities;

namespace Journal.ClientLib
{
    public interface IJournalClient
    {
        IUser CurrentUser { get; }
    }
    

    public class JournalClient : IJournalClient
    {
        public IUser CurrentUser => throw new NotImplementedException();

        public static JournalClient Connect(IPEndPoint endPoint, string login, string password)
        {
            throw new NotImplementedException();
        }

        public static Task<JournalClient> ConnectAsync(IPEndPoint endPoint, string login, string password)
        {
            throw new NotImplementedException();
        }
    }


}
