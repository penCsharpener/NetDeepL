using NetDeepL.Abstractions;

namespace NetDeepL.Implementations
{
    public partial class NetDeepL : INetDeepL
    {
        internal static DependencyInjection _dep;

        public static INetDeepL CreateClient(string apiKey, NetDeepLOptions options)
        {
            _dep ??= new DependencyInjection();
            return _dep.GetClient(apiKey, options);
        }
    }
}
