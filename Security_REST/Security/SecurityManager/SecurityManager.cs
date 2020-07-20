using System;
using Security_REST.DAOs;
using Security_REST.DAOs.Abstracts;
using Security_REST.Models.DataModels;
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
            _numberOfUsersAddedWithActualKey = UtilsConstants._ZERO;
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
            //TODO
            // _oDAO.InsertUser(pUser, _oSolidDataManager.DesencryptFile(_USER_TABLE_NAME));
            // _numberOfUsersAddedWithActualKey++;
        }

        public bool ValidateUser(User pUser)
        {
            throw new System.NotImplementedException();
        }

        private void GenerateKeyPair(out KeyPair oKeyPair)
        {
            _oRSAManager.CreateKeyPair(out oKeyPair);
        }

        private void SaveUserKeyPairOnDB(KeyPair pKeyPair)
        {
            string[] oLinesArray;
            this.GetLinesArrayFromAFile(out oLinesArray);
            _oDAO.InsertKeyPair(
                pKeyPair, 
                oLinesArray[UtilsConstants._ONE].Split(UtilsConstants._COME));
        }
        
        private void ChangeDBEncriptation()
        {
            throw new System.NotImplementedException();
        }

        public void CreateSecurityTables()
        {
            string[] oLinesArray;
            this.GetLinesArrayFromAFile(out oLinesArray);

            if(oLinesArray[UtilsConstants._ZERO].Contains("true"))
                return;

            this.CreateTableByFirstTime(oLinesArray);
            var fileToPersist = this.GetFileToPersistFromLinesArray(oLinesArray);
            UtilsStreamWritters.GetInstance().WritteStringToFile(fileToPersist, this.GetFilePath());
            this.CreateFirstPublicKeyPair(oLinesArray);
        }

        private void CreateFirstPublicKeyPair(string[] oLinesArray)
        {
            KeyPair oKeyPair;
            _oRSAManager.CreateKeyPair(out oKeyPair);
            _oDAO.InsertKeyPair(
                oKeyPair, 
                oLinesArray[UtilsConstants._THREE].Split(UtilsConstants._COME));
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
                oTableAndColumnsNamesArray = oLinesArray[i].Split(UtilsConstants._COME);
                Query oQuery;
                _oDAO.GetCreateQuery(out oQuery);
                oQuery.query = oQuery.query.Replace(
                                UtilsConstants._TABLE_NAME, oTableAndColumnsNamesArray[UtilsConstants._ZERO]);
                oQuery.query = oQuery.query.Replace(
                                string.Concat(UtilsConstants._COLUMN_NAME, UtilsConstants._ONE_STRING),
                                oTableAndColumnsNamesArray[UtilsConstants._ONE]);
                oQuery.query = oQuery.query.Replace(
                                    string.Concat(UtilsConstants._COLUMN_NAME, UtilsConstants._TWO_STRING),
                                    oTableAndColumnsNamesArray[UtilsConstants._TWO]);
                oQuery.query = oQuery.query.Replace(
                                    string.Concat(UtilsConstants._COLUMN_NAME, UtilsConstants._THREE),
                                    oTableAndColumnsNamesArray[UtilsConstants._THREE]);
                _oDAO.CreateTable(oQuery);
            }
            oLinesArray[UtilsConstants._ZERO] = "true";
        }

        private void GetLinesArrayFromAFile(out string[] oLinesArray)
        {
            var file = UtilsStreamReaders.GetInstance().
                                        ReadStreamFile(this.GetFilePath());
            oLinesArray = file.Split(Environment.NewLine);
        }

        private void ChangePublicAndPrivateKey()
        {

        }

    }
}