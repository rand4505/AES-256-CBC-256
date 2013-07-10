using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Data;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace AESEncryptAndDecrypt
{
    public class SimplerAES
    {
        private byte[] mkey = { 202, 42, 168, 24, 48, 57, 76, 72, 113, 215, 114, 12, 17, 12, 152, 109, 41, 85, 90, 70, 68, 184, 196, 134, 175, 129, 117, 235, 135, 176, 153, 145 };
        public byte[] key{ set { mkey = value; } get { return mkey; } }
        private byte[] mvector = { 244, 74, 91, 11, 223, 37, 57, 145, 64, 135, 42, 57, 76, 28, 14, 116, 244, 74, 91, 11, 223, 37, 57, 145, 64, 135, 42, 57, 76, 28, 14, 116 };
        public byte[] vector{ get { return mvector; } set { mvector = value; } }
        private ICryptoTransform encryptor, decryptor;
        private UTF8Encoding encoder;
        private RijndaelManaged rm;
        private static string Salt = "I see THE siloutta of a MAN, skallamoo skallamoo TILL you do THE fandango, thunderbolts AND lightning, NOT as frightning as the NSA";
        private const int SizeOfBuffer = 1024 * 8;

        public SimplerAES() {
            rm = new RijndaelManaged();
            rm.KeySize = 256;
            rm.BlockSize = 256;
            encryptor = rm.CreateEncryptor(mkey, mvector);
            decryptor = rm.CreateDecryptor(mkey, mvector);
            encoder = new UTF8Encoding();
        }

        public SimplerAES(byte[] InnerK, byte[] InnerV) {
            rm = new RijndaelManaged();
            rm.KeySize = 256;
            rm.BlockSize = 256;
            encryptor = rm.CreateEncryptor(mkey, mvector);
            decryptor = rm.CreateDecryptor(mkey, mvector);
            encoder = new UTF8Encoding();
        }

        public bool FileOps(string File, string Mode, string Password) {
            if (Mode == "Encrypt") {
                return EncryptFile(File, File + "(encrypted)", Password);
            }
            if (Mode == "Decrypt") { 
                return DecryptFile(File, File + "(decrypted)", Password);
            }
            return false;
        }

        public void UpDateArrays() {
            rm = new RijndaelManaged();
            rm.KeySize = 256;
            rm.BlockSize = 256;
            encryptor = rm.CreateEncryptor(mkey, mvector);
            decryptor = rm.CreateDecryptor(mkey, mvector);
            encoder = new UTF8Encoding();
        }

        public string Encrypt(string unencrypted) {
            string Retme = "";
            try {
                Retme = Convert.ToBase64String(Encrypt(encoder.GetBytes(unencrypted)));
            }
            catch (Exception ex) {
                Retme = "ERROR " +ex.Message.ToString();
            }
            return Retme;
        }

        public string Decrypt(string encrypted) {
            string Retme = "";
            try {
                Retme = encoder.GetString(Decrypt(Convert.FromBase64String(encrypted)));
            }
            catch (Exception ex) {
                Retme = "ERROR NOT ENCRYPTED: " + ex.Message.ToString();
            }
            return Retme;
        }

        public string EncryptAndReturn(string unencrypted) {
            return (Encrypt(unencrypted));
        }
        
        public string DecryptAndReturn(string encrypted) {
            return Decrypt(encrypted);
        }

        public byte[] Encrypt(byte[] buffer) {
            return Transform(buffer, encryptor);
        }

        public byte[] Decrypt(byte[] buffer) {
            return Transform(buffer, decryptor);
        }

        protected byte[] Transform(byte[] buffer, ICryptoTransform transform) {
            MemoryStream stream = new MemoryStream();
            using (CryptoStream cs = new CryptoStream(stream, transform, CryptoStreamMode.Write)) {
                cs.Write(buffer, 0, buffer.Length);
            }
            return stream.ToArray();
        }

        internal static bool EncryptFile(string inputPath, string outputPath, string password) {
            var input = new FileStream(inputPath, FileMode.Open, FileAccess.Read);
            var output = new FileStream(outputPath, FileMode.OpenOrCreate, FileAccess.Write);
            var algorithm = new RijndaelManaged {KeySize = 256, BlockSize = 256};
            var key = new Rfc2898DeriveBytes(password, Encoding.ASCII.GetBytes(Salt));
 
            algorithm.Key = key.GetBytes(algorithm.KeySize/8);
            algorithm.IV = key.GetBytes(algorithm.BlockSize/8);
 
            using (var encryptedStream = new CryptoStream(output, algorithm.CreateEncryptor(), CryptoStreamMode.Write)) {
                CopyStream(input, encryptedStream);
            }

            return true;
        }
 
        internal static bool DecryptFile(string inputPath, string outputPath, string password)
        {
            var input = new FileStream(inputPath, FileMode.Open, FileAccess.Read);
            var output = new FileStream(outputPath, FileMode.OpenOrCreate, FileAccess.Write);
            var algorithm = new RijndaelManaged {KeySize = 256, BlockSize = 256};
            var key = new Rfc2898DeriveBytes(password, Encoding.ASCII.GetBytes(Salt));
 
            algorithm.Key = key.GetBytes(algorithm.KeySize/8);
            algorithm.IV = key.GetBytes(algorithm.BlockSize/8);
 
            try {
                using (var decryptedStream = new CryptoStream(output, algorithm.CreateDecryptor(), CryptoStreamMode.Write)) {
                    CopyStream(input, decryptedStream);
                }
            }
            catch (CryptographicException) {
                throw new InvalidDataException("Please supply a correct password");
            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }
            return true;
        }
 
        private static void CopyStream(Stream input, Stream output)
        {
            using (output)
            using (input)
            {
                byte[] buffer = new byte[SizeOfBuffer];
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    output.Write(buffer, 0, read);
                }
            }
        }
    }
}
