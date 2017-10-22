using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace InjectorConfigWakfu
{
    class Program
    {
        static Process process { get; set; }
        static string gamePath { get; set; }

        static void Main(string[] args)
        {
            GetPath();
            StartUpdater();
            Console.WriteLine("Stand WakfuHandler for inject in config!");
            Injector();
        }

        public static void StartUpdater()
        {
            if (File.Exists($"{gamePath}\\Wakfu.exe"))
            {
                Console.WriteLine("Execute Wakfu Updater.");
                Process.Start($"{gamePath}\\Wakfu.exe");
            }
            else
            {
                Console.WriteLine($"Game path invalid. Impossible get Wakfu.exe");
                GetPath();
            }
        }

        public static void GetPath()
        {
            if (!File.Exists($"{AppContext.BaseDirectory}\\gamepath"))
            {
                Console.Write("Enter your game path : ");
                File.WriteAllText($"{AppContext.BaseDirectory}\\gamepath", Console.ReadLine());
            }
            else
                gamePath = File.ReadAllLines($"{AppContext.BaseDirectory}\\gamepath")[0];
        }

        public static void Injector()
        {
            do
            {
                var processes = Process.GetProcesses();
                if(processes.Length > 0)
                    process = processes.FirstOrDefault(x => x.MainWindowTitle == "WAKFU");
            } while (process == null);
            lineChanger("dispatchAddresses=127.0.0.1:5558;80", $"{gamePath}\\game\\config.properties", 332);
        }

        static void lineChanger(string newText, string fileName, int line_to_edit)
        {
            string[] arrLine = File.ReadAllLines(fileName);
            arrLine[line_to_edit - 1] = newText;
            File.WriteAllLines(fileName, arrLine);
        }
    }
}
