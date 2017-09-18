//-----------------------------------------------------------------------
// <copyright file="ProgressProcessingRing.cs" company="Genesys Source">
//      Copyright (c) 2017 Genesys Source. All rights reserved.
//      All rights are reserved. Reproduction or transmission in whole or in part, in
//      any form or by any means, electronic, mechanical or otherwise, is prohibited
//      without the prior written consent of the copyright owner.
// </copyright>
//-----------------------------------------------------------------------
using Genesys.Extensions;
using Genesys.Framework.Worker;
using Windows.UI.Xaml;

namespace Framework.UserControls
{
    /// <summary>
    /// ProgressRing element with a processing message, and a separate result message
    /// </summary>
    public sealed partial class ProgressProcessingRing : ReadOnlyControl
    {
        private string processingMessageBackup = TypeExtension.DefaultString;
        private string successMessageBackup = TypeExtension.DefaultString;

        /// <summary>
        /// Wraps text next to progress
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        public string TextProcessingMessage
        {
            get
            {
                return TextProcessing.Text;
            }
            set
            {
                TextProcessing.Text = value;
            }
        }

        /// <summary>
        /// Wraps text next to success
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        public string TextResultMessage
        {
            get
            {
                return TextResult.Text;
            }
            set
            {
                TextResult.Text = value;
            }
        }

        /// <summary>
        /// Handle visibility for all child controls
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        public new Visibility Visibility
        {
            get
            {
                return PanelRoot.Visibility;
            }
            set
            {
                PanelRoot.Visibility = value;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ProgressProcessingRing()
        {
            InitializeComponent();
            Loaded += ProgressProcessingControl_Loaded;
        }

        /// <summary>
        /// Binds controls to the data 
        /// </summary>
        /// <typeparam name="TModel">Model of this page</typeparam>
        /// <param name="modelData">Data to bind to page</param>
        protected override void BindModelData(object modelData)
        {
        }

        /// <summary>
        /// Hide the control if this page is navigated to again
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProgressProcessingControl_Loaded(object sender, RoutedEventArgs e)
        {
        }

        /// <summary>
        /// Starts the processing with pre-set processing message
        /// </summary>
        /// <param name="processingMessage">Message to show while processing</param>
        public void StartProcessing(string processingMessage)
        {
            processingMessageBackup = TextProcessing.Text;
            TextProcessing.Text = processingMessage;
            StartProcessing();
        }

        /// <summary>
        /// Starts the processing with pre-set processing message
        /// </summary>
        public void StartProcessing()
        {
            PanelRoot.Visibility = Visibility.Visible;
            PanelProcessing.Visibility = Visibility.Visible;
            TextResult.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Cancels processing with no message and no display of processing results.
        /// </summary>
        public void CancelProcessing()
        {
            PanelRoot.Visibility = Visibility.Collapsed;
            RestoreMessages();
        }

        /// <summary>
        /// Stops processing, and displays an error or optional success message
        /// When stopped, makes all controls Collapsed, and TextResult Visible.
        /// </summary>
        /// <param name="results">WorkerResult result of back end call</param>
        /// <param name="successMessage">UI determined success message</param>
        public void StopProcessing(WorkerResult results, string successMessage)
        {
            TextResult.Text = successMessage;
            StopProcessing(results);
        }

        /// <summary>
        /// Stops processing, and displays an error or optional success message
        /// When stopped, makes all controls Collapsed, and TextResult Visible.
        /// </summary>
        /// <param name="results">WorkerResult result of back end call</param>
        public void StopProcessing(WorkerResult results)
        {
            if (results.FailedRules.Count > 0)
            {
                TextResult.Text = results.FailedRules.FirstOrDefaultSafe().Value;
            }
            PanelRoot.Visibility = Visibility.Visible;
            PanelProcessing.Visibility = Visibility.Collapsed;
            TextResult.Visibility = Visibility.Visible;
            RestoreMessages();
        }

        /// <summary>
        /// Clears any temporary messages and restores message from markup
        /// </summary>
        private void RestoreMessages()
        {
            TextProcessingMessage = processingMessageBackup != TypeExtension.DefaultString ? processingMessageBackup : TextProcessingMessage;
            processingMessageBackup = TypeExtension.DefaultString;
            TextResultMessage = successMessageBackup != TypeExtension.DefaultString ? successMessageBackup : TextResultMessage;
            successMessageBackup = TypeExtension.DefaultString;
        }
    }
}