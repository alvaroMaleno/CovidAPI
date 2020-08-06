using System.IO;
using System.Runtime.CompilerServices;

namespace Security_REST.Utils
{
    public class UtilsStreamReaders
    {
        private static UtilsStreamReaders _instance;

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static UtilsStreamReaders GetInstance(){
            if(_instance is null){
                _instance = new UtilsStreamReaders();
            }
            return _instance;
        }
        private UtilsStreamReaders(){

        }

        public string ReadStreamFile(string pPath){
            string file;
            using (StreamReader oJsonStream = File.OpenText(pPath))
            {
                file = oJsonStream.ReadToEnd();
            }
            return file;
        }

    }
}