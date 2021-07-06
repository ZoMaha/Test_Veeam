using System;
using System.Diagnostics;
using System.ComponentModel;
using System.Threading;

namespace monitor
{
    class Program
    {
        static void Main(string[] args)
        {
            string processName;
            int lifetime;
            int checkFrequency;
            try
            {
                processName = args[0];
                lifetime = Int32.Parse(args[1]);
                checkFrequency = Int32.Parse(args[2]);
            }
            catch
            {
                processName = "notepad";
                lifetime = 5;
                checkFrequency = 1;
            }

            try
            {

                while (true == true)
                {
                    foreach (var process in Process.GetProcessesByName(processName))
                    {
                        DateTime date = process.StartTime;
                        DateTime now = DateTime.Now;
                        TimeSpan timeSpan = now - date;
                        if (timeSpan.TotalMinutes >= lifetime)
                        {
                            process.Kill();
                        }
                        Thread.Sleep(checkFrequency * 60000);
                    }
                }
            }
            catch
            {
                Console.WriteLine("ERROR");
            }
            
        }
    }
}

