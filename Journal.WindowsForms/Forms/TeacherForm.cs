using System.Windows.Forms;

using Journal.ClientLib;
using Journal.WindowsForms.ViewModels;
using Journal.WindowsForms.FormUtils;

namespace Journal.WindowsForms.Forms
{
    public partial class TeacherForm : Form
    {
        public TeacherForm(JournalClient journalClient)
        {
            InitializeComponent();
            JournalClient = journalClient;

            _viewModel = new TeacherFormViewModel(this, JournalClient);
            _InitBindings(_viewModel);
        }

        public JournalClient JournalClient { get; private set; }


        private TeacherFormViewModel _viewModel;

        private void _InitBindings(TeacherFormViewModel viewModel)
        {
            userNameLabel.Bind(viewModel, c => c.Text, vm => vm.UserName);

            logoutMenuItem.Click += viewModel.LogoutClicked;

            subjectsListBox.Bind(viewModel, c => c.DataSource, vm => vm.Subjects);
            subjectsListBox.Bind(viewModel, c => c.SelectedIndex, vm => vm.SelectedSubjectIndex);
            subjectsListBox.DoubleClick += viewModel.ShowJournalForm;

            groupsComboBox.Bind(viewModel, c => c.DataSource, vm => vm.Groups);
            groupsComboBox.Bind(viewModel, c => c.SelectedIndex, vm => vm.GroupsSelectedIndex);
        }
    }
}
