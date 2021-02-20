using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Journal.WindowsForms.Forms
{
    public partial class MainForm : Form
    {
        public MainForm(  )
        {
            InitializeComponent();
        }

    }

    public class MainFormViewModel : ViewModel
    {
        public string UserName 
        {
            get => _userName;
            set
            {
                _userName = value;
                InvokePropertyChanged();
            }
        }

        private string _userName = string.Empty;
    }
}
