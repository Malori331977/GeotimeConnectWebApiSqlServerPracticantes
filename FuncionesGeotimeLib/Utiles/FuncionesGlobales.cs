using System.Security.Cryptography;
using System.Text;
namespace GeotimeFuncionesLib.Utiles
{
    public static class FuncionesGlobales
    {
        private static string enc_pss = "daptesa_erx_jodabe_slifer_abcdef@ghijkl123456789_";
        /// <summary>
        /// Global_encrypt: encritpa cadenas con el metodo SHA256
        /// </summary>
        /// <param name="texto"></param>
        /// <returns>Cadena encriptada en SHA256</returns>
        public static string Global_encrypt(string texto)
        {
            SHA1 sha1 = SHA1.Create();
            byte[] textOriginal = Encoding.Default.GetBytes(texto);
            byte[] hash = sha1.ComputeHash(textOriginal);
            StringBuilder cadena = new StringBuilder();
            foreach (byte i in hash)
            {
                cadena.AppendFormat("{0:x2}", i);
            }
            return cadena.ToString();
        }

        private static RijndaelManaged GetRijndaelManaged(String secretKey)
        {
            var keyBytes = new byte[32];
            var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);
            Array.Copy(secretKeyBytes, keyBytes, Math.Min(keyBytes.Length, secretKeyBytes.Length));

            var ivBytes = new byte[16];
            var secretIvBytes = Encoding.UTF8.GetBytes(secretKey);
            Array.Copy(secretIvBytes, ivBytes, Math.Min(ivBytes.Length, secretIvBytes.Length));

            return new RijndaelManaged
            {
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7,
                KeySize = 256,
                BlockSize = 128,
                Key = keyBytes,
                IV = ivBytes
            };
        }
        private static byte[] Encrypt(byte[] plainBytes, RijndaelManaged rijndaelManaged)
        {
            return rijndaelManaged.CreateEncryptor()
                .TransformFinalBlock(plainBytes, 0, plainBytes.Length);
        }

        private static byte[] Decrypt(byte[] encryptedData, RijndaelManaged rijndaelManaged)
        {
            return rijndaelManaged.CreateDecryptor()
                .TransformFinalBlock(encryptedData, 0, encryptedData.Length);
        }

        /// <summary>
        /// Encrypts plaintext using AES 128bit key and a Chain Block Cipher and returns a base64 encoded string
        /// </summary>
        /// <param name="plainText">Plain text to encrypt</param>
        /// <param name="key">Secret key</param>
        /// <returns>Base64 encoded string</returns>
        public static String Encrypt(String plainText)
        {
            var plainBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(Encrypt(plainBytes, GetRijndaelManaged(enc_pss)));
        }

        /// <summary>
        /// Decrypts a base64 encoded string using the given key (AES 128bit key and a Chain Block Cipher)
        /// </summary>
        /// <param name="encryptedText">Base64 Encoded String</param>
        /// <param name="key">Secret Key</param>
        /// <returns>Decrypted String</returns>
        public static String Decrypt(String encryptedText)
        {
            var encryptedBytes = Convert.FromBase64String(encryptedText);
            return Encoding.UTF8.GetString(Decrypt(encryptedBytes, GetRijndaelManaged(enc_pss)));
        }

        public static string HexadecimalToString(string Data)
        {
            string Data1 = "";
            string sData = "";
            int tamper = 0;
            if (Data.Split('X').Length == 2)
            {
                while (Data.Split('X').GetValue(0).ToString().Length > 0)
                {
                    try
                    {
                        Data1 = System.Convert.ToChar(System.Convert.ToUInt32(Data.Substring(0, 2), 16)).ToString();
                        tamper = tamper + Convert.ToInt32(Convert.ToChar(Data1));
                        sData = sData + Data1;
                        Data = Data.Substring(2, Data.Length - 2);
                    }
                    catch (Exception a)
                    {
                        return a.Message;
                    }
                }
                if (String.Format("{0:X}", Convert.ToUInt32(Convert.ToChar(tamper))) == Data.Substring(1).ToString())
                {
                    return sData;
                }
                else
                {
                    return "uj0p3JS3GCnpWGePH9a1BhqMXJub3q4BfTLB0aDqyvY=(/)HDVGpjDaouGU3+S6XFqUEg==)/(HDVGpjDaouGU3+S6XFqUEg==";
                }
            }
            else
            {
                return "uj0p3JS3GCnpWGePH9a1BhqMXJub3q4BfTLB0aDqyvY=(/)HDVGpjDaouGU3+S6XFqUEg==)/(HDVGpjDaouGU3+S6XFqUEg==";
            }
        }
        public static string StringToHexadecimal(string Data)
        {
            string sValue;
            string sHex = "";
            int tamper = 0;
            foreach (char c in Data.ToCharArray())
            {
                sValue = String.Format("{0:X}", Convert.ToUInt32(c));
                tamper = tamper + Convert.ToInt32(c);
                sHex = sHex + sValue;
            }
            sHex = sHex + "X" + String.Format("{0:X}", Convert.ToUInt32(Convert.ToChar(tamper)));
            return sHex;
        }
    }
}
