
using Security_REST.DAOs.Interfaces;
using Security_REST.Models.QueryModels;

namespace Security_REST.Controllers.DAOs.CreateTableOperations
{
    public interface ICreate<in R> : IQuery
    {
        public bool CreateTable(R pConnector, Query pQuery);
        public bool CreateTable(R pConnector, string pPath);
        public bool CreateNamedDataTable(R pConnector, string pPath, params string[] pTableName);

    }
}