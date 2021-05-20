using System;
using System.Windows.Forms;
using System.Collections.Generic;

using Journal.WindowsForms.ViewModels;
using Journal.WindowsForms.FormUtils;
using Journal.ClientLib;
using Journal.Common.Entities;
using Journal.Common.Extensions;
using Journal.ClientLib.Infrastructure;

#nullable enable

namespace Journal.WindowsForms.Forms
{
    public partial class AdminPanelForm : Form
    {
        public AdminPanelForm(JournalClient journalClient)
        {
            InitializeComponent();

            _journalClient = journalClient;

            CheckForIllegalCrossThreadCalls = false;
            _adminPanelViewModel = new AdminPanelViewModel(journalClient, this);
            _InitAdminPanelFormBinding(_adminPanelViewModel);
        }

        private AdminPanelViewModel _adminPanelViewModel;
        private IJournalClient _journalClient;

        private void _InitAdminPanelFormBinding(AdminPanelViewModel viewModel)
        {
            #region AdminPanelFormViewModel

            //usersGridView.CellFormatting += UsersGridView_CellFormatting;
            usersGridView.Bind( viewModel, c => c.DataSource, vm => vm.Users );
            usersGridView.DataError += (object sender, DataGridViewDataErrorEventArgs e) => 
            {
            };

            currentPageTextBox.TextChanged += CurrentPageTextBox_TextChanged;
            currentPageTextBox.Bind( viewModel, c => c.Text, vm => vm.CurrentPage );
            //currentPageTextBox.DataBindings.Add(nameof(TextBox.Text), viewModel, nameof(viewModel.CurrentPage));

            predPageButton.Click += viewModel.ScrollLeft;
            predPageButton.Bind( viewModel, c => Enabled, vm => vm.CanScrollLeft );
            //predPageButton.DataBindings.Add(nameof(predPageButton.Enabled), viewModel, nameof(viewModel.CanScrollLeft));

            nextPageButton.Click += viewModel.ScrollRight;
            nextPageButton.Bind( viewModel, c => c.Enabled, vm => vm.CanScrollRight );
            //nextPageButton.DataBindings.Add(nameof(nextPageButton.Enabled), viewModel, nameof(viewModel.CanScrollRight));

            usersCountLabel.Bind( viewModel, c => c.Text, vm => vm.UsersCount );
            //usersCountLabel.DataBindings.Add(nameof(usersCountLabel.Text), viewModel, nameof(viewModel.UsersCount));

            #endregion

            logoutMenuItem.Click += viewModel.LogoutClicked;
        }
                
        private void CurrentPageTextBox_TextChanged(object? sender, EventArgs e)
        {
            TextBox? textBox = sender as TextBox;
            if (textBox != null)
            {
                string text = textBox.Text;
                if (!string.IsNullOrWhiteSpace( text ))
                {
                    int selectionIndex = textBox.SelectionStart;
                    for (int charIndex = 0 ; charIndex < text.Length ; charIndex++)
                    {
                        if (!char.IsDigit( text[charIndex] ))
                        {
                            text = text.Remove( charIndex, 1 );
                            if (charIndex < selectionIndex)
                                selectionIndex--;
                            charIndex--;
                        }
                    }

                    text = int.Parse( text ).ToString( );

                    if (text != textBox.Text)
                    {
                        textBox.Text = text;
                        textBox.SelectionStart = selectionIndex;
                    }
                }
                else
                {
                    textBox.Text = "0";
                    textBox.SelectionStart = textBox.Text.Length;
                    textBox.SelectionLength = 0;
                    currentPageTextBox_KeyPress( sender, new KeyPressEventArgs( '0' ) );
                }
            }
        }

        private void UsersGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            Type? valueType = e.Value?.GetType();
            if (valueType == typeof(UserRole))
            {
                UserRole originalValue = (UserRole)e.Value!;

                string formattedValue;
                using (IEnumerator<UserRole> enumerator = originalValue.GetFlags().GetEnumerator())
                {
                    if (enumerator.MoveNext())
                    {
                        formattedValue = enumerator.Current.ToString();
                        while (enumerator.MoveNext())
                        {
                            formattedValue += ", " + enumerator.Current.ToString();
                        }
                    }
                    else
                        formattedValue = "{\"empty role\"}";
                }
                e.Value = formattedValue;
            }
        }
                
        private void currentPageTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            _adminPanelViewModel.CurrentPage = int.Parse(((TextBox)sender).Text);
        }

        private void specialtiesButton_Click(object sender, EventArgs e)
        {
            using SpecialtiesForm specForm = new SpecialtiesForm(this._journalClient);
            specForm.ShowDialog();
        }

        private void subjectsButton_Click(object sender, EventArgs e)
        {
            using SubjectsConstructorForm subjForm = new SubjectsConstructorForm(this._journalClient);
            subjForm.ShowDialog();
        }
    }
}
