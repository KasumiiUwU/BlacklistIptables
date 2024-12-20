﻿using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using BlacklistIptables.Models.Request;
using BlacklistIptables.Services;
using CoreApiResponse;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace BlacklistIptables.Controllers;

[Route("api/[controller]")]
[Controller]
public class AlertController : BaseController
{
    private readonly IpTableServiceInterface _iptablesService;

    public AlertController(IpTableServiceInterface iptablesService)
    {
        _iptablesService = iptablesService;
    }


    [HttpPost("BlacklistIp")]
    public async Task<IActionResult> BlacklistIp([FromBody] AlertData alert)
    {
        
        Console.WriteLine(alert.Ip);
        if (!IPAddress.TryParse(alert.Ip, out var ipAddress))
        {
            return CustomResult($"Invalid IP address. {alert.Ip}", HttpStatusCode.BadRequest);
        }

        if (_iptablesService.IsIpBlacklisted(alert.Ip))
        {
            return CustomResult("IP is already blacklisted!", HttpStatusCode.BadRequest);
        }
        
        var result = _iptablesService.BlockIpWithIptables(alert.Ip);
        if (result)
        {
            return CustomResult($"IP {alert.Ip} has been successfully blacklisted.");
        }

        return CustomResult( "Failed to update iptables.", HttpStatusCode.InternalServerError);
    }

    [HttpPost("DeleteBlacklistIp")]
    public async Task<IActionResult> DeleteBlacklistIp([FromBody] AlertData alert)
    {
        if (!IPAddress.TryParse(alert.Ip, out var ipAddress))
        {
            return CustomResult("Invalid IP address.", HttpStatusCode.BadRequest);
        }

        var result = _iptablesService.DeleteBlacklistRule(alert.Ip);
        if (result)
        {
            return CustomResult($"IP {alert.Ip} has been successfully remove from blacklisted.");
        }
        
        return CustomResult( "Failed to update iptables.", HttpStatusCode.InternalServerError);
    }
    
    
}