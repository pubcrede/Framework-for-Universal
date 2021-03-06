﻿//-----------------------------------------------------------------------
// <copyright file="Global.asax.cs" company="Genesys Source">
//      Copyright (c) 2017 Genesys Source. All rights reserved.
//      All rights are reserved. Reproduction or transmission in whole or in part, in
//      any form or by any means, electronic, mechanical or otherwise, is prohibited
//      without the prior written consent of the copyright owner.
// </copyright>
//-----------------------------------------------------------------------
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Framework.WebApp
{
    /// <summary>
    /// Global application startup, events and exception handling
    /// </summary>
    public class MvcApplication : System.Web.HttpApplication
    {
        /// <summary>
        /// Called on startup of the application
        /// </summary>
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);            
            ModelBinders.Binders.Add(typeof(string), new Genesys.Extras.Web.Http.MvcModelBinder()); // Ensure string posts to server are not null
        }
    }
}
