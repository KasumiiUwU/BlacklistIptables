namespace BlacklistIptables.Services;

public interface IpTableServiceInterface
{ 
    public bool BlockIpWithIptables(string ip);
    
    public bool DeleteBlacklistRule(string ip);

    public bool IsIpBlacklisted(string ip);
}