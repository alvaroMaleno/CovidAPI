using System.IO;
using System.Runtime.CompilerServices;

namespace CoVid.Utils
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
            using (StreamReader jsonStream = File.OpenText(pPath))
            {
                file = jsonStream.ReadToEnd();
            }
            return file;
        }

    }
}