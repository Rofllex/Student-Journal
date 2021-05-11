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

            groupsListBox.Bind(viewModel, c => c.DataSource, vm => vm.GroupsList);
            groupsListBox.Bind(viewModel, c => c.SelectedIndex, vm => vm.SelectedGroupIndex);

            studentsListBox.Bind(viewModel, c => c.DataSource, vm => vm.Students);
            studentsListBox.Bind(viewModel, c => c.SelectedIndex, vm => vm.StudentIndex);
            studentsListBox.DoubleClick += viewModel.SelectStudent;

            selectedStudentsListBox.Bind(viewModel, c => c.DataSource, vm => vm.SelectedStudents);
            selectedStudentsListBox.Bind(viewModel, c => c.SelectedIndex, vm => vm.SelectedStudentIndex);
            selectedStudentsListBox.DoubleClick += viewModel.RemoveSelectedStudent;

            subjectsComboBox.Bind(viewModel, c => c.DataSource, c => c.Subjects);
            subjectsComboBox.Bind(viewModel, c => c.SelectedIndex, c => c.SelectedSubjectIndex);

            selectGradeComboBox.Bind(viewModel, c => c.DataSource, vm => vm.Grades);
            selectGradeComboBox.Bind(viewModel, c => c.SelectedIndex, vm => vm.SelectedGradeIndex);
            selectGradeComboBox.Format += viewModel.GradeTextFormat;

            pasteGradeButton.Click += viewModel.PasteGradeButton;

            clearSelectedStudentsButton.Click += viewModel.ClearSelectedStudents;

            logoutMenuItem.Click += viewModel.LogoutClicked;

            swapStudentsButton.Click += viewModel.SwapStudents;
        }
    }
}
