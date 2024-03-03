using System.Text;

namespace UrlShortenerWeb.Helpers
{
    /**
     * ShortURL: Bijective conversion between natural numbers (IDs) and short strings
     *
     * ShortURL.GenerateUniqueCode() return random number of NumberOfCharsInShortUrl chars
     * 
     * Features:
     * + large alphabet (51 chars) and thus very short resulting strings
     */
    public class ShortUrlHelper
    {
        private const string Alphabet = "23456789bcdfghjkmnpqrstvwxyzBCDFGHJKLMNPQRSTVWXYZ-_";
        private const int NumberOfCharsInShortUrl = 8;
        private static Random _random = new Random();

        public static string GenerateUniqueCode()
        {
            var codeChars = new char[NumberOfCharsInShortUrl];
            while (true) 
            {
                for (int i = 0; i < NumberOfCharsInShortUrl; i++)
                {
                    var rendomIndex = _random.Next(Alphabet.Length-1);
                    codeChars[i] = Alphabet[rendomIndex];
                }
                var code = new string(codeChars);
                return code;
            }
        }
    }
}
