﻿//-----------------------------------------------------------------------
// <copyright file="CustomerSearch.cs" company="Genesys Source">
//      Copyright (c) Genesys Source. All rights reserved.
//      All rights are reserved. Reproduction or transmission in whole or in part, in
//      any form or by any means, electronic, mechanical or otherwise, is prohibited
//      without the prior written consent of the copyright owner.
// </copyright>
//-----------------------------------------------------------------------
using Framework.Entity;
using Genesys.Extensions;
using Genesys.Framework.Application;
using Framework.Pages;
using Framework.UserControls;
using Genesys.Framework.Worker;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Framework.Application;

namespace Framework.Pages
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class CustomerSearch : SaveablePage
    {
        /// <summary>
        /// Uri to this resource
        /// </summary>
        public static Uri Uri = new Uri("/Pages/Customer/CustomerSearch.xaml", UriKind.RelativeOrAbsolute);

        /// <summary>
        /// Controller route that handles requests for this page
        /// </summary>
        public override string ControllerName { get; } = "CustomerSearch";

        /// <summary>
        /// ViewModel holds model and is responsible for server calls, navigation, etc.
        /// </summary>
        public UniversalViewModel<CustomerSearchModel> MyViewModel { get; }

        /// <summary>
        /// Search Results collection bound to ListView
        /// </summary>
        public ObservableCollection<CustomerModel> Results { get; set; } = new ObservableCollection<CustomerModel>();

        /// <summary>
        /// Page and controls have been loaded
        /// </summary>
        /// <param name="sender">Sender of this event call</param>
        /// <param name="e">Event arguments</param>
        protected override void Page_Loaded(object sender, RoutedEventArgs e)
        {
            base.Page_Loaded(sender, e);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public CustomerSearch()
        {
            InitializeComponent();
            TextID.KeyDown += MapEnterKey;
            TextFirstName.KeyDown += MapEnterKey;
            TextLastName.KeyDown += MapEnterKey;
            MyViewModel = new UniversalViewModel<CustomerSearchModel>(ControllerName);
        }

        /// <summary>
        /// Handles click events on list
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event args. (CustomerSearchModel)e.ClickedItem</param>
        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var model = e.ClickedItem.CastOrFill<CustomerSearchModel>();
            MyViewModel.Navigate(typeof(CustomerSummary), model);
        }

        /// <summary>
        /// Sets model data, binds to controls and handles event that introduce new model data to page
        /// </summary>
        /// <param name="sender">Sender of event</param>
        /// <param name="e">Event arguments</param>
        protected override void Page_ModelReceived(object sender, NewModelReceivedEventArgs e)
        {
            BindModel(MyViewModel.MyModel = new CustomerSearchModel());
        }

        /// <summary>
        /// Binds new model data to screen
        /// </summary>
        /// <param name="modelData"></param>
        protected override void BindModel(object modelData)
        {
            MyViewModel.MyModel = modelData.CastOrFill<CustomerSearchModel>();
            DataContext = MyViewModel.MyModel;
            SetBinding(ref this.TextID, MyViewModel.MyModel.ID.ToString().Replace(TypeExtension.DefaultInteger.ToString(), ""), "ID");
            SetBinding(ref this.TextFirstName, MyViewModel.MyModel.FirstName, "FirstName");
            SetBinding(ref this.TextLastName, MyViewModel.MyModel.LastName, "LastName");
        }

        /// <summary>
        /// Link actual XAML buttons to base class
        ///  A XAML template will eliminate need for this.
        /// </summary>
        protected override OkCancel OkCancel
        {
            get { return ButtonOkCancel; }
            set { ButtonOkCancel = value; }
        }
        /// <summary>
        /// Processes any page data via workflow
        /// </summary>
        public override async Task<WorkerResult> Process(object sender, RoutedEventArgs e)
        {
            var returnValue = new WorkerResult();
            var searchUri = new Uri(MyApplication.MyWebService.ToString().AddLast("/") + "CustomerSearch");

            BindModel(await MyViewModel.Sender.SendPostAsync<CustomerSearchModel>(searchUri, MyViewModel.MyModel));
            this.ListResults.ItemsSource = MyViewModel.MyModel.Results;
            if (MyViewModel.MyModel.Results.Count > 0)
            {
                OkCancel.TextResultMessage = "Customer matches listed below";
                this.StackResults.Visibility = Visibility.Visible;
            } else
            {
                OkCancel.TextResultMessage = "No results found";
                this.StackResults.Visibility = Visibility.Collapsed;
            }
            returnValue.ReturnData = MyViewModel.MyModel.Serialize();
            return returnValue;
        }

        /// <summary>
        /// Cancels the  and/or process
        /// </summary>
        /// <param name="sender">Sender of event</param>
        /// <param name="e">Event arguments</param>
        public override void Cancel(object sender, RoutedEventArgs e)
        {
            MyViewModel.GoBack();
        }
    }
}
