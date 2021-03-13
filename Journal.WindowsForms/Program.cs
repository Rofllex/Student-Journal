using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;
using System.Reflection;

using Newtonsoft.Json;

using Journal.Common;
using Journal.Common.Entities;

using Journal.ClientLib;

#nullable enable

namespace Journal.WindowsForms
{
    static class Program
    {
        static Program()
        {
            ExecutableDirectoryPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location!)!;
            AuthenticationConfigPath = Path.Combine(ExecutableDirectoryPath!, AUTHENTICATION_FILE_PATH);
        }

        private const string AUTHENTICATION_FILE_PATH = "auth.json";
        
        private static readonly string AuthenticationConfigPath;

        public static readonly string ExecutableDirectoryPath;



        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            Application.SetHighDpiMode( HighDpiMode.SystemAware );
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault( false );

            ViewModels.AuthenticationModel? authenticationModel = _LoadAuthenticationModel( AuthenticationConfigPath );

            Forms.AuthenticationForm authenticationForm = new Forms.AuthenticationForm( authenticationModel );
            if ( authenticationForm.ShowDialog() == DialogResult.OK )
            {
                authenticationModel = authenticationForm.ViewModel.Model;
                if ( !authenticationModel.RememberPassword )
                    authenticationModel.Password = string.Empty;
                _SaveAuthenticationModel( AuthenticationConfigPath, authenticationModel );

                JournalClient? journalClient = authenticationForm.JournalClient;
                Debug.Assert( journalClient != null );

                Form currentForm;
                switch (journalClient.CurrentUser.Role)
                {
                    case UserRole.Admin:
                        currentForm = new Forms.AdminPanelForm( authenticationForm.JournalClient );
                        break;
                    case UserRole.Student:
                        currentForm = new Forms.StudentForm( journalClient );
                        break;
                    case UserRole.Teacher:
                        currentForm = new Forms.TeacherForm( journalClient );
                        break;
                    default:
                        throw new InvalidProgramException("Ни одна из форм не найдена.");
                }
                                
                Application.Run( currentForm );
            }
        }

        private static void CurrentDomain_UnhandledException( object sender, UnhandledExceptionEventArgs e )
        {
            if ( !Debugger.IsAttached )
            {
                string crashLogsDirectory = Path.Combine( ExecutableDirectoryPath, "crashlogs" );
                if ( !Directory.Exists( crashLogsDirectory ) )
                    Directory.CreateDirectory( crashLogsDirectory );
                string crashLogFileName = DateTime.Now.ToString( "MM.dd-HH.mm.ss.txt" )
                     , crashLogFilePath = Path.Combine( crashLogsDirectory, crashLogFileName );
                StreamWriter sw;
                if ( File.Exists( crashLogFilePath ) )
                    sw = File.CreateText( crashLogFileName );
                else
                {
                    sw = File.AppendText( crashLogFilePath );
                    sw.WriteLine( new string( '-', 30 ) );
                }

                sw.WriteLine( e.ExceptionObject.ToString() );
                sw.Dispose();
            }
        }

        private static ViewModels.AuthenticationModel? _LoadAuthenticationModel(string configFilePath)
        {
            if ( File.Exists( configFilePath ) )
            {
                try
                {
                    using (StreamReader reader = File.OpenText( configFilePath ))
                    {
                        return JsonConvert.DeserializeObject<ViewModels.AuthenticationModel>( reader.ReadToEnd() );
                    }
                }
                catch
                {
                    try
                    {
                        File.Delete( configFilePath );
                    }
                    catch { }

                    return null;
                }
            }
            else
                return null;
        }

        private static void _SaveAuthenticationModel( string configFilePath, ViewModels.AuthenticationModel model )
        {
            if ( model == null )
                throw new ArgumentNullException();
            string serializedModel = JsonConvert.SerializeObject( model );
            StreamWriter sw;
            if ( File.Exists( configFilePath ) )
            {
                FileStream fs = File.Open( configFilePath, FileMode.Open );
                StreamReader reader = new StreamReader( fs );
                if (serializedModel == reader.ReadToEnd())
                {
                    fs.Dispose();
                    return;
                }

                if ( fs.Length > serializedModel.Length )
                    serializedModel += new string( '\0', (int)(fs.Length - serializedModel.Length) );
                sw = new StreamWriter( fs );
                sw.BaseStream.Position = 0;
            }
            else
                sw = File.CreateText( configFilePath );
            try
            {
                sw.Write( serializedModel );
            }
            finally 
            { 
                sw.Dispose(); 
            }
        }
    }
}
