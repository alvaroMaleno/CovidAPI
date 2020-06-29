using System.Collections.Generic;
using System.Runtime.CompilerServices;
using CoVid.Controllers.DAOs.Connection;
using CoVid.Controllers.DAOs.CreateTableOperations;
using CoVid.DAOs.Abstracts;
using CoVid.DAOs.InsertTableOperations;
using CoVid.DAOs.SelectTableOperations;
using CoVid.Models;
using CoVid.Models.QueryModels;

namespace CoVid.Controllers
{
    public class CovidDAOPostgreImpl : DAO
    {
        private PostgreSqlCreateTable _oPostgreSqlCreateTable{get;set;}
        private PostgreSqlInsert _oPostgreSqlInsert{get;set;}
        private PostgreSqlSelect _oPostgreSqlSelect{get;set;}
        private static CovidDAOPostgreImpl _instance;

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static CovidDAOPostgreImpl GetInstance()
        {
            if(_instance is null)
            {
                _instance = new CovidDAOPostgreImpl();
            }

            return _instance;
        }


        private CovidDAOPostgreImpl(){}

        private void SetConnection(out ConnectionPostgreSql pConnection)
        {
            pConnection = new ConnectionPostgreSql();
        }

        public override bool CreateTable(string pPath)
        {
            ConnectionPostgreSql oConnection;
            this.SetConnection(out oConnection);

            if(this._oPostgreSqlCreateTable is null)
            {
                this._oPostgreSqlCreateTable = new PostgreSqlCreateTable();
            }
            
            bool isCreated = _oPostgreSqlCreateTable.CreateTable(oConnection, pPath);

            return isCreated;
        }

        public override bool CreateNamedTable(string pPath, params string[] pTableName)
        {
            ConnectionPostgreSql oConnection;
            this.SetConnection(out oConnection);

            if(this._oPostgreSqlCreateTable is null)
            {
                this._oPostgreSqlCreateTable = new PostgreSqlCreateTable();
            }
            
            bool isCreated = _oPostgreSqlCreateTable.CreateNamedDataTable(oConnection, pPath, pTableName);

            return isCreated;
        }

        public override bool CreateTable(Query pQuery)
        {
            throw new System.NotImplementedException();
        }
    }
}