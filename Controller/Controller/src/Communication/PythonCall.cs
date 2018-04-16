using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Controls;
using Microsoft.VisualBasic;

namespace Controller.Communication
{
    public class PythonCall
    {
        private readonly ProcessStartInfo _pyStartInfo;
        private readonly Process _process = new System.Diagnostics.Process();
        private readonly string _pyPath;
        private string _command = "";
        private Thread _sendTread;
        public PythonCall()
        {
            _pyStartInfo = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = "cmd.exe", //use cmd to run stuff
                    UseShellExecute = false,
                    CreateNoWindow = true, //hide the cmd window
                    WindowStyle = ProcessWindowStyle.Hidden,
                    RedirectStandardOutput = true, //get output
                };
            //change following to be the script you want to run
            // "/c" will run the script and close the window
            // "/k" will keep the window open after running the script
            // might need to disable CreateNoWindow...
            _pyPath = "/C py -3 ../../../pyScripts/iServoController.py";
            //_pyPath = "/K py -3 ../../../pyScripts/HelloWorld.py";
        }

        public string Send(string content)
        {
            _command = content;
//            Console.WriteLine("command: " + _command);
            _sendTread = new Thread(SendingThread);
            _sendTread.SetApartmentState(ApartmentState.MTA);
            _sendTread.Start();
            
            string response = "";
            

//            int counter = 0;
//            while ( counter < 2000)
//            {
//                counter++;
//            }

            //do something useful with the response
            return response;
        }

        public void SendingThread()
        {
            string response = "";

            Console.WriteLine("Command Sent: " + _command);
            //Pass in the arguments
            _pyStartInfo.Arguments = _pyPath + " " + _command;
            _process.StartInfo = _pyStartInfo;
            _process.Start();
            _process.WaitForExit();

            //get the output
            StreamReader responseReader = _process.StandardOutput;
            response = responseReader.ReadToEnd();
            Console.WriteLine(response);   
            
//            _sendTread.Abort();
        }
    }
}