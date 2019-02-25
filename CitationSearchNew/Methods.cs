using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CitationSearchNew
{
    public static class Methods
    {
        // Returns a list of mishnas in the masechect
        public static List<string> ExtractMishnas(string masechet)
        {
            List<string> mishnas = new List<string>();

            // Count the amount of mishnas in the masechet
            MatchCollection mColl = Regex.Matches(masechet, "מתני׳");
            int mCount = mColl.Count;

            // Capture each mishna
            for (int i = 0; i < mCount; i++)
            {
                Match match = mColl[i];
                StringBuilder sBuilder = new StringBuilder();
                char record = masechet[match.Index]; // Start position of the mishna in the masechet

                int j = 1;
                while (record != ':') // An ':' signifies the end of a mishna
                {
                    sBuilder.Append(record); // Append character to string
                    record = masechet[match.Index + j]; // Move to next character
                    j++;
                }
                mishnas.Add(sBuilder.ToString().Replace("מתני׳", "")); // Remove the mishna word
            }

            return mishnas;
        }


        // Returns a list of gemaras in the masechect
        public static List<string> ExtractGemaras(string masechet)
        {
            List<string> gemaras = new List<string>();

            // Count the amount of gemaras in the masechet
            MatchCollection gColl = Regex.Matches(masechet, "גמ׳");
            int gCount = gColl.Count;

            for (int i = 0; i < gCount; i++)
            {
                Match match = gColl[i];
                StringBuilder sBuilder = new StringBuilder();
                char record = masechet[match.Index]; // Similar to extracting mishnas

                int j = 1;
                while (match.Index + j < masechet.Length) // Keep going until the break condition is 
                                                         //     reached
                {
                    // Update record
                    record = masechet[match.Index + j];

                    // If we have reached a mishna, stop, and move on to the next gemara
                    if (record == 'מ')
                    {
                        StringBuilder mishnaCheck = new StringBuilder();
                        mishnaCheck.Append(record);
                        for (int a = 1; a < 5; a++)
                            mishnaCheck.Append(masechet[match.Index + j + a]);
                        if (mishnaCheck.ToString() == "מתני׳")
                            break;
                    }
                    sBuilder.Append(record);
                    j++;
                }

                gemaras.Add(sBuilder.ToString().Replace("גמ׳", "")); // Remove the gemara word
            }

            return gemaras;
        }
        
        // Returns a list of every quote from every gemara, optionally all quotes
        //     from a single gemara
        public static List<string> ExtractAllQuotes(List<string> mishnas, 
            List<string> gemaras, int gemara = -1)
        {
            List<string> quotes = new List<string>();

            // If we are only here for a single gemara, then we should know
            bool notAllQuotes = false;
            if (gemara >= 0)
                notAllQuotes = true;
            else
                gemara = 0;

            for (int i = gemara; i < gemaras.Count; i++)
            {
                MatchCollection mColl = Regex.Matches(gemaras[i], @"(?<=:)(.*?)(?=:)");
                foreach (Match match in mColl)
                {
                    // Remove extraneous characters
                    string val = new string(
                        match.Value
                        .Replace("וכו'", "")
                        .Replace("כו'", "")
                        .Replace("Daf", "")
                        .Replace("a", "").Replace("b", "")
                        .Where(c => char.IsLetter(c) || char.IsWhiteSpace(c)).ToArray());

                    // Checks
                    // ------
                    // Arbitrary value of characters, which no quote is likely to exceed
                    // No 'end of perek' entries
                    // Exists in the mishna (not a 'fake' quote)
                    if (val.Length <= 70
                        && !val.Contains("הדרן עלך")
                        && CheckIfInMishna(mishnas[i], val.Trim())
                        )
                            quotes.Add(val.Trim());
                }

                // Exit if extracting quotes for a single gemara
                if (notAllQuotes)
                    break;
            }

            // Show that there are no quotes for that mishna
            if (quotes.Count == 0)
                quotes.Add("NONE");

            return quotes;
        }
        
        // Checks if the quote is in the mishna. Levenshtein Distance algorithm is applied to
        //     determine if there are quotes that are from the mishna, but with slight differences
        private static bool CheckIfInMishna(string mishna, string quote)
        {
            int pos = 0;
            int len = quote.Length;

            while (pos + len - 1 <= mishna.Length - 1)
            {
                if (mishna.Substring(pos, len) == quote
                    || LevenshteinDistance(quote, mishna.Substring(pos, len)) <= 4)
                    return true;

                pos++;
            }

            return false;
        }

        // Returns the 'distance' between two strings  (ie # of substitutions, deletions, 
        //     and insertions needed to make source equal target)
        public static int LevenshteinDistance(string source, string target)
        {
            int n = source.Length;
            int m = target.Length;

            if (n == 0)
                return m;
            if (m == 0)
                return n;

            // Create matrix
            int[,] matrix = new int[n + 1, m + 1];

            // Initialize matrix values for each string
            for (int i = 0; i <= n; i++)
                matrix[i, 0] = i;
            for (int i = 0; i <= m; i++)
                matrix[0, m] = i;

            //Algorithm
            char sChar, tChar;
            for (int i = 1; i <= n; i++)
            {
                sChar = source[i - 1];
                for (int j = 1; j <= m; j++)
                {
                    tChar = target[j - 1];

                    int cost = 1;
                    if (sChar == tChar)
                        cost = 0;

                    matrix[i, j] = GetMinimum(matrix[i - 1, j] + 1, matrix[i, j - 1] + 1,
                        matrix[i - 1, j - 1] + cost);
                }
            }

            return matrix[n, m];
        }

        // Returns the minimum in a set of integers
        private static int GetMinimum(params int[] vals)
        {
            if (vals.Length == 1)
                return vals[0];

            int min = vals[0];
            for (int i = 1; i < vals.Length; i++)
            {
                int current = vals[i];
                if (current < min)
                    min = current;
            }

            return min;
        }
    }
}
