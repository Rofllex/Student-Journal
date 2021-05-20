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
    public partial class SubjectsConstructorForm : Form
    {
        public SubjectsConstructorForm(IJournalClient client)
        {
            InitializeComponent();

            _viewModel = new SubjectFormViewModel(client);
            _InitBindings(_viewModel);
        }

        private void _InitBindings(SubjectFormViewModel viewModel)
        {
            this.subjectsListBox.Bind(viewModel, c => c.DataSource, vm => vm.Subjects);

            this.newSubjectNameTextBox.Bind(viewModel, c => c.Text, vm => vm.NewSubjectName);

            this.createSubjectButton.Click += viewModel.CreateNewSubject;
        }

        private SubjectFormViewModel _viewModel;
    }
}
