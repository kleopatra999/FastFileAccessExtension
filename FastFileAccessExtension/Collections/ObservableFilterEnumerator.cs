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
using FastFileAccessExtension.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace FastFileAccessExtension.Collections
{
    public class ObservableFilterEnumerator<T> : IEnumerator<T>, IDisposable where T : ISearchable
    {
        private IEnumerator<T> m_Enumerable;
        private object m_Lock;

        public ObservableFilterEnumerator(IEnumerator<T> enumerable, object lockObj)
        {
            m_Enumerable = enumerable;
            m_Lock = lockObj;
            Monitor.Enter(m_Lock);
        }

        public void Dispose()
        {
            m_Enumerable.Dispose();
            Monitor.Exit(m_Lock);
        }

        public T Current
        {
            get
            {
                return m_Enumerable.Current;
            }
        }

        object System.Collections.IEnumerator.Current
        {
            get
            {
                return m_Enumerable.Current;
            }
        }

        public bool MoveNext()
        {
            if (typeof(ISearchable).IsAssignableFrom(typeof(T)) == false)
            {
                return m_Enumerable.MoveNext();
            }

            if (SearchProvider.SearchString == null)
            {
                return m_Enumerable.MoveNext();
            }

            while (m_Enumerable.MoveNext())
            {
                if (((ISearchable)m_Enumerable.Current).GetSearchString().Contains(SearchProvider.SearchString))
                {
                    return true;
                }
            }
            return false;
        }

        public void Reset()
        {
            m_Enumerable.Reset();
        }
    }
}
