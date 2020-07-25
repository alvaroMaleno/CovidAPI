using System;
using System.Runtime.CompilerServices;

namespace Security_REST.Utils
{
    public class UtilsConstants
    {
        internal static readonly string _ZERO_QUERY_STRING = "{0}";
        internal static readonly string _ZERO_STRING = "0";
        internal static readonly string _ONE_QUERY_STRING = "{1}";
        internal static readonly string _ONE_STRING = "1";
        internal static readonly string _TWO_QUERY_STRING = "{2}";
        internal static readonly string _TWO_STRING = "2";
        internal static readonly string _THREE_STRING = "3";
        internal static readonly string _TABLE_NAME = "table_name";
        internal static readonly string _COLUMN_NAME = "column_name";
        internal static readonly string _COME = ",";
        internal static readonly string _ENCRYPT_SPLIT = "/";
        internal static readonly string _INTERROGANT = "?";
        internal static readonly string _PLEASE_ENCRYPT_ERROR = "Please, encrypt data before send it.";
        internal static readonly int _ZERO = 0;
        internal static readonly int _ONE = 1;
        internal static readonly int _TWO = 2;
        internal static readonly int _THREE = 3;
        internal static readonly int _FOUR = 4;

        private static UtilsConstants _instance;

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static UtilsConstants GetInstance(){
            if(_instance is null){
                _instance = new UtilsConstants();
            }
            return _instance;
        }
        private UtilsConstants()
        {

        }
    }
}