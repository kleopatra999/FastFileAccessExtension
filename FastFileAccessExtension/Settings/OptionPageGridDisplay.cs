﻿//
// Copyright 2017 David Roller
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
using System;
using System.ComponentModel;

namespace FastFileAccessExtension.Settings
{
    internal sealed class OptionPageGridDisplay : DialogPage
    {
        public event EventHandler SettingsChanged;

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
                SettingsChanged?.Invoke(this, new EventArgs());
            }
        }

        private bool m_AddFilePath = false;
        [Category("Fast File Access")]
        [DisplayName("2. Add file paths")]
        [Description("Add the full path to the files")]
        public bool AddFilePath
        {
            get
            {
                return m_AddFilePath;
            }
            set
            {
                m_AddFilePath = value;
                SettingsChanged?.Invoke(this, new EventArgs());
            }
        }
    }
}
