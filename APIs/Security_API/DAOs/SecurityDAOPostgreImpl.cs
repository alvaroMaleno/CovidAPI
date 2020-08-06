using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Security_REST.Controllers.DAOs.Connection;
using Security_REST.Controllers.DAOs.CreateTableOperations;
using Security_REST.DAOs.Abstracts;
using Security_REST.DAOs.InsertTableOperations;
using Security_REST.DAOs.SelectTableOperations;
using Security_REST.DAOs.UpdateTableOperations;
using Security_REST.Models.DataModels;
using Security_REST.Models.QueryModels;

namespace Security_REST.DAOs
{
    public class SecurityDAOPostgreImpl : DAO
    {
        private PostgreSqlCreateTable _oPostgreSqlCreateTable{get;set;}
        private PostgreSqlInsert _oPostgreSqlInsert{get;set;}
        private PostgreSqlSelect _oPostgreSqlSelect{get;set;}
        private PostgreSqlUpdate _oPostgreSqlUpdate{get;set;}
        private static SecurityDAOPostgreImpl _instance;

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static SecurityDAOPostgreImpl GetInstance()
        {
            if(_instance is null)
                _instance = new SecurityDAOPostgreImpl();

            return _instance;
        }

        private SecurityDAOPostgreImpl(){}

        private void SetConnection(out ConnectionPostgreSql pConnection)
        {
            pConnection = new ConnectionPostgreSql();
        }

        public override bool CreateTable(string pPath)
        {
            ConnectionPostgreSql oConnection;
            this.SetConnection(out oConnection);

            if(this._oPostgreSqlCreateTable is null)
                this._oPostgreSqlCreateTable = new PostgreSqlCreateTable();
            
            bool isCreated = _oPostgreSqlCreateTable.CreateTable(oConnection, pPath);

            return isCreated;
        }

        public override bool CreateNamedTable(string pPath, params string[] pTableName)
        {
            ConnectionPostgreSql oConnection;
            this.SetConnection(out oConnection);

            if(this._oPostgreSqlCreateTable is null)
                this._oPostgreSqlCreateTable = new PostgreSqlCreateTable();
            
            bool isCreated = _oPostgreSqlCreateTable.CreateNamedDataTable(oConnection, pPath, pTableName);

            return isCreated;
        }

        public override bool CreateTable(Query pQuery)
        {
            ConnectionPostgreSql oConnection;
            this.SetConnection(out oConnection);

            if(this._oPostgreSqlCreateTable is null)
                this._oPostgreSqlCreateTable = new PostgreSqlCreateTable();

            return _oPostgreSqlCreateTable.CreateTable(oConnection, pQuery);
        }

        public override void InsertUser(User pUser, string[] pTableLine)
        {
            ConnectionPostgreSql oConnection;
            this.SetConnection(out oConnection);

            if(_oPostgreSqlInsert is null)
                _oPostgreSqlInsert = PostgreSqlInsert.GetInstance(oConnection);

            _oPostgreSqlInsert.InsertUser(pUser, pTableLine);
        }

        public override void InsertUsers(List<User> pUserList, string[] pTableLine)
        {
            ConnectionPostgreSql oConnection;
            this.SetConnection(out oConnection);

            if(_oPostgreSqlInsert is null)
                _oPostgreSqlInsert = PostgreSqlInsert.GetInstance(oConnection);

            _oPostgreSqlInsert.InsertUsers(pUserList, pTableLine);
        }

        public override void InsertKeyPair(KeyPair pKeyPair, string[] pTableLine)
        {
            ConnectionPostgreSql oConnection;
            this.SetConnection(out oConnection);

            if(_oPostgreSqlInsert is null)
                _oPostgreSqlInsert = PostgreSqlInsert.GetInstance(oConnection);

            _oPostgreSqlInsert.InsertKeyPair(pKeyPair, pTableLine);
        }

        public override void InsertKeyPair(
            KeyPair pKeyPair, 
            User pUser, 
            string[] pTableLine)
        {
            ConnectionPostgreSql oConnection;
            this.SetConnection(out oConnection);

            if(_oPostgreSqlInsert is null)
                _oPostgreSqlInsert = PostgreSqlInsert.GetInstance(oConnection);

            _oPostgreSqlInsert.InsertKeyPair(pKeyPair, pUser, pTableLine);
        }

        public override void SelectKeyPairFromUser(User pUser, string[] pTableLine, out KeyPair pKeyPair)
        {
            ConnectionPostgreSql oConnection;
            this.SetConnection(out oConnection);

            if(_oPostgreSqlSelect is null)
                _oPostgreSqlSelect = PostgreSqlSelect.GetInstance(oConnection);

            _oPostgreSqlSelect.SelectKeyPairFromUser(pUser, pTableLine, out pKeyPair);
        }

        public override void SelectAllKeyPairs(List<KeyPair> pKeyPairList, string pTableName)
        {
            ConnectionPostgreSql oConnection;
            this.SetConnection(out oConnection);

            if(_oPostgreSqlSelect is null)
                _oPostgreSqlSelect = PostgreSqlSelect.GetInstance(oConnection);

            _oPostgreSqlSelect.SelectAllKeyPairs(pKeyPairList, pTableName);
        }

        public override void SelectUser(User pUser, string[] pTableLine, out User pSelectedUser)
        {
            ConnectionPostgreSql oConnection;
            this.SetConnection(out oConnection);

            if(_oPostgreSqlSelect is null)
                _oPostgreSqlSelect = PostgreSqlSelect.GetInstance(oConnection);

            _oPostgreSqlSelect.SelectUser(pUser, pTableLine, out pSelectedUser);
        }

        public override void GetCreateQuery(out Query pQuery)
        {
            if(this._oPostgreSqlCreateTable is null)
                this._oPostgreSqlCreateTable = new PostgreSqlCreateTable();
            _oPostgreSqlCreateTable.SetQuery(string.Empty, out pQuery);
        }

        public override void GetSelectQuery(string pPath, out Query pQuery)
        {
            _oPostgreSqlSelect.SetQuery(pPath, out pQuery);
        }

        public override void UpdatePublicKey(KeyPair pOldKeyPair, KeyPair pNewKeyPair, string[] pTableLine)
        {
            ConnectionPostgreSql oConnection;
            this.SetConnection(out oConnection);

            if(_oPostgreSqlUpdate is null)
                _oPostgreSqlUpdate = PostgreSqlUpdate.GetInstance(oConnection);

            _oPostgreSqlUpdate.UpdatePublicKey(pOldKeyPair, pNewKeyPair, pTableLine);
        }

        public override void UpdatePrivateKey(KeyPair pOldKeyPair, KeyPair pNewKeyPair, string[] pTableLine)
        {
            ConnectionPostgreSql oConnection;
            this.SetConnection(out oConnection);

            if(_oPostgreSqlUpdate is null)
                _oPostgreSqlUpdate = PostgreSqlUpdate.GetInstance(oConnection);

            _oPostgreSqlUpdate.UpdatePrivateKey(pOldKeyPair, pNewKeyPair, pTableLine);
        }

        public override void UpdatePrivateFromPublicKey(KeyPair pOldKeyPair, KeyPair pNewKeyPair, string[] pTableLine)
        {
            ConnectionPostgreSql oConnection;
            this.SetConnection(out oConnection);

            if(_oPostgreSqlUpdate is null)
                _oPostgreSqlUpdate = PostgreSqlUpdate.GetInstance(oConnection);

            _oPostgreSqlUpdate.UpdatePrivateFromPublicKey(pOldKeyPair, pNewKeyPair, pTableLine);
        }
    }
}