using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Journal.ClientLib;
using Journal.ClientLib.Infrastructure;
using Journal.ClientLib.Entities;
using System.Windows.Forms;

namespace Journal.WindowsForms.ViewModels
{
    public class AdminPanelViewModel : ViewModel
    {
        public AdminPanelViewModel(JournalClient client, DataGridView usersGridView) 
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            IControllerManagerFactory factory = new ControllerManagerFactory();
            this._adminPanel = factory.Create<AdminPanelManager>(_client);
            this._usersGridView = usersGridView ?? throw new ArgumentNullException(nameof(usersGridView));
        }
        
        public string UsersOffsetTextBox
        {
            get => _usersOffset.ToString();
            set
            {
                if (int.TryParse(value, out int offset))
                {
                    if (offset < 0)
                        offset = 0;
                    base.ChangeProperty(ref _usersOffset, offset);
                    InvokePropertyChanged();
                }
                else
                    throw new ArgumentException("Неверное значение");
            }
        }

        public string UsersCountTextBox
        {
            get => _usersCount.ToString();
            set
            {
                if (int.TryParse(value, out int count))
                {
                    if (count < 0)
                        count = 0;

                    base.ChangeProperty(ref _usersCount, count);
                    InvokePropertyChanged();
                }
                else
                    throw new ArgumentException("Неверное значение");
            }
        }

        public bool CanLoadUsers
        {
            get => _canLoadUsers;
            set => ChangeProperty(ref _canLoadUsers, value);
        }

        public async void LoadUsers(object sender, EventArgs e)
        {
            IControllerManagerFactory factory = new ControllerManagerFactory();
            AdminPanelManager adminPanel = factory.Create<AdminPanelManager>(_client);
            User[] users = await adminPanel.GetUsersAsync(_usersOffset, _usersCount);
            if (users != null)
            {
                _ClearUsers();
                if (users.Length > 0)
                    _AddUsers(users);
            }
        }

        private int _usersOffset = 0
            , _usersCount = 0;
        private bool _canLoadUsers = true;

        private AdminPanelManager _adminPanel;
        private JournalClient _client;
        private DataGridView _usersGridView;

        private void _AddUsers(User[] users)
        {
            for (int userIndex = 0; userIndex < users.Length; userIndex++)
            {
                User currentUser = users[userIndex];
                _usersGridView.Rows.Add(currentUser.Id
                        , currentUser.FirstName
                        , currentUser.Surname
                        , currentUser.LastName ?? string.Empty
                        , currentUser.PhoneNumber ?? string.Empty
                        , currentUser.Role.ToString()
                        , currentUser);
            }
        }

        private void _ClearUsers()
            => _usersGridView.Rows.Clear();
    }
}
