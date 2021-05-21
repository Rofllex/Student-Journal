using System.Windows.Forms;

using Journal.ClientLib;
using Journal.ClientLib.Entities;
using Journal.WindowsForms.FormUtils;
using Journal.WindowsForms.ViewModels;

namespace Journal.WindowsForms.Forms
{
    public partial class EditStudentForm : Form
    {
        public EditStudentForm(IJournalClient client, Student studentToEdit)
        {
            InitializeComponent();

            _viewModel = new EditStudentViewModel(client, studentToEdit, this);
            _InitBindings(_viewModel);
        }


        private EditStudentViewModel _viewModel;
    
        private void _InitBindings(EditStudentViewModel viewModel)
        {
            studentNameLabel.Bind(viewModel, c => c.Text, vm => vm.StudentName);

            groupsComboBox.Bind(viewModel, c => c.DataSource, vm => vm.Groups);
            groupsComboBox.Bind(viewModel, c => c.SelectedIndex, vm => vm.SelectedGroupIndex);

            saveButton.Click += viewModel.SaveChanges;
        }
    }
}
