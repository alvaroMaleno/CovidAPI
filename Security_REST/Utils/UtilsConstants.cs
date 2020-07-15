using System.Runtime.CompilerServices;

namespace Security_REST.Utils
{
    public class UtilsConstants
    {
        public static readonly string _ZERO_QUERY_STRING = "{0}";
        public static readonly string _ZERO_STRING = "0";
        public static readonly string _ONE_QUERY_STRING = "{1}";
        public static readonly string _ONE_STRING = "1";
        public static readonly string _TWO_STRING = "2";
        public static readonly string _TABLE_NAME = "table_name";
        public static readonly string _COLUMN_NAME = "column_name";
        public static readonly string _COME = ",";

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