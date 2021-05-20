using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Forms;

using Journal.ClientLib;
using Journal.ClientLib.Entities;
using Journal.ClientLib.Infrastructure;
using Journal.Common.Entities;
using Journal.WindowsForms.Models;

namespace Journal.WindowsForms.ViewModels
{
    public class TeacherFormViewModel : ViewModel
    {
        public TeacherFormViewModel(Form callerForm, IJournalClient journalClient)
        {
            _client = journalClient ?? throw new ArgumentNullException(nameof(journalClient));
            _dbManager = new DatabaseManager(journalClient);
            _callerForm = callerForm ?? throw new ArgumentNullException(nameof(callerForm));
            _Initialize();
        }

        public string UserName 
        {
            get => _userName;
            set => ChangeProperty(ref _userName, value);
        }

        /// <summary>
        ///     Список предметов.
        /// </summary>
        public BindingList<SubjectModel> Subjects { get; } = new BindingList<SubjectModel>();

        /// <summary>
        ///     Индекс выбранного предмета.
        /// </summary>
        public int SelectedSubjectIndex
        {
            get => _selectedSubjectIndex;
            set => ChangeProperty(ref _selectedSubjectIndex, value);
        }

        public BindingList<StudentGroupModel> Groups { get; } = new BindingList<StudentGroupModel>();

        public int GroupsSelectedIndex
        {
            get => _groupsSelectedIndex;
            set 
            {
                base.ChangeProperty(ref _groupsSelectedIndex, value);
                if (_groupsSelectedIndex >= 0)
                {
                    _ClearSubjects();
                    _LoadSubjects((StudentGroup)Groups[_groupsSelectedIndex]);
                }
            }
        }


        public void LogoutClicked(object sender, EventArgs __)
            => Program.LogoutForm(_callerForm);

     
        public void ShowJournalForm(object _, EventArgs __)
        {
            if (SelectedSubjectIndex < 0)
                return;
            Subject selectedSubject = (Subject)Subjects[SelectedSubjectIndex];
            StudentGroup selectedGroup = (StudentGroup)Groups[GroupsSelectedIndex];
            using Forms.GradesForm gradesForm = new Forms.GradesForm(_client, selectedSubject, selectedGroup, allowEdit: true);
            _callerForm.Visible = false;
            gradesForm.ShowDialog();
            _callerForm.Visible = true;
        }


        private string _userName = string.Empty;
        private IJournalClient _client;
        private DatabaseManager _dbManager;
        private int _selectedSubjectIndex = -1;
        private Form _callerForm;
        private int _groupsSelectedIndex = -1;

        private async void _Initialize()
        {
            UserName = $"{_client.User.FirstName} { _client.User.Surname }";

            var groups = await _dbManager.GetGroups(0, 1000);
            this._AddGroups(groups);
            if (groups.Length > 0)
                GroupsSelectedIndex = 0;
            //var subjects = await _dbManager.GetSubjects(0, 1000);
            //_AddSubjects(subjects);
        }

        private async void _LoadSubjects(StudentGroup group)
        {
            var subjects = await _dbManager.GetSpecialtySubjects(group.SpecialtyId);
            _AddSubjects(subjects);
        }

        private void _AddSubjects(IEnumerable<Subject> subjects)
        {
            foreach (Subject subj in subjects)
                Subjects.Add(new SubjectModel(subj));
        }
        
        private void _ClearSubjects()
            => Subjects.Clear();
    
        private void _AddGroups(IEnumerable<StudentGroup> groups)
        {
            foreach (StudentGroup group in groups)
                Groups.Add(new StudentGroupModel(group));
        }

        
    }
}
