using System.Windows.Forms;

using Journal.ClientLib;
using Journal.WindowsForms.ViewModels;

#nullable enable

namespace Journal.WindowsForms.Forms
{
    public partial class AuthenticationForm : Form
    {
        public AuthenticationForm( AuthenticationModel? model = null )
        {
            InitializeComponent();
            this.DialogResult = DialogResult.Cancel;
            if ( model != null )
                this.ViewModel = new AuthenticationViewModel( this, model );
            else
                this.ViewModel = new AuthenticationViewModel( this );
            _InitBindings();
        }

        public AuthenticationViewModel ViewModel { get; private set; }

        public JournalClient? JournalClient => ViewModel.JournalClient;
        
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
}
