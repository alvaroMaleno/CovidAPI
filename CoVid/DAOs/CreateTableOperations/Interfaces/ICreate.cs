
using CoVid.DAOs.Interfaces;
using CoVid.Models.QueryModels;

namespace CoVid.Controllers.DAOs.CreateTableOperations
{
    public interface ICreate<in R> : IQuery
    {
        public bool CreateTable(R pConnector, Query pQuery);
        public bool CreateTable(R pConnector, string pPath);
        public bool CreateNamedDataTable(R pConnector, string pPath, params string[] pTableName);

    }
}