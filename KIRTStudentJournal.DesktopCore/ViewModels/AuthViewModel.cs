using KIRTStudentJournal.DesktopCore.Models;
using KIRTStudentJournal.NetLib;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace KIRTStudentJournal.DesktopCore.ViewModels
{
    public class AuthViewModel : NotifyPropertyChangedBase
    {
        private string _message;
        public string Message 
        {
            get => _message;
            set
            {
                if (value.Length > 100)
                    value = value.Substring(0, 100);
                if (value != _message)
                {
                    _message = value;
                    OnPropertyChanged();
                }
            }
        }

        private Color _messageColor = Color.FromArgb(255,0,0,0);
        public Color MessageColor
        {
            get => _messageColor;
            set
            {
                _messageColor = value;
                OnPropertyChanged();
            }
        }

        private AuthModel _authModel = new AuthModel();
        public AuthModel AuthModel 
        {
            get => _authModel;
            set
            {
                _authModel = value;
                OnPropertyChanged();
            }
        }

        private ICommand _signInCommand;
        public ICommand SignInCommand
        {
            get
            {
                return _signInCommand ??
                (_signInCommand = new RelayCommand(async param =>
                {
                    _signInButtonEnabled = false;
                    var password = ((System.Windows.Controls.PasswordBox)param).Password;
                    if (AuthModel.Login.Length >= 5 && password.Length >= 1)
                    {
                        try
                        {
                            JournalClient journal = await JournalClient.SignInAsync(new Uri("https://localhost:5001"), AuthModel.Login, password);
                            ((MainWindowViewModel)App.Current.MainWindow.DataContext).JournalClient = journal;
                            ((MainWindow)App.Current.MainWindow).NavigateToRelative("Views/ProfilePage.xaml");
                        }
                        catch (RequestErrorException e)
                        {
                            Message = e.Error.Message;
                        }
                        catch (ExecuteQueryException e)
                        {
                            Message = "Не удалось получить доступ к серверу";
                            // добавить куда-то вывод исключения
                        }
                        catch (Exception e)
                        {
                            Message = "Возникло иключение";
                        }
                    }
                    else
                        Message = "Неверное заполнение логина или пароля";
                    _signInButtonEnabled = true;
                }));
            }
        }


        private bool _signInButtonEnabled = true;
        public bool SignInButtonEnabled
        {
            get => _signInButtonEnabled;
            set
            {
                _signInButtonEnabled = value;
                OnPropertyChanged();
            }
        }

        public AuthViewModel()
        {
        }
    }

    public class RelayCommand : ICommand
    {
#pragma warning disable CS0067
        public event EventHandler CanExecuteChanged;
#pragma warning restore CS0067
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;
        public RelayCommand (Action<object> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute ?? new Func<object,bool>((_) => true);
        }

        public bool CanExecute(object parameter) => _canExecute(parameter);

        public void Execute(object parameter) => _execute(parameter);
    }
}
