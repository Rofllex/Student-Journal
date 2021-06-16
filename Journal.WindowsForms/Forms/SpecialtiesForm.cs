using System.Windows.Forms;

using Journal.WindowsForms.FormUtils;
using Journal.WindowsForms.ViewModels;
using Journal.ClientLib.Infrastructure;
using Journal.ClientLib;

namespace Journal.WindowsForms.Forms
{
    public partial class SpecialtiesForm : Form
    {
        public SpecialtiesForm(IJournalClient client)
        {
            InitializeComponent();

            IControllerManagerFactory factory = new ControllerManagerFactory();
            _viewModel = new SpecialtiesFormViewModel(factory.Create<DatabaseManager>(client));
            _InitializeBindings(_viewModel);
        }

        private SpecialtiesFormViewModel _viewModel;

        private void _InitializeBindings(SpecialtiesFormViewModel viewModel)
        {
            specialtiesDataGrid.Bind(viewModel, c => c.DataSource, vm => vm.Specialties);

            tabControl1.Bind(viewModel, c => c.SelectedIndex, vm => vm.SelectedTabPageIndex);


            availableSubjectsListBox.Bind(viewModel, c => c.DataSource, vm => vm.AvailableSubjects);
            availableSubjectsListBox.Bind(viewModel, c => c.SelectedIndex, vm => vm.AvailableSubjectSelectedIndex);
            availableSubjectsListBox.DoubleClick += viewModel.AddSubject;

            subjectListBox.Bind(viewModel, c => c.DataSource, vm => vm.Subjects);
            subjectListBox.Bind(viewModel, c => c.SelectedIndex, vm => vm.SubjectSelectedIndex);
            subjectListBox.DoubleClick += viewModel.RemoveSubject;

            specialtyNameTextBox.Bind(viewModel, c => c.Text, vm => vm.NewSpecialtyName);

            specialtyCodeTextBox.Bind(viewModel, c => c.Text, vm => vm.NewSpecialtyCode);

            specialtyCoursesTextBox.Bind(viewModel, c => c.Text, vm => vm.NewSpecialtyMaxCourse);

            specialtyCoursesTextBox.TextChanged += WinFomsHelper.CreateOnlyNumbers();

            createSpecialtyButton.Click += viewModel.CreateNewSpecialty;

            addSubjectButton.Click += viewModel.AddSubject;

            removeSubjectButton.Click += viewModel.RemoveSubject;


        }
    }
}
