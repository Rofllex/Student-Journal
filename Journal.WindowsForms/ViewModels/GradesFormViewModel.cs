using Journal.ClientLib;
using Journal.ClientLib.Entities;
using Journal.ClientLib.Infrastructure;
using Journal.Common.Entities;
using Journal.WindowsForms.Models;

using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;

namespace Journal.WindowsForms.ViewModels
{
    public class GradesFormViewModel : ViewModel
    {
        public GradesFormViewModel(IJournalClient journalClient, Subject subject, StudentGroup group)
        {
            if (subject == null)
                throw new ArgumentNullException(nameof(subject));
            if (group == null)
                throw new ArgumentNullException(nameof(group));
            this._subject = subject ?? throw new ArgumentNullException(nameof(subject));
            this._group = group ?? throw new ArgumentNullException(nameof(group));

            SubjectName = subject.Name;
            GroupName = $"{group.SpecialtyEnt.Name} {group.CurrentCourse}{group.Subgroup}";

            _gradesManager = new GradesManager(journalClient);
            _dbManager = new DatabaseManager(journalClient);
            _Initialize();
        }

        public string SubjectName { get; }

        public string GroupName { get; }
    
        public DataTable Grades 
        {
            get => _gradesTable;
            set => ChangeProperty(ref _gradesTable, value);
        }

        public DateTime Month
        {
            get => _monthDate;
            set 
            {
                _gradesToDataTable.CurrentMonth = value;
                ChangeProperty(ref _monthDate, value);
            }
        }

        public async void NextMonth() 
        {
            Month = Month.AddMonths(1);
            await _LoadGrades(Month);
        }

        public async void PredMonth() 
        {
            Month = Month.AddMonths(-1);
            await _LoadGrades(Month);
        }

        private DataTable _gradesTable = new DataTable();
       
        private DateTime _monthDate;

        private GradesManager _gradesManager;
        private DatabaseManager _dbManager;
        private Subject _subject;
        private StudentGroup _group;
        private Student[] _students;
        private GradesToDataTableAdapter _gradesToDataTable;


        private async void _Initialize()
        {
            _students = await _dbManager.GetStudentsInGroup(_group);
            DateTime now = DateTime.Now;
            DateTime currentMonth = new DateTime(now.Year, now.Month, 1);
            _monthDate = currentMonth;
            base.InvokePropertyChanged(nameof(Month));
            _gradesToDataTable = new GradesToDataTableAdapter(_gradesTable, currentMonth, _students);
            await _LoadGrades(Month);
        }

        private async Task _LoadGrades(DateTime date)
        {
            Grade[] grades = await _gradesManager.GetGradesInMonthAsync(Month, _group, _subject);
            _gradesToDataTable.ClearGrades();
            _gradesToDataTable.SetGrades(grades);
        }

        private class GradesToDataTableAdapter
        {
            public GradesToDataTableAdapter(DataTable dataTable, DateTime currentDate, Student[] students)
            {
                _dataTable = dataTable ?? throw new ArgumentNullException(nameof(dataTable));
                _Initialize(students);
            }

            public DateTime CurrentMonth
            {
                get => _date;
                set
                {
                    _FillColumnsInDateTime(value);
                    _date = value;
                }
            }

            public void SetGrade(Grade grade)
            {
                var row = _userIdToRow[grade.StudentId];
                row[grade.Timestamp.Day] = grade.GradeLevel switch
                {
                    GradeLevel.Five => "5",
                    GradeLevel.Four => "4",
                    GradeLevel.Three => "3",
                    GradeLevel.Two => "2",
                    GradeLevel.Miss => "НБ",
                    GradeLevel.Offset => "Зач",
                    GradeLevel.Fail => "Н/Зач",
                    _ => "err"
                };
            }

            public void SetGrades(IEnumerable<Grade> grades)
            {
                foreach (Grade grade in grades) 
                {
                    SetGrade(grade);
                }
            }

            public void ClearGrades()
            {
                for (int columnIndex = 1; columnIndex < _dataTable.Columns.Count; columnIndex++)
                {
                    for (int rowIndex = 0; rowIndex < _dataTable.Rows.Count; rowIndex++)
                    {
                        _dataTable.Rows[rowIndex][columnIndex] = null;
                    }
                }
            }
           

            private DataTable _dataTable;
            private DateTime _date;
            private Dictionary<int, DataRow> _userIdToRow = new Dictionary<int, DataRow>();

            private void _Initialize(Student[] students)
            {
                _dataTable.Columns.Add("ФИО");
                for (int studentIndex = 0; studentIndex < students.Length; studentIndex++)
                {
                    User user = students[studentIndex].UserEnt;
                    _userIdToRow.TryAdd(user.Id, _dataTable.Rows.Add($"{ user.FirstName} {user.Surname} {user.LastName}"));
                }

                _FillColumnsInDateTime(CurrentMonth);
            }

            private void _FillColumnsInDateTime(DateTime month)
            {
                int daysInMonth = DateTime.DaysInMonth(month.Year, month.Month);
                int daysDifference = daysInMonth - _dataTable.Columns.Count + 1;

                if (daysDifference < 0)
                {
                    while (daysInMonth != _dataTable.Columns.Count - 1)
                        _dataTable.Columns.RemoveAt(_dataTable.Columns.Count - 1);

                    //for (int columnIndex = daysDifference - 1; columnIndex < daysInMonth; columnIndex++)
                    //    _gradesTable.Columns.RemoveAt(columnIndex);
                }
                else
                {
                    for (int columnIndex = _dataTable.Columns.Count; columnIndex < daysInMonth + 1; columnIndex++)
                    {
                        _dataTable.Columns.Add((columnIndex).ToString());
                    }
                }
            }
        }
    }
}
