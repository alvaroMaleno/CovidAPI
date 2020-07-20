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
            pRSACryptoServiceProvider = new RSACryptoServiceProvider();
        }

        public string DesencryptWithPrivateKeyString(string pToDesencrypt, string pPrivateKey)
        {
            RSACryptoServiceProvider oRSACryptoServiceProvider;
            this.CreateRSACryptoServiceProvider(out oRSACryptoServiceProvider);
            oRSACryptoServiceProvider.FromXmlString(pPrivateKey);
            return 
                Encoding.ASCII.GetString(
                    oRSACryptoServiceProvider.Decrypt(
                        Encoding.ASCII.GetBytes(pToDesencrypt), false));
        }

        public string EncryptWithPublicKeyString(string pToEncrypt, string pPublicKey)
        {
            RSACryptoServiceProvider oRSACryptoServiceProvider;
            this.CreateRSACryptoServiceProvider(out oRSACryptoServiceProvider);
            oRSACryptoServiceProvider.FromXmlString(pPublicKey);
            return Encoding.ASCII.GetString(oRSACryptoServiceProvider.Encrypt(Encoding.ASCII.GetBytes(pToEncrypt), false));
        }

    }
}