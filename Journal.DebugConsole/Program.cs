using System;
using System.Threading.Tasks;

using Journal.ClientLib;
using Journal.ClientLib.Infrastructure;
using Journal.ClientLib.Entities;
using Journal.Logging;

namespace Journal.DebugConsole
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Logger.Instance = new ConsoleLogger();

            JournalClient client;
            #region authroize
            try
            {
                client = await JournalClient.ConnectAsync(SERVER_URL, USER_LOGIN, USER_PASSWORD);
            }
            catch (Exception e) 
            {
                Logger.Instance.Fatal("Не удалось получить экземпляр клиента журнала");
                Logger.Instance.Fatal(e);
                return;
            }
            #endregion
        }

        private const string SERVER_URL = "",
                                USER_LOGIN = "",
                                USER_PASSWORD = "";
    }
}
