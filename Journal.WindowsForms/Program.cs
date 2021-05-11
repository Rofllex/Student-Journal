using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;
using System.Linq;
using System.Reflection;

using Newtonsoft.Json;

using Journal.Common;
using Journal.Common.Entities;

using Journal.ClientLib;
using System.Collections.Generic;

#nullable enable

namespace Journal.WindowsForms
{
    static class Program
    {
        public static void LogoutForm(Form form)
        {
            form.DialogResult = DialogResult.Retry;
            form.Close();
        }

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
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            while (true)
            {
                ViewModels.AuthenticationModel? authenticationModel = _LoadAuthenticationModel(AuthenticationConfigPath);

                Forms.AuthenticationForm authenticationForm = new Forms.AuthenticationForm(authenticationModel);
                if (authenticationForm.ShowDialog() == DialogResult.OK)
                {
                    authenticationModel = authenticationForm.ViewModel.Model;
                    if (!authenticationModel.RememberPassword)
                        authenticationModel.Password = string.Empty;
                    _SaveAuthenticationModel(AuthenticationConfigPath, authenticationModel);

                    JournalClient? journalClient = authenticationForm.JournalClient;
                    Debug.Assert(journalClient != null);

                    Form? currentForm = _ChoiseFormByRole(journalClient, journalClient.User.Role);
                    if (currentForm == null)
                    {
                        List<UserRole> roles = new List<UserRole>();
                        foreach (UserRole role in (UserRole[])Enum.GetValues(typeof(UserRole)))
                        {
                            if (journalClient.User.Role.HasFlag(role))
                                roles.Add(role);
                        }

                        Forms.SelectRoleForm selectRoleForm = new Forms.SelectRoleForm(roles);
                        if (selectRoleForm.ShowDialog() != DialogResult.OK)
                            return;
                        else
                        {
                            Debug.Assert(selectRoleForm.SelectedRole.HasValue);
                            currentForm = _ChoiseFormByRole(journalClient, selectRoleForm.SelectedRole.Value);
                            Debug.Assert(currentForm != null);
                        }
                    }

                    DialogResult result = currentForm.ShowDialog();
                    if (result != DialogResult.Retry)
                        break;
                }
                else
                    return;
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

        private static Form? _ChoiseFormByRole(JournalClient client, UserRole role)
            => role switch
            {
                UserRole.Admin => new Forms.AdminPanelForm(client),
                UserRole.Student => new Forms.StudentForm(client),
                UserRole.Teacher => new Forms.TeacherForm(client),
                _ => null
            };
    }
}
