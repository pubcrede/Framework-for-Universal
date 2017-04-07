//-----------------------------------------------------------------------
// <copyright file="EntityWriterTests.cs" company="Genesys Source">
//      Copyright (c) Genesys Source. All rights reserved.
//      All rights are reserved. Reproduction or transmission in whole or in part, in
//      any form or by any means, electronic, mechanical or otherwise, is prohibited
//      without the prior written consent of the copyright owner.
// </copyright>
//-----------------------------------------------------------------------
using Foundation.Entity;
using Genesys.Extensions;
using Genesys.Extras.Mathematics;
using Genesys.Foundation.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Genesys.Foundation.Test
{
    [TestClass()]
    public class EntityWriterTests
    {
        List<int> recycleBin = new List<int>();
        List<CustomerInfo> testEntities = new List<CustomerInfo>()
        {
            new CustomerInfo() {FirstName = "John", MiddleName = "Adam", LastName = "Doe", BirthDate = DateTime.Today.AddYears(Arithmetic.Random(2).Negate()) },
            new CustomerInfo() {FirstName = "Jane", MiddleName = "Michelle", LastName = "Smith", BirthDate = DateTime.Today.AddYears(Arithmetic.Random(2).Negate()) },
            new CustomerInfo() {FirstName = "Xi", MiddleName = "", LastName = "Ling", BirthDate = DateTime.Today.AddYears(Arithmetic.Random(2).Negate()) },
            new CustomerInfo() {FirstName = "Juan", MiddleName = "", LastName = "Gomez", BirthDate = DateTime.Today.AddYears(Arithmetic.Random(2).Negate()) },
            new CustomerInfo() {FirstName = "Maki", MiddleName = "", LastName = "Ishii", BirthDate = DateTime.Today.AddYears(Arithmetic.Random(2).Negate()) }
        };

        /// <summary>
        /// Entity_EntityWriter
        /// </summary>
        [TestMethod()]
        public void Entity_EntityWriter_Create()
        {
            var writer = new EntityWriter<CustomerInfo>();
            var newCustomer = new CustomerInfo();
            var resultCustomer = new CustomerInfo();
            var dbCustomer = new CustomerInfo();
            
            // Create should update original object, and pass back a fresh-from-db object
            newCustomer.Fill(testEntities[Arithmetic.Random(1, 5)]);
            resultCustomer = writer.Save(newCustomer);
            Assert.IsTrue(newCustomer.ID != TypeExtension.DefaultInteger);
            Assert.IsTrue(newCustomer.Key != TypeExtension.DefaultGuid);
            Assert.IsTrue(resultCustomer.ID != TypeExtension.DefaultInteger);
            Assert.IsTrue(resultCustomer.Key != TypeExtension.DefaultGuid);

            // Object in db should match in-memory objects
            dbCustomer = writer.GetByID(resultCustomer.ID);
            Assert.IsTrue(dbCustomer.ID != TypeExtension.DefaultInteger);
            Assert.IsTrue(dbCustomer.Key != TypeExtension.DefaultGuid);
            Assert.IsTrue(dbCustomer.ID == resultCustomer.ID && resultCustomer.ID == newCustomer.ID);
            Assert.IsTrue(dbCustomer.Key == resultCustomer.Key && resultCustomer.Key == newCustomer.Key);

            recycleBin.Add(newCustomer.ID);
        }

        /// <summary>
        /// Entity_EntityWriter
        /// </summary>
        [TestMethod()]
        public void Entity_EntityWriter_Read()
        {
            var writer = new EntityWriter<CustomerInfo>();
            var dbCustomer = new CustomerInfo();
            var lastID = TypeExtension.DefaultInteger;

            Entity_EntityWriter_Create();
            lastID = recycleBin.Last();

            dbCustomer = writer.GetByID(lastID);
            Assert.IsTrue(dbCustomer.ID != TypeExtension.DefaultInteger);
            Assert.IsTrue(dbCustomer.Key != TypeExtension.DefaultGuid);
            Assert.IsTrue(dbCustomer.CreatedDate.Date == DateTime.UtcNow.Date);
        }

        /// <summary>
        /// Entity_EntityWriter
        /// </summary>
        [TestMethod()]
        public void Entity_EntityWriter_Update()
        {
            var writer = new EntityWriter<CustomerInfo>();
            var resultCustomer = new CustomerInfo();
            var dbCustomer = new CustomerInfo();
            var uniqueValue = Guid.NewGuid().ToString().Replace("-", "");
            var lastID = TypeExtension.DefaultInteger;
            var originalID = TypeExtension.DefaultInteger;
            var originalKey = TypeExtension.DefaultGuid;

            Entity_EntityWriter_Create();
            lastID = recycleBin.Last();

            dbCustomer = writer.GetByID(lastID);
            originalID = dbCustomer.ID;
            originalKey = dbCustomer.Key;
            Assert.IsTrue(dbCustomer.ID != TypeExtension.DefaultInteger);
            Assert.IsTrue(dbCustomer.Key != TypeExtension.DefaultGuid);

            dbCustomer.FirstName = uniqueValue;
            resultCustomer = dbCustomer.Update();
            Assert.IsTrue(resultCustomer.ID != TypeExtension.DefaultInteger);
            Assert.IsTrue(resultCustomer.Key != TypeExtension.DefaultGuid);
            Assert.IsTrue(dbCustomer.ID == resultCustomer.ID && resultCustomer.ID == originalID);
            Assert.IsTrue(dbCustomer.Key == resultCustomer.Key && resultCustomer.Key == originalKey);

            dbCustomer = writer.GetByID(originalID);
            Assert.IsTrue(dbCustomer.ID == resultCustomer.ID && resultCustomer.ID == originalID);
            Assert.IsTrue(dbCustomer.Key == resultCustomer.Key && resultCustomer.Key == originalKey);
            Assert.IsTrue(dbCustomer.ID != TypeExtension.DefaultInteger);
            Assert.IsTrue(dbCustomer.Key != TypeExtension.DefaultGuid);
        }

        /// <summary>
        /// Entity_EntityWriter
        /// </summary>
        [TestMethod()]
        public void Entity_EntityWriter_Delete()
        {
            var writer = new EntityWriter<CustomerInfo>();
            var dbCustomer = new CustomerInfo();
            var result = TypeExtension.DefaultBoolean;
            var lastID = TypeExtension.DefaultInteger;
            var originalID = TypeExtension.DefaultInteger;
            var originalKey = TypeExtension.DefaultGuid;

            Entity_EntityWriter_Create();
            lastID = recycleBin.Last();

            dbCustomer = writer.GetByID(lastID);
            originalID = dbCustomer.ID;
            originalKey = dbCustomer.Key;
            Assert.IsTrue(dbCustomer.ID != TypeExtension.DefaultInteger);
            Assert.IsTrue(dbCustomer.Key != TypeExtension.DefaultGuid);
            Assert.IsTrue(dbCustomer.CreatedDate.Date == DateTime.UtcNow.Date);

            result = dbCustomer.Delete();
            Assert.IsTrue(result);

            dbCustomer = writer.GetByID(originalID);
            Assert.IsTrue(dbCustomer.ID != originalID);
            Assert.IsTrue(dbCustomer.Key != originalKey);
            Assert.IsTrue(dbCustomer.ID == TypeExtension.DefaultInteger);
            Assert.IsTrue(dbCustomer.Key == TypeExtension.DefaultGuid);
        }

        /// <summary>
        /// Cleanup all data
        /// </summary>
        [ClassCleanupAttribute()]
        private void Cleanup()
        {
            var reader = new EntityReader<CustomerInfo>();
            foreach (int item in recycleBin)
            {
                reader.GetByID(item).Delete();
            }
        }
    }
}
