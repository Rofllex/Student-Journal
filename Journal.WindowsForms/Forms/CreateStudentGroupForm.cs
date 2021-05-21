using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Journal.WindowsForms.ViewModels;
using Journal.WindowsForms.FormUtils;
using Journal.ClientLib;

namespace Journal.WindowsForms.Forms
{
    public partial class CreateStudentGroupForm : Form
    {
        public CreateStudentGroupForm(IJournalClient client)
        {
            InitializeComponent();

            _viewModel = new CreateStudentGroupFormViewModel(client);
            _InitBindings(_viewModel);
        }

        private void _InitBindings(CreateStudentGroupFormViewModel viewModel)
        {
            specialtyListBox.Bind(viewModel, c => c.DataSource, vm => vm.Specialties);
            specialtyListBox.Bind(viewModel, c => c.SelectedIndex, vm => vm.SpecialtySelectedIndex);

            teacherListBox.Bind(viewModel, c => c.DataSource, vm => vm.Curators);
            teacherListBox.Bind(viewModel, c => c.SelectedIndex, vm => vm.CuratorSelectedIndex);

            selectedStudentsListBox.Bind(viewModel, c => c.DataSource, vm => vm.SelectedStudents);
            selectedStudentsListBox.Bind(viewModel, c => c.SelectedIndex, vm => vm.SelectedStudentsSelectedIndex);
            selectedStudentsListBox.MouseDoubleClick += viewModel.RemoveStudentFromGroup;

            studentsListBox.Bind(viewModel, c => c.DataSource, vm => vm.Students);
            studentsListBox.Bind(viewModel, c => c.SelectedIndex, vm => vm.SelectedStudentIndex);
            studentsListBox.MouseDoubleClick += viewModel.AddStudentToGroup;

            subgroupNumericUpDown.Bind(viewModel, c => c.Value, vm => vm.Subgroup);

            currentCourseNumericUpDown.Bind(viewModel, c => c.Value, vm => vm.CurrentCourse);

            createStudentGroupButton.Click += viewModel.CreateStudentGroup;

            groupsDataGridView.Bind(viewModel, c => c.DataSource, vm => vm.Groups);
        }

        private CreateStudentGroupFormViewModel _viewModel;
    }
}
