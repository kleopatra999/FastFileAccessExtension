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
using EnvDTE80;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FastFileAccessExtension.Controls
{
    public partial class FastFileAccessWindowControl : System.Windows.Controls.UserControl, INotifyPropertyChanged
    {
        private string m_SearchString = "";
        private List<FileInfo> m_SolutionExplorerFiles;
        private SolutionEvents m_SolutionEvents;

        public event PropertyChangedEventHandler PropertyChanged;

        private DTE2 m_DTE;
        public DTE2 DTE
        {
            get
            {
                return m_DTE;
            }
            set
            {
                m_DTE = value;
                this.Initialize();
            }
        }

        public FileInfo SelectedSolutionExplorerFile { get; set; }

        public List<FileInfo> SolutionExplorerFiles
        {
            get
            {
                if (string.IsNullOrWhiteSpace(m_SearchString))
                {
                    return m_SolutionExplorerFiles;
                }
                return m_SolutionExplorerFiles.Where(x => x.Name.Contains(m_SearchString)).ToList();
            }
        }

        public FastFileAccessWindowControl()
        {
            m_SolutionExplorerFiles = new List<FileInfo>();

            this.InitializeComponent();
            this.DataContext = this;
        }

        private void Initialize()
        {
            if(m_DTE == null)
            {
                return;
            }

            m_SolutionEvents = m_DTE.Events.SolutionEvents;
            m_SolutionEvents.Opened += SolutionEvents_Opened;
            m_SolutionEvents.ProjectAdded += SolutionEvents_ProjectAdded;
            m_SolutionEvents.ProjectRemoved += SolutionEvents_ProjectRemoved;
            m_SolutionEvents.QueryCloseSolution += SolutionEvents_QueryCloseSolution;

            this.ParseFiles();
        }

        private void ParseFiles()
        {
            foreach (Project pj in m_DTE.Solution.Projects)
            {
                foreach (ProjectItem item in pj.ProjectItems)
                {
                    TasksFromProject(item);
                    GetProjectItems(item);
                }
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SolutionExplorerFiles"));
        }

        private string FullPathFromItem(ProjectItem item)
        {
            try
            {
                var fullPathProp = item.Properties.Item("FullPath");
                return (string)fullPathProp.Value;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private void TasksFromProject(ProjectItem item)
        {
            var fullPath = FullPathFromItem(item);
            if (string.IsNullOrWhiteSpace(fullPath) == false)
            {
                var info = new FileInfo(fullPath);
                if (info.Exists)
                {
                    m_SolutionExplorerFiles.Add(info);
                }
            }

            var files = GetFilesFromProject(item);
            foreach (var file in files)
            {
                if (file.Exists == false)
                {
                    continue;
                }
                m_SolutionExplorerFiles.Add(file);
            }
        }

        private List<FileInfo> GetFilesFromProject(ProjectItem item)
        {
            List<FileInfo> list = new List<FileInfo>();
            for (short i = 1; i != item.FileCount; ++i)
            {
                try
                {
                    list.Add(new FileInfo(item.FileNames[i]));
                }
                catch (Exception error)
                {
                    throw new Exception("Couldnt get filename from project. I: " +
                        i + " Count: " + item.FileCount + " Project: " + item.Name, error);
                }
            }
            return list;
        }

        private void GetProjectItems(ProjectItem item)
        {
            if (item == null || item.ProjectItems == null)
            {
                return;
            }

            foreach (ProjectItem subitem in item.ProjectItems)
            {
                TasksFromProject(subitem);
                GetProjectItems(subitem);
            }
        }

        private void txtSearchBox_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Down && txtSearchBox.IsFocused)
            {
                lsvActions.Focus();
                lsvActions.SelectedIndex = 0;
                e.Handled = true;
            }
        }

        private void txtSearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            m_SearchString = txtSearchBox.Text;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SolutionExplorerFiles"));
        }

        private void lsvActions_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && this.SelectedSolutionExplorerFile != null)
            {
                e.Handled = true;

                m_DTE.ItemOperations.OpenFile(this.SelectedSolutionExplorerFile.FullName);
            }
        }

        private void SolutionEvents_Opened()
        {
            this.ParseFiles();
        }

        private void SolutionEvents_QueryCloseSolution(ref bool fCancel)
        {
            m_SolutionExplorerFiles.Clear();
        }

        private void SolutionEvents_ProjectRemoved(Project Project)
        {
            this.ParseFiles();
        }

        private void SolutionEvents_ProjectAdded(Project Project)
        {
            this.ParseFiles();
        }
    }
}