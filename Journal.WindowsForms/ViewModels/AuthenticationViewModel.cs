using System;
using System.Threading;
using System.Windows.Forms;

using Journal.ClientLib;
using Journal.Common.Models;

#nullable enable

namespace Journal.WindowsForms.ViewModels
{
    public class AuthenticationViewModel : ViewModel
    {
        public AuthenticationViewModel( Form form )
        {
            _form = form ?? throw new ArgumentNullException( nameof( form ) );
        }

        public AuthenticationViewModel( Form form, AuthenticationModel model ) : this( form )
        {
            Model = model ?? throw new ArgumentNullException( nameof( model ) );
        }

        public AuthenticationViewModel( Form form
                                        , string? ip = null
                                        , string? login = null
                                        , string? password = null
                                        , bool rememberPassword = false ) : this( form )
        {
            Ip = ip ?? string.Empty;
            Login = login ?? string.Empty;
            Password = password ?? string.Empty;
            RememberPassword = rememberPassword;
        }

        #region properties

        public JournalClient? JournalClient { get; private set; }

        public string Ip
        {
            get => Model.Ip;
            set
            {
                if ( value != Model.Ip )
                {
                    Model.Ip = value;
                    InvokePropertyChanged();
                }
            }
        }

        public string Login
        {
            get => Model.Login;
            set
            {
                Model.Login = value;
                InvokePropertyChanged();
            }
        }

        public string Password
        {
            get => Model.Password;
            set
            {
                if (value != Model.Password)
                {
                    Model.Password = value;
                    InvokePropertyChanged();
                }
            }
        }

        public bool RememberPassword
        {
            get => Model.RememberPassword;
            set
            {
                Model.RememberPassword = value;
                InvokePropertyChanged();
            }
        }

        public bool CanAuthorize
        {
            get => Model.CanAuthorize;
            set
            {
                if ( value != Model.CanAuthorize )
                {
                    Model.CanAuthorize = value;
                    base.InvokePropertyChanged();
                }
            }
        }

        public AuthenticationModel Model { get; private set; } = new AuthenticationModel();

        #endregion

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
                    JournalClient = client;
                    _form.DialogResult = DialogResult.OK;
                    _form.Close();
                }
                catch ( ConnectFaillureException )
                {
                    _showError("Не удалось подключиться к серверу");
                }
                catch (RequestErrorException ree)
                {
                    _showError(ree.Error.Message);
                }
            }
            else
            {
                //MessageBox.Show( _form, "Неверный логин или пароль", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error );
                _showError("Неверный логин или пароль");
            }

            CanAuthorize = true;
            _authorizeWaitHandle.Set();
        }

        private EventWaitHandle _authorizeWaitHandle = new EventWaitHandle( true, EventResetMode.ManualReset );

        private Form _form;
    
        private void _showError(string text)
            => MessageBox.Show(owner: _form
                                , text: text
                                , caption: "Ошибка"
                                , buttons: MessageBoxButtons.OK
                                , icon: MessageBoxIcon.Error);
        
    }

    public class AuthenticationModel : ICloneable
    {
        public string Ip { get; set; } = string.Empty;

        public string Login { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public bool RememberPassword { get; set; } = false;

        public bool CanAuthorize { get; set; } = true;

        public object Clone()
            => new AuthenticationModel
            {
                Ip = Ip,
                Login = Login,
                Password = Password,
                RememberPassword = RememberPassword,
                CanAuthorize = CanAuthorize
            };
    }
}
