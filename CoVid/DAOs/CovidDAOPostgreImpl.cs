using System.Collections.Generic;
using System.Runtime.CompilerServices;
using CoVid.Controllers.DAOs.Connection;
using CoVid.Controllers.DAOs.CreateTableOperations;
using CoVid.DAOs.Abstracts;
using CoVid.DAOs.InsertTableOperations;
using CoVid.DAOs.SelectTableOperations;
using CoVid.Models;
using CoVid.Models.InputModels;
using CoVid.Models.QueryModels;

namespace CoVid.Controllers
{
    public class CovidDAOPostgreImpl : CovidDAO
    {
        private ConnectionPostgreSql _oConnectionPostgreSql{get;set;}
        private PostgreSqlCreateTable _oPostgreSqlCreateTable{get;set;}
        private PostgreSqlInsert _oPostgreSqlInsert{get;set;}
        private PostgreSqlSelect _oPostgreSqlSelect{get;set;}

        private readonly int _MAX_TIMES_OF_CONNECTION = 15;
        private int timesConnected = 0;

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
            if(this._oConnectionPostgreSql is null || this.timesConnected == _MAX_TIMES_OF_CONNECTION)
            {
                this.SetConnection();
                this.timesConnected = 0;
            }

            if(this._oPostgreSqlCreateTable is null)
            {
                this._oPostgreSqlCreateTable = new PostgreSqlCreateTable();
            }
            
            bool isCreated = _oPostgreSqlCreateTable.CreateTable(_oConnectionPostgreSql, pPath);
            this.timesConnected++;

            return isCreated;
        }

        public override bool CreateNamedTable(string pPath, params string[] pTableName)
        {
            if(this._oConnectionPostgreSql is null || this.timesConnected == _MAX_TIMES_OF_CONNECTION)
            {
                this.SetConnection();
                this.timesConnected = 0;
            }

            if(this._oPostgreSqlCreateTable is null)
            {
                this._oPostgreSqlCreateTable = new PostgreSqlCreateTable();
            }
            
            bool isCreated = _oPostgreSqlCreateTable.CreateNamedDataTable(_oConnectionPostgreSql, pPath, pTableName);
            this.timesConnected++;

            return isCreated;
        }

        public override bool InsertGeoZone(GeoZone pGeoZone)
        {
            if(this._oConnectionPostgreSql is null || this.timesConnected == _MAX_TIMES_OF_CONNECTION)
            {
                this.SetConnection();
                this.timesConnected = 0;
            }
            _oPostgreSqlInsert = PostgreSqlInsert.GetInstance(_oConnectionPostgreSql);
            _oPostgreSqlInsert.InsertGeoZone(pGeoZone);
            this.timesConnected++;

            return true;
        }

        public override bool InsertGeoZoneList(List<GeoZone> pGeoZone)
        {
            if(this._oConnectionPostgreSql is null || this.timesConnected == _MAX_TIMES_OF_CONNECTION)
            {
                this.SetConnection();
                this.timesConnected = 0;
            }
            _oPostgreSqlInsert = PostgreSqlInsert.GetInstance(_oConnectionPostgreSql);
            _oPostgreSqlInsert.InsertGeoZoneList(pGeoZone);

            this.timesConnected++;

            return true;
        }

        public override bool InsertGeoZoneCountry(GeoZone pGeoZone)
        {
            if(this._oConnectionPostgreSql is null || this.timesConnected == _MAX_TIMES_OF_CONNECTION)
            {
                this.SetConnection();
                this.timesConnected = 0;
            }
            _oPostgreSqlInsert = PostgreSqlInsert.GetInstance(_oConnectionPostgreSql);
            _oPostgreSqlInsert.InsertGeoZoneCountry(pGeoZone);

            this.timesConnected++;

            return true;
        }

        public override bool InsertGeoZoneCountryList(List<GeoZone> pGeoZone)
        {
            if(this._oConnectionPostgreSql is null || this.timesConnected == _MAX_TIMES_OF_CONNECTION)
            {
                this.SetConnection();
                this.timesConnected = 0;
            }
            _oPostgreSqlInsert = PostgreSqlInsert.GetInstance(_oConnectionPostgreSql);
            _oPostgreSqlInsert.InsertGeoZoneCountryList(pGeoZone);
            this.timesConnected++;

            return true;
        }

        public override bool InsertCovidData(CoVidData pCovidData, GeoZone pGeoZone)
        {
            if(this._oConnectionPostgreSql is null || this.timesConnected == _MAX_TIMES_OF_CONNECTION)
            {
                this.SetConnection();
                this.timesConnected = 0;
            }
            _oPostgreSqlInsert = PostgreSqlInsert.GetInstance(_oConnectionPostgreSql);

            _oPostgreSqlInsert.InsertCovidData(pCovidData, pGeoZone);
            this.timesConnected++;

            return true;
        }

        public override bool InsertCovidDataList(List<CoVidData> pCovidData, GeoZone pGeoZone)
        {
            if(this._oConnectionPostgreSql is null || this.timesConnected == _MAX_TIMES_OF_CONNECTION)
            {
                this.SetConnection();
                this.timesConnected = 0;
            }
            _oPostgreSqlInsert = PostgreSqlInsert.GetInstance(_oConnectionPostgreSql);
            _oPostgreSqlInsert.InsertCovidDataList(pCovidData, pGeoZone);
            _oPostgreSqlInsert = null;
            this.timesConnected ++;
            return true;
        }

        public override bool InsertDate(CovidDate pCovidDate)
        {
            if(this._oConnectionPostgreSql is null || this.timesConnected == _MAX_TIMES_OF_CONNECTION)
            {
                this.SetConnection();
                this.timesConnected = 0;
            }
            _oPostgreSqlInsert = PostgreSqlInsert.GetInstance(_oConnectionPostgreSql);

            _oPostgreSqlInsert.InsertDate(pCovidDate);
            this.timesConnected++;

            return true;
        }

        public override bool InsertDateList(List<CovidDate> pCovidDate)
        {
            if(this._oConnectionPostgreSql is null || this.timesConnected == _MAX_TIMES_OF_CONNECTION)
            {
                this.SetConnection();
                this.timesConnected = 0;
            }
            _oPostgreSqlInsert = PostgreSqlInsert.GetInstance(_oConnectionPostgreSql);
            _oPostgreSqlInsert.InsertDateList(pCovidDate);
            this.timesConnected++;

            return true;
        }

        public override bool CreateTable(Query pQuery)
        {
            if(this._oConnectionPostgreSql is null || this.timesConnected == _MAX_TIMES_OF_CONNECTION)
            {
                this.SetConnection();
                this.timesConnected = 0;
            }

            if(this._oPostgreSqlCreateTable is null)
            {
                this._oPostgreSqlCreateTable = new PostgreSqlCreateTable();
            }
            
            bool isCreated = _oPostgreSqlCreateTable.CreateTable(_oConnectionPostgreSql, pQuery);

            return isCreated;
        }

        public override void GetGeoZoneData(CovidData pCovidData, List<GeoZone> pListToComplete)
        {
            if(this._oConnectionPostgreSql is null || this.timesConnected == _MAX_TIMES_OF_CONNECTION)
            {
                this.SetConnection();
                this.timesConnected = 0;
            }
            if(_oPostgreSqlSelect is null)
            {
                this._oPostgreSqlSelect = PostgreSqlSelect.GetInstance(_oConnectionPostgreSql);
            }
            _oPostgreSqlSelect.GetGeoZoneData(pCovidData, pListToComplete);
        }

        public override void GetAllGeoZoneData(CovidData pCovidData, List<GeoZone> pListToComplete)
        {
            throw new System.NotImplementedException();
        }
    }
}