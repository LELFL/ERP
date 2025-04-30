namespace Interfaces;
/// <summary>
/// 加密明文密码。
/// </summary>
public interface IPasswordEncryptor
{
    string Encrypt(string plainPassword);
}
