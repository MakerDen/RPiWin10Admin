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
        const string BASE_IP_ADDRESS = "192.168.1.{0}\tMINWINPC";
        const string FILE_NAME = "hosts";

        const int FIRST_MACHINE = 1;
        const int START_ADDRESS = 101;
        const int MACHINE_COUNT = 10;
        static void Main(string[] args)
        {
            var rootFolder = Directory.CreateDirectory("MakerDen Batch Files");
            var batFolder = rootFolder.CreateSubdirectory("batchCopy");
            var piBatFolder = rootFolder.CreateSubdirectory("piBatFiles");
            var hostsFilesFolder = rootFolder.CreateSubdirectory("HostsFiles");
            for (int i = 0; i < MACHINE_COUNT; i++)
            {
                var machineName = string.Format(BASE_MACHINE_NAME, i + FIRST_MACHINE);
                var ipAddress = string.Format(BASE_IP_ADDRESS, i + START_ADDRESS);
                // create the folder
                var hostsFolder = hostsFilesFolder.CreateSubdirectory(machineName);
                // write the file
                using (var file = File.CreateText(Path.Combine(rootFolder.Name, hostsFilesFolder.Name, hostsFolder.Name, FILE_NAME)))
                {
                    file.WriteLine(ipAddress);
                }
                using (var file = File.CreateText(Path.Combine(rootFolder.Name, batFolder.Name, $"copyfiles{i+FIRST_MACHINE:D2}.bat")))
                {
                    file.WriteLine("@ECHO OFF");
                    file.WriteLine("mkdir c:\\source");
                    file.WriteLine("echo.Copying hosts file");
                    file.WriteLine($"xcopy \"%~dp0\\..\\HostsFiles\\RPi{i+FIRST_MACHINE:D2}\\hosts\" c:\\windows\\system32\\drivers\\etc\\ /Y");
                    file.WriteLine("echo.Copying Background image");
                    file.WriteLine($"xcopy \"%~dp0\\..\\Desktop Images\\RPi{i + FIRST_MACHINE:D2}.jpg\" c:\\users\\Dev\\Pictures\\ /Y");
                    file.WriteLine("echo.Done");
                    file.WriteLine("echo.Copying Pi Network Batch File");
                    file.WriteLine($"xcopy \"%~dp0\\..\\piBatFiles\\RPi192.168.1.{i + START_ADDRESS:D3}.bat\" c:\\source\\ /Y");
                    file.WriteLine("xcopy \"%~dp0\\..\\CloneMakerDen.bat\" c:\\source\\ /Y");
                    file.WriteLine("xcopy \"%~dp0\\..\\ResetMakerDen.bat\" c:\\source\\ /Y");
                    file.WriteLine("xcopy \"%~dp0\\..\\Git-1.9.5-preview20150319.exe\" c:\\source\\ /Y");
                    file.WriteLine("xcopy \"%~dp0\\..\\Reset Labs.lnk\" c:\\users\\Dev\\Desktop\\ /Y");
                    file.WriteLine("start c:\\source\\Git-1.9.5-preview20150319.exe");
                    file.WriteLine("echo.Done");
                    file.WriteLine("PAUSE");
                }

                using (var file = File.CreateText(Path.Combine(rootFolder.Name, piBatFolder.Name, $"RPi192.168.1.{i + START_ADDRESS:D3}.bat")))
                {
                    file.WriteLine("netsh interface ipv4 set dns \"Wi-Fi\" static 192.168.1.1");
                    // file.WriteLine("netsh interface ipv4 add dns name=\"ethernet\" address=10.1.1.6 index=2");
                    file.WriteLine($"netsh interface ipv4 set address \"Wi-Fi\" static 192.168.1.{i+START_ADDRESS} 255.255.255.0 192.168.1.1");
                    file.WriteLine();
                    file.WriteLine("w32tm /resync");
                    file.WriteLine();
                    file.WriteLine($"wmic computersystem where name=\"%COMPUTERNAME%\" call rename name=\"RPi{i + FIRST_MACHINE:D2}\"");
                    file.WriteLine();
                    file.WriteLine("shutdown /r /f /t 10");
                }
            }

        }
    }
}
