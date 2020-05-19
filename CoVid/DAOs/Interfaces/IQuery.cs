
using CoVid.Models.QueryModels;

namespace CoVid.DAOs.Interfaces
{
    public interface IQuery
    {
        public void SetQuery(string pPath, out Query pQuery);
    }
}