
using CoVid.DAOs.Interfaces;

namespace CoVid.Controllers.DAOs.CreateTableOperations
{
    public interface ICreate<in R> : IQuery
    {
        public bool CreateTable(R pConnector, string pPath);
        public bool CreateNamedDataTable(R pConnector, string pPath, string pTableName);

    }
}