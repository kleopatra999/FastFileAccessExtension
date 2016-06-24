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
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Data;

namespace FastFileAccessExtension.Controls
{
    public partial class SortedListView : ListView
    {
        public string DefaultSearchColumn { get; set; }

        public SortedListView()
        {
            InitializeComponent();
            this.Items.Clear();
        }

        protected override void OnItemsSourceChanged(System.Collections.IEnumerable oldValue, System.Collections.IEnumerable newValue)
        {
            base.OnItemsSourceChanged(oldValue, newValue);

            this.DefaultSort(DefaultSearchColumn);

            if (this.ItemsSource is INotifyCollectionChanged)
            {
                var list = this.ItemsSource as INotifyCollectionChanged;
                list.CollectionChanged += list_CollectionChanged;
            }
        }

        private void list_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.DefaultSort(DefaultSearchColumn);
        }

        public void DefaultSort(string column)
        {
            if (string.IsNullOrWhiteSpace(column) || this.ItemsSource == null)
            {
                return;
            }

            try
            {
                ICollectionView resultDataView = CollectionViewSource.GetDefaultView(this.ItemsSource);
                resultDataView.SortDescriptions.Clear();
                resultDataView.SortDescriptions.Add(new SortDescription(column, ListSortDirection.Ascending));
            }
            catch (System.InvalidOperationException)
            {
                // Ignore errors while try to sort icon collumn
            }
        }
    }
}
