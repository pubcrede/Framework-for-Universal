//-----------------------------------------------------------------------
// <copyright file="CustomerController.cs" company="Genesys Source">
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
using System;
using System.Web.Mvc;
using Genesys.Extensions;
using Genesys.Extras.Web.Http;
using Foundation.Entity;
using Genesys.Foundation.Entity;

namespace Foundation.WebServices
{
    /// <summary>
    /// Accepts HttpGet, HttpPut, HttpPost and HttpDelete operations on a customer
    /// </summary>
    public class CustomerController : WebApiController
    {
        public const string ControllerName = "Customer";
        public const string GetActionName = "Get Customer";
        public const string GetAction = "Get";
        public const string PutAction = "Put";
        public const string PostAction = "Post";
        public const string DeleteAction = "Delete";

        /// <summary>
        /// Retrieves customer by ID
        /// </summary>
        /// <returns>Customer that matches the ID, or initialized CustomerModel for not found condition</returns>
        [HttpGet()]
        public CustomerModel Get(string id)
        {
            var reader = new EntityReader<CustomerInfo>();
            CustomerInfo customer = new CustomerInfo();
            CustomerModel model = new CustomerModel();

            customer = reader.GetByID(id.TryParseInt32());
            if (customer.ID != TypeExtension.DefaultInteger)
            {
                model.Fill(customer); // Go back to model for transport
            }

            return model;
        }

        /// <summary>
        /// Creates a new customer
        /// </summary>
        /// <returns></returns>
        [HttpPut()]
        public CustomerModel Put(CustomerModel model)
        {
            CustomerInfo customer = new CustomerInfo();

            customer.Fill(model);
            customer.Save(); // Save screen changes to database.
            model.Fill(customer);  // Go back to model for transport

            return model;
        }

        /// <summary>
        /// Saves changes to a Customer
        /// </summary>
        /// <param name="model">Full customer model worth of data with user changes</param>
        /// <returns>CustomerModel containing customer data</returns>
        [HttpPost()]
        public CustomerModel Post(CustomerModel model)
        {
            var reader = new EntityReader<CustomerInfo>();
            CustomerInfo customer = new CustomerInfo();

            customer = reader.GetByID(model.ID);
            customer.Fill(model); // Overlay all screen edits on-top of the data-access-object, to preserve untouched original data
            customer.Save();
            model.Fill(customer); // Go back to model for transport

            return model;
        }

        /// <summary>
        /// Saves changes to a Customer
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpDelete()]
        public CustomerModel Delete(string id)
        {
            var reader = new EntityReader<CustomerInfo>();
            CustomerInfo customer = new CustomerInfo();
            CustomerModel model = new CustomerModel();

            customer = reader.GetByID(id.TryParseInt32());
            customer.Delete();
            customer = reader.GetByID(id.TryParseInt32()); // Verify delete, success returns empty object
            model.Fill(customer);

            return model;
        }
    }
}
