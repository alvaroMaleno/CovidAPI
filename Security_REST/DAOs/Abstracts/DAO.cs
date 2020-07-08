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
        public abstract void InsertUser(User pUser, string pTableName);
        public abstract void InsertUsers(List<User> pUserList, string pTableName);
        public abstract void InsertKeyPair(KeyPair pKeyPair, string pTableName);
        public abstract void SelectKeyPair(KeyPair pKeyPair, string pTableName);
        public abstract void SelectAllKeyPairs(List<KeyPair> pKeyPairList, string pTableName);
        public abstract void SelectUser(User pUser, string pTableName);
        public abstract void SelectAllUsers(List<User> pUserList, string pTableName);
    }
}