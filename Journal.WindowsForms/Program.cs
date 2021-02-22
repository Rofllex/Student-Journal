using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace Journal.WindowsForms
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode( HighDpiMode.SystemAware );
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault( false );

            Forms.AuthenticationForm authenticationForm = new Forms.AuthenticationForm();
            if (authenticationForm.ShowDialog() == DialogResult.OK )
            {
                Debug.Assert( authenticationForm.JournalClient != null );
                Application.Run( new Forms.MainForm() );
            }
        }
    }
}
