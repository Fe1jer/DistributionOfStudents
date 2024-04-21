using System.Text.RegularExpressions;

namespace Shared.Helpers
{
    public static class TransliterateHelper
    {
        private static readonly Dictionary<string, string> Dictionary = new Dictionary<string, string>()
        {
            {"а", "a"},
            {"б", "b"},
            {"в", "v"},
            {"г", "g"},
            {"д", "d"},
            {"е", "e"},
            {"ё", "e"},
            {"ж", "zh"},
            {"з", "z"},
            {"и", "i"},
            {"й", "i"},
            {"к", "k"},
            {"л", "l"},
            {"м", "m"},
            {"н", "n"},
            {"о", "o"},
            {"п", "p"},
            {"р", "r"},
            {"с", "s"},
            {"т", "t"},
            {"у", "u"},
            {"ф", "f"},
            {"х", "kh"},
            {"ц", "tc"},
            {"ч", "ch"},
            {"ш", "sh"},
            {"щ", "shch"},
            {"ъ", ""},
            {"ы", "y"},
            {"ь", ""},
            {"э", "e"},
            {"ю", "iu"},
            {"я", "ia"},
            {"ў", "u"},
            {"і", "i"},
            {".", "-"},
            {",", "-"},
            {":", "-"},
            {";", "-"},
            {" ", "-"}
        };
        public static string TransliterateString(string value)
        {
            value = Dictionary.Aggregate(value.ToLower(), (current, pair) => current.Replace(pair.Key, pair.Value));
            return Regex.Replace(value, @"[^a-z0-9\s-]", "");
        }
    }
}
