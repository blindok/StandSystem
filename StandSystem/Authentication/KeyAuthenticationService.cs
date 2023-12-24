using System.Security.Cryptography;
using System.Text;

namespace StandSystem.Authentication;

public class KeyAuthenticationService
{
    private Dictionary<string, string> trustedPublicKeys = new Dictionary<string, string>();

    public void AddTrustedPublicKey(string deviceId, string publicKey)
    {
        trustedPublicKeys[deviceId] = publicKey;
    }

    public string? GenerateAndEncryptChallenge(string deviceId)
    {
        if (trustedPublicKeys.TryGetValue(deviceId, out string publicKey))
        {
            using (var rsa = RSA.Create())
            {
                rsa.FromXmlString(publicKey);
                byte[] dataToEncrypt = Encoding.UTF8.GetBytes(Guid.NewGuid().ToString());
                byte[] encryptedData = rsa.Encrypt(dataToEncrypt, RSAEncryptionPadding.Pkcs1);
                return Convert.ToBase64String(encryptedData);
            }
        }
        return null;
    }

    public bool VerifyDecryptedResponse(string deviceId, string encryptedData, string decryptedData)
    {
        if (trustedPublicKeys.TryGetValue(deviceId, out string publicKey))
        {
            using (var rsa = RSA.Create())
            {
                rsa.FromXmlString(publicKey);
                byte[] encryptedDataBytes = Convert.FromBase64String(encryptedData);
                byte[] decryptedDataBytes = Convert.FromBase64String(decryptedData);
                byte[] decryptedDataFromEncrypted = rsa.Decrypt(encryptedDataBytes, RSAEncryptionPadding.Pkcs1);
                return decryptedDataBytes.SequenceEqual(decryptedDataFromEncrypted);
            }
        }
        return false;
    }
}
