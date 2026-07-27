namespace AucaJobPortalDotNet.Helpers;
public class PasswordHasher
{
    public string HashPassword(string plainPassword)
    {
        return BCrypt.Net.BCrypt.HashPassword(plainPassword );
    }
    public bool VerifyPassword(string plainPassword,string storedHash)
    {
        return BCrypt.Net.BCrypt.Verify(plainPassword,storedHash);
    }
}