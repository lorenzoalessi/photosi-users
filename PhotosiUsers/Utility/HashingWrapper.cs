using System.Security.Cryptography;
using System.Text;

namespace PhotosiUsers.Utility;

// Classe di estensione
public static class HashingWrapper
{
    public static string ConvertToSha512(this string value)
    {
        // To byte
        byte[] inputBytes = Encoding.UTF8.GetBytes(value);
        
        // Calcola l'hash SHA-512
        using var sha512 = SHA512.Create();

        var hashBytes = sha512.ComputeHash(inputBytes);
        return Convert.ToBase64String(hashBytes);
    }
}