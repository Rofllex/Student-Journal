using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Journal.ClientLib;

namespace Journal.WindowsForms.Forms
{
    public partial class StudentForm : Form
    {
        public StudentForm(JournalClient journalClient)
        {
            JournalClient = journalClient;
            InitializeComponent();
        }

        public JournalClient JournalClient { get; private set; }
    }
}
