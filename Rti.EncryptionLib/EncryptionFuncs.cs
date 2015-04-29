using System;
using System.Text;
using System.Security.Cryptography;
using System.IO;


namespace Rti.EncryptionLib
{
    public class EncryptionFuncs
    {
        private static byte[] key = { 199, 155, 42, 38, 51, 75, 52, 34, 111, 111, 38, 111, 12, 111, 125, 117, 211, 65, 162, 114, 222, 47, 215, 89, 65, 45, 52, 187, 192, 117, 63, 233 };
        private static byte[] vector = { 135, 62, 187, 107, 13, 5, 140, 120, 217, 117, 220, 110, 77, 31, 111, 137 };
        private ICryptoTransform encryptor, decryptor;
        private UTF8Encoding encoder;

        public EncryptionFuncs()
        {
            var rm = new RijndaelManaged();
            encryptor = rm.CreateEncryptor(key, vector);
            decryptor = rm.CreateDecryptor(key, vector);
            encoder = new UTF8Encoding();
        }

        public string Encrypt(string unencrypted)
        {
            return Convert.ToBase64String(Encrypt(encoder.GetBytes(unencrypted)));
        }

        public string Decrypt(string encrypted)
        {
            return encoder.GetString(Decrypt(Convert.FromBase64String(encrypted)));
        }

        public byte[] Encrypt(byte[] buffer)
        {
            return Transform(buffer, encryptor);
        }

        public byte[] Decrypt(byte[] buffer)
        {
            return Transform(buffer, decryptor);
        }

        protected byte[] Transform(byte[] buffer, ICryptoTransform transform)
        {
            var stream = new MemoryStream();
            using (var cs = new CryptoStream(stream, transform, CryptoStreamMode.Write))
            {
                cs.Write(buffer, 0, buffer.Length);
            }
            return stream.ToArray();
        }

        public string TripleDESEncode(string str)
        {
            try
            {
                if (str.Length == 0)
                    return null;
                TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
                Byte[] iv = new Byte[8];
                des.IV = iv;
                PasswordDeriveBytes pdb = new PasswordDeriveBytes("St$$#ef)!en", Encoding.ASCII.GetBytes("St$$#ef)!en".Length.ToString()));
                des.Key = pdb.CryptDeriveKey("TripleDES", "SHA1", 192, des.IV);
                MemoryStream ms = new MemoryStream((str.Length * 2) - 1);
                CryptoStream encStream =
                    new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
                Byte[] plainbytes = System.Text.Encoding.UTF8.GetBytes(str);

                encStream.Write(plainbytes, 0, plainbytes.Length);
                encStream.FlushFinalBlock();

                Byte[] encryptedBytes = ms.ToArray();
                ms.Position = 0;
                ms.Read(encryptedBytes, 0, (int)(ms.Length));

                encStream.Close();

                return Convert.ToBase64String(encryptedBytes);
            }
            catch
            {
                return null;
            }
        }

        public string TripleDESDecode(string str)
        {
            try
            {
                if (str.Length <= 0)
                    return null;
                TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
                Byte[] iv = new Byte[8]; ;
                des.IV = iv;
                PasswordDeriveBytes pdb = new PasswordDeriveBytes("St$$#ef)!en", Encoding.ASCII.GetBytes("St$$#ef)!en".Length.ToString()));
                des.Key = pdb.CryptDeriveKey("TripleDES", "SHA1", 192, des.IV);
                Byte[] encryptedbytes = Convert.FromBase64String(str);
                MemoryStream ms = new MemoryStream(str.Length);
                CryptoStream decstream = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
                decstream.Write(encryptedbytes, 0, encryptedbytes.Length);
                decstream.FlushFinalBlock();
                Byte[] plainbytes = ms.ToArray();
                ms.Position = 0;
                ms.Read(plainbytes, 0, (int)(ms.Length - 1));
                decstream.Close();
                return System.Text.Encoding.UTF8.GetString(plainbytes);
            }
            catch
            {
                return null;
            }
        }

    }
}
