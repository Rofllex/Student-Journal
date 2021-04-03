using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Journal.WindowsForms.ViewModels;
using Journal.ClientLib;
using Journal.WindowsForms.FormUtils;

namespace Journal.WindowsForms.Forms
{
    public partial class StudentForm : Form
    {
        public StudentForm(JournalClient journalClient)
        {
            JournalClient = journalClient;
            InitializeComponent();

            _vm = new StudentFormViewModel(JournalClient);
            _InitBindings(_vm);
        }

        public JournalClient JournalClient { get; private set; }

        private StudentFormViewModel _vm;

        private void _InitBindings(StudentFormViewModel viewModel)
        {
            studentNameLabel.DataBindings.Add(nameof(studentNameLabel.Text), _vm, nameof(_vm.StudentName));

            groupNameLabel.DataBindings.Add(nameof(groupNameLabel.Text), _vm, nameof(_vm.GroupName));
        }
    }
}
