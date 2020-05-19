namespace CoVid.Processes.PropertiesReader
{
    public interface IPropertiesReader<out R>
    {
        public void ReadProperties<R>(out R pPropertiesClass, string pPath);
    }
}