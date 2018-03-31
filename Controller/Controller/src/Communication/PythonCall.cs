using System;
using System.Diagnostics;
using System.IO;
using Microsoft.VisualBasic;

namespace Controller.Communication
{
    public class PythonCall
    {
        private readonly ProcessStartInfo _pyStartInfo;
        private readonly Process _process = new System.Diagnostics.Process();
        private readonly string _pyPath;
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
            _pyPath = "/C py -3 ../../../pyScripts/HelloWorld.py";
            //_pyPath = "/K py -3 ../../../pyScripts/HelloWorld.py";
        }

        public string Send(string content)
        {
            string response = "";
            
            //Pass in the arguments
            _pyStartInfo.Arguments = _pyPath + " " + content;
            _process.StartInfo = _pyStartInfo;
            _process.Start();
            _process.WaitForExit();

            //get the output
            StreamReader responseReader = _process.StandardOutput;
            response = responseReader.ReadToEnd();
            Console.WriteLine(response);

            //todo: do something useful with the response
            return response;
        }
    }
}