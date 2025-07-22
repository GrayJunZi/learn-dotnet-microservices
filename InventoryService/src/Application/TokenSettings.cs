namespace Application;

public  class TokenSettings
{
    public string Secret { get; set; }
}


public class CacheSettings
{
    public string ConnectionString { get; set; }
    public string ApplicationName { get; set; }
    public int SlidingExpiration { get; set; }
    public bool BypassCache { get; set; }
}