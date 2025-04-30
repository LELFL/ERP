using Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace Utils;
// 基础设施层
public class Md5PasswordEncryptor : IPasswordEncryptor
{
    public string Encrypt(string input)
    {
        // 校验输入
        if (string.IsNullOrEmpty(input))
            return string.Empty;

        // 创建MD5实例（自动释放资源）
        using MD5 md5 = MD5.Create();

        // 将输入字符串转换为UTF8字节数组
        byte[] inputBytes = Encoding.UTF8.GetBytes(input);

        // 计算哈希值
        byte[] hashBytes = md5.ComputeHash(inputBytes);

        // 将哈希字节数组转换为十六进制字符串
        StringBuilder sb = new();
        foreach (byte b in hashBytes)
        {
            sb.Append(b.ToString("x2")); // "x2"表示两位小写十六进制
        }
        return sb.ToString();
    }
}
