using System;
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
            DialogResult dialogResult = authenticationForm.ShowDialog();
            if (dialogResult == DialogResult.OK )
            {
                Application.Run( new Forms.MainForm() );
            }
        }
    }
}
