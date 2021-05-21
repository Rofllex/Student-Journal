using System.Windows.Forms;

using Journal.WindowsForms.ViewModels;
using Journal.WindowsForms.FormUtils;
using Journal.ClientLib;

namespace Journal.WindowsForms.Forms
{
    public partial class CreateUserForm : Form
    {
        public CreateUserForm(IJournalClient client)
        {
            InitializeComponent( );

            _viewModel = new CreateUserViewModel(client);
            _InitBindings(_viewModel);
        }

        private CreateUserViewModel _viewModel;


        private void _InitBindings(CreateUserViewModel viewModel)
        {
            firstNameTextBox.Bind(viewModel, c => c.Text, vm => vm.FirstName);

            surnameTextBox.Bind(viewModel, c => c.Text, vm => vm.Surname);
            
            patronymicTextBox.Bind(viewModel, c => c.Text, vm => vm.Patronymic);
            
            phoneNumberTextBox.Bind(viewModel, c => c.Text, vm => vm.PhoneNumber);

            loginTextBox.Bind(viewModel, c => c.Text, vm => vm.Login);

            passwordTextBox.Bind(viewModel, c => c.Text, vm => vm.Password);

            userRoleComboBox.Bind(viewModel, c => c.DataSource, vm => vm.Roles);
            userRoleComboBox.Bind(viewModel, c => c.SelectedIndex, vm => vm.SelectedRoleIndex);

            createUserButton.Click += viewModel.CreateUser;
        }

    }

}
