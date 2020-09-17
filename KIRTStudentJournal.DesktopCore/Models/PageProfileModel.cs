using System;
using System.Collections.Generic;
using System.Text;

namespace KIRTStudentJournal.DesktopCore.Models
{
    public class PageProfileModel : NotifyPropertyChangedBase
    {
        private string _role = string.Empty;
        public string Role
        {
            get => _role;
            set
            {
                _role = value;
                OnPropertyChanged();
            }
        }
    }
}
