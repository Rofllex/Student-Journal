using Journal.ClientLib;
using Journal.ClientLib.Entities;
using Journal.Common.Entities;
using Journal.ClientLib.Infrastructure;
using Journal.WindowsForms.Models;
using Journal.WindowsForms.Forms;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Forms;



namespace Journal.WindowsForms.ViewModels
{
    public class StudentFormViewModel : ViewModel
    {
        public StudentFormViewModel(Form callerForm, JournalClient journalClient)
        {
            this._journalClient = journalClient ?? throw new ArgumentNullException(nameof(journalClient));
            this._callerForm = callerForm ?? throw new ArgumentNullException(nameof(callerForm));

            IControllerManagerFactory factory = new ControllerManagerFactory();
            _userManager = factory.Create<UsersManager>(journalClient);
            _dbManager = factory.Create<DatabaseManager>(journalClient);

            _Initialize();
        }

        public string StudentName 
        {
            get => _studentName;
            set => base.ChangeProperty(ref _studentName, value);
        }

        public string GroupName
        {
            get => _groupName;
            set => base.ChangeProperty(ref _groupName, value);
        }

        public BindingList<SubjectModel> Subjects { get; } = new BindingList<SubjectModel>();

        public int SelectedSubjectIndex
        {
            get => _selectedSubjectIndex;
            set => ChangeProperty(ref _selectedSubjectIndex, value);
        }

        public void ShowGrades(object _, EventArgs __)
        {
            if (SelectedSubjectIndex < 0)
                return;
            Subject subject = (Subject)Subjects[SelectedSubjectIndex];
            GradesForm form = new GradesForm(_journalClient, subject, _currentGroup);
            _callerForm.Visible = false;
            form.ShowDialog();
            _callerForm.Visible = true;
        }

        public void ExitClicked(object _, EventArgs __)
            => Program.LogoutForm(_callerForm);
        

        private JournalClient _journalClient;
        private UsersManager _userManager;
        private DatabaseManager _dbManager;
        private string _studentName;
        private string _groupName;
        private int _selectedSubjectIndex = -1;
        private StudentGroup _currentGroup;
        private Form _callerForm;

        private async void _Initialize()
        {
            IUser user = _journalClient.User;
            StudentName = $"{user.FirstName} { user.Surname }";

            Student student = await _userManager.GetStudentByIdAsync(user.Id);
            if (student.GroupId.HasValue)
            {
                _currentGroup = await _dbManager.GetStudentGroupAsync(student.GroupId.Value);
                GroupName = $"{_currentGroup.SpecialtyEnt.Name} {_currentGroup.CurrentCourse}{ _currentGroup.Subgroup }";
                IReadOnlyCollection<Subject> subjects = await _dbManager.GetSpecialtySubjects(_currentGroup.SpecialtyId);
                _AddSubjects(subjects);
            }
        }

        private void _AddSubjects(IEnumerable<Subject> subjects)
        {
            foreach (SubjectModel model in subjects.Select(s => new SubjectModel(s)))
                Subjects.Add(model);
        }

        private void _ClearSubjects()
            => Subjects.Clear();
    }
}
