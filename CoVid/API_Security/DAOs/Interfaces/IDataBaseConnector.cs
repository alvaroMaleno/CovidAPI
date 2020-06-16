namespace CoVid.Controllers.DAOs.Interfaces
{
    public interface IDataBaseConnector<out R>
    {
        public void Connect();
        public R GetConnection();
        public bool CloseConnection();
        
    }
}