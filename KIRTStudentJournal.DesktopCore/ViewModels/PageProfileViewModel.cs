using KIRTStudentJournal.DesktopCore.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace KIRTStudentJournal.DesktopCore.ViewModels
{
    public class PageProfileViewModel : NotifyPropertyChangedBase
    {
        private PageProfileModel _pageProfileModel = new PageProfileModel();
        public PageProfileModel PageProfileModel
        {
            get => _pageProfileModel;
            set
            {
                _pageProfileModel = value;
                OnPropertyChanged();
            }
        }
    }
}
