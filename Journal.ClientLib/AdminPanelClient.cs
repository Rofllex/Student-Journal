using System;
using System.Threading.Tasks;

using Journal.ClientLib.Entities;


#nullable enable

namespace Journal.ClientLib
{
    public class AdminPanelClient
    {
        internal AdminPanelClient( IJournalClientQueryExecuter queryExecuter )
        {
            _queryExecuter = queryExecuter ?? throw new ArgumentNullException(nameof(queryExecuter));
        }

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
            return await _queryExecuter.ExecuteQuery<User[]>( "Users", "Get", new string[] { $"offset={offset}", $"count={count}" } );
        }

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
                                await _queryExecuter.ExecuteQuery( "Users", "CreateStudent", new string[]
                                {
                                    $"login={login}",
                                    $"password={password}",
                                    $"firstName={firstName}",
                                    $"surname={surname}",
                                    lastName.Length > 0 ? $"lastName={lastName}" : string.Empty,
                                    phoneNumber.Length > 0 ? $"phoneNumber={phoneNumber}" : string.Empty,
                                    group != null ? $"groupId={group.Id}" : string.Empty
                                } );
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
                        throw new ArgumentNullException( firstName );
                }
                else
                    throw new ArgumentNullException( nameof( password ) );
            }
            else
                throw new ArgumentNullException( nameof( login ) );
        }

        private IJournalClientQueryExecuter _queryExecuter;
    }
}
