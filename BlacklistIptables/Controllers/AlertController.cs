﻿using System.Diagnostics;
using System.Net;
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
    private readonly IpTableServiceInterface _ipPtablesService;

    public AlertController(IpTableServiceInterface ipPtablesService)
    {
        _ipPtablesService = ipPtablesService;
    }


    [HttpPost("BlacklistIp")]
    public async Task<IActionResult> BlacklistIp([FromBody] AlertData alert)
    {
        if (!IPAddress.TryParse(alert.Ip, out var ipAddress))
        {
            return CustomResult("Invalid IP address.", HttpStatusCode.BadRequest);
        }

        var result = _ipPtablesService.BlockIpWithIptables(alert.Ip);
        if (result)
        {
            return CustomResult($"IP {alert.Ip} has been successfully blacklisted.");
        }

        return StatusCode(500, "Failed to update iptables.");
    }

    [HttpPost("DeleteBlacklistIp")]
    public async Task<IActionResult> DeleteBlacklistIp([FromBody] AlertData alert)
    {
        if (!IPAddress.TryParse(alert.Ip, out var ipAddress))
        {
            return CustomResult("Invalid IP address.", HttpStatusCode.BadRequest);
        }
        var result = _ipPtablesService.BlockIpWithIptables(alert.Ip);
        if (result)
        {
            return CustomResult($"IP {alert.Ip} has been successfully blacklisted.");
        }
        
        return StatusCode(500, "Failed to update iptables.");
    }
    
    
}