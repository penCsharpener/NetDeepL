using NetDeepL.Models.Internal;
using System.Threading.Tasks;

namespace NetDeepL.Abstractions {
    internal interface IInternalClient {
        Task<InternalUsage> GetUsage();
    }
}
