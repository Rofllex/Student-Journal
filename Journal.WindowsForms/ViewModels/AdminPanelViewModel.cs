using System;
using System.Collections.Generic;
using System.Text;

using Journal.ClientLib;


namespace Journal.WindowsForms.ViewModels
{
    public class AdminPanelViewModel : ViewModel
    {
        public AdminPanelViewModel(JournalClient client) 
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            if (_client.AdminPanel == null)
                throw new InvalidOperationException("Пользователь не является администратором");
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

        public void LoadUsers(object sender, EventArgs e)
        {
            var adminPanel = _client.AdminPanel;
            
        }

        private int _usersOffset = 0
            , _usersCount = 0;
        private bool _canLoadUsers;

        private JournalClient _client;
    }
}
