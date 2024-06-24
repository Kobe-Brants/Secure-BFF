using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Configuration;

namespace BL.Services.Encryption;

public class TokenEncryptionService : ITokenEncryptionService
{
    private readonly IDataProtector _protector;

    public TokenEncryptionService(IDataProtectionProvider dataProtectionProvider, IConfiguration configuration)
    {
        var key = configuration.GetValue<string>("Encryption:Key") ?? string.Empty;
        _protector = dataProtectionProvider.CreateProtector("key");
    }

    public string Encrypt(string input)
    {
        return _protector.Protect(input);
    }

    public string Decrypt(string input)
    {
        return _protector.Unprotect(input);
    }
}