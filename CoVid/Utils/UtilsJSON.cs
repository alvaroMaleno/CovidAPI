using System.Runtime.CompilerServices;
using System.Text.Json;

namespace CoVid.Utils
{
    public class UtilsJSON
    {
        private static UtilsJSON _instance;

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static UtilsJSON GetInstance(){
            if(_instance is null){
                _instance = new UtilsJSON();
            }
            return _instance;
        }
        private UtilsJSON(){

        }

        public void DeserializeFromString<R>(out R pTargetClass, string pStringToDeserialize){
            pTargetClass = JsonSerializer.Deserialize<R>(pStringToDeserialize);
        }
        public string Serialize<R>(R pObjectToSerialize)
        {
            return JsonSerializer.Serialize<R>(pObjectToSerialize);
        }
    }
}