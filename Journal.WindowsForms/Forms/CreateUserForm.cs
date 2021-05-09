using System.Windows.Forms;

using Journal.WindowsForms.ViewModels;
using Journal.WindowsForms.FormUtils;

namespace Journal.WindowsForms.Forms
{
    public partial class CreateUserForm : Form
    {
        public CreateUserForm()
        {
            InitializeComponent( );

            _viewModel = new CreateUserViewModel();
            _InitBindings(_viewModel);
        }

        private CreateUserViewModel _viewModel;

        private void _InitBindings(CreateUserViewModel viewModel)
        {
            firstNameTextBox.Bind(viewModel, c => c.Text, vm => vm.FirstName);
            surnameTextBox.Bind(viewModel, c => c.Text, vm => vm.Surname);
            patronymicTextBox.Bind(viewModel, c => c.Text, vm => vm.Patronymic);
            phoneNumberTextBox.Bind(viewModel, c => c.Text, vm => vm.PhoneNumber);
        }

    }

}
