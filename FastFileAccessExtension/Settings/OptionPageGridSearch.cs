﻿//
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
using Microsoft.VisualStudio.Shell;
using System.ComponentModel;

namespace FastFileAccessExtension.Settings
{
    internal sealed class OptionPageGridSearch : DialogPage
    {
        public enum SearchType
        {
            Contains,
            Regex
        }

        [Category("Fast File Access")]
        [DisplayName("1. Search type")]
        [Description("Type of search of the search box")]
        public SearchType TypeOfSearch { get; set; }

        private bool m_IgnoreCase = true;
        [Category("Fast File Access")]
        [DisplayName("2. Ignore case")]
        [Description("Case-sensitive or insensitive search")]
        public bool IgnoreCase
        {
            get
            {
                return m_IgnoreCase;
            }
            set
            {
                m_IgnoreCase = value;
            }
        }

        [Category("Fast File Access")]
        [DisplayName("3. Starts with")]
        [Description("The file name has to start with the given text")]
        public bool StartsWith { get; set; }
    }
}
