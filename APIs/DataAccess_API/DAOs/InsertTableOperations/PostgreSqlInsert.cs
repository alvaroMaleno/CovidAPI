using System.Linq;
using System.Text;
using System.Collections.Generic;
using CoVid.Controllers.DAOs;
using CoVid.Controllers.DAOs.Connection;
using CoVid.Models;
using CoVid.Models.QueryModels;
using CoVid.Models.PathModels;
using CoVid.Utils;
using Covid_REST.Utils;

namespace CoVid.DAOs.InsertTableOperations
{
    public class PostgreSqlInsert : ICovidDataBaseInsert
    {
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
            this._INSERT_DATES_QUERY_PATH = oPathsArray.oPaths[UtilsConstants.IntConstants.ZERO];
            this._INSERT_GEOZONE_COUNTRIES_QUERY_PATH = oPathsArray.oPaths[UtilsConstants.IntConstants.THREE];
            this._INSERT_GEOZONE_NAME_QUERY_PATH = oPathsArray.oPaths[UtilsConstants.IntConstants.ONE];
            this._INSERT_GEOZONE_QUERY_PATH = oPathsArray.oPaths[UtilsConstants.IntConstants.TWO];
        }


        public bool InsertCovidData(CoVidData pCovidData, GeoZone pGeoZone)
        {
            Query oQuery;
            this.SetQuery(_INSERT_DATES_QUERY_PATH, out oQuery);
            oQuery.query = oQuery.query.Replace(UtilsConstants.QueryConstants.COUNTRY_NAME, pGeoZone.name);
            this.SetCovidDataQuery(pCovidData, pGeoZone, oQuery);

            return oConnectionPostgreSql.ExecuteCommand(oQuery.query);
        }

        public bool InsertCovidDataList(List<CoVidData> pCovidData, GeoZone pGeoZone)
        {
            Query oQuery;
            this.SetQuery(_INSERT_GEOZONE_NAME_QUERY_PATH, out oQuery);
            oQuery.query = oQuery.query.Replace(UtilsConstants.QueryConstants.COUNTRY_NAME, pGeoZone.geoID);
            this.SetCovidDataListQuery(pCovidData, pGeoZone, oQuery);

            return oConnectionPostgreSql.ExecuteCommand(oQuery.query);
        }

        private void SetCovidDataQuery(CoVidData pCovidData, GeoZone pGeoZone, Query oQuery)
        {
            StringBuilder oStringBuilder = new StringBuilder();
            this.AddCovidDataInsertStatementToStringBuilder(oStringBuilder, pCovidData, pGeoZone, oQuery);
            oQuery.query = oQuery.query.Replace(
                UtilsConstants.QueryConstants.REPLACE_QUERY_CONSTANT, oStringBuilder.ToString());
        }

        private void SetCovidDataListQuery(List<CoVidData> pCovidData, GeoZone pGeoZone, Query oQuery)
        {
            StringBuilder oStringBuilder = new StringBuilder();
            foreach (var oCovidData in pCovidData)
            {
                this.AddCovidDataInsertStatementToStringBuilder(oStringBuilder, oCovidData, pGeoZone, oQuery);
                oStringBuilder.Append(UtilsConstants.StringConstants.COME);
            }
            oStringBuilder.Remove(oStringBuilder.Length - UtilsConstants.IntConstants.ONE, UtilsConstants.IntConstants.ONE);
            oQuery.query = oQuery.query.Replace(
                UtilsConstants.QueryConstants.REPLACE_QUERY_CONSTANT, oStringBuilder.ToString());
        }

        private void AddCovidDataInsertStatementToStringBuilder(
            StringBuilder oStringBuilder,
            CoVidData oCovidData,
            GeoZone pGeoZone,
            Query oQuery)
        {
            oStringBuilder.Append(
                    oQuery.valuesFormat.Replace(
                                UtilsConstants.QueryConstants.ZERO_STRING,
                                string.Join(
                                    string.Empty,
                                    UtilsConstants.StringConstants.REPLACE_SINGLEQUOTE_CONSTANT,
                                    oCovidData.id.ToString(),
                                    UtilsConstants.StringConstants.REPLACE_SINGLEQUOTE_CONSTANT)));
            oStringBuilder.Replace(
                    UtilsConstants.QueryConstants.ONE_STRING,
                    string.Join(
                        string.Empty,
                        UtilsConstants.StringConstants.REPLACE_SINGLEQUOTE_CONSTANT,
                        Utils.UtilsJSON.GetInstance().Serialize(oCovidData),
                        UtilsConstants.StringConstants.REPLACE_SINGLEQUOTE_CONSTANT));
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
                        UtilsConstants.QueryConstants.ZERO_STRING, oCovidDate.id.ToString()).Replace(
                            UtilsConstants.QueryConstants.ONE_STRING,
                            string.Join(
                                string.Empty,
                                UtilsConstants.StringConstants.REPLACE_SINGLEQUOTE_CONSTANT,
                                oCovidDate.date,
                                UtilsConstants.StringConstants.REPLACE_SINGLEQUOTE_CONSTANT)));
                oStringBuilder.Append(UtilsConstants.StringConstants.COME);
            }
            oStringBuilder.Remove(oStringBuilder.Length - UtilsConstants.IntConstants.ONE, UtilsConstants.IntConstants.ONE);
            oQuery.query = oQuery.query.Replace(
                UtilsConstants.QueryConstants.REPLACE_QUERY_CONSTANT, oStringBuilder.ToString());
        }

        private void SetDateQuery(CovidDate pCovidDate, Query oQuery)
        {
            oQuery.valuesFormat = oQuery.valuesFormat.Replace(
                                    UtilsConstants.QueryConstants.ZERO_STRING, pCovidDate.id.ToString()).Replace(
                                                UtilsConstants.QueryConstants.ONE_STRING,
                                                string.Join(
                                                    string.Empty,
                                                    UtilsConstants.StringConstants.REPLACE_SINGLEQUOTE_CONSTANT,
                                                    pCovidDate.date,
                                                    UtilsConstants.StringConstants.REPLACE_SINGLEQUOTE_CONSTANT));

            oQuery.query = oQuery.query.Replace(
                UtilsConstants.QueryConstants.REPLACE_QUERY_CONSTANT, oQuery.valuesFormat);
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
                    UtilsConstants.QueryConstants.ZERO_STRING,
                    string.Join(
                        string.Empty,
                        UtilsConstants.StringConstants.REPLACE_SINGLEQUOTE_CONSTANT,
                        pGeoZone.geoID,
                        UtilsConstants.StringConstants.REPLACE_SINGLEQUOTE_CONSTANT)));

            if (pGeoZone?.father?.geoID != null)
            {
                oStringBuilder.Replace(
                    UtilsConstants.QueryConstants.ONE_STRING,
                    string.Join(
                        string.Empty,
                        UtilsConstants.StringConstants.REPLACE_SINGLEQUOTE_CONSTANT,
                        pGeoZone.father.geoID,
                        UtilsConstants.StringConstants.REPLACE_SINGLEQUOTE_CONSTANT));
            }
            else
            {
                oStringBuilder.Replace(UtilsConstants.QueryConstants.ONE_STRING, UtilsConstants.StringConstants.NULL);
            }

            if (pGeoZone?.sonList.Count >= UtilsConstants.IntConstants.ONE)
                oStringBuilder.Replace(UtilsConstants.QueryConstants.ONE_STRING, UtilsConstants.StringConstants.TRUE);
            else
                oStringBuilder.Replace(UtilsConstants.QueryConstants.ONE_STRING, UtilsConstants.StringConstants.NULL);

            oQuery.query = oQuery.query.Replace(
                UtilsConstants.QueryConstants.REPLACE_QUERY_CONSTANT, oStringBuilder.ToString());
        }

        private void SetGeoZoneListQuery(List<GeoZone> pGeoZone, Query oQuery)
        {
            StringBuilder oStringBuilder = new StringBuilder();

            foreach (var oGeoZone in pGeoZone)
            {
                this.AddGeoZoneInsertStatementToStringBuilder(oStringBuilder, oGeoZone, oQuery);

                oStringBuilder.Append(UtilsConstants.StringConstants.COME);
            }

            oStringBuilder.Remove(oStringBuilder.Length - UtilsConstants.IntConstants.ONE, UtilsConstants.IntConstants.ONE);
            oQuery.query = oQuery.query.Replace(
                UtilsConstants.QueryConstants.REPLACE_QUERY_CONSTANT, oStringBuilder.ToString());
        }

        private void AddGeoZoneInsertStatementToStringBuilder(StringBuilder pStringBuilder, GeoZone pGeoZone, Query oQuery)
        {
            StringBuilder oStringBuilder = new StringBuilder();
            oStringBuilder.Append(
                oQuery.valuesFormat.Replace(
                        UtilsConstants.QueryConstants.ZERO_STRING,
                        string.Join(
                            string.Empty,
                            UtilsConstants.StringConstants.REPLACE_SINGLEQUOTE_CONSTANT,
                            pGeoZone.geoID,
                             UtilsConstants.StringConstants.REPLACE_SINGLEQUOTE_CONSTANT)));
            if (pGeoZone?.father?.geoID != null)
            {
                oStringBuilder.Replace(UtilsConstants.QueryConstants.ONE_STRING,
                    string.Join(
                            string.Empty,
                            UtilsConstants.StringConstants.REPLACE_SINGLEQUOTE_CONSTANT,
                            pGeoZone.geoID,
                             UtilsConstants.StringConstants.REPLACE_SINGLEQUOTE_CONSTANT));
            }
            else
            {
                oStringBuilder.Replace(
                    UtilsConstants.QueryConstants.ONE_STRING, UtilsConstants.StringConstants.NULL);

            }
            
            if (pGeoZone?.sonList?.Count >= UtilsConstants.IntConstants.ONE)
                oStringBuilder.Replace(UtilsConstants.QueryConstants.TWO_STRING, UtilsConstants.StringConstants.TRUE);
            else
                oStringBuilder.Replace(
                    UtilsConstants.QueryConstants.TWO_STRING, UtilsConstants.StringConstants.FALSE);
            
            pStringBuilder.Append(oStringBuilder.ToString());
        }

        private void SetGeoZoneCountryQuery(GeoZone pGeoZone, Query oQuery)
        {
            StringBuilder oStringBuilder = new StringBuilder();

            this.AddGeoZoneCountryInsertStatementToStringBuilder(oStringBuilder, pGeoZone, oQuery);

            oQuery.query = oQuery.query.Replace(
                UtilsConstants.QueryConstants.REPLACE_QUERY_CONSTANT, oStringBuilder.ToString());
        }

        private void AddGeoZoneCountryInsertStatementToStringBuilder(StringBuilder pStringBuilder, GeoZone pGeoZone, Query oQuery)
        {
            if(pGeoZone.geoID.Length > 2 || pGeoZone?.population == null){
                return;
            }
            StringBuilder oStringBuilder = new StringBuilder();
            oStringBuilder.Append(oQuery.valuesFormat);
            oStringBuilder.Replace(
                UtilsConstants.QueryConstants.ZERO_STRING,
                string.Join(
                        string.Empty,
                        UtilsConstants.StringConstants.REPLACE_SINGLEQUOTE_CONSTANT,
                        pGeoZone.geoID,
                        UtilsConstants.StringConstants.REPLACE_SINGLEQUOTE_CONSTANT));
            oStringBuilder.Replace(
                UtilsConstants.QueryConstants.ONE_STRING,
                string.Join(
                        string.Empty,
                        UtilsConstants.StringConstants.REPLACE_SINGLEQUOTE_CONSTANT,
                        pGeoZone.code,
                        UtilsConstants.StringConstants.REPLACE_SINGLEQUOTE_CONSTANT));
            oStringBuilder.Replace(
                UtilsConstants.QueryConstants.TWO_STRING,
                string.Join(
                        string.Empty,
                        UtilsConstants.StringConstants.REPLACE_SINGLEQUOTE_CONSTANT,
                        pGeoZone.name,
                        UtilsConstants.StringConstants.REPLACE_SINGLEQUOTE_CONSTANT));
            oStringBuilder.Replace(
                UtilsConstants.QueryConstants.THREE_STRING,
                pGeoZone.population.ToString());
            pStringBuilder.Append(oStringBuilder.ToString());
        }

        private void SetGeoZoneCountryListQuery(List<GeoZone> pGeoZone, Query oQuery)
        {
            StringBuilder oStringBuilder = new StringBuilder();

            foreach (var oGeoZone in pGeoZone)
            {
                this.AddGeoZoneCountryInsertStatementToStringBuilder(oStringBuilder, oGeoZone, oQuery);
                oStringBuilder.Append(UtilsConstants.StringConstants.COME);
            }

            oStringBuilder.Remove(oStringBuilder.Length - UtilsConstants.IntConstants.ONE, UtilsConstants.IntConstants.ONE);
            oQuery.query = oQuery.query.Replace(
                UtilsConstants.QueryConstants.REPLACE_QUERY_CONSTANT, oStringBuilder.ToString()).Replace(",,", (","));
        }

        public void SetQuery(string pPath, out Query pQuery)
        {
            string query = Utils.UtilsStreamReaders.GetInstance().ReadStreamFile(pPath);
            Utils.UtilsJSON.GetInstance().DeserializeFromString(out pQuery, query);
        }
    }
}