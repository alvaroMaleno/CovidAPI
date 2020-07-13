using Security_REST.Controllers;
using Security_REST.DAOs.Abstracts;
using Security_REST.Models.DataModels;
using Security_REST.Models.PathModels;
using Security_REST.Security.DataManager;
using Security_REST.Security.SecurityManager.Interfaces;
using Security_REST.Utils;

namespace Security_REST.Security.SecurityManager
{
    public class SecurityManager : IUser
    {
        private static SecurityManager _instance;
        private DAO _oDAO;
        private RSAManager _oRSAManager;
        private SolidDataManager _oSolidDataManager;
        private Paths _oPathPersistentFiles;
        private int _numberOfUsersAddedWithActualKey;
        private readonly string _USER_TABLE_NAME;

        private SecurityManager()
        {
            _oDAO = SecurityDAOPostgreImpl.GetInstance();
            _oRSAManager = RSAManager.GetInstance();
            _oSolidDataManager = SolidDataManager.GetInstance();
            _numberOfUsersAddedWithActualKey = 0;
            var file = Utils.UtilsStreamReaders.GetInstance().ReadStreamFile(this.GetFilePath());
            UtilsJSON.GetInstance().DeserializeFromString(out  _oPathPersistentFiles, file);
        }

        private string GetFilePath()
        {
            string selectPaths;
            string so = UtilsSO.GetInstance().GetSO();

            if(so.Contains("unix"))
                selectPaths = @"./Security/DataManager/EncryptedNames/names";
            else
                selectPaths = @".\Security\DataManager\EncryptedNames\names";

            return selectPaths;
        }

        public static SecurityManager GetInstance()
        {
            if(_instance is null)
                _instance = new SecurityManager();
            
            return _instance;
        }
        
        public void AddUser(User pUser)
        {
            _oDAO.InsertUser(pUser, _oSolidDataManager.DesencryptFile(_USER_TABLE_NAME));
            _numberOfUsersAddedWithActualKey++;
        }

        public bool ValidateUser(User pUser)
        {
            throw new System.NotImplementedException();
        }

        private void GenerateKeyPair(out KeyPair oKeyPair)
        {
            _oRSAManager.GetPublicKeyAndPrivateKeyForNewUsers(out oKeyPair);
        }

        private void SaveKeyPairOnDB(KeyPair pKeyPair)
        {
            _oDAO.InsertKeyPair(pKeyPair, _USER_TABLE_NAME);
        }
        
        private void ChangeDBEncriptation()
        {
            throw new System.NotImplementedException();
        }

        private void CreateSecurityTables()
        {
            throw new System.NotImplementedException();
        }

        private void ChangePublicAndPrivateKey()
        {
            //TODO Persitent Files Without Encryptation
            _oSolidDataManager.ChangePersistentFileEncryptation(_oPathPersistentFiles, true);
        }

    }
}