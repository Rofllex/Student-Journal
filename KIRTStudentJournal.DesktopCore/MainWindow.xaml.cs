using KIRTStudentJournal.DesktopCore.ViewModels;
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

namespace KIRTStudentJournal.DesktopCore
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindowViewModel ViewModel => (MainWindowViewModel)DataContext;

        public MainWindow()
        {
            InitializeComponent();
            frame.Navigate(new Uri("Views/AuthPage.xaml", UriKind.Relative));
            ((MainWindowViewModel)DataContext).VisibleLeftSideMenu = false;
        }
    
        public void NavigateToRelative(string relativePath) => frame.Navigate(new Uri(relativePath, UriKind.Relative));
        
    }
}
