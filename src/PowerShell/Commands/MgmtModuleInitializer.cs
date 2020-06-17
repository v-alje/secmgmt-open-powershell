﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Online.SecMgmt.PowerShell.Commands
{
    using System.IO;
    using System.Management.Automation;
    using Factories;
    using Models.Authentication;
    using Utilities;

    /// <summary>
    /// Used to perform actions when the module is intitialized.
    /// </summary>
    public class MgmtModuleInitializer : IModuleAssemblyInitializer
    {
        /// <summary>
        /// Performs the required operations when the module is imported.
        /// </summary>
        public void OnImport()
        {
            if (MgmtSession.Instance.AuthenticationFactory == null)
            {
                MgmtSession.Instance.AuthenticationFactory = new AuthenticationFactory();
            }

            if (MgmtSession.Instance.ClientFactory == null)
            {
                MgmtSession.Instance.ClientFactory = new ClientFactory();
            }

            if (File.Exists(Path.Combine(SharedUtilities.GetUserRootDirectory(), ".SecMgmt", "InMemoryTokenCache")))
            {
                MgmtSession.Instance.RegisterComponent(ComponentKey.TokenCache, () => new InMemoryTokenCache());
            }
            else
            {
                MgmtSession.Instance.RegisterComponent(ComponentKey.TokenCache, () => new PersistentTokenCache());
            }
        }
    }
}