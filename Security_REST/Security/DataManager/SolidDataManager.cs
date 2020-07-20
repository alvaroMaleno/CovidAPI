using Security_REST.Models.DataModels;
using Security_REST.Models.PathModels;
using Security_REST.Utils;

namespace Security_REST.Security.DataManager
{
    public class SolidDataManager
    {
        private static SolidDataManager _instance;
        private RSAManager _oRSAManager;

        private SolidDataManager()
        {
            _oRSAManager = RSAManager.GetInstance();
        }

        public static SolidDataManager GetInstance()
        {
            if(_instance is null)
                _instance = new SolidDataManager();

            return _instance;
        }

        private void EncryptEachPersistentFile(KeyPair pKeyPair, Paths pPaths)
        {
            string dataToWritte;
            foreach (var path in pPaths.oPaths)
            {
                dataToWritte = _oRSAManager.
                                EncryptWithPublicKeyString(
                                    UtilsStreamReaders.GetInstance().ReadStreamFile(path),
                                    pKeyPair.public_string);
                
                UtilsStreamWritters.GetInstance().WritteStringToFile(dataToWritte, path);
            }
        }

        private void DesencryptEachPersistentFile(KeyPair pKeyPair, Paths pPaths)
        {
            string dataToWritte;
            foreach (var path in pPaths.oPaths)
            {
                dataToWritte = _oRSAManager.
                                DesencryptWithPrivateKeyString(
                                    UtilsStreamReaders.GetInstance().ReadStreamFile(path),
                                    pKeyPair.private_string);
                
                UtilsStreamWritters.GetInstance().WritteStringToFile(dataToWritte, path);
            }
        }

        public string DesencryptFile(KeyPair pKeyPair, string pPath)
        {
            return _oRSAManager.
                    DesencryptWithPrivateKeyString(
                        UtilsStreamReaders.GetInstance().ReadStreamFile(pPath),
                        pKeyPair.private_string);
        }

        public string EncryptFile(KeyPair pKeyPair, string pPath)
        {
            return _oRSAManager.
                    EncryptWithPublicKeyString(
                        UtilsStreamReaders.GetInstance().ReadStreamFile(pPath),
                        pKeyPair.public_string);
        }
        
    }
}