using System;
using System.Diagnostics;
using System.ComponentModel;
using System.Threading;
using System.IO;
using System.Text;

namespace monitor
{
    class Program
    {
        private static bool stop = false;
        public static string filename = @"D:\logMonitor\monitorlog";

        static void Main(string[] args)
        {

            string processName;
            int lifetime;
            int checkFrequency;

            try
            {
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
                    Console.WriteLine("You didn't provide input parameters. Logfile didn't create.");
                    Environment.Exit(0);
                }

                System.Console.CancelKeyPress += new ConsoleCancelEventHandler(Console_CancelKeyPress);
                DateTime now = DateTime.Now;
                filename += now.ToShortDateString() + "_" + now.Hour + "." + now.Minute + "." + now.Second + ".txt";

                using (StreamWriter logfile = File.CreateText(filename))
                {
                    string addString = "The \"monitor\" was start at " + now.ToString();
                    logfile.WriteLine(addString);
                    Console.WriteLine(addString);
                }


                while (!stop)
                {

                    using (StreamWriter logfile = new StreamWriter(filename, true, Encoding.UTF8))
                    {
                        string addString = "\"Monitor\" started searching for processes at " + DateTime.Now.ToString();
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
                    }
                    Thread.Sleep(checkFrequency * 60000);
                }
            }
            catch
            {
                Console.WriteLine("ERROR");
            }

        }
        static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            stop = true;
            using (StreamWriter logfile = new StreamWriter(filename, true, Encoding.UTF8))
            {
                string addString = "\"Monitor\" endind its work on " + DateTime.Now.ToString();
                logfile.WriteLine(addString);
                Console.WriteLine(addString);
            }
            e.Cancel = false;
        }
    }

}

