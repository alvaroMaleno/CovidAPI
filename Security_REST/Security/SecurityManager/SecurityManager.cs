using System;
using Security_REST.DAOs;
using Security_REST.DAOs.Abstracts;
using Security_REST.Models.DataModels;
using Security_REST.Models.PathModels;
using Security_REST.Models.QueryModels;
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
        private int _numberOfUsersAddedWithActualKey;
        private readonly string _USER_TABLE_NAME;

        private SecurityManager()
        {
            _oDAO = SecurityDAOPostgreImpl.GetInstance();
            _oRSAManager = RSAManager.GetInstance();
            _oSolidDataManager = SolidDataManager.GetInstance();
            _numberOfUsersAddedWithActualKey = 0;
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

        public void CreateSecurityTables()
        {
            string[] oLinesArray;
            this.GetLinesArrayFromAFile(out oLinesArray);

            if(!oLinesArray[0].Contains("false"))
                return;

            this.CreateTableByFirstTime(oLinesArray);
            var fileToPersist = this.GetFileToPersistFromLinesArray(oLinesArray);
            UtilsStreamWritters.GetInstance().WritteStringToFile(fileToPersist, this.GetFilePath());
            _oSolidDataManager.EncryptFile(this.GetFilePath());
        }


        private string GetFileToPersistFromLinesArray(string[] oLinesArray)
        {
            string toReturn = string.Empty;
            
            foreach (var line in oLinesArray)
                if(string.IsNullOrEmpty(toReturn))
                    toReturn = line;
                else
                    toReturn = string.Concat(toReturn, Environment.NewLine, line);
            
            return toReturn;
        }

        private void CreateTableByFirstTime(string[] oLinesArray)
        {
            string[] oTableAndColumnsNamesArray;
            
            for (int i = 1; i < oLinesArray.Length; i++)
            {
                oTableAndColumnsNamesArray = oLinesArray[i].Split(",");
                Query oQuery;
                _oDAO.GetCreateQuery(out oQuery);
                oQuery.query = oQuery.query.Replace(UtilsConstants._TABLE_NAME, oTableAndColumnsNamesArray[0]);
                oQuery.query = oQuery.query.Replace(
                                string.Concat(UtilsConstants._COLUMN_NAME, UtilsConstants._ONE_STRING),
                                oTableAndColumnsNamesArray[1]);
                oQuery.query = oQuery.query.Replace(
                                    string.Concat(UtilsConstants._COLUMN_NAME, UtilsConstants._TWO_STRING),
                                    oTableAndColumnsNamesArray[2]);
                _oDAO.CreateTable(oQuery);
            }
            oLinesArray[0] = "true";
        }

        private void GetLinesArrayFromAFile(out string[] oLinesArray)
        {
            var file = UtilsStreamReaders.GetInstance().
                                        ReadStreamFile(this.GetFilePath());
            oLinesArray = file.Split(Environment.NewLine);
        }

        private void ChangePublicAndPrivateKey()
        {
            //TODO Persitent Files Without Encryptation
            // _oSolidDataManager.ChangePersistentFileEncryptation(_oPathPersistentFiles, true);
        }

    }
}