namespace NetDeepL
{
    public class NetDeepLOptions
    {
        public NetDeepLOptions(int timeOut = 60000)
        {
            TimeOut = timeOut;
        }

        public int TimeOut { get; set; } = 60000;
    }
}
