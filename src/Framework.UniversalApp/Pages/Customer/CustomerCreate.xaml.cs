//-----------------------------------------------------------------------
// <copyright file="CustomerCreate.cs" company="Genesys Source">
//      Copyright (c) 2017 Genesys Source. All rights reserved.
//      All rights are reserved. Reproduction or transmission in whole or in part, in
//      any form or by any means, electronic, mechanical or otherwise, is prohibited
//      without the prior written consent of the copyright owner.
// </copyright>
//-----------------------------------------------------------------------
using Framework.Entity;
using Genesys.Extensions;
using Genesys.Framework.Application;
using Genesys.Framework.Pages;
using Genesys.Framework.UserControls;
using Genesys.Framework.Worker;
using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace Framework.Pages
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class CustomerCreate : SaveablePage
    {
        /// <summary>
        /// Uri to this resource
        /// </summary>
        public static Uri Uri = new Uri("/Pages/Customer/CustomerCreate.xaml", UriKind.RelativeOrAbsolute);

        /// <summary>
        /// Controller route that handles requests for this page
        /// </summary>
        public override string ControllerName { get; } = "Customer";

        /// <summary>
        /// ViewModel holds model and is responsible for server calls, navigation, etc.
        /// </summary>
        public UniversalViewModel<CustomerModel> MyViewModel { get; }

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
        /// Sets casing
        /// </summary>
        /// <param name="sender">Sender of event</param>
        /// <param name="e">Event arguments</param>
        private void TextAll_LostFocus(object sender, RoutedEventArgs e)
        {
            TextFirstName.Text = this.TextFirstName.Text.ToPascalCase();
            TextLastName.Text = this.TextLastName.Text.ToPascalCase();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public CustomerCreate()
        {
            InitializeComponent();            
            TextFirstName.LostFocus += TextAll_LostFocus;
            TextLastName.LostFocus += TextAll_LostFocus;
            TextFirstName.KeyDown += MapEnterKey;
            TextLastName.KeyDown += MapEnterKey;
            TextBirthDate.KeyDown += MapEnterKey;
            DropDownGender.KeyDown += MapEnterKey;
            MyViewModel = new UniversalViewModel<CustomerModel>(ControllerName);
        }

        /// <summary>
        /// Sets model data, binds to controls and handles event that introduce new model data to page
        /// </summary>
        /// <param name="sender">Sender of event</param>
        /// <param name="e">Event arguments</param>
        protected override void Page_ModelReceived(object sender, NewModelReceivedEventArgs e)
        {
            BindModel(new CustomerModel());
        }

        /// <summary>
        /// Binds new model data to screen
        /// </summary>
        /// <param name="modelData"></param>
        protected override void BindModel(object modelData)
        {
            MyViewModel.MyModel = modelData.CastOrFill<CustomerModel>();
            if (MyViewModel.MyModel.BirthDate == TypeExtension.DefaultDate) MyViewModel.MyModel.BirthDate = DateTime.UtcNow.AddYears(-20);
            DataContext = MyViewModel.MyModel;
            SetBinding(ref this.TextID, MyViewModel.MyModel.ID.ToString(), "ID");
            SetBinding(ref this.TextKey, MyViewModel.MyModel.Key.ToString(), "Key");
            SetBinding(ref this.TextFirstName, MyViewModel.MyModel.FirstName, "FirstName");
            SetBinding(ref this.TextLastName, MyViewModel.MyModel.LastName, "LastName");
            SetBinding(ref this.TextBirthDate, MyViewModel.MyModel.BirthDate, "BirthDate");
            SetBinding(ref this.DropDownGender, MyViewModel.MyModel.GenderSelections(), MyViewModel.MyModel.GenderID, "GenderID");
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

            MyViewModel.MyModel = await MyViewModel.CreateAsync();
            BindModel(MyViewModel.MyModel);
            if (MyViewModel.MyModel.ID == TypeExtension.DefaultInteger)
            {
                returnValue.FailedRules.Add("1025", "Failed to create");
            }

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
