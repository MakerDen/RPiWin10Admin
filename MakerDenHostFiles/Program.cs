using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MakerDenHostFiles
{
    class Program
    {
        const string BASE_MACHINE_NAME = "RPi{0:D2}";
        const string BASE_IP_ADDRESS = "10.7.4.{0}\tMINWINPC";
        const string FILE_NAME = "hosts";

        const int FIRST_MACHINE = 1;
        const int START_ADDRESS = 1;
        const int MACHINE_COUNT = 30;
        static void Main(string[] args)
        {
            var batFolder = Directory.CreateDirectory("batchCopy");
            var piBatFolder = Directory.CreateDirectory("piBatFiles");
            for (int i = 0; i < MACHINE_COUNT; i++)
            {
                var machineName = string.Format(BASE_MACHINE_NAME, i + FIRST_MACHINE);
                var ipAddress = string.Format(BASE_IP_ADDRESS, i + START_ADDRESS);
                // create the folder
                var hostsFolder = Directory.CreateDirectory(machineName);
                // write the file
                using (var file = File.CreateText(Path.Combine(hostsFolder.Name, FILE_NAME)))
                {
                    file.WriteLine(ipAddress);
                }
                using (var file = File.CreateText(Path.Combine(batFolder.Name, $"copyfiles{i+FIRST_MACHINE:D2}.bat")))
                {
                    file.WriteLine("@ECHO OFF");
                    file.WriteLine("echo.Copying hosts file");
                    file.WriteLine($"xcopy \"%~dp0\\..\\Hosts Files\\RPi{i+FIRST_MACHINE:D2}\\hosts\" c:\\windows\\system32\\drivers\\etc\\ /Y");
                    file.WriteLine("echo.Copying Background image");
                    file.WriteLine($"xcopy \"%~dp0\\..\\Desktop Images\\RPi{i + FIRST_MACHINE:D2}.jpg\" c:\\users\\IoT\\Pictures\\ /Y");
                    file.WriteLine("echo.Done");
                    file.WriteLine("echo.Copying Pi Network Batch File");
                    file.WriteLine($"xcopy \"%~dp0\\..\\piBatFiles\\RPi10.7.4.{i + START_ADDRESS:D3}.bat\" c:\\source\\ /Y");
                    file.WriteLine("echo.Done");
                    file.WriteLine("PAUSE");
                }

                using (var file = File.CreateText(Path.Combine(piBatFolder.Name, $"RPi10.7.4.{i + START_ADDRESS:D3}.bat")))
                {
                    file.WriteLine("netsh interface ipv4 set dns \"ethernet\" static 10.1.1.5");
                    file.WriteLine("netsh interface ipv4 add dns name=\"ethernet\" address=10.1.1.6 index=2");
                    file.WriteLine($"netsh interface ipv4 set address \"ethernet\" static 10.7.4.{i+START_ADDRESS:D3} 255.255.0.0 10.7.0.1");
                    file.WriteLine("shutdown /s /t 10");
                }
            }

        }
    }
}
