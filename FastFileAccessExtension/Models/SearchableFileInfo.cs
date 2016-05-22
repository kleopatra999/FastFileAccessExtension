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
using EnvDTE;
using FastFileAccessExtension.Collections;
using System.ComponentModel;
using System.IO;

namespace FastFileAccessExtension.Models
{
    public class SearchableFileInfo : ISearchable, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private FileInfo m_Info;
        public FileInfo Info
        {
            get
            {
                return m_Info;
            }
            set
            {
                m_Info = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Info"));
            }
        }

        private Project m_Project;
        public Project Project
        {
            get
            {
                return m_Project;
            }
            set
            {
                m_Project = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Project"));
            }
        }


        public string SearchString
        {
            get
            {
                return this.Info.Name + " (" + this.Project.Name + ")";
            }
        }

        public SearchableFileInfo()
        {

        }

        public SearchableFileInfo(FileInfo infoOfFile, Project project)
        {
            this.Info = infoOfFile;
            this.Project = project;
        }

        public string GetSearchString()
        {
            return this.SearchString;
        }
    }
}
