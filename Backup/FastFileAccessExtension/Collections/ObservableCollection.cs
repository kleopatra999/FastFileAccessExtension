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
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading;

namespace FastFileAccessExtension.Collections
{
    public class ObservableCollection<T> : IEnumerable<T>, INotifyCollectionChanged
    {
        public delegate void UpdateItemDelegate(T item);
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        protected List<T> m_List;

        public int Count
        {
            get
            {
                try
                {
                    Monitor.Enter(m_List);
                    return m_List.Count;
                }
                finally
                {
                    Monitor.Exit(m_List);
                }
            }
        }

        public ObservableCollection()
        {
            m_List = new List<T>();
        }

        public void Add(T value)
        {
            try
            {
                Monitor.Enter(m_List);
                m_List.Add(value);
            }
            finally
            {
                Monitor.Exit(m_List);
            }
            this.Refresh(NotifyCollectionChangedAction.Add, value);
        }

        public bool Remove(T value)
        {
            int index = -1;
            try
            {
                Monitor.Enter(m_List);
                for (int i = 0; i < m_List.Count; ++i)
                {
                    if (m_List[i].Equals(value))
                    {
                        m_List.RemoveAt(i);
                        index = i;
                        break;
                    }
                }

                if (index < 0)
                {
                    return false;
                }
            }
            finally
            {
                Monitor.Exit(m_List);
            }
            this.Refresh(NotifyCollectionChangedAction.Remove, value, index);
            return true;
        }

        public void Clear()
        {
            try
            {
                Monitor.Enter(m_List);
                m_List.Clear();
            }
            finally
            {
                Monitor.Exit(m_List);
            }
            this.Refresh();
        }

        public bool UpdateItem(T search, UpdateItemDelegate foundItemFunc)
        {
            bool result = false;
            foreach (var item in m_List)
            {
                if (foundItemFunc != null && item.Equals(search))
                {
                    foundItemFunc(item);
                    result = true;
                }
            }
            return result;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return m_List.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return m_List.GetEnumerator();
        }

        private void Refresh(NotifyCollectionChangedAction action, object changedItem, int index = -1)
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(action, changedItem, index));
        }

        public void Refresh()
        {
            this.Refresh(NotifyCollectionChangedAction.Reset, null);
        }
    }
}
