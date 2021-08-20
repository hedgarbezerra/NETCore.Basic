﻿using Microsoft.Extensions.Configuration;
using NETCore.Basic.Util.Configuration;
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
    public interface IEncryption
    {
        string Encrypt(string plainText);
        //byte[] Encrypt(byte[] plainTextBytes);
        string Decrypt(string cipherText);
        //byte[] Decrypt(byte[] cipherTextBytesWithSaltAndIv, out int decryptedByteCount);
    }
    /// <summary>
    /// Classe estática voltada a encriptação
    /// </summary>
    public class Encryption : IEncryption
    {
        public Encryption(IAPIConfigurations configuration)
        {
            _key = Convert.FromBase64String(configuration.CryptoKey);
        }
        private byte[] _key;

        public string Decrypt(string value)
        {
            var ivAndCipherText = Convert.FromBase64String(value);
            using var aes = Aes.Create();
            var ivLength = aes.BlockSize / 8;
            aes.IV = ivAndCipherText.Take(ivLength).ToArray();
            aes.Key = _key;
            using var cipher = aes.CreateDecryptor();
            var cipherText = ivAndCipherText.Skip(ivLength).ToArray();
            var text = cipher.TransformFinalBlock(cipherText, 0, cipherText.Length);
            return Encoding.UTF8.GetString(text);
        }

        public string Encrypt(string value)
        {
            using var aes = Aes.Create();
            aes.Key = _key;
            aes.GenerateIV();
            using var cipher = aes.CreateEncryptor();
            var text = Encoding.UTF8.GetBytes(value);
            var cipherText = cipher.TransformFinalBlock(text, 0, text.Length);
            return Convert.ToBase64String(aes.IV.Concat(cipherText).ToArray());
        }
    }

}
