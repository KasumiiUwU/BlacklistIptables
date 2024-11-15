using System;
using System.Diagnostics;

namespace BlacklistIptables.Services;

public class IPtablesService : IpTableServiceInterface
{
    public bool BlockIpWithIptables(string ip)
    {
        try
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = "/bin/bash",
                Arguments = $"-c \"sudo iptables -A INPUT -s {ip} -j DROP\"",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            using var process = Process.Start(startInfo);
            process.WaitForExit();
            return process.ExitCode == 0;
        }
        catch (Exception e)
        {
            // Log exception details here using preferred logging method
            return false;
        }
    }

    public bool DeleteBlacklistRule(string ip)
    {
        try
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = "/bin/bash",
                Arguments = $"-c \"sudo iptables -D INPUT -s {ip} -j DROP\"",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            using var process = Process.Start(startInfo);
            process.WaitForExit();
            return process.ExitCode == 0;
        }
        catch (Exception e)
        {
            // Log exception details here using preferred logging method
            return false;
        }
    }

    public bool IsIpBlacklisted(string ip)
    {
        try
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = "/bin/bash",
                Arguments = $"-c \"sudo iptables -C INPUT -s {ip} -j DROP\"",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            //var process = ExecuteIptablesCommand($"-C INPUT -s {ip} -j DROP");
            using var process = Process.Start(startInfo);
            process.WaitForExit();
            
            var output = process.StandardOutput.ReadToEnd();
            var error = process.StandardError.ReadToEnd();

            // Log output and error for debugging purposes (or handle them as needed)
            Console.WriteLine($"Output: {output}");
            Console.WriteLine($"Error: {error}");
            
            if (error.Contains("Bad rule"))
            {
                return false;
            }
            
            return process.ExitCode == 0;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Exception occurred while checking IP blacklist status: {e.Message}");
            return true;
        }
       
    }
}