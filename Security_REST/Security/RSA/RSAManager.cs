using System;
using System.Security.Cryptography;
using System.Text;
using Security_REST.Models.DataModels;

namespace Security_REST.Security
{
    public class RSAManager
    {
        //TODO To change encryptation must be a method to delete old keys after desencrypt each file.
        private static RSAManager _instance;
        private readonly string _KEY_CONTAINER_NAME = "GenericContainer";

        public static RSAManager GetInstance()
        {
            if(_instance is null)
                _instance = new RSAManager();
            
            return _instance;
        }

        private RSAManager()
        {
        }

        private void CreateKeyPair(out KeyPair pKeyPair, bool pSaveKeys)
        {
            RSACryptoServiceProvider oRSACryptoServiceProvider;
            
            this.CreateRSACryptoServiceProvider(out oRSACryptoServiceProvider, pSaveKeys);
            
            var publicKey = oRSACryptoServiceProvider.ToXmlString(false);
            var privateKey = oRSACryptoServiceProvider.ToXmlString(true);
            pKeyPair = new KeyPair(publicKey, privateKey);
        }

        private void CreateRSACryptoServiceProvider(out RSACryptoServiceProvider pRSACryptoServiceProvider, bool pSaveKeys)
        {
            if(pSaveKeys)
            {
                var oCspParameters = new CspParameters
                {
                    KeyContainerName = _KEY_CONTAINER_NAME
                };
                pRSACryptoServiceProvider = new RSACryptoServiceProvider(oCspParameters);
            }
            else
            {
                pRSACryptoServiceProvider = new RSACryptoServiceProvider();
            }
        }

        private string GetPublicKey()
        {
            RSACryptoServiceProvider oRSACryptoServiceProvider;
            
            this.CreateRSACryptoServiceProvider(out oRSACryptoServiceProvider, true);
            return oRSACryptoServiceProvider.ToXmlString(false);
        }

        private string GetPrivateKey()
        {
            RSACryptoServiceProvider oRSACryptoServiceProvider;
            
            this.CreateRSACryptoServiceProvider(out oRSACryptoServiceProvider, true);
            return oRSACryptoServiceProvider.ToXmlString(true);
        }

        public void SubstituteKeyPair()
        {
            // KeyPair oKeyPair;
            // this.CreateKeyPair(out oKeyPair, true);
            throw new NotImplementedException();
        }

        public void GetPublicKeyAndPrivateKeyForNewUsers(out KeyPair pKeyPair)
        {
            this.CreateKeyPair(out pKeyPair, false);
        }

        public string DesencryptWithOwnKeyString(string pToDesencrypt)
        {
            RSACryptoServiceProvider oRSACryptoServiceProvider;
            
            this.CreateRSACryptoServiceProvider(out oRSACryptoServiceProvider, true);
            
            return Encoding.ASCII.GetString(oRSACryptoServiceProvider.Decrypt(Encoding.ASCII.GetBytes(pToDesencrypt), false));
        }

        public string EncryptWithOwnKeyString(string pToEncrypt)
        {
            RSACryptoServiceProvider oRSACryptoServiceProvider;
            
            this.CreateRSACryptoServiceProvider(out oRSACryptoServiceProvider, true);
            
            return Encoding.ASCII.GetString(oRSACryptoServiceProvider.Encrypt(Encoding.ASCII.GetBytes(pToEncrypt), false));
        }

        public string DesencryptWithPrivateKeyString(string pToDesencrypt, string pPrivateKey)
        {
            RSACryptoServiceProvider oRSACryptoServiceProvider;
            this.CreateRSACryptoServiceProvider(out oRSACryptoServiceProvider, false);
            oRSACryptoServiceProvider.FromXmlString(pPrivateKey);
            return 
                Encoding.ASCII.GetString(
                    oRSACryptoServiceProvider.Decrypt(
                        Encoding.ASCII.GetBytes(pToDesencrypt), false));
        }

        public string EncryptWithPublicKeyString(string pToEncrypt, string pPublicKey)
        {
            RSACryptoServiceProvider oRSACryptoServiceProvider;
            this.CreateRSACryptoServiceProvider(out oRSACryptoServiceProvider, false);
            oRSACryptoServiceProvider.FromXmlString(pPublicKey);
            return Encoding.ASCII.GetString(oRSACryptoServiceProvider.Encrypt(Encoding.ASCII.GetBytes(pToEncrypt), false));
        }

    }
}