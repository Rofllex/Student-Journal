using System;
using System.Collections.Generic;
using System.Text;

using Journal.ClientLib.Entities;
using Journal.ClientLib.Infrastructure;

namespace Journal.WindowsForms.ViewModels
{
    public class DatabasePageViewModel : ViewModel
    {
        public DatabasePageViewModel(DatabaseManager databaseManager)
        {
            _databaseManager = databaseManager ?? throw new ArgumentNullException(nameof(databaseManager));
        }

        public string SpecialtyName
        {
            get => _specialtyName;
            set => ChangeProperty(ref _specialtyName, value);
        }

        public string SpecialtyCode
        {
            get => _specialtyCode;
            set => ChangeProperty(ref _specialtyCode, value);
        }

        public int MaxCourse
        {
            get => _maxCourse;
            set => ChangeProperty(ref _maxCourse, value);
        }

        public async void CreateSpecialtyButtonClicked(object _, EventArgs __)
        {
            if (string.IsNullOrWhiteSpace(_specialtyName))
            {
                _ShowError("Название специальности не может быть пустым или состоять из пробелов");
                return;
            }

            if (string.IsNullOrWhiteSpace(_specialtyCode))
            {
                _ShowError("Код специальности не может быть пустым");
                return;
            }

            if (_maxCourse <= 0 || _maxCourse > 4)
            {
                _ShowError("Кол-во курсов не может быть меньше или равно 0 или больше 4-х");
                return;
            }

            Specialty specialty;
            try
            {
                specialty = await _databaseManager.CreateSpecialtyAsync(SpecialtyName, SpecialtyCode, MaxCourse);
            }
            catch (Exception e)
            {
                _ShowError(e.ToString(), "Произошла ошибка при выполнении запроса");
            }
        }

        private string _specialtyName
            , _specialtyCode;
        private int _maxCourse;

        private DatabaseManager _databaseManager;

        private void _ShowError(string message, string title = "Ошибка")
            => System.Windows.Forms.MessageBox.Show(message
                                                    , title
                                                    , System.Windows.Forms.MessageBoxButtons.OK
                                                    , System.Windows.Forms.MessageBoxIcon.Error);
    }
}
