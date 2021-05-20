using System;
using System.Collections.Generic;
using System.ComponentModel;

using Journal.Common.Entities;
using Journal.Common.Extensions;
using Journal.ClientLib.Entities;

namespace Journal.WindowsForms.Models
{
    public class UserModel : GenericModuleBase<User>
    {
        public static UserModel[] FromUsers(IEnumerable<User> users)
        {
            using IEnumerator<User> enumerator = users.GetEnumerator();
            if (enumerator.MoveNext())
            {
                List<UserModel> modelsList = new List<UserModel>();
                do
                {
                    modelsList.Add(new UserModel(enumerator.Current));
                } while (enumerator.MoveNext());
                return modelsList.ToArray();
            }
            else
                return Array.Empty<UserModel>();
        }

        public static explicit operator User(UserModel model) => model.Original;

        public UserModel(User user) : base (user) { }

        [DisplayName("Id")]
        public int Id => Original.Id;

        [DisplayName("Имя")]
        public string FirstName => Original.FirstName;

        [DisplayName("Фамилия")]
        public string Surname => Original.Surname;

        [DisplayName("Отчество")
            , DefaultValue("")]
        public string Patronymic => Original.LastName;

        [DisplayName("Номер")
            , DefaultValue("")]
        public string PhoneNumber => Original.PhoneNumber;

        [DisplayName("Роль")]
        public string Role
        {
            get
            {
                using IEnumerator<UserRole> rolesEnumerator = Original.Role.GetFlags().GetEnumerator();
                if (rolesEnumerator.MoveNext())
                {
                    string result = rolesEnumerator.Current.ToString();
                    while (rolesEnumerator.MoveNext())
                        result += $", { rolesEnumerator.Current }";
                    return result;
                }
                else
                    return string.Empty;
            }
        }
    }
}
