using CoVid.Models.QueryModels;

namespace CoVid.DAOs.Abstracts
{
    public abstract class DAO
    {
        public abstract bool CreateTable(Query pQuery);
        public abstract bool CreateTable(string pPath);
        public abstract bool CreateNamedTable(string pPath, params string[] pTableName);
    }
}