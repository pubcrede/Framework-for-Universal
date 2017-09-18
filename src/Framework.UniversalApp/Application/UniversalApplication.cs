//-----------------------------------------------------------------------
// <copyright file="UniversalApplication.cs" company="Genesys Source">
//      Copyright (c) 2017 Genesys Source. All rights reserved.
//      All rights are reserved. Reproduction or transmission in whole or in part, in
//      any form or by any means, electronic, mechanical or otherwise, is prohibited
//      without the prior written consent of the copyright owner.
// </copyright>
//-----------------------------------------------------------------------
using Framework.Themes;
using Genesys.Extensions;
using Genesys.Extras.Configuration;
using Genesys.Extras.Net;
using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Framework.Application
{
    /// <summary>
    /// Global application information
    /// </summary>
    public abstract class UniversalApplication : Windows.UI.Xaml.Application, IUniversalApplication
    {
        /// <summary>
        /// Configuration data, XML .config style
        /// </summary>
        public IConfigurationManager ConfigurationManager { get; protected set; } = new ConfigurationManagerSafe();

        /// <summary>
        /// MyWebService
        /// </summary>
        public virtual Uri MyWebService { get { return new Uri(this.ConfigurationManager.AppSettingValue("MyWebService"), UriKind.RelativeOrAbsolute); } }

        /// <summary>
        /// Entry point Screen (Typically login screen)
        /// </summary>
        public abstract Type StartupPage { get; }

        /// <summary>
        /// Home dashboard screen
        /// </summary>
        public abstract Type HomePage { get; }

        /// <summary>
        /// Error screen
        /// </summary>
        public abstract Type ErrorPage { get; }

        /// <summary>
        /// Returns currently active window
        /// </summary>
        public static new Window Current
        {
            get
            {                
                return Window.Current;
            }
        }

        /// <summary>
        /// Returns currently active page type
        /// </summary>
        public Type CurrentPage
        {
            get
            {
                return UniversalApplication.Current.GetType(); ;
            }
        }

        /// <summary>
        /// Returns currently active page type
        /// </summary>
        public Frame CurrentFrame
        {
            get
            {
                return (Frame)Window.Current.Content;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public UniversalApplication() : base()
        {
            OnObjectInitialize();            
        }

        /// <summary>
        /// Loads config data
        /// </summary>
        /// <returns></returns>
        public async Task LoadDataAsync()
        {
            var localFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
            localFolder = await localFolder.GetFolderAsync("App_Data");
            var AppSettingsFile = await localFolder.GetFileAsync("AppSettings.config");
            var AppSettingsStream = await Windows.Storage.FileIO.ReadTextAsync(AppSettingsFile);
            var connectStringsFile = await localFolder.GetFileAsync("ConnectionStrings.config");
            var connectStringsStream = await Windows.Storage.FileIO.ReadTextAsync(connectStringsFile);
            ConfigurationManager = new ConfigurationManagerSafe(AppSettingsStream, connectStringsStream);
        }

        /// <summary>
        /// Wakes up any sleeping processes, and MyWebService chain
        /// </summary>
        /// <returns></returns>
        public virtual async Task WakeServicesAsync()
        {
            if (MyWebService.ToString() == TypeExtension.DefaultString)
            {
                HttpRequestGetString Request = new HttpRequestGetString(MyWebService.ToString());
                Request.ThrowExceptionWithEmptyReponse = false;
                await Request.SendAsync();
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
        /// Can this screen go back
        /// </summary>
        public bool CanGoBack { get { return RootFrame.CanGoBack; } }

        /// <summary>
        /// Can this screen go forward
        /// </summary>
        public bool CanGoForward { get { return RootFrame.CanGoForward; } }

        /// <summary>
        /// Navigates back to previous screen
        /// </summary>
        public void GoBack() { RootFrame.GoBack(); }

        /// <summary>
        /// Navigates forward to next screen
        /// </summary>
        public void GoForward() { RootFrame.GoForward(); }

        /// <summary>
        /// Navigates to a page via type.
        /// Typically in Universal apps
        /// </summary>
        /// <param name="destination">Destination page Uri</param>
        public bool Navigate(Type destinationPageType) { return RootFrame.Navigate(destinationPageType); }

        /// <summary>
        /// Navigates to a page via type.
        /// Typically in Universal apps
        /// </summary>
        /// <param name="destination">Destination page Uri</param>
        /// <param name="dataToPass">Data to be passed to the destination page</param>
        public bool Navigate<TModel>(Type destinationPageType, TModel dataToPass) { return RootFrame.Navigate(destinationPageType, dataToPass); }
        
        /// <summary>
        /// New model to load
        /// </summary>
        public event ObjectInitializeEventHandler ObjectInitialize;

        /// <summary>
        /// OnObjectInitialize()
        /// </summary>
        protected async void OnObjectInitialize()
        {
            await this.LoadDataAsync();
            await this.WakeServicesAsync();
            if (ObjectInitialize != null)
            {
                ObjectInitialize(this, new EventArgs());
            }
        }

        /// <summary>
        /// Workflow beginning. No custom to return.
        /// </summary>
        /// <param name="sender">Sender of event</param>
        /// <param name="e">Event arguments</param>
        public delegate void ObjectInitializeEventHandler(object sender, EventArgs e);        
    }
}