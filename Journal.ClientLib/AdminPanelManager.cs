using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Journal.ClientLib.Entities;
using Journal.ClientLib.Infrastructure;

#nullable enable

namespace Journal.ClientLib
{
    public class AdminPanelManager : ControllerManagerBase
    {
        public AdminPanelManager( IClientQueryExecuter queryExecuter ) : base (queryExecuter) 
        {
        }

        private AdminPanelManager() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        /// <exception cref="WrongStatusCodeException"></exception>
        /// <exception cref="WrongStatusCodeException"></exception>
        public async Task<User[]?> GetUsersAsync(int offset, int count )
        {
            if ( count < 0 )
                throw new ArgumentOutOfRangeException( nameof( count ) );
            else if ( count == 0 )
                return Array.Empty<User>();

            if ( offset < 0 )
                offset = 0;
            return await QueryExecuter.ExecuteGetQuery<User[]>("Users", "Get", new Dictionary<string, string>() 
            {
                ["offset"] = offset.ToString(),
                ["count"] =count.ToString()
            });
        }

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
            , string lastName
            , string phoneNumber
            , StudentGroup? group = null )
        {
            if ( !string.IsNullOrWhiteSpace( login ) )
            {
                if ( !string.IsNullOrWhiteSpace( password ) )
                {
                    if ( !string.IsNullOrWhiteSpace( firstName ) )
                    {
                        if ( !string.IsNullOrWhiteSpace( surname ) )
                        {
                            try
                            {
                                Dictionary<string, string> args = new Dictionary<string, string>()
                                {
                                    ["login"] = login,
                                    ["password"] = password,
                                    ["firstName"] = firstName,
                                    ["surname"] = surname
                                };

                                if (lastName.Length > 0)
                                    args["lastName"] = lastName;
                                if (phoneNumber.Length > 0)
                                    args["phoneNumber"] = phoneNumber;
                                if (group != null)
                                    args["groupId"] = group.Id.ToString();

                                await QueryExecuter.ExecuteGetQuery("Users", "CreateStudent", args);

                                //await _queryExecuter.ExecuteQuery( "Users", "CreateStudent", new string[]
                                //{
                                //    $"login={login}",
                                //    $"password={password}",
                                //    $"firstName={firstName}",
                                //    $"surname={surname}",
                                //    lastName.Length > 0 ? $"lastName={lastName}" : string.Empty,
                                //    phoneNumber.Length > 0 ? $"phoneNumber={phoneNumber}" : string.Empty,
                                //    group != null ? $"groupId={group.Id}" : string.Empty
                                //} );
                                return true;

                            }
                            catch (WrongStatusCodeException)
                            {
                                return false;
                            }
                        }
                        else
                            throw new ArgumentNullException( nameof( surname ) );
                    }
                    else
                        throw new ArgumentNullException( nameof(firstName) );
                }
                else
                    throw new ArgumentNullException( nameof( password ) );
            }
            else
                throw new ArgumentNullException( nameof( login ) );
        }
    }
}
