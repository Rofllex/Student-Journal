using System;
using System.ComponentModel;

using Journal.ClientLib;
using Journal.ClientLib.Infrastructure;
using Journal.ClientLib.Entities;
using Journal.WindowsForms.Models;
using System.Windows.Forms;

namespace Journal.WindowsForms.ViewModels
{
    public class CreateStudentGroupFormViewModel : ViewModel
    {
        public CreateStudentGroupFormViewModel(IJournalClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _dbManager = new DatabaseManager(_client);
            _Initialize();
        }

        public BindingList<StudentGroupModel> Groups
        {
            get => _studentGroups;
            set => ChangeProperty(ref _studentGroups, value);
        }


        public BindingList<StudentModel> Students 
        {
            get => _students;
            set => ChangeProperty(ref _students, value);
        }

        public int SelectedStudentIndex
        {
            get => _studentSelectedIndex;
            set => ChangeProperty(ref _studentSelectedIndex, value);
        }

        public BindingList<StudentModel> SelectedStudents
        {
            get => _selectedStudents;
            set => ChangeProperty(ref _selectedStudents, value);
        }

        public int SelectedStudentsSelectedIndex
        {
            get => _selectedStudentsSelectedIndex;
            set => ChangeProperty(ref _selectedStudentsSelectedIndex, value);
        }
        
        public BindingList<UserModel> Curators
        {
            get => _curators; 
            set => ChangeProperty(ref _curators, value);
        }

        public int CuratorSelectedIndex
        {
            get => _curatorsSelectedIndex;
            set => ChangeProperty(ref _curatorsSelectedIndex, value);
        }

        public BindingList<SpecialtyModel> Specialties
        {
            get => _specialties;
            set => ChangeProperty(ref _specialties, value);
        }

        public int SpecialtySelectedIndex
        {
            get => _specialtySelectedIndex;
            set => ChangeProperty(ref _specialtySelectedIndex, value);
        }

        public int CurrentCourse
        {
            get => _currentCourse;
            set => ChangeProperty(ref _currentCourse, value);
        }

        public int Subgroup
        {
            get => _subgroup;
            set => ChangeProperty(ref _subgroup, value);
        }


        public async void CreateStudentGroup(object _, EventArgs __)
        {
            User curator;

            if (CuratorSelectedIndex > -1)
            {
                curator = (User)Curators[CuratorSelectedIndex];
            }
            else
                return;

            Specialty specialty;
            if (SpecialtySelectedIndex > -1)
                specialty = (Specialty)Specialties[SpecialtySelectedIndex];
            else
                return;

            try
            {
                await _dbManager.CreateStudentGroupAsync(specialty.Id, curator.Id, Subgroup, CurrentCourse);
            }
            catch (Exception e) 
            {
                MessageBox.Show("Не удалось создать группу студентов.\nEx:" + e.ToString());
                return;
            }
        }

        public void AddStudentToGroup(object _, EventArgs __)
        {
            var selectedStudent = _students[SelectedStudentIndex];
            _students.Remove(selectedStudent);
            _selectedStudents.Add(selectedStudent);
        }

        public void RemoveStudentFromGroup(object _, EventArgs __)
        {
            var selectedStudent = _selectedStudents[SelectedStudentsSelectedIndex];
            _selectedStudents.Remove(selectedStudent);
            _students.Add(selectedStudent);
        }


        private BindingList<StudentModel> _students = new BindingList<StudentModel>(),
                                            _selectedStudents = new BindingList<StudentModel>();
        private int _studentSelectedIndex = -1,
                        _selectedStudentsSelectedIndex = -1;

        private BindingList<UserModel> _curators = new BindingList<UserModel>();
        private int _curatorsSelectedIndex = -1;

        private BindingList<SpecialtyModel> _specialties = new BindingList<SpecialtyModel>();
        private int _specialtySelectedIndex = -1;

        private int _currentCourse = 1;
        private int _subgroup = 1;


        private BindingList<StudentGroupModel> _studentGroups = new BindingList<StudentGroupModel>();




        private IJournalClient _client;
        private DatabaseManager _dbManager;



        private async void _Initialize()
        {
            //Student[] students = await _client.GetStudentsWithoutGroupAsync();
            //foreach (Student student in students)
            //    _students.Add(new StudentModel(student));
            //InvokePropertyChanged(nameof(Students));

            User[] curators = await _client.GetTeachers();
            foreach (User user in curators)
                _curators.Add(new UserModel(user));

            Specialty[] specialties = await _dbManager.GetSpecialtiesAsync(0, 100);
            foreach (Specialty specialty in specialties)
                _specialties.Add(new SpecialtyModel(specialty));

            StudentGroup[] groups = await _dbManager.GetGroups(0, 1000);
            foreach (StudentGroup group in groups)
                Groups.Add(new StudentGroupModel(group));
        }
    }
}
