//-----------------------------------------------------------------------
// <copyright file="ProgressProcessingRing.cs" company="Genesys Source">
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
using Genesys.Extensions;
using Genesys.Foundation.Process;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Framework.UserControls
{
    /// <summary>
    /// ProgressProcessingControl
    /// </summary>
    public sealed partial class ProgressProcessingRing : ReadOnlyControl
    {
        /// <summary>
        /// Mode of this control, should only show error or processing at once
        /// </summary>
        private enum Modes
        {
            Processing = 1,
            Errored = 2,
            Hidden = 3,
            Success = 4
        }
        
        /// <summary>
        /// Wraps text next to progress
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        public string TextProgress
        {
            get
            {
                return TextProgressMessage.Text;
            }
            set
            {
                TextProgressMessage.Text = value;
            }
        }
       
        /// <summary>
        /// Wraps text next to success
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        public string TextSuccess
        {
            get
            {
                return TextSuccessMessage.Text;
            }
            set
            {
                if (value != TypeExtension.DefaultString)
                {
                    TextSuccessMessage.Text = value;
                    Mode = Modes.Success;
                }
                else
                {
                    base.Visibility = Visibility.Collapsed;
                }
            }
        }

        /// <summary>
        /// Wraps text next to error
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        public string TextError
        {
            get
            {
                return TextErrorMessage.Text;
            }
            set
            {
                if (value != TypeExtension.DefaultString)
                {
                    TextErrorMessage.Text = value;
                    Mode = Modes.Errored;
                } else
                {
                    Visibility = Visibility.Collapsed;
                }
            }
        }

        /// <summary>
        /// Wraps text next to error
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        public string TextCancel
        {
            get
            {
                return TextCancelMessage.Text;
            }
            set
            {
                if (value != TypeExtension.DefaultString)
                {
                    TextCancelMessage.Text = value;
                    Mode = Modes.Errored;
                } else
                {
                    Visibility = Visibility.Collapsed;
                }
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
                return ProgressRingIndeterminate.Visibility;
            }
            set
            {
                if (value == Visibility.Collapsed)
                {
                    Mode = Modes.Hidden;
                }
                else
                {
                    Mode = Modes.Processing;
                }
            }
        }

        private Modes mode = Modes.Processing;

        /// <summary>
        /// Handles for errors vs. processing. Default is processing, yet hidden
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        private Modes Mode
        {
            get
            {
                return mode;
            }
            set
            {
                ProgressRingIndeterminate.Visibility = Visibility.Collapsed;
                TextProgressMessage.Visibility = Visibility.Collapsed;
                TextSuccessMessage.Visibility = Visibility.Collapsed;
                TextErrorMessage.Visibility = Visibility.Collapsed;
                switch (value)
                {
                    case Modes.Processing:
                        ProgressRingIndeterminate.Visibility = Visibility.Visible;
                        TextProgressMessage.Visibility = Visibility.Visible;
                        break;
                    case Modes.Errored:
                        TextErrorMessage.Visibility = Visibility.Visible;
                        break;
                    case Modes.Success:
                        TextSuccessMessage.Visibility = Visibility.Visible;
                        break;
                }
            }
        }
        
        /// <summary>
        /// Constructor
        /// </summary>
        public ProgressProcessingRing()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Binds controls to the data 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="modelData"></param>
        protected override void BindModelData(object modelData)
        {
        }

        /// <summary>
        /// Partial and controls have been loaded
        /// </summary>
        /// <param name="sender">Sender of this event call</param>
        /// <param name="e">Event arguments</param>
        protected override void Partial_Loaded(object sender, RoutedEventArgs e)
        {
            Mode = Modes.Errored;
            base.Partial_Loaded(sender, e);
        }
       
        /// <summary>
        /// Starts the processing
        /// </summary>
        public void StartProcessing(string processingMessage = "")
        {
            if (processingMessage != TypeExtension.DefaultString)
            {
                TextProgress = processingMessage;
            }
            Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Stops processing, and displays an error or optional success message
        /// </summary>
        /// <param name="results">ProcessResult result of back end call</param>
        /// <param name="successMessage">UI determined success message</param>
        public void StopProcessing(ProcessResult results, string successMessage = "")
        {
            // Fill with errors and/or success. The app will figure out what to do
            if (results.FailedRules.Count > 0)
            {
                TextError = results.FailedRules.FirstOrDefaultSafe().Value;
            } else
            {
                TextSuccess = successMessage;
            }
        }
    }
}
