using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.ComponentModel;

using Journal.ClientLib;
using Journal.ClientLib.Infrastructure;
using Journal.ClientLib.Entities;
using Journal.WindowsForms.Models;
using System.Collections;

namespace Journal.WindowsForms.ViewModels
{
    public class AdminPanelViewModel : ViewModel
    {
        public AdminPanelViewModel(JournalClient client, Form form) 
        {
            _callerForm = form ?? throw new ArgumentNullException(nameof(form));
            _client = client ?? throw new ArgumentNullException(nameof(client));
            IControllerManagerFactory factory = new ControllerManagerFactory();
            this._adminPanel = factory.Create<AdminManager>(_client);
            Task.Run(() => 
            {
                Task<int> usersCountTask = _adminPanel.GetUsersCountAsync();
                usersCountTask.Wait();
                UsersCount = usersCountTask.Result;
                CurrentPage = 0;
            });
        }
        
        public int CurrentPage
        {
            get => _currentPage;
            set
            {
                ChangeProperty(ref _currentPage, value);
                _ = _LoadUsers();
            }
        }

        public bool CanScrollLeft
        {
            get => _canScrollLeft;
            set => ChangeProperty(ref _canScrollLeft, value);
        }

        public bool CanScrollRight
        {
            get => _canScrollRight;
            set => ChangeProperty(ref _canScrollRight, value);
        }

        public int UsersCount
        {
            get => _usersCount;
            set => ChangeProperty(ref _usersCount, value);
        }

        public BindingList<UserModel> Users { get; private set; } = new BindingList<UserModel>();


        public void ScrollRight(object sender, EventArgs e)
            => CurrentPage++;
        
        public void ScrollLeft(object sender, EventArgs e)
            => CurrentPage--;
        
        public void LogoutClicked(object sender, EventArgs e)
        {
            _callerForm.DialogResult = DialogResult.Retry;
            _callerForm.Close();
        }

        private Form _callerForm;

        private const int USERS_PER_PAGE = 50;

        private int _currentPage = 0;
        private bool _canScrollLeft, _canScrollRight;
        private int _usersCount = 0;

        private AdminManager _adminPanel;
        private JournalClient _client;
        
        private void _AddUsers(User[] users)
        {
            foreach (UserModel model in UserModel.FromUsers(users))
                Users.Add(model);
        }

        private async Task _LoadUsers()
        {
            User[] users = await _adminPanel.GetUsersAsync(_currentPage * USERS_PER_PAGE, USERS_PER_PAGE);
            if (users != null)
            {
                _ClearUsers();
                if (users.Length > 0)
                {
                    _AddUsers(users);
                    CanScrollRight = (_currentPage + 1) * USERS_PER_PAGE < UsersCount;
                    CanScrollLeft = _currentPage > 0;
                }
                else
                    CanScrollLeft = CanScrollRight = false;
            }
        }

        private void _ClearUsers()
            => Users.Clear();
    }
}
