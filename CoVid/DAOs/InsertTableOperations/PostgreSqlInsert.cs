using System.Linq;
using System.Text;
using System.Collections.Generic;
using CoVid.Controllers.DAOs;
using CoVid.Controllers.DAOs.Connection;
using CoVid.Models;
using CoVid.Models.QueryModels;
using CoVid.Models.PathModels;
using CoVid.Utils;

namespace CoVid.DAOs.InsertTableOperations
{
    public class PostgreSqlInsert : ICovidDataBaseInsert
    {
        private readonly int _ONE = 1;
        private readonly string _REPLACE_QUERY_CONSTANT = "?";
        private readonly string _REPLACE_SINGLEQUOTE_CONSTANT = "'";
        private readonly string _THREE_STRING = "{3}";
        private readonly string _TWO_STRING = "{2}";
        private readonly string _ONE_STRING = "{1}";
        private readonly string _ZERO_STRING = "{0}";
        private readonly string _COME = ",";
        private readonly string _NULL = "null";
        private string _INSERT_DATES_QUERY_PATH;
        private string _INSERT_GEOZONE_NAME_QUERY_PATH;
        private string _INSERT_GEOZONE_QUERY_PATH;
        private string _INSERT_GEOZONE_COUNTRIES_QUERY_PATH;
        public ConnectionPostgreSql oConnectionPostgreSql { get; set; }
        private static PostgreSqlInsert _instance; 

        private PostgreSqlInsert(ConnectionPostgreSql pConnectionPostgreSql)
        {
            this.oConnectionPostgreSql = pConnectionPostgreSql;
            this.SetPaths();
        }

        public static PostgreSqlInsert GetInstance(ConnectionPostgreSql pConnectionPostgreSql)
        {
            if(_instance is null){_instance = new PostgreSqlInsert(pConnectionPostgreSql);}
            return _instance;
        }

        private void SetPaths()
        {
            string createTablePaths = string.Empty;
            string so = UtilsSO.GetInstance().GetSO();

            if(so.Contains("unix"))
            {
                createTablePaths = @"./Processes/InitialDataInsertion/insertData_Unix_Paths.json";
            }
            else
            {
                createTablePaths = @".\Processes\InitialDataInsertion\insertData_Windows_Paths.json";
            }
            
            var paths = UtilsStreamReaders.GetInstance().ReadStreamFile(createTablePaths);
            Paths oPathsArray;
            UtilsJSON.GetInstance().DeserializeFromString(out oPathsArray, paths);
            this._INSERT_DATES_QUERY_PATH = oPathsArray.oPaths[0];
            this._INSERT_GEOZONE_COUNTRIES_QUERY_PATH = oPathsArray.oPaths[3];
            this._INSERT_GEOZONE_NAME_QUERY_PATH = oPathsArray.oPaths[1];
            this._INSERT_GEOZONE_QUERY_PATH = oPathsArray.oPaths[2];
        }


        public bool InsertCovidData(CoVidData pCovidData, GeoZone pGeoZone)
        {
            Query oQuery;
            this.SetQuery(_INSERT_DATES_QUERY_PATH, out oQuery);
            oQuery.query = oQuery.query.Replace("country_name", pGeoZone.name);
            this.SetCovidDataQuery(pCovidData, pGeoZone, oQuery);

            return oConnectionPostgreSql.ExecuteCommand(oQuery.query);
        }

        public bool InsertCovidDataList(List<CoVidData> pCovidData, GeoZone pGeoZone)
        {
            Query oQuery;
            this.SetQuery(_INSERT_DATES_QUERY_PATH, out oQuery);
            oQuery.query = oQuery.query.Replace("country_name", pGeoZone.geoID);
            this.SetCovidDataListQuery(pCovidData, pGeoZone, oQuery);

            return oConnectionPostgreSql.ExecuteCommand(oQuery.query);
        }

        private void SetCovidDataQuery(CoVidData pCovidData, GeoZone pGeoZone, Query oQuery)
        {
            StringBuilder oStringBuilder = new StringBuilder();
            this.AddCovidDataInsertStatementToStringBuilder(oStringBuilder, pCovidData, pGeoZone, oQuery);
            oQuery.query = oQuery.query.Replace(
                _REPLACE_QUERY_CONSTANT, oStringBuilder.ToString());
        }

        private void SetCovidDataListQuery(List<CoVidData> pCovidData, GeoZone pGeoZone, Query oQuery)
        {
            StringBuilder oStringBuilder = new StringBuilder();
            foreach (var oCovidData in pCovidData)
            {
                this.AddCovidDataInsertStatementToStringBuilder(oStringBuilder, oCovidData, pGeoZone, oQuery);
                oStringBuilder.Append(_COME);
            }
            oStringBuilder.Remove(oStringBuilder.Length - _ONE, _ONE);
            oQuery.query = oQuery.query.Replace(
                _REPLACE_QUERY_CONSTANT, oStringBuilder.ToString());
        }

        private void AddCovidDataInsertStatementToStringBuilder(
            StringBuilder oStringBuilder,
            CoVidData oCovidData,
            GeoZone pGeoZone,
            Query oQuery)
        {
            oStringBuilder.Append(
                    oQuery.valuesFormat.Replace(
                        _ZERO_STRING,
                        string.Join(
                            string.Empty,
                            _REPLACE_SINGLEQUOTE_CONSTANT,
                            pGeoZone.geoID,
                             _REPLACE_SINGLEQUOTE_CONSTANT)).Replace(
                                _ONE_STRING,
                                string.Join(
                                    string.Empty,
                                    _REPLACE_SINGLEQUOTE_CONSTANT,
                                    oCovidData.id.ToString(),
                                    _REPLACE_SINGLEQUOTE_CONSTANT)));
            oStringBuilder.Replace(
                    _TWO_STRING, Utils.UtilsJSON.GetInstance().Serialize(oCovidData));
        }

        public bool InsertDate(CovidDate pCovidDate)
        {
            Query oQuery;
            this.SetQuery(_INSERT_DATES_QUERY_PATH, out oQuery);
            this.SetDateQuery(pCovidDate, oQuery);

            return oConnectionPostgreSql.ExecuteCommand(oQuery.query);
        }

        public bool InsertDateList(List<CovidDate> pCovidDate)
        {
            Query oQuery;
            this.SetQuery(_INSERT_DATES_QUERY_PATH, out oQuery);
            this.SetDateListQuery(pCovidDate, oQuery);

            return oConnectionPostgreSql.ExecuteCommand(oQuery.query);
        }

        private void SetDateListQuery(List<CovidDate> pCovidDate, Query oQuery)
        {
            StringBuilder oStringBuilder = new StringBuilder();
            foreach (var oCovidDate in pCovidDate)
            {
                oStringBuilder.Append(
                    oQuery.valuesFormat.Replace(
                        _ZERO_STRING, oCovidDate.id.ToString()).Replace(
                            _ONE_STRING,
                            string.Join(
                                string.Empty,
                                _REPLACE_SINGLEQUOTE_CONSTANT,
                                oCovidDate.date,
                                _REPLACE_SINGLEQUOTE_CONSTANT)));
                oStringBuilder.Append(_COME);
            }
            oStringBuilder.Remove(oStringBuilder.Length - _ONE, _ONE);
            oQuery.query = oQuery.query.Replace(
                _REPLACE_QUERY_CONSTANT, oStringBuilder.ToString());
        }

        private void SetDateQuery(CovidDate pCovidDate, Query oQuery)
        {
            oQuery.valuesFormat = oQuery.valuesFormat.Replace(
                                    _ZERO_STRING, pCovidDate.id.ToString()).Replace(
                                                _ONE_STRING,
                                                string.Join(
                                                    string.Empty,
                                                    _REPLACE_SINGLEQUOTE_CONSTANT,
                                                    pCovidDate.date,
                                                    _REPLACE_SINGLEQUOTE_CONSTANT));

            oQuery.query = oQuery.query.Replace(
                _REPLACE_QUERY_CONSTANT, oQuery.valuesFormat);
        }

        public bool InsertGeoZone(GeoZone pGeoZone)
        {
            Query oQuery;
            this.SetQuery(_INSERT_GEOZONE_QUERY_PATH, out oQuery);
            this.SetGeoZoneQuery(pGeoZone, oQuery);

            return oConnectionPostgreSql.ExecuteCommand(oQuery.query);
        }

        public bool InsertGeoZoneList(List<GeoZone> pGeoZone)
        {
            Query oQuery;
            this.SetQuery(_INSERT_GEOZONE_QUERY_PATH, out oQuery);
            this.SetGeoZoneListQuery(pGeoZone, oQuery);

            return oConnectionPostgreSql.ExecuteCommand(oQuery.query);
        }

        public bool InsertGeoZoneCountry(GeoZone pGeoZone)
        {
            Query oQuery;
            this.SetQuery(_INSERT_GEOZONE_COUNTRIES_QUERY_PATH, out oQuery);
            this.SetGeoZoneCountryQuery(pGeoZone, oQuery);

            return oConnectionPostgreSql.ExecuteCommand(oQuery.query);
        }

        public bool InsertGeoZoneCountryList(List<GeoZone> pGeoZone)
        {
            Query oQuery;
            this.SetQuery(_INSERT_GEOZONE_COUNTRIES_QUERY_PATH, out oQuery);
            this.SetGeoZoneCountryListQuery(pGeoZone, oQuery);

            return oConnectionPostgreSql.ExecuteCommand(oQuery.query);
        }

        private void SetGeoZoneQuery(GeoZone pGeoZone, Query oQuery)
        {
            StringBuilder oStringBuilder = new StringBuilder();
            oStringBuilder.Append(
                oQuery.valuesFormat.Replace(
                    _ZERO_STRING,
                    string.Join(
                        string.Empty,
                        _REPLACE_SINGLEQUOTE_CONSTANT,
                        pGeoZone.geoID,
                        _REPLACE_SINGLEQUOTE_CONSTANT)));

            if (pGeoZone?.father?.geoID != null)
            {
                oStringBuilder.Replace(
                    _ONE_STRING,
                    string.Join(
                        string.Empty,
                        _REPLACE_SINGLEQUOTE_CONSTANT,
                        pGeoZone.father.geoID,
                        _REPLACE_SINGLEQUOTE_CONSTANT));
            }
            else
            {
                oStringBuilder.Replace(_ONE_STRING, _NULL);
            }
            if (pGeoZone?.sonList.Count >= _ONE)
            {
                oStringBuilder.Replace(_ONE_STRING, "true");
            }
            else
            {
                oStringBuilder.Replace(_ONE_STRING, _NULL);
            }

            oQuery.query = oQuery.query.Replace(
                _REPLACE_QUERY_CONSTANT, oStringBuilder.ToString());
        }

        private void SetGeoZoneListQuery(List<GeoZone> pGeoZone, Query oQuery)
        {
            StringBuilder oStringBuilder = new StringBuilder();

            foreach (var oGeoZone in pGeoZone)
            {
                this.AddGeoZoneInsertStatementToStringBuilder(oStringBuilder, oGeoZone, oQuery);

                oStringBuilder.Append(_COME);
            }

            oStringBuilder.Remove(oStringBuilder.Length - _ONE, _ONE);
            oQuery.query = oQuery.query.Replace(
                _REPLACE_QUERY_CONSTANT, oStringBuilder.ToString());
        }

        private void AddGeoZoneInsertStatementToStringBuilder(StringBuilder pStringBuilder, GeoZone pGeoZone, Query oQuery)
        {
            StringBuilder oStringBuilder = new StringBuilder();
            oStringBuilder.Append(
                oQuery.valuesFormat.Replace(
                        _ZERO_STRING,
                        string.Join(
                            string.Empty,
                            _REPLACE_SINGLEQUOTE_CONSTANT,
                            pGeoZone.geoID,
                             _REPLACE_SINGLEQUOTE_CONSTANT)));
            if (pGeoZone?.father?.geoID != null)
            {
                oStringBuilder.Replace(_ONE_STRING,
                    string.Join(
                            string.Empty,
                            _REPLACE_SINGLEQUOTE_CONSTANT,
                            pGeoZone.geoID,
                             _REPLACE_SINGLEQUOTE_CONSTANT));
            }
            else
            {
                oStringBuilder.Replace(
                    _ONE_STRING, _NULL);

            }
            if (pGeoZone?.sonList?.Count >= _ONE)
            {
                oStringBuilder.Replace(_TWO_STRING, "true");
            }
            else
            {
                oStringBuilder.Replace(
                    _TWO_STRING, "false");
            }
            pStringBuilder.Append(oStringBuilder.ToString());
        }

        private void SetGeoZoneCountryQuery(GeoZone pGeoZone, Query oQuery)
        {
            StringBuilder oStringBuilder = new StringBuilder();

            this.AddGeoZoneCountryInsertStatementToStringBuilder(oStringBuilder, pGeoZone, oQuery);

            oQuery.query = oQuery.query.Replace(
                _REPLACE_QUERY_CONSTANT, oStringBuilder.ToString());
        }

        private void AddGeoZoneCountryInsertStatementToStringBuilder(StringBuilder pStringBuilder, GeoZone pGeoZone, Query oQuery)
        {
            StringBuilder oStringBuilder = new StringBuilder();
            oStringBuilder.Append(oQuery.valuesFormat);
            oStringBuilder.Replace(
                _ZERO_STRING,
                string.Join(
                        string.Empty,
                        _REPLACE_SINGLEQUOTE_CONSTANT,
                        pGeoZone.geoID,
                        _REPLACE_SINGLEQUOTE_CONSTANT));
            oStringBuilder.Replace(
                _ONE_STRING,
                string.Join(
                        string.Empty,
                        _REPLACE_SINGLEQUOTE_CONSTANT,
                        pGeoZone.code,
                        _REPLACE_SINGLEQUOTE_CONSTANT));
            oStringBuilder.Replace(
                _TWO_STRING,
                string.Join(
                        string.Empty,
                        _REPLACE_SINGLEQUOTE_CONSTANT,
                        pGeoZone.name,
                        _REPLACE_SINGLEQUOTE_CONSTANT));
            oStringBuilder.Replace(
                _THREE_STRING,
                pGeoZone.population.ToString());
            pStringBuilder.Append(oStringBuilder.ToString());
        }

        private void SetGeoZoneCountryListQuery(List<GeoZone> pGeoZone, Query oQuery)
        {
            StringBuilder oStringBuilder = new StringBuilder();

            foreach (var oGeoZone in pGeoZone)
            {
                this.AddGeoZoneCountryInsertStatementToStringBuilder(oStringBuilder, oGeoZone, oQuery);
                oStringBuilder.Append(_COME);
            }

            oStringBuilder.Remove(oStringBuilder.Length - _ONE, _ONE);
            oQuery.query = oQuery.query.Replace(
                _REPLACE_QUERY_CONSTANT, oStringBuilder.ToString());
        }

        public void SetQuery(string pPath, out Query pQuery)
        {
            string query = Utils.UtilsStreamReaders.GetInstance().ReadStreamFile(pPath);
            Utils.UtilsJSON.GetInstance().DeserializeFromString(out pQuery, query);
        }
    }
}