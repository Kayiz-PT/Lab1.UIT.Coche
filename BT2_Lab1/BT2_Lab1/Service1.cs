using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.IO;
namespace BT2_Lab1
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }
        Timer timer = new Timer();
        //Closing the program func
        private void Closeprocess()
        {
            string[] pid = new string[10];
            int i = 0;
            Process p = new Process();
            foreach (var process in Process.GetProcessesByName("UniKeyNT"))
            {
                pid[i] = process.Id.ToString();
                i += 1;
            }
            for (int j = 0; j <= i - 1; j++)
            {
                Process proc = Process.GetProcessById(Convert.ToInt32(pid[j]));
                proc.Kill();
            }
        }
        protected override void OnStart(string[] args)
        {
            if (DateTime.Now.ToString("ddd") == "Wed") //Doi sang thu 4
            {
                Process.Start("D:\\Downloads\\Compressed\\UniKey\\UnikeyNT.exe");
                WriteToFile("Process start!");
            }
            WriteToFile("Checking start at " + DateTime.Now + " Name: UniKeyNT");
            timer.Elapsed += new ElapsedEventHandler(OnElapsedTime);
            timer.Interval = 5000;
            timer.Enabled = true;
            
        }
        //private void RestartService(string serviceName, int timeoutMilliseconds)
        //{
        //    ServiceController service = new ServiceController(serviceName);
        //    try
        //    {
        //        int millisec1 = Environment.TickCount;
        //        TimeSpan timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds);

        //        service.Stop();
        //        service.WaitForStatus(ServiceControllerStatus.Stopped, timeout);
        //        // count the rest of the timeout
        //        int millisec2 = Environment.TickCount;
        //        timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds - (millisec2 - millisec1));

        //        service.Start();
        //        service.WaitForStatus(ServiceControllerStatus.Running, timeout);
        //    }
        //    catch
        //    {
        //        // ...
        //    }
        //}
        protected override void OnStop()
        {
            WriteToFile("Checking is stopped at " + DateTime.Now);
        }
        private void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            if (DateTime.Now.ToString("ddd") != "Wed")
            {
                Process[] processes = Process.GetProcessesByName("UniKeyNT");
                Closeprocess();
                if (processes.Length > 0)
                    WriteToFile("\n|\n|---" + DateTime.Now + "    Status: Running");
                else
                {
                    WriteToFile("The process is not enabled");
                }
                Array.Clear(processes, 0, processes.Length);
            }
            else
            {
                Process[] processes = Process.GetProcessesByName("UniKeyNT");
                if (processes.Length > 0)
                    WriteToFile("\n|\n|---" + DateTime.Now + "    Status: Running");
                else
                {
                    WriteToFile("The process is not enabled");
                }
                Array.Clear(processes, 0, processes.Length);
            }
        }
        public void WriteToFile(string Message)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Logs";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filepath = AppDomain.CurrentDomain.BaseDirectory +
           "\\Logs\\ServiceLog_" + DateTime.Now.Date.ToShortDateString().Replace('/', '_') +
           ".txt";
            if (!File.Exists(filepath))
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
        }
    }
}
