using System.Security.Cryptography;

namespace Security_REST.Security
{
    public class RSAManager
    {
        private RSACryptoServiceProvider _oRSASCryptoServiceProvider;
        private static RSAManager _instance;

        public static RSAManager GetInstance()
        {
            if(_instance is null)
                _instance = new RSAManager();
            
            return _instance;
        }

        private RSAManager(){}

        public object CreateKeyPair()
        {
            return null;
        }

        public object GetPublicKey()
        {
            return null;
        }

        public object GetPrivateKey()
        {
            return null;
        }

        public object SubstituteKeyPair()
        {
            return null;
        }

        public object GetPublicKeyToNewUsers()
        {
            return null;
        }

        public object GetPrivateKeyToNewUsers()
        {
            return null;
        }

        public void ChangePublicAndPrivateKeyToNewUsers()
        {

        }

    }
}