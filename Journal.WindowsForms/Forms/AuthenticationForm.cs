using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
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
            this.DialogResult = DialogResult.Cancel;
            this.ViewModel = model ?? new AuthenticationViewModel( this );
            _InitBindings();
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

            ipTextBox.DataBindings.Add(
                propertyName: nameof( ipTextBox.Text )
                , dataSource: ViewModel
                , dataMember: nameof( ViewModel.Ip ) );

            authorizeButton.DataBindings.Add(
                propertyName: nameof( authorizeButton.Enabled )
                , dataSource: ViewModel
                , dataMember: nameof( ViewModel.CanAuthorize ) );

            authorizeButton.Click += ViewModel.AuthorizeButtonClicked;
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
        public string Ip
        {
            get => _ip;
            set
            {
                if (value != _ip)
                {
                    _ip = value;
                    InvokePropertyChanged();
                }
            }
        }

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

        public bool CanAuthorize
        {
            get => _canAuthorize;
            set
            {
                if (value != _canAuthorize)
                {
                    _canAuthorize = value;
                    base.InvokePropertyChanged();
                }
            }
        }

        public string ServerUrl { get; set; } = string.Empty;

        public AuthenticationViewModel( Form form )
        {
            _form = form ?? throw new ArgumentNullException( nameof( form ) );
        }

        public AuthenticationViewModel( Form form
                                        , string ip = null
                                        , string login = null
                                        , string password = null
                                        , bool rememberPassword = false
                                        , string serverUrl = null ) : this( form )
        {
            Ip = ip ?? string.Empty;
            Login = login ?? string.Empty;
            Password = password ?? string.Empty;
            RememberPassword = rememberPassword;
            ServerUrl = serverUrl ?? string.Empty;
        }


        private EventWaitHandle _authorizeWaitHandle = new EventWaitHandle( true, EventResetMode.ManualReset );

        public async void AuthorizeButtonClicked( object? o, EventArgs e )
        {
            if ( !_authorizeWaitHandle.WaitOne( 0 ) )
                return;
            _authorizeWaitHandle.Reset();
            CanAuthorize = false;

            if ( ( Login?.Length ?? 0 ) >= 4
                && ( Password?.Length ?? 0 ) >= 4 )
            {
                try
                {
                    JournalClient client = await JournalClient.ConnectAsync( $"http://{Ip}/", Login!, Password! );
                    throw new NotImplementedException();
                }
                catch ( NotImplementedException )
                {
                    throw;
                }
                catch ( ConnectFaillureException )
                {
                    MessageBox.Show( _form
                                    , "Не удалось подключиться к серверу"
                                    , "Ошибка подключения"
                                    , MessageBoxButtons.OK
                                    , MessageBoxIcon.Error );
                }
            }
            else
            {
                MessageBox.Show( _form, "Неверный логин или пароль", "Ошибка" );
            }

            CanAuthorize = true;
            _authorizeWaitHandle.Set();
        }

        private bool _canAuthorize = true;
        private Form _form;
        private string _ip = string.Empty;
        private string _login = string.Empty
                        , _password = string.Empty;
        private bool _rememberPassword;
    }
}
