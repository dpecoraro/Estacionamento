namespace GreenVille.Portal
{
    public class AppConfiguration
    {
        public string API_URL_PRD { get; set; }

        public string API_URL_DEV { get; set; }

        public string GetApiURL()
        {
#if DEBUG
            return API_URL_DEV;
#else
            return API_URL_PRD;
#endif
        }
    }
}
