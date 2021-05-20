using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

using Journal.ClientLib;
using Journal.ClientLib.Entities;
using Journal.ClientLib.Infrastructure;
using Journal.Common.Models;
using Journal.WindowsForms.Models;

namespace Journal.WindowsForms.ViewModels
{
    public class SubjectFormViewModel : ViewModel
    {
        public SubjectFormViewModel(IJournalClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _dbManager = new DatabaseManager(_client);
            _Initialize();
        }

        public BindingList<SubjectModel> Subjects { get; } = new BindingList<SubjectModel>();

        public string NewSubjectName 
        {
            get => _newSubjectName;
            set => ChangeProperty(ref _newSubjectName, value);
        }

        public async void CreateNewSubject(object s, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NewSubjectName))
            {
                MessageBox.Show("Название предмета не может быть пустым", "Ошибка");
                return;
            }

            try
            {
                Subject subj = await _dbManager.CreateSubject(NewSubjectName);
                _AddSubject(subj);
            }
            catch (RequestErrorException ree) 
            {
                if (ree.Error != null)
                {
                    MessageBox.Show(ree.Error.Message, "Ошибка");
                }
                else
                    MessageBox.Show(ree.Message, "Ошибка");

                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Ошибка");
            }
            
        }

        private string _newSubjectName = string.Empty;
        private IJournalClient _client;
        private DatabaseManager _dbManager;

        private async void _Initialize()
        {
            Subject[] subjects = await _dbManager.GetSubjects(0, 1000);
            _AddSubjects(subjects);
        }

        private void _AddSubjects(IEnumerable<Subject> subjects)
        {
            foreach (Subject subj in subjects)
                Subjects.Add(new SubjectModel(subj));
        }

        private void _AddSubject(Subject subject)
            => Subjects.Add(new SubjectModel(subject));
    }
}
