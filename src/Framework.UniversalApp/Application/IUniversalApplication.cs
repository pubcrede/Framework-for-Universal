//-----------------------------------------------------------------------
// <copyright file="IUniversalApplication.cs" company="Genesys Source">
//      Copyright (c) Genesys Source. All rights reserved.
//      All rights are reserved. Reproduction or transmission in whole or in part, in
//      any form or by any means, electronic, mechanical or otherwise, is prohibited
//      without the prior written consent of the copyright owner.
// </copyright>
//-----------------------------------------------------------------------
using Genesys.Framework.Application;
using System;

namespace Framework.Application
{
    /// <summary>
    /// Global application information
    /// </summary>
    public interface IUniversalApplication : IApplication, INavigateType
    {
        /// <summary>
        /// Currently active page Uri
        /// </summary>
        Type CurrentPage { get; }

        /// <summary>
        /// Entry point Screen (Typically login screen)
        /// </summary>
        Type StartupPage { get; }

        /// <summary>
        /// Home dashboard screen
        /// </summary>
        Type HomePage { get; }

        /// <summary>
        /// Error screen
        /// </summary>
        Type ErrorPage { get; }        
    }
}