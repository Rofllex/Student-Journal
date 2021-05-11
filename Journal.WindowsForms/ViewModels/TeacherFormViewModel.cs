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

        public BindingList<StudentGroupModel> GroupsList
        {
            get => _groupsList;
            set => ChangeProperty(ref _groupsList, value);
        }

        public int SelectedGroupIndex
        {
            get => _selectedGroupIndex;
            set 
            {
                ChangeProperty(ref _selectedGroupIndex, value);
                _LoadStudents();
            }
        }

        public BindingList<StudentModel> Students
        {
            get => _studentsList;
            set => ChangeProperty(ref _studentsList, value);
        }

        public int StudentIndex
        {
            get => _studentIndex;
            set => ChangeProperty(ref _studentIndex, value);
        }

        public BindingList<StudentModel> SelectedStudents
        {
            get => _selectedStudentsList;
            set => ChangeProperty(ref _selectedStudentsList, value);
        }

        public int SelectedStudentIndex
        {
            get => _selectedStudentIndex;
            set => ChangeProperty(ref _selectedStudentIndex, value);
        }

        public BindingList<GradeLevel> Grades { get; } = new BindingList<GradeLevel>((GradeLevel[])Enum.GetValues(typeof(GradeLevel)));

        /// <summary>
        ///     Индекс выбранной оценки
        /// </summary>
        public int SelectedGradeIndex
        {
            get => _selectedGradeIndex;
            set => ChangeProperty(ref _selectedGradeIndex, value);
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

        public void GradeTextFormat(object s, ListControlConvertEventArgs e)
        {
            if (e.Value == null)
            {
                e.Value = "null";
                return;
            }

            GradeLevel? grade = e.Value as GradeLevel?;
            if (!grade.HasValue)
                return;
            e.Value = grade switch
            {
                GradeLevel.Miss => "НБ",
                GradeLevel.Offset => "Зачет",
                GradeLevel.Fail => "Незачет",
                GradeLevel.Two => "Неудовлетворительно",
                GradeLevel.Three => "Удовлетворительно",
                GradeLevel.Four => "Хорошо",
                GradeLevel.Five => "Отлично",
                _ => "unknown"
            };
        }

        public void PasteGradeButton(object _, EventArgs __)
        {
            // Выставление оценки выбранному студенту
            MessageBox.Show("Типа оценка выставлена");
        }

        
        public void PasteMultiplyButton(object _, EventArgs __)
        {
            // Выставление оценок выбранным студентам
        }

        public void SelectStudent(object _, EventArgs __)
        {
            if (StudentIndex < 0)
                return;
            var student = Students[StudentIndex];
            Students.Remove(student);
            SelectedStudents.Add(student);
        }

        public void RemoveSelectedStudent(object _, EventArgs __)
        {
            if (SelectedStudentIndex < 0)
                return;
            var student = SelectedStudents[SelectedStudentIndex];
            SelectedStudents.Remove(student);
            Students.Add(student);
        }

        public void ClearSelectedStudents(object _, EventArgs __)
        {
            while (SelectedStudents.Count > 0)
            {
                StudentModel student = SelectedStudents[0];
                SelectedStudents.Remove(student);
                Students.Add(student);
            }
            SelectedStudentIndex = -1;
        }

        public void LogoutClicked(object sender, EventArgs __)
            => Program.LogoutForm(_callerForm);

        public void SwapStudents(object _, EventArgs __)
        {
            BindingList<StudentModel> temp = Students;
            Students = SelectedStudents;
            SelectedStudents = temp;
        }


        private string _userName = string.Empty;
        private IJournalClient _client;
        private BindingList<StudentGroupModel> _groupsList = new BindingList<StudentGroupModel>();
        private BindingList<StudentModel> _studentsList = new BindingList<StudentModel>();
        private BindingList<StudentModel> _selectedStudentsList = new BindingList<StudentModel>();
        private DatabaseManager _dbManager;
        private int _selectedGroupIndex = -1;
        private int _studentIndex = -1;
        private int _selectedStudentIndex = -1;
        private int _selectedGradeIndex = -1;
        private int _selectedSubjectIndex = -1;
        private Form _callerForm;

        private async void _Initialize()
        {
            UserName = $"{_client.User.FirstName} { _client.User.Surname }";

            StudentGroup[] groups = await _dbManager.GetGroups();
            _AddGroups(groups);
            if (groups.Length > 0)
                SelectedGroupIndex = 0;
        }

        private void _AddGroups(IEnumerable<StudentGroup> groups) 
        {
            foreach (StudentGroup group in groups)
                GroupsList.Add(new StudentGroupModel(group));
        }

        private void _AddStudents(IEnumerable<Student> students)
        {
            foreach (Student student in students)
                Students.Add(new StudentModel(student));
        }

        private void _AddSubjects(IEnumerable<Subject> subjects)
        {
            foreach (Subject subj in subjects)
                Subjects.Add(new SubjectModel(subj));
        }

        private void _AddSelectedStudents(IEnumerable<Student> students)
        {
            foreach (Student student in students)
                SelectedStudents.Add(new StudentModel(student));
        }

        private void _ClearSubjects()
            => Subjects.Clear();

        private void _ClearStudents()
            => Students.Clear();

        private void _ClearSelectedStudents()
            => SelectedStudents.Clear();

        private async void _LoadStudents()
        {
            _ClearStudents();
            Student[] students;
            StudentGroup group = (StudentGroup)GroupsList[SelectedGroupIndex];
            try
            {
                students = await _dbManager.GetStudentsInGroup(group);
            }
            catch (Exception e)
            {
                MessageBox.Show("Не удалось загрузить список студентов\n" + e.ToString());
                return;
            }
            _AddStudents(students);

            _ClearSubjects();
            _AddSubjects(await _dbManager.GetSpecialtySubjects(group.SpecialtyId));
            if (Subjects.Count > 0)
                SelectedSubjectIndex = 0;
            else
                SelectedSubjectIndex = -1;
        }
    }
}
