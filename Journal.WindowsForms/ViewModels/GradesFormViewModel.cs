﻿using Journal.ClientLib;
using Journal.ClientLib.Entities;
using Journal.ClientLib.Infrastructure;
using Journal.Common.Entities;
using Journal.Common.Models;

using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Journal.WindowsForms.ViewModels
{
    public class GradesFormViewModel : ViewModel
    {
        public GradesFormViewModel(IJournalClient journalClient, Form callerForm, Subject subject, StudentGroup group, ContextMenuStrip gradesContextMenu, bool allowEdit = false)
        {
            if (subject == null)
                throw new ArgumentNullException(nameof(subject));
            if (group == null)
                throw new ArgumentNullException(nameof(group));
            this._subject = subject ?? throw new ArgumentNullException(nameof(subject));
            this._group = group ?? throw new ArgumentNullException(nameof(group));
            this._gradesContextMenu = gradesContextMenu ?? throw new ArgumentNullException(nameof(gradesContextMenu));
            this._allowEdit = allowEdit;
            this._callerForm = callerForm ?? throw new ArgumentNullException(nameof(callerForm));

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

        public void GradesCellClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (_allowEdit && e.Button == MouseButtons.Right
                 && e.ColumnIndex > 0 && e.RowIndex >= 0)
            {
                int day = _gradesToDataTable.GetDayByColumnIndex(e.ColumnIndex);
                DateTime timestamp = new DateTime(_monthDate.Year, _monthDate.Month, day);
                if (timestamp >= DateTime.Now.AddDays(1))
                    return;
                DataGridView gridView = (DataGridView)sender;
                if (gridView.SelectedCells.Count == 2)
                    gridView.SelectedCells[1].Selected = false;
                int studentId = _gradesToDataTable.GetStudentIdByRowIndex(e.RowIndex);
                _gradesContextMenu.Items.Clear();
                foreach (GradeLevel gradeLevel in Enum.GetValues(typeof(GradeLevel)))
                {
                    _gradesContextMenu.Items.Add(Culture.GradeFormatProvider.ShortFormat(gradeLevel), null, (_, __) =>
                    {
                        int subjectId = _subject.Id;
                        Task.Run(() =>
                        {
                            Grade grade;
                            try
                            {
                                Task<Grade> pasteGradeTask = _gradesManager.Paste(studentId, _subject.Id, gradeLevel, timestamp: timestamp);
                                pasteGradeTask.Wait();
                                grade = pasteGradeTask.Result;
                            }
                            catch (AggregateException ae)
                            {
                                ae.Handle(e =>
                                {
                                    if (e.GetType() != typeof(RequestErrorException))
                                        return false;
                                    RequestErrorException rex = (RequestErrorException)e;
                                    MessageBox.Show(rex.Error.Message);
                                    return true;
                                });
                                
                                return;
                            }
                            _gradesToDataTable.SetGrade(grade);
                        });

                    });
                }

                _gradesContextMenu.Show(Cursor.Position);
            }
        }

        private DataTable _gradesTable = new DataTable();
       
        private DateTime _monthDate;

        private GradesManager _gradesManager;
        private DatabaseManager _dbManager;
        private Subject _subject;
        private StudentGroup _group;
        private Student[] _students;
        private GradesToDataTableAdapter _gradesToDataTable;
        private readonly bool _allowEdit;
        private ContextMenuStrip _gradesContextMenu;
        private readonly Form _callerForm;

        private async void _Initialize()
        {
            _students = await _dbManager.GetStudentsInGroup(_group);
            DateTime now = DateTime.Now;
            DateTime currentMonth = new DateTime(now.Year, now.Month, 1);
            _monthDate = currentMonth;
            base.InvokePropertyChanged(nameof(Month));
            _gradesToDataTable = new GradesToDataTableAdapter(_gradesTable, currentMonth, _students);
            try
            {
                await _LoadGrades(Month);
            }
            catch (RequestErrorException e)
            {
                MessageBox.Show(e.Error.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _callerForm.Close();
            }
        }

        private async Task _LoadGrades(DateTime date)
        {
            Grade[] grades = await _gradesManager.GetGradesInMonthAsync(date, _group, _subject);
            _gradesToDataTable.ClearGrades();
            _gradesToDataTable.SetGrades(grades);
        }

        private class GradesToDataTableAdapter
        {
            public GradesToDataTableAdapter(DataTable dataTable, DateTime currentDate, Student[] students)
            {
                _dataTable = dataTable ?? throw new ArgumentNullException(nameof(dataTable));
                _date = currentDate;
                _Initialize(students);
            }

            public DateTime CurrentMonth
            {
                get => _date;
                set
                {
                    if (_date.Year != value.Year
                         || _date.Month != value.Month)
                    {
                        _FillColumnsInDateTime(value);
                        _date = value;
                    }
                }
            }

            /// <summary>
            ///     Кол-во пользователей.
            /// </summary>
            public int UsersCount
                => _userIdToRow.Count;

            /// <summary>
            ///     Кол-во дней.
            /// </summary>
            public int DaysCount
                => _dataTable.Columns.Count - 1;

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

            public bool ContainsStudent(Student student)
                => _userIdToRow.ContainsKey(student.UserId);

            public int GetStudentIdByRowIndex(int rowIndex)
                => _userIdToRow.FirstOrDefault(p => ReferenceEquals(p.Value, _dataTable.Rows[rowIndex])).Key;

            public int GetDayByColumnIndex(int columnIndex)
                => columnIndex;

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
