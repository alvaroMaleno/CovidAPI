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

        public override bool InsertGeoZone(GeoZone pGeoZone)
        {
            ConnectionPostgreSql oConnection;
            this.SetConnection(out oConnection);

            _oPostgreSqlInsert = PostgreSqlInsert.GetInstance(oConnection);
            _oPostgreSqlInsert.InsertGeoZone(pGeoZone);

            return true;
        }

        public override bool InsertGeoZoneList(List<GeoZone> pGeoZone)
        {
            ConnectionPostgreSql oConnection;
            this.SetConnection(out oConnection);

            _oPostgreSqlInsert = PostgreSqlInsert.GetInstance(oConnection);
            _oPostgreSqlInsert.InsertGeoZoneList(pGeoZone);

            return true;
        }

        public override bool InsertGeoZoneCountry(GeoZone pGeoZone)
        {
            ConnectionPostgreSql oConnection;
            this.SetConnection(out oConnection);

            _oPostgreSqlInsert = PostgreSqlInsert.GetInstance(oConnection);
            _oPostgreSqlInsert.InsertGeoZoneCountry(pGeoZone);

            return true;
        }

        public override bool InsertGeoZoneCountryList(List<GeoZone> pGeoZone)
        {
            ConnectionPostgreSql oConnection;
            this.SetConnection(out oConnection);

            _oPostgreSqlInsert = PostgreSqlInsert.GetInstance(oConnection);
            _oPostgreSqlInsert.InsertGeoZoneCountryList(pGeoZone);

            return true;
        }

        public override bool InsertCovidData(CoVidData pCovidData, GeoZone pGeoZone)
        {
            ConnectionPostgreSql oConnection;
            this.SetConnection(out oConnection);

            _oPostgreSqlInsert = PostgreSqlInsert.GetInstance(oConnection);

            _oPostgreSqlInsert.InsertCovidData(pCovidData, pGeoZone);

            return true;
        }

        public override bool InsertCovidDataList(List<CoVidData> pCovidData, GeoZone pGeoZone)
        {
            ConnectionPostgreSql oConnection;
            this.SetConnection(out oConnection);

            _oPostgreSqlInsert = PostgreSqlInsert.GetInstance(oConnection);
            _oPostgreSqlInsert.InsertCovidDataList(pCovidData, pGeoZone);
            _oPostgreSqlInsert = null;
            
            return true;
        }

        public override bool InsertDate(CovidDate pCovidDate)
        {
            ConnectionPostgreSql oConnection;
            this.SetConnection(out oConnection);

            _oPostgreSqlInsert = PostgreSqlInsert.GetInstance(oConnection);
            _oPostgreSqlInsert.InsertDate(pCovidDate);

            return true;
        }

        public override bool InsertDateList(List<CovidDate> pCovidDate)
        {
            ConnectionPostgreSql oConnection;
            this.SetConnection(out oConnection);

            _oPostgreSqlInsert = PostgreSqlInsert.GetInstance(oConnection);
            _oPostgreSqlInsert.InsertDateList(pCovidDate);

            return true;
        }

        public override bool CreateTable(Query pQuery)
        {
            ConnectionPostgreSql oConnection;
            this.SetConnection(out oConnection);

            if(this._oPostgreSqlCreateTable is null)
            {
                this._oPostgreSqlCreateTable = new PostgreSqlCreateTable();
            }
            
            bool isCreated = _oPostgreSqlCreateTable.CreateTable(oConnection, pQuery);

            return isCreated;
        }

        public override void GetGeoZoneData(CovidData pCovidData, List<GeoZone> pListToComplete)
        {
            ConnectionPostgreSql oConnection;
            this.SetConnection(out oConnection);

            if(_oPostgreSqlSelect is null)
            {
                this._oPostgreSqlSelect = PostgreSqlSelect.GetInstance(oConnection);
            }
            _oPostgreSqlSelect.GetGeoZoneData(pCovidData, pListToComplete);
        }

        public override void GetAllGeoZoneData(CovidData pCovidData, List<GeoZone> pListToComplete)
        {
            ConnectionPostgreSql oConnection;
            this.SetConnection(out oConnection);

            if(_oPostgreSqlSelect is null)
            {
                this._oPostgreSqlSelect = PostgreSqlSelect.GetInstance(oConnection);
            }
            _oPostgreSqlSelect.GetAllGeoZoneData(pCovidData, pListToComplete);
        }

        public override void GetAllCountries(List<GeoZone> pCovidCountryList)
        {
            ConnectionPostgreSql oConnection;
            this.SetConnection(out oConnection);

            if(_oPostgreSqlSelect is null)
            {
                this._oPostgreSqlSelect = PostgreSqlSelect.GetInstance(oConnection);
            }
            _oPostgreSqlSelect.GetAllCountries(pCovidCountryList);
        }

        public override void GetAllDates(List<CovidDate> pCovidDateList)
        {
            ConnectionPostgreSql oConnection;
            this.SetConnection(out oConnection);

            if(_oPostgreSqlSelect is null)
            {
                this._oPostgreSqlSelect = PostgreSqlSelect.GetInstance(oConnection);
            }
            _oPostgreSqlSelect.GetAllDates(pCovidDateList);
        }
    }
}