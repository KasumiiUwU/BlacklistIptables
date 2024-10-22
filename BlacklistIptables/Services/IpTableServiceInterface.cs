namespace BlacklistIptables.Services;

public interface IpTableServiceInterface
{ 
    public bool BlockIpWithIptables(string ip);
    
    public bool DeteleBlacklistRule(string ip);
    
}