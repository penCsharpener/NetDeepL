using System;
using System.Collections.Generic;
using System.Linq;
using NetDeepL.Models;
using NetDeepL.Models.Internal;
using NetDeepL.Models.Parameters;

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
            return new TranslationReponse()
            {
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
            return new Usage()
            {
                CharacterCount = usage.character_count,
                CharacterLimit = usage.character_limit,
            };
        }

        /// <summary>
        /// bool condition evaluates whether to make the option explicit.
        /// The default will not be made explicite.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dict"></param>
        /// <param name="condition"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        internal static IList<KeyValuePair<string, string>> AddIf(this IList<KeyValuePair<string, string>> dict, bool condition, string key, string value)
        {
            if (condition)
            {
                dict.Add(key, value);
            }

            return dict;
        }

        internal static IList<KeyValuePair<string, string>> AddIf(this IList<KeyValuePair<string, string>> dict, bool condition, string key, IList<string> values)
        {
            if (condition)
            {
                dict.Add(key, string.Join(",", values));
            }

            return dict;
        }

        internal static IList<KeyValuePair<string, string>> Add(this IList<KeyValuePair<string, string>> dict, string key, string value)
        {
            dict.Add(new KeyValuePair<string, string>(key, value));
            return dict;
        }

        internal static IList<KeyValuePair<string, string>> AddOptionalParameters(this IList<KeyValuePair<string, string>> dict, TranslationRequestParameters parameters)
        {
            dict.AddIf(parameters.SourceLanguage != Languages.Undefined, TranslationParameterNames.SOURCE_LANG, parameters.SourceLanguage.ToString());
            dict.AddIf(!parameters.SplitSentences, TranslationParameterNames.SPLITTING_TAGS, parameters.SplitSentences ? "1" : "0");
            dict.AddIf(parameters.PreserveFormatting, TranslationParameterNames.PRESERVE_FORMATTING, parameters.PreserveFormatting ? "1" : "0");
            dict.AddIf(parameters.TagHandling != TagHandlingOptions.None, TranslationParameterNames.TAG_HANDLING, parameters.TagHandling.ToString());
            dict.AddIf(parameters.NonSplittingTags?.Count > 0, TranslationParameterNames.NON_SPLITTING_TAGS, parameters.NonSplittingTags);
            if (parameters.TagHandling == TagHandlingOptions.Xml)
            {
                dict.AddIf(!parameters.OutlineDetection, TranslationParameterNames.OUTLINE_DETECTION, parameters.OutlineDetection ? "1" : "0");
            }
            dict.AddIf(parameters.SplittingTags?.Count > 0, TranslationParameterNames.SPLITTING_TAGS, parameters.SplittingTags);
            dict.AddIf(parameters.IgnoreTags?.Count > 0, TranslationParameterNames.IGNORE_TAGS, parameters.IgnoreTags);

            return dict;
        }

        // https://stackoverflow.com/a/4171296/6454517
        internal static IEnumerable<Enum> GetFlags(Enum input)
        {
            foreach (Enum value in Enum.GetValues(input.GetType()))
            {
                if (input.HasFlag(value))
                {
                    yield return value;
                }
            }
        }


    }
}
