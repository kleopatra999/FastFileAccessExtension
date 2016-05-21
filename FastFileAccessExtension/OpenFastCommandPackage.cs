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
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell;
using FastFileAccessExtension.Command;

namespace FastFileAccessExtension
{
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)] // Info on this package for Help/About
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [Guid(OpenFastCommandPackage.PackageGuidString)]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "pkgdef, VS and vsixmanifest are valid VS terms")]
    [ProvideToolWindow(typeof(FastFileAccessExtension.Controls.FastFileAccessWindow))]
    public sealed class OpenFastCommandPackage : Package
    {
        public const string PackageGuidString = "5FCB948B-472E-489A-9BC7-F6C1A1E3889D";
        
        public OpenFastCommandPackage()
        {

        }

        #region Package Members
        protected override void Initialize()
        {
            OpenFastCommand.Initialize(this);
            base.Initialize();
        }
        #endregion
    }
}
