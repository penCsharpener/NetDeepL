using System.Threading.Tasks;

namespace NetDeepL.TranslationWorker.Abstractions
{
    public interface IWorkbookTranslator
    {
        Task TranslateAsync();
    }
}
