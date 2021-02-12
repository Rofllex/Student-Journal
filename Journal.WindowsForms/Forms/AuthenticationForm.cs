using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

using Journal.ClientLib;

#nullable enable

namespace Journal.WindowsForms.Forms
{
    public partial class AuthenticationForm : Form
    {
        public AuthenticationForm( AuthenticationViewModel? model = null )
        {
            InitializeComponent();
            this.ViewModel = model ?? new AuthenticationViewModel();
            _InitBindings();
            this.DialogResult = DialogResult.Cancel;
        }

        public AuthenticationViewModel ViewModel { get; private set; }
        public JournalClient? JournalClient { get; private set; }
        
        /// <summary>
        ///     Инициализация привязок контролов.
        /// </summary>
        private void _InitBindings()
        {
            loginTextBox.DataBindings.Add( 
                propertyName: nameof( loginTextBox.Text )
                , dataSource: ViewModel
                , dataMember: nameof( ViewModel.Login ) );

            passwordTextBox.DataBindings.Add( 
                propertyName: nameof( passwordTextBox.Text )
                , dataSource: ViewModel
                , dataMember: nameof( ViewModel.Password ) );

            rememberPasswordCheckbox.DataBindings.Add( 
                propertyName: nameof( rememberPasswordCheckbox.Checked )
                , dataSource: ViewModel
                , dataMember: nameof( ViewModel.RememberPassword ) );
        }


        private bool _isAuthorizeNow = false;

        private async void authorizeButton_Click( object sender, EventArgs e )
        {
            if ( !_isAuthorizeNow )
                return;
            _isAuthorizeNow = true;
            AuthenticationViewModel viewModel = ViewModel;
            if ((viewModel.Login?.Length ?? 0) >= 5
                && (viewModel.Password?.Length ?? 0) >= 5 )
            {
                try
                {
                    await JournalClient.ConnectAsync( "http://localhost:5001/", viewModel.Login!, viewModel.Password! );
                }
                catch (Exception ex) 
                {
                    MessageBox.Show("Не удалось авторизоваться\n" + ex.ToString());
                }
                finally
                {
                   
                }
            }
            else
            {
                MessageBox.Show(this, "Неверный логин или пароль", "Ошибка");
            }

            _isAuthorizeNow = false;
        }
    }


    public abstract class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = (_,__)=> { };
    
        protected void InvokePropertyChanged([CallerMemberName] string memberName = "" )
        {
            PropertyChanged( this, new PropertyChangedEventArgs( memberName ) );
        }
    }

    public class AuthenticationViewModel : ViewModel
    {
        public string Login 
        {
            get => _login;
            set
            {
                _login = value;
                InvokePropertyChanged();
            }
        }

        public string Password 
        {
            get => _password;
            set
            {
                _password = value;
                InvokePropertyChanged();
            }
        }
        
        public bool RememberPassword 
        {
            get => _rememberPassword;
            set
            {
                _rememberPassword = value;
                InvokePropertyChanged();
            }
        }

        public string ServerUrl { get; set; } = string.Empty;


        private string _login = string.Empty
                        , _password = string.Empty;
        private bool _rememberPassword;
    }
}
