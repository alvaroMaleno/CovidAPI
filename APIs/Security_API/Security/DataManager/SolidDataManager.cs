using System;
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

        public string GetFilePath()
        {
            string selectPaths;
            string so = UtilsSO.GetInstance().GetSO();

            if(so.Contains("unix"))
                selectPaths = @"./Security/DataManager/EncryptedNames/names";
            else
                selectPaths = @".\Security\DataManager\EncryptedNames\names";

            return selectPaths;
        }

        public void GetLinesArrayFromAFile(out string[] oLinesArray)
        {
            var file = UtilsStreamReaders.GetInstance().
                                        ReadStreamFile(this.GetFilePath());
            oLinesArray = file.Split(Environment.NewLine);
        }

        public string GetFileToPersistFromLinesArray(string[] oLinesArray)
        {
            string toReturn = string.Empty;
            
            foreach (var line in oLinesArray)
                if(string.IsNullOrEmpty(toReturn))
                    toReturn = line;
                else
                    toReturn = string.Concat(toReturn, Environment.NewLine, line);
            
            return toReturn;
        }

        public void EncryptEachPersistentFile(KeyPair pKeyPair, Paths pPaths)
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

        public void DescryptEachPersistentFile(KeyPair pKeyPair, Paths pPaths)
        {
            string dataToWritte;
            foreach (var path in pPaths.oPaths)
            {
                dataToWritte = _oRSAManager.
                                DecryptWithPrivateKeyString(
                                    UtilsStreamReaders.GetInstance().ReadStreamFile(path),
                                    pKeyPair);
                
                UtilsStreamWritters.GetInstance().WritteStringToFile(dataToWritte, path);
            }
        }

        public string DecryptFile(KeyPair pKeyPair, string pPath)
        {
            return _oRSAManager.
                    DecryptWithPrivateKeyString(
                        UtilsStreamReaders.GetInstance().ReadStreamFile(pPath),
                        pKeyPair);
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