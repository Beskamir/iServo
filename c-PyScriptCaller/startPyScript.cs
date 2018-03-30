using System;
using System.Diagnostics;
using System.IO;
 
 public static string startPYScript(string fullFILEPATH, string arg1, string arg2)
        {
            //harcoded lolol
            string python = @"C:\Python34\python.exe"; //You will need to add python.exe, it does not show up on inter script

            // python app to call  ALSO HARDCODED FULL PATH
            string myPythonApp = fullFILEPATH;

            //parameters to send Python script 
            string x = arg1;
            string y = arg2;

            // Create new process start info 
            ProcessStartInfo myProcessStartInfo = new ProcessStartInfo(python);

            // make sure we can read the output from stdout 
            myProcessStartInfo.UseShellExecute = false;
            myProcessStartInfo.RedirectStandardOutput = true;

            // start python app with 3 arguments  
            // 1st arguments is pointer to itself,  
            // 2nd and 3rd are actual arguments we want to send 
            myProcessStartInfo.Arguments = myPythonApp + " " + x + " " + y;

            Process myProcess = new Process();
            // assign start information to the process 
            myProcess.StartInfo = myProcessStartInfo;

            Console.WriteLine("Calling Python script with arguments" + " " + x + " " + y);
            // start the process 
            myProcess.Start();

            // Read the standard output of the app we called.  
            // in order to avoid deadlock we will read output first 
            // and then wait for process terminate: 
            StreamReader myStreamReader = myProcess.StandardOutput;
            //string myString = myStreamReader.ReadLine();

            /*if you need to read multiple lines, you might use: */
            string myString = myStreamReader.ReadToEnd();

            // wait exit signal from the app we called and then close it. 
            myProcess.WaitForExit();
            myProcess.Close();

            return myString;
        }
        static void Main(string[] args)
        {
            Console.Write("YO, Started\n");
            String response = Program.startPYScript("C:\\Users\\asjad\\Desktop\\inter.py","BOOMBOOM", "Numb1 mnumb2");
            Console.Write(response);
        }
