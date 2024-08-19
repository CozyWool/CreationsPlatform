using System.Security.Cryptography;
using System.Text;

namespace CreationsPlatformWebApplication.Helpers;

public static class SecurityHelper
{
    public static string GenerateSaltedHash(string password, string salt)
    {
        var passwordBytes = Encoding.Unicode.GetBytes(password);
        var saltBytes = Encoding.Unicode.GetBytes(salt);

        var passwordWithSaltBytes = new byte[passwordBytes.Length + saltBytes.Length];
        passwordBytes.CopyTo(passwordWithSaltBytes, 0);
        saltBytes.CopyTo(passwordWithSaltBytes, passwordBytes.Length);
        return Convert.ToBase64String(SHA256.HashData(passwordWithSaltBytes));
    }

    public static bool PasswordMatch(string password, string salt, string hash)
    {
        return hash == GenerateSaltedHash(password, salt);
    }
}