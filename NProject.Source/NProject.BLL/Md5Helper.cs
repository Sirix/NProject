using System.Security.Cryptography;

namespace NProject.BLL
{
    public static class MD5
    {
        public static string EncryptMD5(string value)
        {
            var md5 = new MD5CryptoServiceProvider();
            var valueArray = System.Text.Encoding.ASCII.GetBytes(value);
            valueArray = md5.ComputeHash(valueArray);
            var encrypted = "";
            for (var i = 0; i < valueArray.Length; i++)
                encrypted += valueArray[i].ToString("x2").ToLower();
            return encrypted;
        }
    }
}