using Journal.ClientLib;
using Journal.ClientLib.Entities;
using Journal.Common.Entities;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Journal.WindowsForms.ViewModels
{
    public class StudentFormViewModel : ViewModel
    {
        public StudentFormViewModel(JournalClient journalClient)
        {
            this._journalClient = journalClient ?? throw new ArgumentNullException(nameof(journalClient));

           
            
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

        private JournalClient _journalClient;
        private string _studentName;
        private string _groupName;
    
        private async Task _LoadCurrentStudent()
        {
            IUser user = _journalClient.User;
            _studentName = $"{user.FirstName} { user.Surname }";
            
        }
    }
}
