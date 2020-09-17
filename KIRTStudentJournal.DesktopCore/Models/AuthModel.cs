using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace KIRTStudentJournal.DesktopCore.Models
{
    public class AuthModel : NotifyPropertyChangedBase
    {
        private string _login = string.Empty;
        public string Login 
        {
            get => _login;
            set 
            {
                if (value != _login)
                {
                    _login = value;
                    OnPropertyChanged();
                }
            } 
        }

        private bool _remember = false;
        public bool Remember
        {
            get => _remember;
            set
            {
                if (value != _remember)
                {
                    _remember = value;
                    OnPropertyChanged();
                }
            }
        }
    }

    public abstract class NotifyPropertyChangedBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = (_, __) => { };

        protected void OnPropertyChanged([CallerMemberName] string caller = "") => PropertyChanged(this, new PropertyChangedEventArgs(caller));
    }
}
