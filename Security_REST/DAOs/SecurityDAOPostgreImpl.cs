using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Security_REST.Controllers.DAOs.Connection;
using Security_REST.Controllers.DAOs.CreateTableOperations;
using Security_REST.DAOs.Abstracts;
using Security_REST.DAOs.InsertTableOperations;
using Security_REST.DAOs.SelectTableOperations;
using Security_REST.Models.DataModels;
using Security_REST.Models.QueryModels;

namespace Security_REST.DAOs
{
    public class SecurityDAOPostgreImpl : DAO
    {
        private PostgreSqlCreateTable _oPostgreSqlCreateTable{get;set;}
        private PostgreSqlInsert _oPostgreSqlInsert{get;set;}
        private PostgreSqlSelect _oPostgreSqlSelect{get;set;}
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

        public override void InsertUser(User pUser, string pTableName)
        {
            ConnectionPostgreSql oConnection;
            this.SetConnection(out oConnection);

            if(_oPostgreSqlInsert is null)
                _oPostgreSqlInsert = PostgreSqlInsert.GetInstance(oConnection);

            _oPostgreSqlInsert.InsertUser(pUser, pTableName);
        }

        public override void InsertUsers(List<User> pUserList, string pTableName)
        {
            ConnectionPostgreSql oConnection;
            this.SetConnection(out oConnection);

            if(_oPostgreSqlInsert is null)
                _oPostgreSqlInsert = PostgreSqlInsert.GetInstance(oConnection);

            _oPostgreSqlInsert.InsertUsers(pUserList, pTableName);
        }

        public override void InsertKeyPair(KeyPair pKeyPair, string pTableName)
        {
            ConnectionPostgreSql oConnection;
            this.SetConnection(out oConnection);

            if(_oPostgreSqlInsert is null)
                _oPostgreSqlInsert = PostgreSqlInsert.GetInstance(oConnection);

            _oPostgreSqlInsert.InsertKeyPair(pKeyPair, pTableName);
        }

        public override void SelectKeyPair(KeyPair pKeyPair, string pTableName)
        {
            ConnectionPostgreSql oConnection;
            this.SetConnection(out oConnection);

            if(_oPostgreSqlSelect is null)
                _oPostgreSqlSelect = PostgreSqlSelect.GetInstance(oConnection);

            _oPostgreSqlSelect.SelectKeyPair(pKeyPair, pTableName);
        }

        public override void SelectAllKeyPairs(List<KeyPair> pKeyPairList, string pTableName)
        {
            ConnectionPostgreSql oConnection;
            this.SetConnection(out oConnection);

            if(_oPostgreSqlSelect is null)
                _oPostgreSqlSelect = PostgreSqlSelect.GetInstance(oConnection);

            _oPostgreSqlSelect.SelectAllKeyPairs(pKeyPairList, pTableName);
        }

        public override void SelectUser(User pUser, string pTableName)
        {
            ConnectionPostgreSql oConnection;
            this.SetConnection(out oConnection);

            if(_oPostgreSqlSelect is null)
                _oPostgreSqlSelect = PostgreSqlSelect.GetInstance(oConnection);

            _oPostgreSqlSelect.SelectUser(pUser, pTableName);
        }

        public override void SelectAllUsers(List<User> pUserList, string pTableName)
        {
            ConnectionPostgreSql oConnection;
            this.SetConnection(out oConnection);

            if(_oPostgreSqlSelect is null)
                _oPostgreSqlSelect = PostgreSqlSelect.GetInstance(oConnection);

            _oPostgreSqlSelect.SelectAllUsers(pUserList, pTableName);
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
    }
}