using System;
using System.Collections.Generic;
using System.ComponentModel;

using Journal.Common.Entities;
using Journal.Common.Extensions;
using Journal.ClientLib.Entities;

namespace Journal.WindowsForms.Models
{

    public class UserModel : GenericModel<User>
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
                using IEnumerator<UserRole> rolesEnumerator = Original.Role.GetContainsFlags().GetEnumerator();
                if (rolesEnumerator.MoveNext())
                {
                    string result = Culture.RoleFormatProvider.Format(rolesEnumerator.Current);
                    while (rolesEnumerator.MoveNext())
                        result += $", { Culture.RoleFormatProvider.Format(rolesEnumerator.Current) }";
                    return result;
                }
                else
                    return string.Empty;
            }
        }

        public override string ToString()
            => $"{FirstName} {Surname} {Patronymic}";
    }
}
