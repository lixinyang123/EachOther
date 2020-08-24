using System.Net.Http;

namespace EachOther.Services
{
    public class NotifyService
    {
        public void PushNotify(string sckey, string text, string desp)
        {
            string url = $" https://sc.ftqq.com/{sckey}.send?text={text}&desp={desp}";

            new HttpClient().GetAsync(url);
        }
    }
}