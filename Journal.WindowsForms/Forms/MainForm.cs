using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Journal.WindowsForms.Forms
{
    public partial class MainForm : Form
    {
        public MainForm(  )
        {
            InitializeComponent();

            _viewModel = new MainFormViewModel( this );
            menuStrip.ForeColor = Color.White;
            userNameLabel.DataBindings.Add( nameof( Label.Text ), _viewModel, nameof( MainFormViewModel.UserName ) );
#if DEBUG
            menuStrip.Items.Add( "Сервис", null, _viewModel.OpenServiceMenuClicked );
#endif

        }


        private MainFormViewModel _viewModel;
    }

    public class MainFormViewModel : ViewModel
    {
        public MainFormViewModel( Form mainForm )
        {
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

        public void OpenServiceMenuClicked(object _, EventArgs __ )
        {
            IsVisible = false;
            using ( ServiceForm serviceForm = new ServiceForm() )
            {
                serviceForm.ShowDialog();
            }

            IsVisible = true;
        }

        public void LogoutButtonClicked( object _, EventArgs __ )
            => _form.Close();

        private bool _isVisible = true;
        private Form _form;
        private string _userName = string.Empty;
    }
}
