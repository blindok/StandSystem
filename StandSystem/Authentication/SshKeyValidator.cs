namespace StandSystem.Authentication;

public interface ISshKeyValidator
{
    bool IsValid(string SshKey);
}

public class SshKeyValidator : ISshKeyValidator
{
    public bool IsValid(string SshKey)
    {
        // logic
        return true;
    }
}
