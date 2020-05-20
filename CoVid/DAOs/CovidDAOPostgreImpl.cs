using System.Collections.Generic;
using System.Runtime.CompilerServices;
using CoVid.Controllers.DAOs.Connection;
using CoVid.Controllers.DAOs.CreateTableOperations;
using CoVid.DAOs.Abstracts;
using CoVid.DAOs.InsertTableOperations;
using CoVid.Models;
using CoVid.Models.QueryModels;

namespace CoVid.Controllers
{
    public class CovidDAOPostgreImpl : CovidDAO
    {
        private ConnectionPostgreSql _oConnectionPostgreSql{get;set;}
        private PostgreSqlCreateTable _oPostgreSqlCreateTable{get;set;}
        private PostgreSqlInsert _oPostgreSqlInsert{get;set;}

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

        private void SetConnection()
        {
            this._oConnectionPostgreSql = new ConnectionPostgreSql();
        }

        public override bool CreateTable(string pPath)
        {
            if(this._oConnectionPostgreSql is null)
            {
                this.SetConnection();
            }

            if(this._oPostgreSqlCreateTable is null)
            {
                this._oPostgreSqlCreateTable = new PostgreSqlCreateTable();
            }
            
            bool isCreated = _oPostgreSqlCreateTable.CreateTable(_oConnectionPostgreSql, pPath);

            return isCreated;
        }

        public override bool CreateNamedTable(string pPath, params string[] pTableName)
        {
            if(this._oConnectionPostgreSql is null)
            {
                this.SetConnection();
            }

            if(this._oPostgreSqlCreateTable is null)
            {
                this._oPostgreSqlCreateTable = new PostgreSqlCreateTable();
            }
            
            bool isCreated = _oPostgreSqlCreateTable.CreateNamedDataTable(_oConnectionPostgreSql, pPath, pTableName);

            return isCreated;
        }

        public override bool InsertGeoZone(GeoZone pGeoZone)
        {
            if(this._oConnectionPostgreSql is null)
            {
                SetConnection();
            }
            _oPostgreSqlInsert = PostgreSqlInsert.GetInstance(_oConnectionPostgreSql);
            _oPostgreSqlInsert.InsertGeoZone(pGeoZone);
            _oPostgreSqlInsert = null;

            return true;
        }

        public override bool InsertGeoZoneList(List<GeoZone> pGeoZone)
        {
            if(this._oConnectionPostgreSql is null)
            {
                SetConnection();
            }
            _oPostgreSqlInsert = PostgreSqlInsert.GetInstance(_oConnectionPostgreSql);
            _oPostgreSqlInsert.InsertGeoZoneList(pGeoZone);

            _oPostgreSqlInsert = null;
            return true;
        }

        public override bool InsertGeoZoneCountry(GeoZone pGeoZone)
        {
            if(this._oConnectionPostgreSql is null)
            {
                SetConnection();
            }
            _oPostgreSqlInsert = PostgreSqlInsert.GetInstance(_oConnectionPostgreSql);
            _oPostgreSqlInsert.InsertGeoZoneCountry(pGeoZone);

            _oPostgreSqlInsert = null;
            return true;
        }

        public override bool InsertGeoZoneCountryList(List<GeoZone> pGeoZone)
        {
            if(this._oConnectionPostgreSql is null)
            {
                SetConnection();
            }
            _oPostgreSqlInsert = PostgreSqlInsert.GetInstance(_oConnectionPostgreSql);
            _oPostgreSqlInsert.InsertGeoZoneCountryList(pGeoZone);
            _oPostgreSqlInsert = null;

            return true;
        }

        public override bool InsertCovidData(CoVidData pCovidData, GeoZone pGeoZone)
        {
            if(this._oConnectionPostgreSql is null)
            {
                SetConnection();
            }
            _oPostgreSqlInsert = PostgreSqlInsert.GetInstance(_oConnectionPostgreSql);

            _oPostgreSqlInsert.InsertCovidData(pCovidData, pGeoZone);
            _oPostgreSqlInsert = null;

            return true;
        }

        public override bool InsertCovidDataList(List<CoVidData> pCovidData, GeoZone pGeoZone)
        {
            if(this._oConnectionPostgreSql is null)
            {
                SetConnection();
            }
            _oPostgreSqlInsert = PostgreSqlInsert.GetInstance(_oConnectionPostgreSql);
            _oPostgreSqlInsert.InsertCovidDataList(pCovidData, pGeoZone);
            _oPostgreSqlInsert = null;

            return true;
        }

        public override bool InsertDate(CovidDate pCovidDate)
        {
            if(this._oConnectionPostgreSql is null)
            {
                SetConnection();
            }
            _oPostgreSqlInsert = PostgreSqlInsert.GetInstance(_oConnectionPostgreSql);

            _oPostgreSqlInsert.InsertDate(pCovidDate);
            _oPostgreSqlInsert = null;

            return true;
        }

        public override bool InsertDateList(List<CovidDate> pCovidDate)
        {
            if(this._oConnectionPostgreSql is null)
            {
                SetConnection();
            }
            _oPostgreSqlInsert = PostgreSqlInsert.GetInstance(_oConnectionPostgreSql);
            _oPostgreSqlInsert.InsertDateList(pCovidDate);
            _oPostgreSqlInsert = null;

            return true;
        }

        public override bool CreateTable(Query pQuery)
        {
            if(this._oConnectionPostgreSql is null)
            {
                this.SetConnection();
            }

            if(this._oPostgreSqlCreateTable is null)
            {
                this._oPostgreSqlCreateTable = new PostgreSqlCreateTable();
            }
            
            bool isCreated = _oPostgreSqlCreateTable.CreateTable(_oConnectionPostgreSql, pQuery);

            return isCreated;
        }
    }
}