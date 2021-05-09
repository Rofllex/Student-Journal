using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

using Journal.ClientLib.Entities;
using Journal.ClientLib.Infrastructure;

using Newtonsoft.Json.Linq;

#nullable enable

namespace Journal.ClientLib
{
    [RoleManagerRestriction(Common.Entities.UserRole.Admin)]
    public class AdminManager : ControllerManagerBase
    {
        public AdminManager( IClientQueryExecuter queryExecuter ) : base (queryExecuter) 
        {
        }

        private AdminManager() { }

        /// <summary>
        ///     Получить список пользователей.
        /// </summary>
        /// <param name="offset">Смещение</param>
        /// <param name="count">Кол-во</param>
        /// <exception cref="WrongStatusCodeException"></exception>
        public async Task<User[]?> GetUsersAsync(int offset, int count )
        {
            if ( count < 0 )
                throw new ArgumentOutOfRangeException( nameof( count ) );
            else if ( count == 0 )
                return Array.Empty<User>();
                        
            if (offset < 0)
                throw new ArgumentOutOfRangeException(nameof(offset));
            JObject response = await QueryExecuter.ExecuteGetQuery<JObject>("Users", "GetUsers", new Dictionary<string, string>()
            {
                ["offset"] = offset.ToString(),
                ["count"] = count.ToString()
            });
            Debug.Assert(response["users"] != null);
            return response["users"]!.ToObject<User[]>();
        }

        /// <summary>
        ///     Получить кол-во студентов.
        /// </summary>
        public Task<int> GetUsersCountAsync()
            => QueryExecuter.ExecuteGetQuery<int>("Users", "GetUsersCount");
        

        /// <summary>
        ///     Создать студента.
        /// </summary>
        /// <param name="login">
        ///     Логин
        /// </param>
        /// <param name="password">
        ///     Пароль
        /// </param>
        /// <param name="firstName">
        ///     имя
        /// </param>
        /// <param name="surname">
        ///     Фамилия
        /// </param>
        /// <param name="lastName">
        ///     Отчество
        /// </param>
        /// <param name="phoneNumber">
        ///     Номер телефона
        /// </param>
        /// <param name="group">
        ///     Группа.
        /// </param>
        /// <returns></returns>
        public async Task<bool> CreateStudentAsync(string login
            , string password
            , string firstName
            , string surname
            , string? lastName = null
            , string? phoneNumber = null
            , StudentGroup? group = null)
        {
            if (string.IsNullOrWhiteSpace(login))
                throw new ArgumentNullException(nameof(login));
            if (!string.IsNullOrWhiteSpace(password))
                throw new ArgumentNullException(nameof(password));
            if (!string.IsNullOrWhiteSpace(firstName))
                throw new ArgumentNullException(nameof(firstName));
            if (!string.IsNullOrWhiteSpace(surname))
                throw new ArgumentNullException(nameof(surname));
            try
            {
                Dictionary<string, string> queryArgs = new Dictionary<string, string>()
                {
                    ["login"] = login,
                    ["password"] = password,
                    ["firstName"] = firstName,
                    ["surname"] = surname
                };

                if (!string.IsNullOrWhiteSpace(lastName))
                    queryArgs["lastName"] = lastName;
                if (!string.IsNullOrWhiteSpace(phoneNumber))
                    queryArgs["phoneNumber"] = phoneNumber;
                if (group != null)
                    queryArgs["groupId"] = group.Id.ToString();

                await QueryExecuter.ExecuteGetQuery("Users", "CreateStudent", queryArgs);
                return true;
            }
            catch (WrongStatusCodeException)
            {
                return false;
            }
        }
    
    }
}
