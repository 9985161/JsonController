using System.Net;

namespace jsonpg01.Models
{
    // This will pull down our json string then we can use it in a list
    public class MyJSON
    {
        public static string RecieveJSON(string url)
        {
            using(WebClient wc = new WebClient())
            {
                string json = wc.DownloadString(url);
                return json;
            }
        }
    }
}