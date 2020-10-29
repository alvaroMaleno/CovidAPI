using System;
namespace Covid_REST.Utils
{
    public struct UtilsConstants
    {
        public struct UrlConstants
        {
            public const string URL_DATA_REST = "https://covidpaucasesnoves.azurewebsites.net/CovidDataBase";
            public const string URL_SECURITY_REST = "https://security-api.azurewebsites.net/Security";
        }

        public struct POSTMethodsConstants
        {
            public const string GET_ALL_GEO_ZONE_FOR_ALL_DATES = "GetAllGeoZoneDataForAllDates";
            public const string GET_ALL_DATES = "GetAllDates";
            public const string GET_ALL_COUNTRIES = "GetAllCountries";
            public const string GET_GEO_ZONE_DATA = "GetGeoZoneData";
        }

        public struct IntConstants
        {
            public const Int32 MS_IN_A_DAY = 60 * 60 * 24 * 1000;
            public const int ZERO = 0;
            public const int ONE = 1;
            public const int HOURS_IN_A_DAY = 24;
        }

        public struct StringConstants
        {
            public const string TRUE = "true";
            public const string RIGHT_BAR = "/";
        }

    }
}