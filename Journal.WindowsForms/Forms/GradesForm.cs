using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Journal.WindowsForms.ViewModels;
using Journal.WindowsForms.FormUtils;
using Journal.ClientLib.Entities;
using Journal.ClientLib;
using System.Globalization;

namespace Journal.WindowsForms.Forms
{
    public partial class GradesForm : Form
    {
        public GradesForm(JournalClient journalClient, Subject subject, StudentGroup group)
        {
            InitializeComponent();

            

            _viewModel = new GradesFormViewModel(journalClient, subject, group);
            _InitBinding(_viewModel);
            
        }

        private GradesFormViewModel _viewModel;
    
        private void _InitBinding(GradesFormViewModel viewModel)
        {
            gradesGridView.Bind(viewModel, c => c.DataSource, vm => vm.Grades);

            predMonthButton.Click += (_, __) => { viewModel.PredMonth(); };

            nextMonthButton.Click += (_, __) => { viewModel.NextMonth(); };

            subjectNameLabel.Bind(viewModel, c => c.Text, vm => vm.SubjectName);

            monthNameLabel.Bind(viewModel, c => c.Text, vm => vm.Month, true, DataSourceUpdateMode.OnPropertyChanged, "", "MMMM", CultureInfo.CurrentCulture);
        }
    }
}
