namespace Security_REST.Security.SecurityManager.Interfaces
{
    public interface IUser
    {
        public bool ValidateUser(object pUser);
        public void AddUser(object pUser);
    }
}