using NetDeepL.Models;
using NetDeepL.Models.Internal;
using System;

namespace NetDeepL.Extensions {
    internal static class InternalModelExtensions {

        // source https://stackoverflow.com/a/16104/6454517
        internal static T ParseEnum<T>(this string value) {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        internal static TranslationReponse ToReponse(this TrReponse response) {
            return new TranslationReponse() {
                DetectedSourceLanguage = response.detected_source_language.ParseEnum<Languages>(),
                Text = response.text
            };
        }
    }
}
