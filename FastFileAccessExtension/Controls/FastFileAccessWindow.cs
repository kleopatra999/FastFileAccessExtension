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
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell;
using EnvDTE80;
using EnvDTE;
using Microsoft.VisualStudio.Shell.Interop;

namespace FastFileAccessExtension.Controls
{
    [Guid("75283e39-e7fc-4362-9284-1f49c0bf991d")]
    public class FastFileAccessWindow : ToolWindowPane
    {
        private FastFileAccessWindowControl m_Control;
        public WindowEvents WindowEvents { get; private set; }

        public FastFileAccessWindow() : base(null)
        {
            this.Caption = "Fast File Access";

            m_Control = new FastFileAccessWindowControl();
            this.Content = m_Control;
        }

        public override void OnToolWindowCreated()
        {
            base.OnToolWindowCreated();

            var dte = GetService(typeof(SDTE)) as DTE2;
            Events2 events = (Events2)dte.Events;

            m_Control.Initialize(dte);

            this.WindowEvents = events.get_WindowEvents(null);
            this.WindowEvents.WindowActivated += new _dispWindowEvents_WindowActivatedEventHandler(WindowEvents_WindowActivated);
        }

        private void WindowEvents_WindowActivated(Window gotFocus, Window lostFocus)
        {
            if (gotFocus.Caption != this.Caption)
            {
                return;
            }

            var dte = GetService(typeof(SDTE)) as DTE2;
            m_Control.Initialize(dte);
        }
    }
}
