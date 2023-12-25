using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Security.Cryptography;
using System.Text;

namespace StandSystem.Controllers;

[ApiController]
[Route("api/ssh-auth")]
public class SshAuthController : ControllerBase
{
    private Dictionary<string, string> authorizedPublicKeys = new Dictionary<string, string>();

    [HttpPost("add-key")]
    public IActionResult AddPublicKey([FromBody] PublicKeyModel model)
    {
        authorizedPublicKeys[model.UserId] = model.PublicKey;
        return Ok("Public key added successfully");
    }

    [HttpPost("authenticate")]
    public IActionResult Authenticate([FromBody] AuthRequestModel model)
    {
        if (authorizedPublicKeys.TryGetValue(model.UserId, out string storedPublicKey))
        {
            if (IsValidKey(model.PublicKey, storedPublicKey))
            {
                return Ok("User authenticated successfully");
            }
        }
        return BadRequest("Authentication failed");
    }

    private bool IsValidKey(string presentedKey, string storedKey)
    {
        using (var sha256 = SHA256.Create())
        {
            byte[] presentedKeyBytes = Encoding.UTF8.GetBytes(presentedKey);
            byte[] storedKeyBytes = Convert.FromBase64String(storedKey);

            byte[] presentedKeyHash = sha256.ComputeHash(presentedKeyBytes);

            return presentedKeyHash.SequenceEqual(storedKeyBytes);
        }
    }

    [HttpPost("generate-challenge")]
    public IActionResult GenerateAndEncryptChallenge([FromBody] ChallengeRequestModel model)
    {
        if (authorizedPublicKeys.TryGetValue(model.UserId, out string publicKey))
        {
            try
            {
                using (var rsa = RSA.Create())
                {
                    rsa.FromXmlString(publicKey);
                    byte[] challenge = Encoding.UTF8.GetBytes(Guid.NewGuid().ToString("N"));

                    byte[] encryptedChallenge = rsa.Encrypt(challenge, RSAEncryptionPadding.Pkcs1);
                    string base64EncryptedChallenge = Convert.ToBase64String(encryptedChallenge);

                    return Ok(new { Challenge = base64EncryptedChallenge });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to generate challenge: {ex.Message}");
            }
        }
        return BadRequest("User not found or public key not available");
    }
}

public class ChallengeRequestModel
{
    public string UserId { get; set; }
}

public class PublicKeyModel
{
    public string UserId { get; set; }
    public string PublicKey { get; set; }
}

public class AuthRequestModel
{
    public string UserId { get; set; }
    public string PublicKey { get; set; }
}
