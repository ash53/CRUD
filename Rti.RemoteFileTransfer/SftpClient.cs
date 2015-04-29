using System;
using System.Diagnostics;
using System.IO;
using System.Linq;


namespace Rti.RemoteFileTransfer
{
    public class SftpClient
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger
            (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private string _Host;
        private string _User;
        private string _Password;

        private string _BatchFilePath = Constants.BinDirectory + @"batch.txt";
        private string _PsftpPath = Constants.BinDirectory + @"psftp"; /* Change this to the location 
    		    of the PSTFP app.  Do not include the '.exe' file extension. */
        public string Outputs = ""; /* Stores the outputs and errors of PSFTP */

        /* Object Constructor for standard usage */
        public SftpClient(string host, string user, string password)
        {
            _Host = host;
            _User = user;
            _Password = password;
        }

        /* Object Constructor for Threading */
        /* Specify the commands carefully */
        public SftpClient(string host, string user, string password, string[] commands)
        {
            _Host = host;
            _User = user;
            _Password = password;
            GenerateBatchFile(commands);
        }

        /* Retrieve files from the server */
        public bool Get(string remoteFilename, string localFile)
        {
            var commands = new string[1];
            //commands[0] = @"get " + Constants.ImportImageSftpPath + remoteFilename + " " + localFile;
            commands[0] = @"get " + remoteFilename + " " + localFile;

            GenerateBatchFile(commands);

            return Run();
        }

        /* Send file from your computer */
        public bool Put(string localFile, string remoteFilename)
        {
            var commands = new string[2];
            //commands[0] = @"put " + localFile + @" " + Constants.ImportImageSftpPath + remoteFilename;
            commands[0] = @"put " + localFile + @" " + remoteFilename;
            //change the permission
            commands[1] = @"chmod 666 " + remoteFilename;
            GenerateBatchFile(commands);
            
            return Run();
        }

        /* Use this to send other SFTP commands (CD, DIR, etc.) */
        public bool SendCommands(string[] commands)
        {
            GenerateBatchFile(commands);

            return Run();
        }

        /* Create a text file with a list of commands to be fed into PSFTP */
        private void GenerateBatchFile(string[] commands)
        {
            try
            {
                StreamWriter batchWriter = new StreamWriter(_BatchFilePath);

                /* Write each command to the batch file */
                for (int i = 0; i < commands.Count(); i++)
                {
                    Log.Debug(commands[i]);
                    batchWriter.WriteLine(commands[i]);
                }

                /* Command to close the connection */
                batchWriter.WriteLine(@"bye");

                batchWriter.Close();
            }
            catch (Exception ex) { Log.Error(ex.ToString()); }

            return;
        }

        /* Run the commands, store the outputs */

        private bool Run()
        {
            bool psftpStatus = true;

            /* Execute PSFTP as a System.Diagnostics.Process using the supplied login info and generated batch file */
            try
            {
                ProcessStartInfo psftpStartInfo = new ProcessStartInfo(_PsftpPath,
                    _User + @"@" + _Host + @" -pw " + _Password + @" -batch -be -b " + _BatchFilePath);

                var myCommand = _PsftpPath + " " + _User + @"@" + _Host + @" -pw <PW> -batch -be -b " + _BatchFilePath;
                Debug.Print("Command:[" + myCommand + "]");
                Log.Debug(myCommand);

                /* Allows redirecting inputs/outputs of PSFTP to your app */
                psftpStartInfo.RedirectStandardInput = true;
                psftpStartInfo.RedirectStandardOutput = true;
                psftpStartInfo.RedirectStandardError = true;
                psftpStartInfo.UseShellExecute = false;

                Process psftpProcess = new Process();
                psftpProcess.StartInfo = psftpStartInfo;
                psftpProcess.Start();

                /* Streams for capturing outputs and errors as well as taking ownership of the input */
                StreamReader psftpOutput = psftpProcess.StandardOutput;
                StreamReader psftpError = psftpProcess.StandardError;
                StreamWriter psftpInput = psftpProcess.StandardInput;

                while (!psftpOutput.EndOfStream)
                {
                    try
                    {
                        /* This is usefule for commands other than 'put' or 'get' 
                         and for catching errors. */
                        Outputs += psftpOutput.ReadLine();
                        Outputs += psftpError.ReadLine();
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex.ToString());
                    }
                }

                psftpOutput.Close();
                psftpError.Close();
                psftpInput.Close();
                psftpProcess.WaitForExit();
 
                Log.DebugFormat("Sftp Process exit code:[{0}]", psftpProcess.ExitCode);

                psftpProcess.Dispose();

                psftpStartInfo = null;
                psftpProcess = null;
            }
            catch (Exception ex)
            {
                psftpStatus = false;
                Log.Error(ex.ToString());
            }

            /* Delete the batch file */
            try
            {
                //File.Delete(_BatchFilePath);
            }
            catch (Exception ex)
            {
                psftpStatus = false;
                Log.Error(ex.ToString());
            }

            Log.DebugFormat("Exit, Status:[{0}], [{1}]", psftpStatus, Outputs);
            return psftpStatus;
        }
    }
}

static class ArrayExtensions
{
    public static void ForEach<T>(this T[] array, Func<T,T> action)
    {
        for (int i = 0; i < array.Length; i++)
        {
            array[i] = action(array[i]);
        }
    }

    public static void ForEach<T>(this T[] array, Action<T> action)
    {
        for (int i = 0; i < array.Length; i++)
        {
            action(array[i]);
        }
    }   
}