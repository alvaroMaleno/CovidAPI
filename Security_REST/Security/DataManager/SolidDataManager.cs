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

        private void EncryptEachPersistentFile(Paths pPaths)
        {
            string dataToWritte;
            foreach (var path in pPaths.oPaths)
            {
                dataToWritte = _oRSAManager.
                                EncryptWithOwnKeyString(
                                    UtilsStreamReaders.GetInstance().ReadStreamFile(path));
                
                UtilsStreamWritters.GetInstance().WritteStringToFile(dataToWritte, path);
            }
        }

        private void DesencryptEachPersistentFile(Paths pPaths)
        {
            string dataToWritte;
            foreach (var path in pPaths.oPaths)
            {
                dataToWritte = _oRSAManager.
                                DesencryptWithOwnKeyString(
                                    UtilsStreamReaders.GetInstance().ReadStreamFile(path));
                
                UtilsStreamWritters.GetInstance().WritteStringToFile(dataToWritte, path);
            }
        }

        public void ChangePersistentFileEncryptation(Paths pPaths, bool pIsEncrypted)
        {
            if(pIsEncrypted)
                this.DesencryptEachPersistentFile(pPaths);
                
            _oRSAManager.SubstituteKeyPair();
            this.EncryptEachPersistentFile(pPaths);
        }

        public string DesencryptFile(string pPath)
        {
            return _oRSAManager.
                    DesencryptWithOwnKeyString(
                        UtilsStreamReaders.GetInstance().ReadStreamFile(pPath));
        }

        public string EncryptFile(string pPath)
        {
            return _oRSAManager.
                    EncryptWithOwnKeyString(
                        UtilsStreamReaders.GetInstance().ReadStreamFile(pPath));
        }
        
    }
}