
using Security_REST.Models.QueryModels;

namespace Security_REST.DAOs.Interfaces
{
    public interface IQuery
    {
        public void SetQuery(string pPath, out Query pQuery);
    }
}