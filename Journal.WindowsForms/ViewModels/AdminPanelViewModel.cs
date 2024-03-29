﻿using System;
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
using System.Linq;

namespace Journal.WindowsForms.ViewModels
{
    public class AdminPanelViewModel : ViewModel
    {
        public AdminPanelViewModel(JournalClient client, Form form, ContextMenuStrip contextMenu) 
        {
            _callerForm = form ?? throw new ArgumentNullException(nameof(form));
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _contextMenu = contextMenu ?? throw new ArgumentNullException(nameof(contextMenu));
            IControllerManagerFactory factory = new ControllerManagerFactory();
            this._adminPanel = factory.Create<AdminManager>(_client);
            this._usersManager = factory.Create<UsersManager>(_client);
            Task.Run(() => 
            {
                Task<int> usersCountTask = _adminPanel.GetUsersCountAsync();
                usersCountTask.Wait();
                UsersCount = usersCountTask.Result;
                CurrentPage = 1;
            });

            Users.Add(new UserModel(new User(0, "", "", "", "")));
        }
        
        public int CurrentPage
        {
            get => _currentPage;
            set
            {
                ChangeProperty(ref _currentPage, value);
                _LoadUsers(value - 1);
                //_callerForm.Refresh();
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

        public BindingList<UserModel> Users { get; } = new BindingList<UserModel>();


        public void ScrollRight(object sender, EventArgs e)
            => CurrentPage++;
        
        public void ScrollLeft(object sender, EventArgs e)
            => CurrentPage--;
        
        public void LogoutClicked(object sender, EventArgs e)
        {
            _callerForm.DialogResult = DialogResult.Retry;
            _callerForm.Close();
        }

        public async void UsersCellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {


            if (e.Button == MouseButtons.Right)
            {
                if (e.RowIndex > -1 && e.RowIndex < Users.Count)
                {
                    UserModel selectedUser = Users[e.RowIndex];
                    if (selectedUser.Original.Role.HasFlag(Common.Entities.UserRole.Student))
                    {
                        _contextMenu.Items.Clear();
                        Student student = await _usersManager.GetStudentByIdAsync(selectedUser.Original.Id);
                        _contextMenu.Items.Add("Редактировать", null, (_, __) => 
                        {
                            using Forms.EditStudentForm editStudentForm = new Forms.EditStudentForm(_client, student);
                            editStudentForm.ShowDialog();
                        });
                        _contextMenu.Show(Cursor.Position);
                    }
                }
            }
        }

        private Form _callerForm;

        private const int USERS_PER_PAGE = 15;

        private int _currentPage = 1;
        private bool _canScrollLeft, _canScrollRight;
        private int _usersCount = 0;

        private AdminManager _adminPanel;
        private JournalClient _client;
        private UsersManager _usersManager;

        private ContextMenuStrip _contextMenu;

        private async void _LoadUsers(int pageIndex)
        {
            await Task.Delay(50);

            User[] users;
            try
            {
                users = await _adminPanel.GetUsersAsync(pageIndex * USERS_PER_PAGE, USERS_PER_PAGE);
            }
            catch (Exception e)
            {
                MessageBox.Show("Не удалось загрузить список пользователей.\nEx: " + e.ToString());

                return;
            }
            
            if (users != null && users.Length > 0)
            {
                while (Users.Count > users.Length)
                    Users.RemoveAt(Users.Count - 1);
                
                if (users.Length >= Users.Count)
                {
                    int userIndex = 0;
                    for (; userIndex < Users.Count; userIndex++)
                    {
                        Users[userIndex] = new UserModel(users[userIndex]);
                    }

                    while (userIndex < users.Length)
                    {
                        Users.Add(new UserModel(users[userIndex]));
                        userIndex++;
                    }
                }
                
                CanScrollRight = (_currentPage) * USERS_PER_PAGE < UsersCount;
                CanScrollLeft = _currentPage > 1;
            }
            InvokePropertyChanged(nameof(Users));
        }

    }
}
