using System.Diagnostics;
using System.IO;

namespace Controller.Communication
{
    public class PythonCall
    {
        private string _pathCompiler = @"C:\Program Files(x86)\Microsoft Visual Studio\Shared\Python36_64\python.exe";
        private string _pathScript = @"C:\Work\Education\cpsc584-HRI\iServos\Controller\Controller\src\Communication\test.py";
        private ProcessStartInfo pyStartInfo;
        public PythonCall()
        {
            pyStartInfo = new ProcessStartInfo(_pathCompiler);

            pyStartInfo.UseShellExecute = false;
            pyStartInfo.RedirectStandardOutput = true;
        }

        public string send(string content)
        {
            string response = "";
            pyStartInfo.Arguments = _pathScript + " " + content;
            
//            Process pyProcess = new Process();
//            pyProcess.StartInfo = pyStartInfo;
//            pyProcess.Start();
//
//
//            StreamReader responseReader = pyProcess.StandardOutput;
//            response = responseReader.ReadLine();
//
//            pyProcess.WaitForExit();

            return response;
        }
    }
}