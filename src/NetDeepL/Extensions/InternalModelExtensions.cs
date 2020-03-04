using NetDeepL.Models;
using NetDeepL.Models.Internal;
using System;

namespace NetDeepL.Extensions {
    internal static class InternalModelExtensions {

        // source https://stackoverflow.com/a/16104/6454517
        internal static T ParseEnum<T>(this string value) {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        internal static TranslationReponse ToReponse(this InternalTranslationReponse response) {
            return new TranslationReponse() {
                DetectedSourceLanguage = response.detected_source_language.ParseEnum<Languages>(),
                Text = response.text
            };
        }

        internal static Usage ToResponse(this InternalUsage usage) {
            return new Usage() {
                CharacterCount = usage.character_count,
                CharacterLimit = usage.character_limit,
            };
        }
    }
}
