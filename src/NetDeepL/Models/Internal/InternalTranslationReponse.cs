namespace NetDeepL.Models.Internal
{
    public class InternalTranslationReponse
    {
        public TranslationElement[] translations { get; set; }
    }

    public class TranslationElement
    {
        public string detected_source_language { get; set; }
        public string text { get; set; }
    }
}
