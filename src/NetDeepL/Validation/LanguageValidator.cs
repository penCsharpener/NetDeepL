using NetDeepL.Models;

namespace NetDeepL.Validation
{
    public static class SourceLanguage
    {
        public static bool IsValid(Languages lang)
        {
            return lang switch
            {
                Languages.EN_GB => false,
                Languages.EN_US => false,
                Languages.PT_BR => false,
                Languages.PT_PT => false,
                _ => true
            };
        }
    }

    public static class TargetLanguage
    {
        public static bool IsValid(Languages lang)
        {
            return lang switch
            {
                Languages.EN => false,
                Languages.PT => false,
                Languages.Undefined => false,
                _ => true
            };
        }
    }
}
