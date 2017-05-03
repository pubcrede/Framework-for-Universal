﻿//-----------------------------------------------------------------------
// <copyright file="JsonNetFormatter.cs" company="Genesys Source">
//      Copyright (c) 2017 Genesys Source. All rights reserved.
//      All rights are reserved. Reproduction or transmission in whole or in part, in
//      any form or by any means, electronic, mechanical or otherwise, is prohibited
//      without the prior written consent of the copyright owner.
// </copyright>
//-----------------------------------------------------------------------
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Diagnostics.CodeAnalysis;
using Genesys.Extensions;

namespace Foundation.WebServices
{
    /// <summary>
    /// Adds JSON.NET formatter to handle ISO 8601 dates
    /// Use: Global.asax.cs
    ///  protected void Application_Start(object sender, EventArgs e)
    ///  {
    ///    GlobalConfiguration.Configuration.Formatters.Insert(0, new JsonNetFormatter( new JsonSerializerSettings())); // Web API to use JSON.NET serializer instead of DataContractJsonSerializer
    ///    GlobalConfiguration.Configuration.Formatters.Insert(0, new JsonNetFormatter());
    ///  }
    /// </summary>
    public class JsonNetFormatter : MediaTypeFormatter
    {
        private JsonSerializerSettings jsonSerializerSettingsValue;
        private Encoding utf8Encoding = new UTF8Encoding(false, true);
        private MediaTypeHeaderValue jsonMediaType = new MediaTypeHeaderValue("application/json");

        /// <summary>
        /// Constructor
        /// </summary>
        public JsonNetFormatter() : base()
        {
            SupportedMediaTypes.Add(jsonMediaType);
            SupportedEncodings.Add(utf8Encoding);
        }
        /// <summary>
        /// Constructor
        /// Derives from System.Net.Http.Formatting.MediaTypeFormatter
        /// </summary>
        /// <param name="jsonSerializerSettings"></param>
        public JsonNetFormatter(JsonSerializerSettings jsonSerializerSettings) : this()
        {
            jsonSerializerSettingsValue = jsonSerializerSettings ?? new JsonSerializerSettings();
        }

        /// <summary>
        /// Queries whether this System.Net.Http.Formatting.MediaTypeFormatter can deserializean
        /// object of the specified type
        /// </summary>
        /// <param name="type">The type to deserialize</param>
        /// <returns>true if the System.Net.Http.Formatting.MediaTypeFormatter can deserialize the type; otherwise, false.</returns>
        public override bool CanReadType(Type type)
        {
            return true;
        }

        /// <summary>
        ///     Queries whether this System.Net.Http.Formatting.MediaTypeFormatter can serializean
        ///     object of the specified type.
        /// </summary>
        /// <param name="type">The type to serialize</param>
        /// <returns>true if the System.Net.Http.Formatting.MediaTypeFormatter can serialize the type; otherwise, false.</returns>
        public override bool CanWriteType(Type type)
        {
            return true;
        }

        /// <summary>
        /// Asynchronously deserializes an object of the specified type.
        /// </summary>
        /// <param name="type">The type of the object to deserialize</param>
        /// <param name="readStream">The System.IO.Stream to read</param>
        /// <param name="content">The System.Net.Http.HttpContent, if available. It may be null</param>
        /// <param name="formatterLogger">The System.Net.Http.Formatting.IFormatterLogger to log events to</param>
        /// <returns>A System.Threading.Tasks.Task whose result will be an object of the given type</returns>
        [SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
        public override Task<object> ReadFromStreamAsync(Type type, Stream readStream, HttpContent content, IFormatterLogger formatterLogger)
        {
            var serializer = JsonSerializer.Create(jsonSerializerSettingsValue);

            return Task.Factory.StartNew(() =>
            {
                using (var streamReader = new StreamReader(readStream, utf8Encoding))
                {
                    using (var jsonTextReader = new JsonTextReader(streamReader))
                    {
                        return serializer.Deserialize(jsonTextReader, type);
                    }
                }
            });
        }

        /// <summary>
        /// Asynchronously writes an object of the specified type
        /// </summary>
        /// <param name="type">The type of the object to write</param>
        /// <param name="value">The object value to write. It may be null</param>
        /// <param name="writeStream">The System.IO.Stream to which to write</param>
        /// <param name="content">The System.Net.Http.HttpContent if available. It may be null</param>
        /// <param name="transportContext">The System.Net.TransportContext if available. It may be null</param>
        /// <returns>A System.Threading.Tasks.Task that will perform the write</returns>
        public override Task WriteToStreamAsync(Type type, object value, Stream writeStream, HttpContent content, TransportContext transportContext)
        {
            var serializer = JsonSerializer.Create(jsonSerializerSettingsValue);

            return Task.Factory.StartNew(() =>
            {
                using (var jsonTextWriter = new JsonTextWriter(new StreamWriter(writeStream, utf8Encoding)) { DateFormatString = DateTimeExtension.Formats.ISO8601F, CloseOutput = false })
                {
                    serializer.Serialize(jsonTextWriter, value);
                    jsonTextWriter.Flush();
                }
            });
        }
    }
}


    