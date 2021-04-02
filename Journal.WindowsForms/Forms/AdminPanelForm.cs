using System;
using System.Windows.Forms;

using Journal.WindowsForms.ViewModels;
using Journal.ClientLib;

using Journal.WindowsForms.FormUtils;

#nullable enable

namespace Journal.WindowsForms.Forms
{
    public partial class AdminPanelForm : Form
    {
        public AdminPanelForm(JournalClient journalClient)
        {
            InitializeComponent();

            AdminPanelViewModel viewModel = new AdminPanelViewModel(journalClient, usersGridView);
            
            // offsetTextBox
            offsetTextBox.DataBindings.Add(nameof(offsetTextBox.Text), viewModel, nameof(viewModel.UsersOffsetTextBox));
            offsetTextBox.TextChanged += WinFomsHelper.OnlyNumbers;

            // countTextBox
            countTextBox.DataBindings.Add(nameof(countTextBox.Text), viewModel, nameof(viewModel.UsersCountTextBox));
            countTextBox.TextChanged += WinFomsHelper.OnlyNumbers;

            // loadUsersButton
            loadUsersButton.DataBindings.Add(nameof(loadUsersButton.Enabled), viewModel, nameof(viewModel.CanLoadUsers));
            loadUsersButton.Click += viewModel.LoadUsers;
        }
    }
}
