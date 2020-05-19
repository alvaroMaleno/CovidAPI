namespace CoVid.Models.InputModels
{
    public class User
    {
        public string id{get;set;}
        public string pass{get;set;}

        public User(string pId, string pPass)
        {
            this.id = pId;
            this.pass = pPass;
        }
    }
}