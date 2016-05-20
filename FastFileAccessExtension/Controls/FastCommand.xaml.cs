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
using System;
using EnvDTE80;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Input;
using EnvDTE;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace FastFileAccessExtension.Controls
{
    public partial class FastCommand : System.Windows.Window, INotifyPropertyChanged
    {
        private DTE2 m_DTE;
        private string m_SearchString = "";
        private List<FileInfo> m_SolutionExplorerFiles;

        public event PropertyChangedEventHandler PropertyChanged;

        public FileInfo SelectedSolutionExplorerFile { get; set; }

        public List<FileInfo> SolutionExplorerFiles
        {
            get
            {
                if (string.IsNullOrWhiteSpace(m_SearchString))
                {
                    return this.m_SolutionExplorerFiles;
                }
                return this.m_SolutionExplorerFiles.Where(x => x.Name.Contains(m_SearchString)).ToList();
            }
        }

        public FastCommand(DTE2 dTE)
        {
            m_SolutionExplorerFiles = new List<FileInfo>();

            InitializeComponent();
            this.DataContext = this;

            m_DTE = dTE;

            var version = Assembly.GetExecutingAssembly().GetName().Version;
            this.Title = this.Title + " (v" + version.ToString() + ")";

            Initialize();

            this.Loaded += (sender, e) => MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
        }

        private void Initialize()
        {
            foreach (Project pj in m_DTE.Solution.Projects)
            {
                foreach (ProjectItem item in pj.ProjectItems)
                {
                    TasksFromProject(item);
                    GetProjectItems(item);
                }
            }
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

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                e.Handled = true;
                this.Close();
            }
        }

        private void txtSearchBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Down && txtSearchBox.IsFocused)
            {
                lsvActions.Focus();
                lsvActions.SelectedIndex = 0;
                e.Handled = true;
            }
        }

        private void lsvActions_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up && lsvActions.SelectedIndex == 0)
            {
                txtSearchBox.Focus();
                e.Handled = true;
            }
        }

        private void txtSearchBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            m_SearchString = txtSearchBox.Text;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SolutionExplorerFiles"));
        }
    }
}
