using Security_REST.Models.DataModels;

namespace Security_REST.Security.SecurityManager.Interfaces
{
    public interface IUser
    {
        public bool ValidateUser(User pUser);
        public void AddUser(User pUser);
    }
}