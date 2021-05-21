using Journal.ClientLib;
using Journal.ClientLib.Entities;
using Journal.ClientLib.Infrastructure;
using Journal.Common.Models;
using Journal.WindowsForms.Models;

using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace Journal.WindowsForms.ViewModels
{
    public class EditStudentViewModel : ViewModel
    {
        public EditStudentViewModel(IJournalClient client, Student currentStudent, Form callerForm)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _callerForm = callerForm ?? throw new ArgumentNullException(nameof(callerForm));
            _student = currentStudent ?? throw new ArgumentNullException(nameof(currentStudent));
            _dbManager = new DatabaseManager(_client);
            _usersmanager = new UsersManager(_client);
            _originalGroupIndex = currentStudent.GroupId.HasValue ? currentStudent.GroupId.Value : Int32.MinValue;

            _Initialize();
        }

        public BindingList<StudentGroupModel> Groups { get; private set; }
        
        public int SelectedGroupIndex 
        {
            get => _currentGroupIndex;
            set => ChangeProperty(ref _currentGroupIndex, value);
        }

        public string StudentName
        {
            get => _studentName;
            set => ChangeProperty(ref _studentName, value);
        }

        public async void SaveChanges(object _, EventArgs __)
        {
            if (SelectedGroupIndex > -1)
            {
                StudentGroup selectedGroup = (StudentGroup)Groups[SelectedGroupIndex];
                if (selectedGroup.Id != _originalGroupIndex)
                {
                    try
                    {
                        await _dbManager.SetStudentGroupAsync(_student, selectedGroup);
                        _callerForm.Close();
                    }
                    catch(RequestErrorException ree) 
                    {
                        MessageBox.Show(ree.Message);
                    }
                    catch(ExecuteQueryException eqe)
                    {
                        MessageBox.Show(eqe.ToString());
                    }
                }
            }
        }

        private string _studentName;
        private int _currentGroupIndex = -1;

        private readonly int _originalGroupIndex;

        private IJournalClient _client;
        private DatabaseManager _dbManager;
        private UsersManager _usersmanager;
        private Student _student;
        private Form _callerForm;

        private async void _Initialize()
        {
            var groups = await _dbManager.GetGroups(0, 1000);
            Groups = new BindingList<StudentGroupModel>(groups.Select(g => new StudentGroupModel(g)).ToList());
            InvokePropertyChanged(nameof(Groups));
            if (_student.GroupId.HasValue)
            {
                StudentGroup currentUserGroup = await _dbManager.GetStudentGroupAsync(_student.GroupId.Value);
                SelectedGroupIndex = Groups.IndexOf(Groups.First(g => g.Original.Id == currentUserGroup.Id));
            }

            User user = await _usersmanager.GetUserByIdAsync(_student.UserId);
            StudentName = $"{ user.FirstName } { user.Surname }";
        }
    }

}
