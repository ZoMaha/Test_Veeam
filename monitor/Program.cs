using System;
using System.Diagnostics;
using System.ComponentModel;
using System.Threading;
using System.IO;

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
                lifetime = 2;
                checkFrequency = 1;
                Console.WriteLine("You didn't provide input parameters");
            }

            try
            {
                DateTime now = DateTime.Now;
                string filename = @"D:\logMonitor\monitorlog" + now.ToShortDateString() + "_" + now.Hour + "." + now.Minute + "." + now.Second + ".txt";
                //File logfile = File.CreateText(filename);
                using (StreamWriter logfile = File.CreateText(filename))
                {
                    string addString = "The \"monitor\" was start at " + now.ToString();
                    logfile.WriteLine(addString);
                    Console.WriteLine(addString);
                    while (Console.ReadKey().Key != ConsoleKey.Escape)
                    {
                        addString = "\"Monitor\" started searching for processes at " + DateTime.Now.ToString();
                        logfile.WriteLine(addString);
                        Console.WriteLine(addString);
                        foreach (var process in Process.GetProcessesByName(processName))
                        {
                            addString = "Process named \"" + processName + "\" was found at " + DateTime.Now.ToString();
                            logfile.WriteLine(addString);
                            Console.WriteLine(addString);

                            DateTime date = process.StartTime;
                            now = DateTime.Now;
                            TimeSpan timeSpan = now - date;
                            addString = "The lifetime of process with name \"" + processName + "\" is " + timeSpan.TotalMinutes + " minutes";
                            logfile.WriteLine(addString);
                            Console.WriteLine(addString);
                            if (timeSpan.TotalMinutes >= lifetime)
                            {
                                process.Kill();
                                addString = "Process named \"" + processName + "\" killed on " + DateTime.Now.ToString();
                                logfile.WriteLine(addString);
                                Console.WriteLine(addString);
                            }
                            Console.WriteLine();
                        }
                        Thread.Sleep(checkFrequency * 60000);
                    }
                    addString = "\"Monitor\" endind its work on " + DateTime.Now.ToString();
                    logfile.WriteLine(addString);
                    Console.WriteLine(addString);
                    Environment.Exit(0);
                }
            }
            catch
            {
                Console.WriteLine("ERROR");
            }
            
        }
    }
}

