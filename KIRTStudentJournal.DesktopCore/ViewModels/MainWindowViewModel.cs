using KIRTStudentJournal.DesktopCore.Models;
using KIRTStudentJournal.NetLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace KIRTStudentJournal.DesktopCore.ViewModels
{
    public class MainWindowViewModel : NotifyPropertyChangedBase
    {
        private JournalClient _journalClient;
        /// <summary>
        /// Может быть null.
        /// </summary>
        public JournalClient JournalClient 
        {
            get => _journalClient;
            set
            {
                _journalClient = value;
                OnPropertyChanged();
            }
        }
    }
}
