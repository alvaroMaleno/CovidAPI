using CoVid.DAOs.Abstracts;
using Security_REST.Security.DataManager;
using Security_REST.Security.SecurityManager.Interfaces;

namespace Security_REST.Security.SecurityManager
{
    public class SecurityManager : IUser
    {
        private static SecurityManager _instance;
        private DAO _oDAO;
        private RSAManager _oRSAManager;
        private SolidDataManager _oSolidDataManager;
        private int _numberOfUsersAddedWithActualKey;

        private SecurityManager()
        {

        }

        public static SecurityManager GetInstance()
        {
            if(_instance is null)
                _instance = new SecurityManager();
            
            return _instance;
        }
        
        public void AddUser(object pUser)
        {
            throw new System.NotImplementedException();
        }

        public bool ValidateUser(object pUser)
        {
            throw new System.NotImplementedException();
        }

        private object GenerateKeyPair()
        {
            throw new System.NotImplementedException();
        }

        private void SaveKeyPairOnDB(object pKeyPair)
        {
            throw new System.NotImplementedException();
        }
        
        private void ChangeDBEncriptation()
        {
            throw new System.NotImplementedException();
        }

        private void CreateSecurityTables()
        {
            throw new System.NotImplementedException();
        }

        private void ChangePublicAndPrivateKeyToNewUsers()
        {
            throw new System.NotImplementedException();
        }

        public object GetActualPublicKeyToNewUsers()
        {
            throw new System.NotImplementedException();
        }

    }
}