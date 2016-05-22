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
using Microsoft.VisualStudio.Shell;
using System.ComponentModel;

namespace FastFileAccessExtension.Settings
{
    internal sealed class OptionPageGridDisplay : DialogPage
    {
        private bool m_AddProjectName = false;
        [Category("Fast File Access")]
        [DisplayName("1. Add project name")]
        [Description("Add the project name to the files")]
        public bool AddProjectName
        {
            get
            {
                return m_AddProjectName;
            }
            set
            {
                m_AddProjectName = value;
            }
        }

        private bool m_FullFileName = false;
        [Category("Fast File Access")]
        [DisplayName("2. Show full file paths")]
        [Description("Show the full paths instead of the names")]
        public bool FullFileName
        {
            get
            {
                return m_FullFileName;
            }
            set
            {
                m_FullFileName = value;
            }
        }
    }
}
