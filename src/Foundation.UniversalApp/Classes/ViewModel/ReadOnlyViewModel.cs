//-----------------------------------------------------------------------
// <copyright file="ReadOnlyViewModel.cs" company="Genesys Source">
//      Licensed to the Apache Software Foundation (ASF) under one or more 
//      contributor license agreements.  See the NOTICE file distributed with 
//      this work for additional information regarding copyright ownership.
//      The ASF licenses this file to You under the Apache License, Version 2.0 
//      (the 'License'); you may not use this file except in compliance with 
//      the License.  You may obtain a copy of the License at 
//       
//        http://www.apache.org/licenses/LICENSE-2.0 
//       
//       Unless required by applicable law or agreed to in writing, software  
//       distributed under the License is distributed on an 'AS IS' BASIS, 
//       WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.  
//       See the License for the specific language governing permissions and  
//       limitations under the License. 
// </copyright>
//-----------------------------------------------------------------------
using Foundation.Applications;
using Genesys.Extras.Net;
using Genesys.Foundation.Application;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Foundation.ViewModels
{
    /// <summary>
    /// ViewModel holds model and is responsible for server calls, navigation, etc.
    /// </summary>
    public class ReadOnlyViewModel<TModel> : IViewModel<TModel> where TModel : new()
    {
        /// <summary>
        /// Model data
        /// </summary>
        public TModel Model { get; set; } = new TModel();

        /// <summary>
        /// Sender of main Http Verbs
        /// </summary>
        public HttpVerbSender HttpSender { get; set; } = new HttpVerbSender();

        /// <summary>
        /// Property changed event handler for INotifyPropertyChanged
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Property changed event for INotifyPropertyChanged
        /// </summary>
        /// <param name="propertyName">String name of property</param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Send is about to begin
        /// </summary>
        public event SendBeginEventHandler SendBegin;

        /// <summary>
        /// Send is complete
        /// </summary>
        public event SendBeginEventHandler SendEnd;

        /// <summary>
        /// Workflow beginning. No custom to return.
        /// </summary>
        /// <param name="sender">Sender of event</param>
        /// <param name="e">Event arguments</param>
        public delegate void SendBeginEventHandler(object sender, EventArgs e);

        /// <summary>
        /// OnSendBegin()
        /// </summary>
        public virtual void OnSendBegin() { if (this.SendBegin != null) { SendBegin(this, EventArgs.Empty); } }

        /// <summary>
        /// OnSendEnd()
        /// </summary>
        public virtual void OnSendEnd() { if (this.SendEnd != null) { SendEnd(this, EventArgs.Empty); } }

        /// <summary>
        /// Application instance
        /// </summary>
        public UniversalApplication MyApplication { get { return (UniversalApplication)Windows.UI.Xaml.Application.Current; } }

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public ReadOnlyViewModel()
            : base()
        {
        }

        /// <summary>
        /// Can this screen go back
        /// </summary>
        protected bool CanGoBack { get { return MyApplication.RootFrame.CanGoBack; } }

        /// <summary>
        /// Navigates back to previous screen
        /// </summary>
        public virtual void GoBack() { MyApplication.RootFrame.GoBack(); }

        /// <summary>
        /// Navigates to any screen
        /// </summary>
        /// <param name="destination"></param>
        /// <param name="dataToPass"></param>
        public void Navigate(Type destination, object dataToPass = null) { MyApplication.RootFrame.Navigate(destination, dataToPass); }

        /// <summary>
        /// Instantiates and transmits all data to the middle tier web service that will execute the workflow
        /// </summary>
        /// <returns></returns>
        protected virtual async Task<TDataOut> SendGetAsync<TDataOut>(string fullUrl) where TDataOut : new()
        {
            return await HttpSender.SendGetAsync<TDataOut>(fullUrl);
        }

        /// <summary>
        /// Instantiates and transmits all data to the middle tier web service that will execute the workflow
        /// </summary>
        /// <returns></returns>
        protected virtual async Task<TDataOut> SendGetAsync<TDataOut>(Uri fullUrl) where TDataOut : new()
        {
            return await HttpSender.SendGetAsync<TDataOut>(fullUrl);
        }

        /// <summary>
        /// Instantiates and transmits all data to the middle tier web service that will execute the workflow
        /// </summary>
        /// <returns></returns>
        protected virtual async Task<TDataInOut> SendPostAsync<TDataInOut>(string fullUrl, TDataInOut itemToSend)
        {
            return await HttpSender.SendPostAsync<TDataInOut>(fullUrl, itemToSend);
        }

        /// <summary>
        /// Instantiates and transmits all data to the middle tier web service that will execute the workflow
        /// </summary>
        /// <returns></returns>
        protected virtual async Task<TDataInOut> SendPostAsync<TDataInOut>(Uri fullUrl, TDataInOut itemToSend)
        {
            return await HttpSender.SendPostAsync<TDataInOut>(fullUrl, itemToSend);
        }

        /// <summary>
        /// Instantiates and transmits all data to the middle tier web service that will execute the workflow
        /// </summary>
        /// <returns></returns>
        protected virtual async Task<TDataInOut> SendPutAsync<TDataInOut>(string fullUrl, TDataInOut itemToSend)
        {
            return await HttpSender.SendPutAsync<TDataInOut>(fullUrl, itemToSend);
        }

        /// <summary>
        /// Instantiates and transmits all data to the middle tier web service that will execute the workflow
        /// </summary>
        /// <returns></returns>
        protected virtual async Task<TDataInOut> SendPutAsync<TDataInOut>(Uri fullUrl, TDataInOut itemToSend)
        {
            return await HttpSender.SendPutAsync<TDataInOut>(fullUrl, itemToSend);
        }

        /// <summary>
        /// Instantiates and transmits all data to the middle tier web service that will execute the workflow
        /// </summary>
        /// <returns></returns>
        protected virtual async Task<TDataOut> SendDeleteAsync<TDataOut>(string fullUrl) where TDataOut : new()
        {
            return await HttpSender.SendDeleteAsync<TDataOut>(fullUrl);
        }

        /// <summary>
        /// Instantiates and transmits all data to the middle tier web service that will execute the workflow
        /// </summary>
        /// <returns></returns>
        protected virtual async Task<TDataOut> SendDeleteAsync<TDataOut>(Uri fullUrl) where TDataOut : new()
        {
            return await HttpSender.SendDeleteAsync<TDataOut>(fullUrl);
        }
    }
}
