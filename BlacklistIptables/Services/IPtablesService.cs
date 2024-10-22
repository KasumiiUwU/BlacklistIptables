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
            using (var process = Process.Start(startInfo))
            {
                process.WaitForExit();
                return process.ExitCode == 0;
            }
        }
        catch (Exception e)
        {
            // Log exception details here using preferred logging method
            return false;
        }
    }

    public bool DeteleBlacklistRule(string ip)
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
            using (var process = Process.Start(startInfo))
            {
                process.WaitForExit();
                return process.ExitCode == 0;
            }
        }
        catch (Exception e)
        {
            // Log exception details here using preferred logging method
            return false;
        }
    }
}