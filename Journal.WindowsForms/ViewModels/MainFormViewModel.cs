using System;
using System.Windows.Forms;

using Journal.ClientLib;
using Journal.WindowsForms.Forms;

namespace Journal.WindowsForms.ViewModels
{
    public class MainFormViewModel : ViewModel
    {
        public MainFormViewModel( Form mainForm, JournalClient journalClient )
        {
            _journalClient = journalClient ?? throw new ArgumentNullException( nameof( journalClient ) );
            _userName = $"{journalClient.User.FirstName} {journalClient.User.LastName}";
            _form = mainForm ?? throw new ArgumentNullException( nameof( mainForm ) );
        }

        public string UserName
        {
            get => _userName;
            set
            {
                _userName = value;
                InvokePropertyChanged();
            }
        }

        public bool IsVisible
        {
            get => _isVisible;
            set
            {
                if ( value != _isVisible )
                {
                    if ( value )
                        _form.Show();
                    else
                        _form.Hide();
                    _isVisible = value;
                }
            }
        }

        public void OpenServiceMenu(object _, EventArgs __ )
        {
            IsVisible = false;
            using ( ServiceForm serviceForm = new ServiceForm() )
            {
                serviceForm.ShowDialog();
            }

            IsVisible = true;
        }

        public void OpenAdminPanel(object _, EventArgs __ )
        {

        }

        public void LogoutButtonClicked( object _, EventArgs __ )
            => _form.Close();

        private JournalClient _journalClient;
        private bool _isVisible = true;
        private Form _form;
        private string _userName = string.Empty;
    }
}
