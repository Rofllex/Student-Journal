using System.Windows.Forms;
using System.Globalization;

using Journal.WindowsForms.ViewModels;
using Journal.WindowsForms.FormUtils;
using Journal.ClientLib.Entities;
using Journal.ClientLib;

namespace Journal.WindowsForms.Forms
{
    public partial class GradesForm : Form
    {
        public GradesForm(IJournalClient journalClient, Subject subject, StudentGroup group, bool allowEdit = false)
        {
            InitializeComponent();

            _viewModel = new GradesFormViewModel(journalClient, this, subject, group, gradesContextMenu, allowEdit);
            _InitBinding(_viewModel);
            
        }

        private GradesFormViewModel _viewModel;

        private void _InitBinding(GradesFormViewModel viewModel)
        {
            gradesGridView.CellMouseClick += (object sender, DataGridViewCellMouseEventArgs e) => 
            {
                DataGridView gridView = (DataGridView)sender;
                if (e.ColumnIndex == 0)
                {
                    gridView[e.ColumnIndex, e.RowIndex].Selected = false;
                    return;
                }
                else if (e.Button == MouseButtons.Right)
                    gridView[e.ColumnIndex, e.RowIndex].Selected = true;

            };

            

            gradesGridView.Bind(viewModel, c => c.DataSource, vm => vm.Grades);

            predMonthButton.Click += (_, __) => { viewModel.PredMonth(); };

            nextMonthButton.Click += (_, __) => { viewModel.NextMonth(); };

            subjectNameLabel.Bind(viewModel, c => c.Text, vm => vm.SubjectName);

            monthNameLabel.Bind(viewModel, c => c.Text, vm => vm.Month, true, DataSourceUpdateMode.OnPropertyChanged, "", "MMMM", CultureInfo.CurrentCulture);

            gradesGridView.CellMouseClick += viewModel.GradesCellClick;
        }
    }
}
