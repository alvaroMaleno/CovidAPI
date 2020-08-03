using System;
namespace Covid_REST.Utils
{
    public struct UtilsConstants
    {
        public struct UrlConstants
        {
            public const string URL_TO_COMPLETE_DATA = "https://api.covid19api.com/country/";
            public const string EU_URL = "https://opendata.ecdc.europa.eu/covid19/casedistribution/json/";
        }

        public struct IntConstants
        {
            public const Int32 MS_IN_A_DAY = 60 * 60 * 24 * 1000;
            public const int ZERO = 0;
            public const int ONE = 1;
            public const int TWO = 2;
            public const int THREE = 3;
            public const int FOUR = 4;
            public const int HOURS_IN_A_DAY = 24;
        }

        public struct PararellConstants
        {
            public const int MAX_NUMBER_OF_TASKS = 300;
        }

        public struct StringConstants
        {
            public const string NULL = "null";
            public const string TRUE = "true";
            public const string FALSE = "false";
            public const string RIGHT_BAR = "/";
            public const string REPLACE_SINGLEQUOTE_CONSTANT = "'";
            public const string COME = ",";
        }

        public struct DateConstants
        {
            public const string API_DATE_FORMAT = "dd/MM/yyyy";
        }

        public struct QueryConstants
        {
            public const string ZERO_STRING = "{0}";
            public const string ONE_STRING = "{1}";
            public const string TWO_STRING = "{2}";
            public const string THREE_STRING = "{3}";
            public const string TABLE_NAME = "table_name";
            public const string COUNTRY_NAME = "country_name";
            public const string REPLACE_QUERY_CONSTANT = "?";
        }

    }
}