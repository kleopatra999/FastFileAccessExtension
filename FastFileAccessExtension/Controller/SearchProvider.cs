//
// Copyright 2016 David Roller
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//  http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
using FastFileAccessExtension.Settings;
using Microsoft.VisualStudio.Shell;
using System;
using System.Text.RegularExpressions;

namespace FastFileAccessExtension.Controller
{
    internal static class SearchProvider
    {
        private static string m_SearchString = string.Empty;
        public static string SearchString
        {
            get
            {
                return m_SearchString;
            }
            set
            {
                m_SearchString = value;
            }
        }

        public static bool IsMatch(string text, Package package)
        {
            var page = (OptionPageGridSearch)package.GetDialogPage(typeof(OptionPageGridSearch));
            if (page != null)
            {
                if (page.TypeOfSearch == OptionPageGridSearch.SearchType.Contains)
                {
                    return ContainsSearch(text, package);
                }
                else if (page.TypeOfSearch == OptionPageGridSearch.SearchType.Regex)
                {
                    return RegexSearch(text, package);
                }
                else if (page.TypeOfSearch == OptionPageGridSearch.SearchType.Levenshtein)
                {
                    return LevenshteinSearch(text, package);
                }
                else if (page.TypeOfSearch == OptionPageGridSearch.SearchType.WordBasedLevenshtein)
                {
                    return WordBasedLevenshteinSearch(text, package);
                }
            }
            return true;
        }

        private static bool ContainsSearch(string text, Package package)
        {
            var page = (OptionPageGridSearch)package.GetDialogPage(typeof(OptionPageGridSearch));
            if (page != null)
            {
                string search = SearchString;
                string textCase = text;

                if (page.IgnoreCase)
                {
                    search = search.ToLower();
                    textCase = textCase.ToLower();
                }

                if (page.StartsWith)
                {
                    return textCase.StartsWith(search);
                }
                else
                {
                    return textCase.Contains(search);
                }
            }
            return true;
        }

        private static bool RegexSearch(string text, Package package)
        {
            var page = (OptionPageGridSearch)package.GetDialogPage(typeof(OptionPageGridSearch));
            if (page != null)
            {
                RegexOptions options = RegexOptions.None;
                if (page.IgnoreCase)
                {
                    options |= RegexOptions.IgnoreCase;
                }

                string search = SearchString;
                if (page.StartsWith)
                {
                    search = "^" + search;
                }

                var reg = new Regex(search, options);
                return reg.IsMatch(text);
            }
            return true;
        }

        public static int LevenshteinDistance(string text, string text2, Package package)
        {
            var page = (OptionPageGridSearch)package.GetDialogPage(typeof(OptionPageGridSearch));
            if (page != null)
            {
                string textCase = text;
                string textCase2 = text2;

                if (page.IgnoreCase)
                {
                    textCase = textCase.ToLower();
                    textCase2 = textCase2.ToLower();
                }
                return LevenshteinDistanceCompute(textCase, textCase2);
            }
            return 0;
        }

        private static bool LevenshteinSearch(string text, Package package)
        {
            var page = (OptionPageGridSearch)package.GetDialogPage(typeof(OptionPageGridSearch));
            if (page != null)
            {
                int distance = LevenshteinDistance(text, SearchString, package);
                return distance < page.MaxLevenshteinDistance;
            }
            return true;
        }

        public static int WordBasedLevenshteinDistance(string text, string text2, Package package)
        {
            var page = (OptionPageGridSearch)package.GetDialogPage(typeof(OptionPageGridSearch));
            if (page != null)
            {
                string textCase = text;
                string textCase2 = text2;

                if (page.IgnoreCase)
                {
                    textCase = textCase.ToLower();
                    textCase2 = textCase2.ToLower();
                }

                int minDistance = int.MaxValue;
                foreach (var word in textCase.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    int distance = LevenshteinDistanceCompute(textCase2, word.Trim());
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                    }
                }
                return minDistance;
            }
            return 0;
        }

        private static bool WordBasedLevenshteinSearch(string text, Package package)
        {
            var page = (OptionPageGridSearch)package.GetDialogPage(typeof(OptionPageGridSearch));
            if (page != null)
            {
                int distance = WordBasedLevenshteinDistance(text, SearchString, package);
                return distance < page.MaxLevenshteinDistance;
            }
            return true;
        }

        private static int LevenshteinDistanceCompute(string string1, string string2)
        {
            int n = string1.Length;
            int m = string2.Length;
            int[,] distance = new int[n + 1, m + 1];

            if (n == 0)
            {
                return m;
            }

            if (m == 0)
            {
                return n;
            }

            for (int i = 0; i <= n; distance[i, 0] = i++) { }
            for (int j = 0; j <= m; distance[0, j] = j++) { }

            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= m; j++)
                {
                    int cost = (string2[j - 1] == string1[i - 1]) ? 0 : 1;
                    distance[i, j] = Math.Min(
                        Math.Min(distance[i - 1, j] + 1, distance[i, j - 1] + 1),
                        distance[i - 1, j - 1] + cost);
                }
            }
            return distance[n, m];
        }
    }
}
