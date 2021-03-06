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
using System.Collections.Generic;
using System.ComponentModel;

namespace FastFileAccessExtension.Collections
{
    public class ObservableFilterCollection<T> : ObservableCollection<T>, IEnumerable<T>
        where T : ISearchable, INotifyPropertyChanged
    {
        private Package m_Package;

        public ObservableFilterCollection(Package package)
        {
            m_Package = package;
        }

        new public IEnumerator<T> GetEnumerator()
        {
            return new ObservableFilterEnumerator<T>(base.GetEnumerator(), m_Package, base.m_List);
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return new ObservableFilterEnumerator<T>(base.GetEnumerator(), m_Package, base.m_List);
        }
    }
}
