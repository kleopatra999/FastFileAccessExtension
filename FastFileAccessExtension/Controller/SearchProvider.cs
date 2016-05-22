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
using System.Text.RegularExpressions;

namespace FastFileAccessExtension.Controller
{
    internal static class SearchProvider
    {
        public static string SearchString { get; set; }

        public static bool IsMatch(string text, Package package)
        {
            var page = (OptionPageGridSearch)package.GetDialogPage(typeof(OptionPageGridSearch));
            if (page != null)
            {
                if(page.TypeOfSearch == OptionPageGridSearch.SearchType.Contains)
                {
                    return ContainsSearch(text, package);
                }
                else if(page.TypeOfSearch == OptionPageGridSearch.SearchType.Regex)
                {
                    return RegexSearch(text, package);
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

                if(page.IgnoreCase)
                {
                    search = search.ToLower();
                    textCase = textCase.ToLower();
                }

                if(page.StartsWith)
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
                if(page.IgnoreCase)
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
    }
}
