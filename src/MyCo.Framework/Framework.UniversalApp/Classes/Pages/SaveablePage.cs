﻿//-----------------------------------------------------------------------
// <copyright file="SaveablePage.cs" company="Genesys Source">
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
using Framework.UserControls;
using Genesys.Extensions;
using Genesys.Extras.Collections;
using Genesys.Foundation.Process;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;

namespace Framework.Pages
{
    /// <summary>
    /// Screen base for authenticated users
    /// </summary>
    public abstract class SaveablePage : ReadOnlyPage
    {
        /// <summary>
        /// OK cancel buttons for this processing screen
        /// </summary>
        protected abstract OkCancel OkCancel { get; set; }

        /// <summary>
        /// Result of the screens process
        /// </summary>
        public ProcessResult Result { get; private set; } = new ProcessResult();

        /// <summary>
        /// Constructor
        /// </summary>
        public SaveablePage() : base()
        {
        }

        /// <summary>
        /// Page Loaded, can interact with controls as everything is loaded
        /// </summary>
        /// <param name="sender">Sender of event</param>
        /// <param name="e">Event arguments</param>
        protected override void Page_Loaded(object sender, RoutedEventArgs e)
        {
            base.Page_Loaded(sender, e);
            OkCancel.OnOKClicked += OK_Click;
            OkCancel.OnCancelClicked += Cancel_Click;
        }

        /// <summary>
        /// OK button
        /// </summary>
        /// <param name="sender">Sender of event</param>
        /// <param name="e">Event arguments</param>
        protected async void OK_Click(object sender, RoutedEventArgs e)
        {
            OkCancel.StartProcessing();
            Result = await this.Process(sender, e);
            OkCancel.StopProcessing(this.Result);
        }

        /// <summary>
        /// Cancel Button
        /// </summary>
        /// <param name="sender">Sender of event</param>
        /// <param name="e">Event arguments</param>
        protected void Cancel_Click(object sender, RoutedEventArgs e)
        {
            OkCancel.CancelProcessing();
            Cancel(sender, e);
        }

        /// <summary>
        /// Processes a forms business function
        /// </summary>
        public abstract Task<ProcessResult> Process(object sender, RoutedEventArgs e);

        /// <summary>
        /// Processes a forms business function
        /// </summary>
        public abstract void Cancel(object sender, RoutedEventArgs e);

        /// <summary>
        /// Map the enter key
        /// </summary>
        /// <param name="sender">Sender of event</param>
        /// <param name="e">Event arguments</param>
        protected virtual void MapEnterKey(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter && e.KeyStatus.RepeatCount == 1)
            {
                OK_Click(sender, e);
            }
        }

        /// <summary>
        /// Binds a string to a Image
        /// </summary>
        /// <param name="item"></param>
        /// <param name="bindingProperty"></param>
        public void SetBinding(ref Image item, string bindingProperty)
        {
            item.SetBinding(Image.SourceProperty, new Binding() { Path = new PropertyPath(bindingProperty), Mode = BindingMode.OneWay });
        }


        /// <summary>
        /// Binds a string to a TextBlock
        /// </summary>
        /// <param name="item"></param>
        /// <param name="initialValue"></param>
        /// <param name="bindingProperty"></param>
        public void SetBinding(ref TextBlock item, string initialValue, string bindingProperty)
        {
            item.SetBinding(TextBlock.TextProperty, new Binding() { Path = new PropertyPath(bindingProperty), Mode = BindingMode.OneWay });
        }

        /// <summary>
        /// Binds a string to a TextBox
        /// </summary>
        /// <param name="item"></param>
        /// <param name="initialValue"></param>
        /// <param name="bindingProperty"></param>
        public void SetBinding(ref TextBox item, string initialValue, string bindingProperty)
        {
            item.SetBinding(TextBox.TextProperty, new Binding() { Path = new PropertyPath(bindingProperty), Mode = BindingMode.TwoWay });
        }

        /// <summary>
        /// Binds a string to a TextBox
        /// </summary>
        /// <param name="item"></param>
        /// <param name="initialValue"></param>
        /// <param name="bindingProperty"></param>
        public void SetBinding(ref TextBox item, DateTime initialValue, string bindingProperty)
        {
            item.SetBinding(TextBox.TextProperty, new Binding() { Path = new PropertyPath(bindingProperty), Mode = BindingMode.TwoWay });
        }

        /// <summary>
        /// Binds a string to a PasswordBox
        /// </summary>
        /// <param name="item"></param>
        /// <param name="initialValue"></param>
        /// <param name="bindingProperty"></param>
        public void SetBinding(ref PasswordBox item, string initialValue, string bindingProperty)
        {
            item.SetBinding(PasswordBox.PasswordCharProperty, new Binding() { Path = new PropertyPath(bindingProperty), Mode = BindingMode.TwoWay });
        }

        /// <summary>
        /// Binds a string to a DatePicker
        /// </summary>
        /// <param name="item"></param>
        /// <param name="initialValue"></param>
        /// <param name="bindingProperty"></param>
        public void SetBinding(ref DatePicker item, DateTime initialValue, string bindingProperty)
        {
            item.SetBinding(DatePicker.DateProperty, new Binding() { Path = new PropertyPath(bindingProperty), Mode = BindingMode.TwoWay });
        }

        /// <summary>
        /// Binds a standard key-value pair to a ComboBox
        /// </summary>
        /// <param name="item"></param>
        /// <param name="collection"></param>
        /// <param name="selectedKey"></param>
        /// <param name="bindingProperty"></param>
        public void SetBinding(ref ComboBox item, List<KeyValuePair<int, string>> collection, int selectedKey, string bindingProperty)
        {
            item.ItemsSource = collection;
            item.DisplayMemberPath = "Value";
            item.SelectedValuePath = "Key";
            item.SetBinding(ComboBox.SelectedValueProperty, new Binding() { Path = new PropertyPath(bindingProperty), Mode = BindingMode.TwoWay });
            // Handle for no state
            if (collection.Count == 1)
            {
                selectedKey = TypeExtension.DefaultInteger;
            }
            item.SelectedValue = selectedKey;
        }

        /// <summary>
        /// Binds a standard key-value pair to a ComboBox
        /// </summary>
        /// <param name="item"></param>
        /// <param name="collection"></param>
        /// <param name="selectedKey"></param>
        /// <param name="bindingProperty"></param>
        public void SetBinding(ref ComboBox item, List<KeyValuePair<Guid, string>> collection, int selectedKey, string bindingProperty)
        {
            item.ItemsSource = collection;
            item.DisplayMemberPath = "Value";
            item.SelectedValuePath = "Key";
            item.SetBinding(ComboBox.SelectedValueProperty, new Binding() { Path = new PropertyPath(bindingProperty), Mode = BindingMode.TwoWay });
            // Handle for no state
            if (collection.Count == 1)
            {
                selectedKey = TypeExtension.DefaultInteger;
            }
            item.SelectedValue = selectedKey;
        }

        /// <summary>
        /// Binds a standard key-value pair to a ComboBox
        /// </summary>
        /// <param name="item"></param>
        /// <param name="collection"></param>
        /// <param name="selectedKey"></param>
        /// <param name="bindingProperty"></param>
        public void SetBinding(ref ComboBox item, KeyValueListString collection, string selectedKey, string bindingProperty)
        {
            item.ItemsSource = collection;
            item.DisplayMemberPath = "Value";
            item.SelectedValuePath = "Key";
            item.SetBinding(ComboBox.SelectedValueProperty, new Binding() { Path = new PropertyPath(bindingProperty), Mode = BindingMode.TwoWay });
            // Handle for no state
            if (collection.Count == 1) { selectedKey = collection.FirstOrDefaultSafe().Key; }
            item.SelectedValue = selectedKey;
        }
    }
}