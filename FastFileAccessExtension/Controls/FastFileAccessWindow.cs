﻿//
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
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell;

namespace FastFileAccessExtension.Controls
{
    [Guid("75283e39-e7fc-4362-9284-1f49c0bf991d")]
    public class FastFileAccessWindow : ToolWindowPane
    {
        public FastFileAccessWindow() : base(null)
        {
            this.Caption = "Fast File Access";

            //var dte = this.ServiceProvider.GetService(typeof(SDTE)) as DTE2;
            this.Content = new FastFileAccessWindowControl(null);
        }
    }
}
