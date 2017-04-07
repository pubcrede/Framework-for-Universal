//-----------------------------------------------------------------------
// <copyright file="ReadOnlyDatabaseNameTests.cs" company="Genesys Source">
//      Copyright (c) Genesys Source. All rights reserved.
//      All rights are reserved. Reproduction or transmission in whole or in part, in
//      any form or by any means, electronic, mechanical or otherwise, is prohibited
//      without the prior written consent of the copyright owner.
// </copyright>
//-----------------------------------------------------------------------
using Foundation.Entity;
using Genesys.Foundation.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Genesys.Foundation.Test
{
    [TestClass()]
    public class EntityReaderTests
    {
        /// <summary>
        /// ReadOnlyDatabase context and connection
        /// </summary>
        [TestMethod()]
        public void Entity_EntityReader_Read()
        {
            var reader = new EntityReader<CustomerType>();
            var results = reader.GetAll();
            Assert.IsTrue(results.Count() > 0);
        }
    }
}
