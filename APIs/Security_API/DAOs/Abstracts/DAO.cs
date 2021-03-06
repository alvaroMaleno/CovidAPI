using System.Collections.Generic;
using Security_REST.Models.DataModels;
using Security_REST.Models.QueryModels;

namespace Security_REST.DAOs.Abstracts
{
    public abstract class DAO
    {
        public abstract bool CreateTable(Query pQuery);
        public abstract bool CreateTable(string pPath);
        public abstract bool CreateNamedTable(string pPath, params string[] pTableName);
        public abstract void GetCreateQuery(out Query pQuery);
        public abstract void InsertUser(User pUser, string[] pTableLine);
        public abstract void InsertUsers(List<User> pUserList, string[] pTableLine);
        public abstract void InsertKeyPair(KeyPair pKeyPair, string[] pTableLine);
        public abstract void InsertKeyPair(KeyPair pKeyPair, User pUser, string[] pTableLine);
        public abstract void SelectKeyPairFromUser(User pUser, string[] pTableLine, out KeyPair pKeyPair);
        public abstract void SelectAllKeyPairs(List<KeyPair> pKeyPairList, string pTableName);
        public abstract void SelectUser(User pUser, string[] pTableLine, out User pSelectedUser);
        public abstract void GetSelectQuery(string pPath, out Query pQuery);
        public abstract void UpdatePublicKey(KeyPair pOldKeyPair, KeyPair pNewKeyPair, string[] pTableLine);
        public abstract void UpdatePrivateKey(KeyPair pOldKeyPair, KeyPair pNewKeyPair, string[] pTableLine);
        public abstract void UpdatePrivateFromPublicKey(KeyPair pOldKeyPair, KeyPair pNewKeyPair, string[] pTableLine);
    }
}