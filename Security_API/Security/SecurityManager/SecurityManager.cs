using System.Threading;
using System;
using System.Collections.Generic;
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
        private int _numberOfUsesOfActualKey;
        private int _MAX_NUMBER_OF_USES_OF_ACTUAL_KEY = 100;

        private SecurityManager()
        {
            _oDAO = SecurityDAOPostgreImpl.GetInstance();
            _oRSAManager = RSAManager.GetInstance();
            _oSolidDataManager = SolidDataManager.GetInstance();

            string[] oLinesArray;
            _oSolidDataManager.GetLinesArrayFromAFile(out oLinesArray);
            int.TryParse(oLinesArray[oLinesArray.Length - UtilsConstants._ONE], out _numberOfUsesOfActualKey);
        }

        public static SecurityManager GetInstance()
        {
            if(_instance is null)
                _instance = new SecurityManager();
            
            return _instance;
        }
        
        public void GetAPIKeyPair(out KeyPair pKeyPair)
        {
            string[] oLinesArray;
            _oSolidDataManager.GetLinesArrayFromAFile(out oLinesArray); 
            this.GetAPIKeypairFromDB(out pKeyPair, oLinesArray);
        }

        public void AddUser(User pUser)
        {
            string[] oLinesArray;
            _oSolidDataManager.GetLinesArrayFromAFile(out oLinesArray); 

            KeyPair oKeyPair;
            this.GetAPIKeypairFromDB(out oKeyPair, oLinesArray);

            try
            {
                pUser.email = _oRSAManager.DesencryptWithPrivateKeyString(pUser.email, oKeyPair);
                pUser.pass = _oRSAManager.DesencryptWithPrivateKeyString(pUser.pass, oKeyPair);
            }
            catch (System.Exception)
            {
                pUser.public_key = UtilsConstants._PLEASE_ENCRYPT_ERROR;
            }

            KeyPair oUserKeyPair;
            this.GetUserKeyPairAndEncryptedUser(pUser, oKeyPair, out oUserKeyPair);
            this.InsertUserWithKeyPair(pUser, oUserKeyPair, oLinesArray);

            pUser.public_key = oUserKeyPair.public_string;
            pUser.email = _oRSAManager.EncryptWithPublicKeyString(pUser.email, oUserKeyPair.public_string);
        }

        private void InsertUserWithKeyPair(User pUser, KeyPair pUserKeyPair, string[] pLinesArray)
        {
            _oDAO.InsertUser(pUser, pLinesArray[UtilsConstants._ONE].Split(UtilsConstants._COME));
            _oDAO.InsertKeyPair(
                pUserKeyPair, 
                pUser, 
                pLinesArray[UtilsConstants._TWO].Split(UtilsConstants._COME));
            this.SetNumberOfUsersAddedWithActualKey(_numberOfUsesOfActualKey + UtilsConstants._ONE);
        }

        private void GetUserKeyPairAndEncryptedUser(User pUser, KeyPair pKeyPair, out KeyPair pUserKeyPair)
        {
            _oRSAManager.CreateKeyPair(out pUserKeyPair);
            pUser.pass = _oRSAManager.EncryptWithPublicKeyString(pUser.pass, pUserKeyPair.public_string);
            pUserKeyPair.private_string = this.EncryptByChunk(pUserKeyPair.private_string, pKeyPair);
        }

        private string EncryptByChunk(string pToEncrypt, KeyPair pKeyPair)
        {
            var oPrivateArray = pToEncrypt.Split(UtilsConstants._ENCRYPT_SPLIT);

            for (int i = UtilsConstants._ZERO; i < oPrivateArray.Length; i++)
                oPrivateArray[i] = _oRSAManager.EncryptWithPublicKeyString(
                                        oPrivateArray[i], pKeyPair.public_string);

            return String.Join(UtilsConstants._COME, oPrivateArray);
        }

        private string DecryptByChunk(string pToDecrypt, KeyPair pKeyPair)
        {
            var oPrivateArray = pToDecrypt.Split(UtilsConstants._COME);

            for (int i = UtilsConstants._ZERO; i < oPrivateArray.Length; i++)
                oPrivateArray[i] = _oRSAManager.DesencryptWithPrivateKeyString(
                                        oPrivateArray[i], pKeyPair);

            return String.Join(UtilsConstants._ENCRYPT_SPLIT, oPrivateArray);
        }

        private void SetNumberOfUsersAddedWithActualKey(int pNumber)
        {
            _numberOfUsesOfActualKey = pNumber;

            if(_numberOfUsesOfActualKey >= _MAX_NUMBER_OF_USES_OF_ACTUAL_KEY)
            {
                this.SetNumberOfUsersAddedWithActualKey(UtilsConstants._ZERO);
                Thread oThread = new Thread(
                new ThreadStart(ChangeDBEncriptation));
                oThread.Start();
            }
            
            string[] oLinesArray;
            _oSolidDataManager.GetLinesArrayFromAFile(out oLinesArray);
            oLinesArray[oLinesArray.Length - UtilsConstants._ONE] = _numberOfUsesOfActualKey.ToString();
            UtilsStreamWritters.GetInstance().WritteStringToFile(
                _oSolidDataManager.GetFileToPersistFromLinesArray(oLinesArray), 
                _oSolidDataManager.GetFilePath());

        }

        private void GetAPIKeypairFromDB(out KeyPair pKeyPair, string[] pLinesArray)
        {
            List<KeyPair> oKeyPairList = new List<KeyPair>(UtilsConstants._THREE);

            _oDAO.SelectAllKeyPairs(
                oKeyPairList, 
                pLinesArray[UtilsConstants._THREE].Split(UtilsConstants._COME)[UtilsConstants._ZERO].Trim());

            pKeyPair = new KeyPair(
                oKeyPairList[UtilsConstants._ZERO].public_string, 
                oKeyPairList[UtilsConstants._ZERO].private_string);
        }

        public bool ValidateUser(User pUser)
        {
            try
            {
                string[] oLinesArray;
                _oSolidDataManager.GetLinesArrayFromAFile(out oLinesArray);

                KeyPair oUserKeyPair;
                _oDAO.SelectKeyPairFromUser(
                    pUser, 
                    oLinesArray[UtilsConstants._TWO].Split(UtilsConstants._COME),
                    out oUserKeyPair);
                
                KeyPair oKeyPair;
                this.GetAPIKeyPair(out oKeyPair);

                oUserKeyPair.private_string = this.DecryptByChunk(oUserKeyPair.private_string, oKeyPair);
                pUser.email = _oRSAManager.DesencryptWithPrivateKeyString(pUser.email, oUserKeyPair);
                pUser.pass = _oRSAManager.DesencryptWithPrivateKeyString(pUser.pass, oUserKeyPair);

                User oSelectedUser;
                _oDAO.SelectUser(
                    pUser, 
                    oLinesArray[UtilsConstants._ONE].Split(UtilsConstants._COME),
                    out oSelectedUser);

                oSelectedUser.pass = _oRSAManager.DesencryptWithPrivateKeyString(oSelectedUser.pass, oUserKeyPair);
                this.SetNumberOfUsersAddedWithActualKey(_numberOfUsesOfActualKey + UtilsConstants._ONE);
                
                if(string.IsNullOrEmpty(oSelectedUser.pass) || oSelectedUser.pass != pUser.pass)
                    return false;

            }
            catch (System.Exception)
            {
                return false;
            }

            return true;
        }

        private void ChangeDBEncriptation()
        {
            KeyPair oOldKeyPair;
            this.GetAPIKeyPair(out oOldKeyPair);

            KeyPair oNewKeyPair;
            _oRSAManager.CreateKeyPair(out oNewKeyPair);

            string[] oLinesArray;
            _oSolidDataManager.GetLinesArrayFromAFile(out oLinesArray);

            List<KeyPair> oKeyPairList = new List<KeyPair>();
            _oDAO.SelectAllKeyPairs(
                oKeyPairList, 
                oLinesArray[UtilsConstants._TWO].Split(UtilsConstants._COME)[UtilsConstants._ZERO].Trim());

            this.ChangeKeyPairListEncryption(
                oOldKeyPair, 
                oNewKeyPair, 
                oKeyPairList);
            
            this.UpdateAPIKeyPair(
                oOldKeyPair, 
                oNewKeyPair, 
                oLinesArray[UtilsConstants._THREE].Split(UtilsConstants._COME));

            _oDAO.InsertKeyPair(oOldKeyPair, oLinesArray[UtilsConstants._FOUR].Split(UtilsConstants._COME));

            this.UpdateUsersKeyPair(
                oKeyPairList,
                oOldKeyPair, 
                oNewKeyPair, 
                oLinesArray[UtilsConstants._TWO].Split(UtilsConstants._COME));

        }

        private void UpdateUsersKeyPair(
            List<KeyPair> pKeyPairList, 
            KeyPair pOldKeyPair, 
            KeyPair pNewKeyPair, 
            string[] pTableLine)
        {
            foreach (var f_oUserKeyPair in pKeyPairList)
            {
                _oDAO.UpdatePrivateFromPublicKey(f_oUserKeyPair, f_oUserKeyPair, pTableLine);
                Thread.Sleep(500);
            }
        }

        private void UpdateAPIKeyPair(KeyPair pOldKeyPair, KeyPair pNewKeyPair, string[] pTableLine)
        {
            _oDAO.UpdatePrivateKey(pOldKeyPair, pNewKeyPair, pTableLine);
            _oDAO.UpdatePublicKey(pOldKeyPair, pNewKeyPair, pTableLine);
        }

        private void ChangeKeyPairListEncryption(
            KeyPair pOldKeyPair, 
            KeyPair pNewKeyPair, 
            List<KeyPair> pKeyPairList)
        {
            foreach (var f_oKeyPair in pKeyPairList)
            {
                f_oKeyPair.private_string = this.DecryptByChunk(f_oKeyPair.private_string, pOldKeyPair);
                f_oKeyPair.private_string = this.EncryptByChunk(f_oKeyPair.private_string, pNewKeyPair);
            }
        }

        public void CreateSecurityTables()
        {
            string[] oLinesArray;
            _oSolidDataManager.GetLinesArrayFromAFile(out oLinesArray);

            if(oLinesArray[UtilsConstants._ZERO].Contains("true"))
                return;

            this.CreateTableByFirstTime(oLinesArray);
            UtilsStreamWritters.GetInstance().WritteStringToFile(
                _oSolidDataManager.GetFileToPersistFromLinesArray(oLinesArray), 
                _oSolidDataManager.GetFilePath());
            
            KeyPair oKeyPair;
            _oRSAManager.CreateKeyPair(out oKeyPair);
            
            _oDAO.InsertKeyPair(
                oKeyPair, 
                oLinesArray[UtilsConstants._THREE].Split(UtilsConstants._COME));
        }

        private void CreateTableByFirstTime(string[] oLinesArray)
        {
            string[] oTableAndColumnsNamesArray;
            
            for (int i = UtilsConstants._ONE; i < oLinesArray.Length - UtilsConstants._ONE; i++)
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

    }
}