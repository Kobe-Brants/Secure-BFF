namespace BL.Services.Encryption;

public interface ITokenEncryptionService
{ 
    string Encrypt(string input);
    string Decrypt(string input);
}