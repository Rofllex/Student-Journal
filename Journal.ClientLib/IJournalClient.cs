using Journal.Common.Entities;
using Journal.ClientLib.Infrastructure;

#nullable enable

namespace Journal.ClientLib
{
    /// <summary>
    ///     Базовый код клиента дневника.
    /// </summary>
    public interface IJournalClient
    {
        /// <summary>
        ///     Абстракция вызова методов сервера.
        /// </summary>
        IClientQueryExecuter QueryExecuter { get; }
        
        /// <summary>
        ///     Авторизованный пользователь
        /// </summary>
        IUser User { get; }
        
        /// <summary>
        /// Проверить токен авторизации
        /// </summary>
        /// <returns></returns>
        bool CheckToken();

        /// <summary>
        /// Обновить токен авторизации
        /// </summary>
        void RefreshToken();
    }
}
