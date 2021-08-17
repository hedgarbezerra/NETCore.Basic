using NETCore.Basic.Util.Helper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace NETCore.Basic.Util.Crypto
{
    /// <summary>
    /// Classe estática voltada a encriptação
    /// </summary>
    public static class Encryption
    {
        private const int _keySize = 256;
        private const int _derivationIterations = 1000;
        private static ConfigurationKeys _configurationKeys = new ConfigurationKeys();
        /// <summary>
        /// Utiliza o método de sobrecarga com mesmo nome para encriptar um texto a partir de uma string
        /// </summary>
        /// <param name="plainText">Qualquer texto/string que deva ser encriptado</param>
        /// <returns>string com texto encriptografado</returns>
        public static string Encrypt(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            var plainTextArrEncrypted = Encrypt(plainTextBytes);

            return Convert.ToBase64String(plainTextArrEncrypted);
        }

        /// <summary>
        /// Faz a encriptação de quaisquer arrays de bytes utilizando uma chave definida no webconfig/appconfig
        /// </summary>
        /// <param name="plainTextBytes"></param>
        /// <returns>byte array do texto encriptografado</returns>
        public static byte[] Encrypt(byte[] plainTextBytes)
        {
            var saltStringBytes = Generate256BitsOfRandomEntropy();
            var ivStringBytes = Generate256BitsOfRandomEntropy();

            string encryptionKey = _configurationKeys.Encryption;

            using (var password = new Rfc2898DeriveBytes(encryptionKey, saltStringBytes, _derivationIterations))
            {
                var keyBytes = password.GetBytes(_keySize / 8);
                using (var symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.BlockSize = 256;
                    symmetricKey.Mode = CipherMode.CBC;
                    symmetricKey.Padding = PaddingMode.PKCS7;
                    using (var encryptor = symmetricKey.CreateEncryptor(keyBytes, ivStringBytes))
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                            {
                                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                                cryptoStream.FlushFinalBlock();

                                var cipherTextBytes = saltStringBytes;
                                cipherTextBytes = cipherTextBytes.Concat(ivStringBytes).ToArray();
                                cipherTextBytes = cipherTextBytes.Concat(memoryStream.ToArray()).ToArray();
                                memoryStream.Close();
                                cryptoStream.Close();

                                return cipherTextBytes;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Utilizando um método de sobrecarga faz a descriptográfia do texto encriptado
        /// </summary>
        /// <param name="cipherText">O texto encriptado pelo sistema</param>
        /// <returns>string com texto descriptografado</returns>
        public static string Decrypt(string cipherText)
        {
            var cipherTextBytesWithSaltAndIv = Convert.FromBase64String(cipherText);

            var decryptedCipherTextBytes = Decrypt(cipherTextBytesWithSaltAndIv, out int decryptedBytesCount);

            return Encoding.UTF8.GetString(decryptedCipherTextBytes, 0, decryptedBytesCount);

        }

        /// <summary>
        /// Faz a descriptografia do byte array recebido
        /// </summary>
        /// <param name="cipherTextBytesWithSaltAndIv">recebe o byte array criptografado</param>
        /// <param name="decryptedByteCount">Parâmetro de saída decryptedByteCount com quantidade de bytes da operação</param>
        /// <returns>byte array do texto criptografado</returns>
        public static byte[] Decrypt(byte[] cipherTextBytesWithSaltAndIv, out int decryptedByteCount)
        {
            // Get the saltbytes by extracting the first 32 bytes from the supplied cipherText bytes.
            var saltStringBytes = cipherTextBytesWithSaltAndIv.Take(_keySize / 8).ToArray();
            // Get the IV bytes by extracting the next 32 bytes from the supplied cipherText bytes.
            var ivStringBytes = cipherTextBytesWithSaltAndIv.Skip(_keySize / 8).Take(_keySize / 8).ToArray();
            // Get the actual cipher text bytes by removing the first 64 bytes from the cipherText string.
            var cipherTextBytes = cipherTextBytesWithSaltAndIv.Skip((_keySize / 8) * 2).Take(cipherTextBytesWithSaltAndIv.Length - ((_keySize / 8) * 2)).ToArray();

            string encryptionKey = ConfigurationManager.AppSettings["encryptionKey"];

            using (var password = new Rfc2898DeriveBytes(encryptionKey, saltStringBytes, _derivationIterations))
            {
                var keyBytes = password.GetBytes(_keySize / 8);
                using (var symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.BlockSize = 256;
                    symmetricKey.Mode = CipherMode.CBC;
                    symmetricKey.Padding = PaddingMode.PKCS7;

                    using (var decryptor = symmetricKey.CreateDecryptor(keyBytes, ivStringBytes))
                    {
                        using (var memoryStream = new MemoryStream(cipherTextBytes))
                        {
                            using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                            {
                                var plainTextBytes = new byte[cipherTextBytes.Length];
                                decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);

                                memoryStream.Close();
                                cryptoStream.Close();

                                return plainTextBytes;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Método privado prove um byte array aleatório para criptografia
        /// </summary>
        /// <returns>retorna um byte array de 256 bits aleatório</returns>
        private static byte[] Generate256BitsOfRandomEntropy()
        {
            var randomBytes = new byte[32]; // 32 Bytes will give us 256 bits.
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                // Fill the array with cryptographically secure random bytes.
                rngCsp.GetBytes(randomBytes);
            }
            return randomBytes;
        }
    }

}
