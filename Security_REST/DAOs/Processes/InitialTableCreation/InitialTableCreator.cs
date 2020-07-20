using Security_REST.DAOs.Abstracts;
using Security_REST.Security.SecurityManager;

namespace Security_REST.DAOs.Processes.InitialTableCreation
{
    public class InitialTableCreator
    {
        private static InitialTableCreator _instance;
        private DAO _oDAO;
        private SecurityManager _oSecurityManager;

        private InitialTableCreator()
        {
            _oDAO = SecurityDAOPostgreImpl.GetInstance();
            _oSecurityManager = SecurityManager.GetInstance();
        }

        public static InitialTableCreator GetInstance(){
            if(_instance is null)
                _instance = new InitialTableCreator();
            return _instance;
        }

        public void InitialTableCreation()
        {
            _oSecurityManager.CreateSecurityTables();
        }
    }
}