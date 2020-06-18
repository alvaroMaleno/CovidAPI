using CoVid.Utils;
using CoVid.Controllers.DAOs.Connection;

namespace CoVid.Processes.PropertiesReader
{
    public class PostgreSqlPropertiesReader : IPropertiesReader<ConnectionPostgreProperties>
    {
        public void ReadProperties<ConnectionPostgreProperties>(out ConnectionPostgreProperties pProperties, string pPath)
        {
            string file = UtilsStreamReaders.GetInstance().ReadStreamFile(pPath);
            UtilsJSON.GetInstance().DeserializeFromString(out pProperties, file);
        }
    }
}