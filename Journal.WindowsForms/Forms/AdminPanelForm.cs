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
    public partial class AdminPanelForm : Form
    {
        public AdminPanelForm(JournalClient journalClient)
        {
            InitializeComponent();
        }

        private void tabControl1_SelectedIndexChanged( object sender, EventArgs e )
        {

        }
    }
}
