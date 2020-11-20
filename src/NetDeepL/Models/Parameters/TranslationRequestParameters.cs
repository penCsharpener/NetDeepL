using System.Collections.Generic;

namespace NetDeepL.Models.Parameters
{
    public class TranslationRequestParameters
    {
        public Languages SourceLanguage { get; set; } = Languages.Undefined;
        public bool SplitSentences { get; set; } = true;
        public bool PreserveFormatting { get; set; }
        public Formality Formality { get; set; } = Formality.Default;
        public TagHandlingOptions TagHandling { get; set; } = TagHandlingOptions.None;
        public List<string> NonSplittingTags { get; set; } = new List<string>();
        public bool OutlineDetection { get; set; }
        public List<string> SplittingTags { get; set; } = new List<string>();
        public List<string> IgnoreTags { get; set; } = new List<string>();

        public TranslationRequestParameters AddNonSplittingTags(params string[] tags)
        {
            (NonSplittingTags ?? (NonSplittingTags = new List<string>())).AddRange(tags);
            return this;
        }

        public TranslationRequestParameters AddSplittingTags(params string[] tags)
        {
            (SplittingTags ?? (SplittingTags = new List<string>())).AddRange(tags);
            return this;
        }

        public TranslationRequestParameters AddIgnoreTags(params string[] tags)
        {
            (IgnoreTags ?? (IgnoreTags = new List<string>())).AddRange(tags);
            return this;
        }
    }
}
