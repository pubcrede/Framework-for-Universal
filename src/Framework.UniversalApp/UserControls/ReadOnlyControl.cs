//-----------------------------------------------------------------------
// <copyright file="ReadOnlyControl.cs" company="Genesys Source">
//      Copyright (c) 2017 Genesys Source. All rights reserved.
//      All rights are reserved. Reproduction or transmission in whole or in part, in
//      any form or by any means, electronic, mechanical or otherwise, is prohibited
//      without the prior written consent of the copyright owner.
// </copyright>
//-----------------------------------------------------------------------
using Genesys.Framework.Application;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Genesys.Framework.UserControls
{
    /// <summary>
    /// Common UI functions
    /// </summary>
    public abstract class ReadOnlyControl : UserControl
    {
        /// <summary>
        /// MyApplication instance
        /// </summary>
        public UniversalApplication MyApplication { get { return (UniversalApplication)Windows.UI.Xaml.Application.Current; } }

        /// <summary>
        /// Constructor
        /// </summary>
        public ReadOnlyControl() : base()
        {
            Loaded += Partial_Loaded;
            SizeChanged += Partial_SizeChanged;
        }

        /// <summary>
        /// Page_Load event handler
        /// </summary>
        /// <param name="sender">Sender of event</param>
        /// <param name="e">Event arguments</param>
        protected virtual void Partial_Loaded(object sender, RoutedEventArgs e)
        {
        }

        /// <summary>
        /// Page_SizeChanged event handler
        /// </summary>
        /// <param name="sender">Sender of event</param>
        /// <param name="e">Event arguments</param>
        protected void Partial_SizeChanged(object sender, SizeChangedEventArgs e)
        {
        }
        
        /// <summary>
        /// Binds all model data to the screen controls
        /// </summary>
        /// <param name="modelData">Data to bind to controls</param>
        protected abstract void BindModelData(object modelData);

        /// <summary>
        /// Workflow beginning. No custom to return.
        /// </summary>
        /// <param name="sender">Sender of event</param>
        /// <param name="e">Event arguments</param>
        public delegate void SendBeginEventHandler(object sender, EventArgs e);
    }
}