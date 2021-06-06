using System;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;

using Journal.WindowsForms.Models;
using Journal.ClientLib.Infrastructure;
using Journal.ClientLib.Entities;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Journal.WindowsForms.ViewModels
{
    public class SpecialtiesFormViewModel : ViewModel
    {
        public SpecialtiesFormViewModel(DatabaseManager dbManager)
        {
            _dbManager = dbManager ?? throw new ArgumentNullException(nameof(dbManager));

            _LoadSpecialties();
        }

        #region shared

        public int SelectedTabPageIndex 
        {
            get => _selectedTabPageIndex;
            set 
            {
                ChangeProperty(ref _selectedTabPageIndex, value);
                if (value == 1)
                    _ = _InitSpecialtiesConstructor();
            }
        }

        #endregion

        #region specialties browser

        public BindingList<SpecialtyModel> Specialties
        {
            get => _specialties;
            set => ChangeProperty(ref _specialties, value);
        }

        #endregion

        #region specialty constructor

        public BindingList<SubjectModel> AvailableSubjects 
        {
            get => _availableSubjects;
            set => ChangeProperty(ref _availableSubjects, value);
        }

        public int AvailableSubjectSelectedIndex
        {
            get => _availableSubjectSelectedIndex;
            set => ChangeProperty(ref _availableSubjectSelectedIndex, value);
        }

        public BindingList<SubjectModel> Subjects 
        {
            get => _subjects;
            set => ChangeProperty(ref _subjects, value);
        } 

        public int SubjectSelectedIndex
        {
            get => _subjectSelectedIndex;
            set => ChangeProperty(ref _subjectSelectedIndex, value);
        }

        public string NewSpecialtyName
        {
            get => _newSpecialtyName;
            set => ChangeProperty(ref _newSpecialtyName, value);
        }

        public string NewSpecialtyCode
        {
            get => _newSpecialtyCode;
            set => ChangeProperty(ref _newSpecialtyCode, value);
        }

        public int NewSpecialtyMaxCourse
        {
            get => _newSpecialtyMaxCourse;
            set => ChangeProperty(ref _newSpecialtyMaxCourse, value);
        }

        public async void CreateNewSpecialty(object sender, EventArgs e)
        {
            #region Проверка входных параметров

            if (string.IsNullOrWhiteSpace(NewSpecialtyCode))
            {
                _ShowError("Код специальности не может быть пуст");
                return;
            }
            else if (string.IsNullOrWhiteSpace(NewSpecialtyName))
            {
                _ShowError("Название специальности не может быть пустым");

                return;

            }
            else if (NewSpecialtyMaxCourse <= 0 || NewSpecialtyMaxCourse > 4)
            {
                _ShowError("Кол-во курсов у специальности не может быть меньше 0 и больше 4");
                return;
            }

            #endregion

            int[] subjectIds = Subjects.Select(m => m.Original.Id).ToArray();
            Specialty specialty = await _dbManager.CreateSpecialtyAsync(NewSpecialtyName, NewSpecialtyCode, NewSpecialtyMaxCourse, subjectIds);
            _AddSpecialties(specialty);
        }

        public void AddSubject(object sender, EventArgs e)
        {
            if (AvailableSubjectSelectedIndex > -1)
            {
                SubjectModel selectedSubject = AvailableSubjects[AvailableSubjectSelectedIndex];
                AvailableSubjects.Remove(selectedSubject);
                
                Subjects.Add(selectedSubject);
            }
        }

        public void RemoveSubject(object sender, EventArgs e)
        {
            if (SubjectSelectedIndex > -1)
            {
                SubjectModel selectedSubject = Subjects[SubjectSelectedIndex];
                Subjects.Remove(selectedSubject);
                AvailableSubjects.Add(selectedSubject);
            }
        }

        private async Task _InitSpecialtiesConstructor()
        {
            _availableSubjects.Clear();
            Subject[] subjects = await _dbManager.GetSubjects(0, 1000);
            foreach (Subject subj in subjects)
                _availableSubjects.Add(new SubjectModel(subj));
        }

        #endregion

        private BindingList<SpecialtyModel> _specialties = new BindingList<SpecialtyModel>();

        private BindingList<SubjectModel> _subjects = new BindingList<SubjectModel>();
        private BindingList<SubjectModel> _availableSubjects = new BindingList<SubjectModel>();

        private DatabaseManager _dbManager;
        private string _newSpecialtyName,
                        _newSpecialtyCode;
        private int _newSpecialtyMaxCourse;

        private int _selectedTabPageIndex = 0;
        private int _availableSubjectSelectedIndex,
                        _subjectSelectedIndex;

        private void _ShowError(string message, string caption = "Ошибка")
            => MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);

        private void _ClearSpecialties()
            => _specialties.Clear();

        private void _AddSpecialties(IEnumerable<Specialty> specialties)
        {
            foreach (Specialty specialty in specialties)
                _specialties.Add(new SpecialtyModel(specialty));
        }

        private void _AddSpecialties(params Specialty[] specialties)
            => this._AddSpecialties((IEnumerable<Specialty>)specialties);
    
        private async void _LoadSpecialties()
        {
            Specialty[] specialties = await _dbManager.GetSpecialtiesAsync(0, 10);
            _AddSpecialties(specialties);
        }
    }
}
