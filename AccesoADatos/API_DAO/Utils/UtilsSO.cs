namespace CoVid.Utils
{
    public class UtilsSO
    {
        private static UtilsSO _instance;

        private UtilsSO(){}

        public static UtilsSO GetInstance()
        {
            if(_instance is null)
            {
                _instance = new UtilsSO();
            }
            return _instance;
        }

        public string GetSO()
        {
            System.OperatingSystem osInfo = System.Environment.OSVersion; 
            return osInfo.Platform.ToString().ToLower();
        }
    }
}