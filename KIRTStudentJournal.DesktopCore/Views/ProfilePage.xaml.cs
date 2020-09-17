using KIRTStudentJournal.DesktopCore.ViewModels;
using KIRTStudentJournal.NetLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KIRTStudentJournal.DesktopCore.Views
{
    /// <summary>
    /// Логика взаимодействия для ProfilePage.xaml
    /// </summary>
    public partial class ProfilePage : Page
    {
        public ProfilePage()
        {
            InitializeComponent();
            var viewModel = (PageProfileViewModel)DataContext;
            var journal = ((MainWindowViewModel)App.Current.MainWindow?.DataContext)?.JournalClient;
            if (journal != null)
            {
                viewModel.PageProfileModel.Role = Enum.GetName(typeof(Role), journal.Role);
            }
            else
            {
                viewModel.PageProfileModel.Role = "null";
            }
        }
    }
}
