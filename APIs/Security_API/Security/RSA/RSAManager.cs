using System;
using System.Security.Cryptography;
using System.Text;
using Security_REST.DAOs;
using Security_REST.DAOs.Abstracts;
using Security_REST.Models.DataModels;

namespace Security_REST.Security
{
    public class RSAManager
    {
        private static RSAManager _instance;
        private DAO _oDAO;

        public static RSAManager GetInstance()
        {
            if(_instance is null)
                _instance = new RSAManager();
            
            return _instance;
        }

        private RSAManager()
        {
            _oDAO = SecurityDAOPostgreImpl.GetInstance();
        }

        public void CreateKeyPair(out KeyPair pKeyPair)
        {
            RSACryptoServiceProvider oRSACryptoServiceProvider;
            this.CreateRSACryptoServiceProvider(out oRSACryptoServiceProvider);
            
            var publicKey = oRSACryptoServiceProvider.ToXmlString(false);
            var privateKey = oRSACryptoServiceProvider.ToXmlString(true);
            pKeyPair = new KeyPair(publicKey, privateKey);
        }

        private void CreateRSACryptoServiceProvider(out RSACryptoServiceProvider pRSACryptoServiceProvider)
        {
            pRSACryptoServiceProvider = new RSACryptoServiceProvider(3072);
        }

        public string DecryptWithPrivateKeyString(string pToDecrypt, KeyPair pKeyPair)
        {
            RSACryptoServiceProvider oRSACryptoServiceProvider;
            this.CreateRSACryptoServiceProvider(out oRSACryptoServiceProvider);

            oRSACryptoServiceProvider.FromXmlString(pKeyPair.private_string);
            var ToDecrypt = Convert.FromBase64String(pToDecrypt);
            return 
                Encoding.ASCII.GetString(
                    oRSACryptoServiceProvider.Decrypt(ToDecrypt, false));
        }

        public string EncryptWithPublicKeyString(string pToEncrypt, string pPublicKey)
        {
            RSACryptoServiceProvider oRSACryptoServiceProvider;
            this.CreateRSACryptoServiceProvider(out oRSACryptoServiceProvider);
            oRSACryptoServiceProvider.FromXmlString(pPublicKey);
            return 
                Convert.ToBase64String(
                    oRSACryptoServiceProvider.Encrypt(
                        Encoding.ASCII.GetBytes(pToEncrypt), false));
        }

    }
}