namespace Security_REST.Security.DataManager
{
    public class SolidDataManager
    {
        private static SolidDataManager _instance;
        private RSAManager _oRSAManager;

        private SolidDataManager(){}

        public static SolidDataManager GetInstance()
        {
            if(_instance is null)
                _instance = new SolidDataManager();

            return _instance;
        }

        private void EncryptEachPersistentFile(){}

        public string DesencryptFile(string pPath)
        {
            return string.Empty;
        }
        
    }
}