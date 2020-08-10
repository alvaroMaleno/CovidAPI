using System.Runtime.CompilerServices;

namespace Security_REST.Utils
{
    public class UtilsStreamWritters
    {
        private static UtilsStreamWritters _instance;

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static UtilsStreamWritters GetInstance(){
            if(_instance is null){
                _instance = new UtilsStreamWritters();
            }
            return _instance;
        }

        private UtilsStreamWritters()
        {
        }

        public void WritteStringToFile(string pStringToWritte, string pPath)
        {
            System.IO.File.WriteAllText(pPath, pStringToWritte);
        }
    }
}