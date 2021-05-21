using System;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

using Journal.ClientLib;
using Journal.Common.Entities;
using Journal.Common.Models;
using Journal.WindowsForms.Models;

namespace Journal.WindowsForms.ViewModels
{
    public class CreateUserViewModel : ViewModel
    {
        public CreateUserViewModel(IJournalClient client)
        {
            this._client = client ?? throw new ArgumentNullException(nameof(client));
            _adminManager = new AdminManager(client.QueryExecuter);

            var roles = ((UserRole[])Enum.GetValues(typeof(UserRole))).Except(ExceptRoles);
            Roles = new BindingList<RoleModel>(roles.Select(r => new RoleModel(r)).ToList());
            SelectedRoleIndex = 0;


        }

        public string Login 
        {
            get => _login;
            set => ChangeProperty(ref _login, value);
        }

        public string Password 
        {
            get => _password;
            set => ChangeProperty(ref _password, value);
        }

        public string FirstName 
        {
            get => _firstName;
            set => ChangeProperty( ref _firstName, value );
        }

        public string Surname 
        {
            get => _surname;
            set => ChangeProperty( ref _surname, value );
        }

        public string Patronymic
        {
            get => _lastName;
            set => ChangeProperty( ref _lastName, value );
        }

        public string PhoneNumber
        {
            get => _phoneNumber;
            set => ChangeProperty( ref _phoneNumber, value );
        }

        public BindingList<RoleModel> Roles { get; }

        public int SelectedRoleIndex 
        {
            get => _selectedRoleIndex;
            set => ChangeProperty(ref _selectedRoleIndex, value);
        }

        public async void CreateUser(object sender, EventArgs e)
        {
            string errorMessage = null;
            if (string.IsNullOrWhiteSpace(Login))
                errorMessage = "Логин не может быть пустым";
            else if (Login.Length < 6)
                errorMessage = "Логин не может быть меньше 6 символов";

            if (string.IsNullOrWhiteSpace(Password))
                errorMessage = "Пароль не может быть пустым";
            else if (Password.Length < 6)
                errorMessage = "Пароль не может быть меньше 6 символов";

            if (string.IsNullOrWhiteSpace(FirstName))
                errorMessage = "Поле \"имя\" не заполнено";
            else if (FirstName.Length < 3)
                errorMessage = "Имя не может быть меньше 3 символов";

            if (string.IsNullOrWhiteSpace(Surname))
                errorMessage = "Поле \"фамилия\" не заполнено";
            else if (Surname.Length < 3)
                errorMessage = "Фамилия не может быть меньше 3 символов";

            if (SelectedRoleIndex < 0)
                errorMessage = "Роль не выбрана";

            if (errorMessage != null)
            {
                _ShowError(errorMessage);
                return;
            }

            try
            {
                ClientLib.Entities.User user = await _adminManager.CreateUserAsync(Login, Password, Roles[SelectedRoleIndex].Original, FirstName, Surname, Patronymic, PhoneNumber);
            }
            catch (RequestErrorException ree) 
            {
                _ShowError(ree.Message);
            }
            catch (Exception ex)
            {
                _ShowError(ex.ToString());
            }
        }

        private string _firstName
                        , _surname
                        , _lastName
                        , _phoneNumber
                        , _login
                        , _password;

        private int _selectedRoleIndex = -1;

        private IJournalClient _client;
        private AdminManager _adminManager;

        private void _ShowError(string text)
            => MessageBox.Show(text, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);

        private static readonly IImmutableList<UserRole> ExceptRoles = ImmutableList.Create(UserRole.StudentParent);
    }

    
}
