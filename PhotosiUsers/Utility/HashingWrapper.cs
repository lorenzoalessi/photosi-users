using System.Security.Cryptography;
using System.Text;

namespace PhotosiUsers.Utility;

// Classe di estensione
public static class HashingWrapper
{
    public static string ConvertToSha512(this string value)
    {
        // To byte
        var inputBytes = Encoding.UTF8.GetBytes(value);

        // Calcola l'hash SHA-512
        var hashBytes = SHA512.HashData(inputBytes);
        return Convert.ToBase64String(hashBytes);
    }
}