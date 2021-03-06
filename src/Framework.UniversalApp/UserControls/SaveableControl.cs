﻿//-----------------------------------------------------------------------
// <copyright file="SaveableControl.cs" company="Genesys Source">
//      Copyright (c) 2017 Genesys Source. All rights reserved.
//      All rights are reserved. Reproduction or transmission in whole or in part, in
//      any form or by any means, electronic, mechanical or otherwise, is prohibited
//      without the prior written consent of the copyright owner.
// </copyright>
//-----------------------------------------------------------------------
using Genesys.Extensions;
using Genesys.Extras.Collections;
using Genesys.Framework.Worker;
using Genesys.Framework.Validation;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using System.Diagnostics.CodeAnalysis;

namespace Framework.UserControls
{
    /// <summary>
    /// Screen base for authenticated users
    /// </summary>
    public abstract class SaveableControl : ReadOnlyControl
    {
        /// <summary>
        /// OK cancel buttons for this processing screen
        /// </summary>
        protected abstract OkCancel OkCancel { get; set; }

        /// <summary>
        /// Result of the screens process
        /// </summary>
        public WorkerResult Result { get; private set; } = new WorkerResult();

        /// <summary>
        /// Throws Exception if any UI elements overrun their text max length
        /// </summary>
        public bool ThrowExceptionOnTextOverrun { get; set; } = TypeExtension.DefaultBoolean;

        /// <summary>
        /// Constructor
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SaveableControl() : base()
        {
#if (DEBUG)
            ThrowExceptionOnTextOverrun = true;
#endif
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
        public abstract Task<WorkerResult> Process(object sender, RoutedEventArgs e);

        /// <summary>
        /// Processes a forms business function
        /// </summary>
        public abstract void Cancel(object sender, RoutedEventArgs e);

        /// <summary>
        /// Validates text length, etc.
        /// </summary>
        public abstract bool Validate();

        /// <summary>
        /// Validates Text Message
        /// </summary>
        /// <param name="uiControl">Control holding original text</param>
        /// <param name="textMessage">Text to validate length</param>
        public bool ValidateTextLength(Control uiControl, string textMessage)
        {
            Validator<Control> controlValidator = new Validator<Control>();
            TextBlock testControl = new TextBlock() { Text = textMessage };
            controlValidator.BusinessRules.Add(new ValidationRule<Control>("ActualWidth", item => item.ActualHeight <= testControl.ActualHeight));
            return controlValidator.IsValid(uiControl);
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