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
using FastFileAccessExtension.Collections;
using FastFileAccessExtension.Controller;
using FastFileAccessExtension.Models;
using Microsoft.VisualStudio.Shell;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Controls;
using System.Windows.Input;

namespace FastFileAccessExtension.Controls
{
    public partial class FastFileAccessWindowControl : System.Windows.Controls.UserControl, INotifyPropertyChanged
    {
        private Package m_Package;
        public Package Package
        {
            get
            {
                return m_Package;
            }
            set
            {
                m_Package = value;
                this.SolutionExplorerFiles = new ObservableFilterCollection<SearchableFileInfo>(m_Package);
            }
        }

        private SolutionEvents m_SolutionEvents;
        private DocumentEvents m_DocumentEvents;
        private ProjectItemsEvents m_ProjectItemsEvents;

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

        public SearchableFileInfo SelectedSolutionExplorerFile { get; set; }

        public ObservableFilterCollection<SearchableFileInfo> SolutionExplorerFiles { get; private set; }

        public FastFileAccessWindowControl()
        {
            this.InitializeComponent();
            this.DataContext = this;

            lsvActions.DefaultSearchColumn = "Position";
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

            m_ProjectItemsEvents = m_DTE.Events.MiscFilesEvents;
            m_ProjectItemsEvents.ItemAdded += ProjectItemsEvents_ItemAdded;
            m_ProjectItemsEvents.ItemRemoved += ProjectItemsEvents_ItemRemoved;

            m_DocumentEvents = m_DTE.Events.DocumentEvents;
            m_DocumentEvents.DocumentSaved += DocumentEvents_DocumentSaved;

            this.ParseFiles();
        }

        private void ParseFiles()
        {
            if(SolutionExplorerFiles == null)
            {
                return;
            }

            this.SolutionExplorerFiles.Clear();
            foreach (Project pj in m_DTE.Solution.Projects)
            {
                foreach (ProjectItem item in pj.ProjectItems)
                {
                    TasksFromProject(item, pj);
                    GetProjectItems(item, pj);
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

        private void TasksFromProject(ProjectItem item, Project project)
        {
            var fullPath = FullPathFromItem(item);
            if (string.IsNullOrWhiteSpace(fullPath) == false)
            {
                var info = new FileInfo(fullPath);
                if (info.Exists)
                {
                    this.SolutionExplorerFiles.Add(new SearchableFileInfo(info, project, this.Package));
                }
            }

            var files = GetFilesFromProject(item);
            foreach (var file in files)
            {
                if (file.Exists == false)
                {
                    continue;
                }
                this.SolutionExplorerFiles.Add(new SearchableFileInfo(file, project, this.Package));
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

        private void GetProjectItems(ProjectItem item, Project project)
        {
            if (item == null || item.ProjectItems == null)
            {
                return;
            }

            foreach (ProjectItem subitem in item.ProjectItems)
            {
                TasksFromProject(subitem, project);
                GetProjectItems(subitem, project);
            }
        }

        private void txtSearchBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Down)
            {
                lsvActions.SelectedIndex = 0;
                ListViewItem item = lsvActions.ItemContainerGenerator.ContainerFromIndex(0) as ListViewItem;
                item.Focus();

                e.Handled = true;
            }
        }

        private void txtSearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchProvider.SearchString = txtSearchBox.Text;
            this.SolutionExplorerFiles.Refresh();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SolutionExplorerFiles"));
        }

        private void lsvActions_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                m_DTE.ItemOperations.OpenFile(this.SelectedSolutionExplorerFile.Info.FullName);
            }
            else if(e.Key == Key.Up && lsvActions.SelectedIndex == 0)
            {
                lsvActions.SelectedItem = null;
                txtSearchBox.Focus();
                e.Handled = true;
            }
        }

        private void SolutionEvents_Opened()
        {
            this.ParseFiles();
        }

        private void SolutionEvents_QueryCloseSolution(ref bool fCancel)
        {
            this.SolutionExplorerFiles.Clear();
        }

        private void SolutionEvents_ProjectRemoved(Project Project)
        {
            this.ParseFiles();
        }

        private void SolutionEvents_ProjectAdded(Project Project)
        {
            this.ParseFiles();
        }

        private void ProjectItemsEvents_ItemRemoved(ProjectItem ProjectItem)
        {
            this.ParseFiles();
        }

        private void ProjectItemsEvents_ItemAdded(ProjectItem ProjectItem)
        {
            this.ParseFiles();
        }

        private void DocumentEvents_DocumentSaved(Document Document)
        {
            this.ParseFiles();
        }

        private void lsvActions_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(SelectedSolutionExplorerFile == null)
            {
                return;
            }
            m_DTE.ItemOperations.OpenFile(this.SelectedSolutionExplorerFile.Info.FullName);
        }
    }
}