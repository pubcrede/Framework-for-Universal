//-----------------------------------------------------------------------
// <copyright file="ReadOnlyViewModel.cs" company="Genesys Source">
//      Copyright (c) 2017 Genesys Source. All rights reserved.
//      All rights are reserved. Reproduction or transmission in whole or in part, in
//      any form or by any means, electronic, mechanical or otherwise, is prohibited
//      without the prior written consent of the copyright owner.
// </copyright>
//-----------------------------------------------------------------------
using Framework.Themes;
using Genesys.Framework.Application;
using Genesys.Framework.Entity;
using Genesys.Framework.Operation;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Framework.Application
{
    /// <summary>
    /// ViewModel holds model and is responsible for server calls, navigation, etc.
    /// </summary>
    public class UniversalViewModel<TModel> : CrudViewModel<TModel>, ICrudAsync<TModel, int>, INavigateType<TModel> where TModel : ModelEntity<TModel>, new()
    {
        /// <summary>
        /// Currently running application
        /// </summary>
        public override IApplication MyApplication { get { return (UniversalApplication)Windows.UI.Xaml.Application.Current; } protected set { } }

        /// <summary>
        /// Returns currently active window
        /// </summary>
        public static Window Current
        {
            get
            {
                return Window.Current;
            }
        }

        /// <summary>
        /// Gets the root frame of the application
        /// </summary>
        /// <returns></returns>
        public Frame RootFrame
        {
            get
            {
                var returnValue = new Frame();
                var masterLayout = new GenericLayout();
                if (Current.Content is GenericLayout)
                {
                    masterLayout = (GenericLayout)Current.Content;
                    returnValue = masterLayout.ContentFrame;
                } else if (Current.Content is Frame)
                {
                    returnValue = (Frame)Current.Content;
                }
                return returnValue;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="myApplication"></param>
        public UniversalViewModel(string webServiceControllerName) 
            : base(webServiceControllerName)
        {
        }

        /// <summary>
        /// Navigates to a page via type.
        /// Typically in Universal apps
        /// </summary>
        /// <param name="destination">Destination page Uri</param>
        public bool Navigate(Type destinationPageType) { return this.Navigate(destinationPageType); }

        /// <summary>
        /// Navigates to a page via type.
        /// Typically in Universal apps
        /// </summary>
        /// <param name="destination">Destination page Uri</param>
        /// <param name="dataToPass">Data to be passed to the destination page</param>
        public bool Navigate(Type destinationPageType, TModel dataToPass) { return RootFrame.Navigate(destinationPageType, dataToPass); }

        /// <summary>
        /// Navigates to a page via type.
        /// Typically in Universal apps
        /// </summary>
        /// <param name="destination">Destination page Uri</param>
        /// <param name="dataToPass">Data to be passed to the destination page</param>
        public bool Navigate<T>(Type destinationPageType, T dataToPass) { return RootFrame.Navigate(destinationPageType, dataToPass); }
    }
}
