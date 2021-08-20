using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace NETCore.Basic.Util.Crypto
{
    public interface IHashing
    {
        string ComputeHash(string plainText, byte[] saltBytes = null);
        bool VerifyHash(string plainText, string hashValue);
    }
    /// <summary>
    /// Classe estática com métodos de geração e comparação de hashes que utilizam salt bytes
    /// </summary>
    public class Hashing : IHashing
    {
        private readonly RNGCryptoServiceProvider _rngProvider = new RNGCryptoServiceProvider();
        /// <summary>
        /// provedor do serviço de criptografia e hashing
        /// </summary>
        /// <summary>
        /// Método que retorna uma string hasheada a partir de criptografia de 256 bits com salt, sendo impossível a descriptografia
        /// </summary>
        /// <param name="plainText">string com texto limpo</param>
        /// <param name="saltBytes">salt bytes opcionais, caso não seja preenchido será gerado</param>
        /// <returns>string a partir do parâmetro plainText hasheada com salt inserido ou gerado</returns>
        public string ComputeHash(string plainText, byte[] saltBytes = null)
        {
            if (saltBytes == null)
            {
                int minSaltSize = 4;
                int maxSaltSize = 8;

                Random random = new Random();
                int saltSize = random.Next(minSaltSize, maxSaltSize);

                saltBytes = new byte[saltSize];


                _rngProvider.GetNonZeroBytes(saltBytes);
            }

            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            byte[] plainTextWithSaltBytes =
                    new byte[plainTextBytes.Length + saltBytes.Length];

            for (int i = 0; i < plainTextBytes.Length; i++)
                plainTextWithSaltBytes[i] = plainTextBytes[i];

            for (int i = 0; i < saltBytes.Length; i++)
                plainTextWithSaltBytes[plainTextBytes.Length + i] = saltBytes[i];

            HashAlgorithm hash = new SHA256Managed();


            byte[] hashBytes = hash.ComputeHash(plainTextWithSaltBytes);

            byte[] hashWithSaltBytes = new byte[hashBytes.Length +
                                                saltBytes.Length];

            for (int i = 0; i < hashBytes.Length; i++)
                hashWithSaltBytes[i] = hashBytes[i];

            for (int i = 0; i < saltBytes.Length; i++)
                hashWithSaltBytes[hashBytes.Length + i] = saltBytes[i];

            string hashValue = Convert.ToBase64String(hashWithSaltBytes);

            return hashValue;
        }

        /// <summary>
        /// Método verifica se o texto limpo gera o mesmo byte array que o texto com hash retornando um boolean
        /// </summary>
        /// <param name="plainText">texto limpo sem qualquer criptografia</param>
        /// <param name="hashValue">texto gerado pelo método de hash do sistema</param>
        /// <returns>retorna um boolean com a validação dos byte arrays gerados</returns>
        public bool VerifyHash(string plainText, string hashValue)
        {
            byte[] hashWithSaltBytes = Convert.FromBase64String(hashValue);

            int hashSizeInBits = 256;
            int hashSizeInBytes;

            hashSizeInBytes = hashSizeInBits / 8;

            if (hashWithSaltBytes.Length < hashSizeInBytes)
                return false;

            byte[] saltBytes = new byte[hashWithSaltBytes.Length -
                                        hashSizeInBytes];

            for (int i = 0; i < saltBytes.Length; i++)
                saltBytes[i] = hashWithSaltBytes[hashSizeInBytes + i];

            string expectedHashString = ComputeHash(plainText, saltBytes);

            return hashValue == expectedHashString;
        }

        /// <summary>
        /// Método retorna uma string aleatória podendo ser utilizadas para senhas, ou quaisquer outras coisas
        /// </summary>
        /// <param name="tamanhoString">int com o tamanho desejado da string</param>
        /// <returns>string aleatória utilizando letras, caracteres especiais e números</returns>
        public string RandomString(int tamanhoString = 8)
        {
            char[] chars = @"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890@#$%&".ToCharArray();

            byte[] data = new byte[4 * tamanhoString];

            _rngProvider.GetBytes(data);

            StringBuilder result = new StringBuilder(tamanhoString);

            for (int i = 0; i < tamanhoString; i++)
            {
                var rnd = BitConverter.ToUInt32(data, i * 4);
                var idx = rnd % chars.Length;

                result.Append(chars[idx]);
            }

            return result.ToString();
        }
    }

}
