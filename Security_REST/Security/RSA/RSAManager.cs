using System;
using System.Security.Cryptography;

namespace Security_REST.Security
{
    public class RSAManager
    {
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

        private void CreateKeyPair(out Tuple<string, string> pPublicAndPrivateKey, bool pSaveKeys)
        {
            RSACryptoServiceProvider oRSACryptoServiceProvider;
            
            this.CreateRSACryptoServiceProvider(out oRSACryptoServiceProvider, pSaveKeys);
            
            var publicKey = oRSACryptoServiceProvider.ToXmlString(false);
            var privateKey = oRSACryptoServiceProvider.ToXmlString(true);
            pPublicAndPrivateKey = new Tuple<string, string>(publicKey, privateKey);
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

        public string GetPublicKey()
        {
            RSACryptoServiceProvider oRSACryptoServiceProvider;
            
            this.CreateRSACryptoServiceProvider(out oRSACryptoServiceProvider, true);
            return oRSACryptoServiceProvider.ToXmlString(false);
        }

        public string GetPrivateKey()
        {
            RSACryptoServiceProvider oRSACryptoServiceProvider;
            
            this.CreateRSACryptoServiceProvider(out oRSACryptoServiceProvider, true);
            return oRSACryptoServiceProvider.ToXmlString(true);
        }

        public void SubstituteKeyPair()
        {
            Tuple<string, string> oPublicAndPrivateKey;
            this.CreateKeyPair(out oPublicAndPrivateKey, true);
        }

        public void GetPublicKeyAndPrivateKeyToNewUsers(out Tuple<string, string> pPublicAndPrivateKey)
        {
            this.CreateKeyPair(out pPublicAndPrivateKey, false);
        }

    }
}