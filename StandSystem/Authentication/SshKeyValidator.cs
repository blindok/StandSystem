namespace StandSystem.Authentication;

public interface ISshKeyValidator
{
    bool IsValid(string SshKey, string ip);
}

public class SshKeyValidator : ISshKeyValidator
{
    public bool IsValid(string SshKey, string ip)
    {
        
        return true;
    }
}
