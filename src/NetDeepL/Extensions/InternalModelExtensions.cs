using System;
using System.Linq;
using NetDeepL.Models;
using NetDeepL.Models.Internal;

namespace NetDeepL.Extensions
{
    internal static class InternalModelExtensions
    {

        // source https://stackoverflow.com/a/16104/6454517
        internal static T ParseEnum<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        internal static TranslationReponse ToReponse(this TranslationElement trElement)
        {
            return new TranslationReponse() {
                DetectedSourceLanguage = trElement.detected_source_language.ParseEnum<Languages>(),
                Text = trElement.text
            };
        }

        internal static TranslationReponse[] ToResponses(this InternalTranslationReponse response)
        {
            return response.translations.Select(x => x.ToReponse()).ToArray();
        }

        internal static Usage ToResponse(this InternalUsage usage)
        {
            return new Usage() {
                CharacterCount = usage.character_count,
                CharacterLimit = usage.character_limit,
            };
        }
    }
}
