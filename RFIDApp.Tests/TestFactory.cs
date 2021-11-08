using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Primitives;
using RFIDApp.Models;

namespace RFIDApp.Tests
{
    public class TestFactory
    {
        public static IEnumerable<RFIDTable> Data()
        {
            return new List<RFIDTable>
            {
                new RFIDTable("0E00010001"),
                new RFIDTable("0E00020002"),
                new RFIDTable("0E00030003")

            };
        }

        private static Dictionary<string, StringValues> CreateDictionary(string key, string value)
        {
            var qs = new Dictionary<string, StringValues>
            {
                { key, value }
            };
            return qs;
        }

        public static HttpRequest CreateHttpRequestWithQueryString(string queryStringKey, string queryStringValue)
        {
            var context = new DefaultHttpContext();
            var request = context.Request;
            request.Query = new QueryCollection(CreateDictionary(queryStringKey, queryStringValue));
            return request;
        }

        public static HttpRequest CreateHttpRequestWithBody(string body)
        {
           
            var context = new DefaultHttpContext();
            var request = context.Request;
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            
            writer.Write(body);
            writer.Flush();
            stream.Position = 0;
             
            request.Body = stream;

            return request;
        }

    }
}
