using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Xunit;
using RFIDApp.Functions;
using RFIDApp.Models;
using Newtonsoft.Json;
using RFIDApp.Tests.Helpers;
using Microsoft.Extensions.Logging.Abstractions;

namespace RFIDApp.Tests
{
    public class RFIDFunctionTests
    {
        private readonly ILogger logger = NullLoggerFactory.Instance.CreateLogger("Null Logger");

        [Fact]
        public async void Http_trigger_create_should_return_rfid_tagAsync()
        {
            var rfid = new RFIDTable("0E00010001");
            var request = TestFactory.CreateHttpRequestWithBody(JsonConvert.SerializeObject(rfid));
            var collector = new AsyncCollector<RFIDTable>();
            OkObjectResult response = (OkObjectResult)await Create.Run(request, collector, logger);
            Assert.Equal($"RFID tag id: {rfid.TagId} is saved to table storage", response.Value);
        }


        [Fact]
        public async void Http_trigger_create_empty_tagid_should_return_bad_request_object_result()
        {
            var rfid = new RFIDTable(null);
            var request = TestFactory.CreateHttpRequestWithBody(JsonConvert.SerializeObject(rfid));
            var collector = new AsyncCollector<RFIDTable>();
            BadRequestObjectResult response = (BadRequestObjectResult)await Create.Run(request, collector, logger);
            Assert.Equal("TagId value is missing", response.Value);
        }

        [Fact]
        public async void Http_trigger_create_null_body_result_should_return_bad_request_object_result()
        {
            
            var request = TestFactory.CreateHttpRequestWithBody(null);
            var collector = new AsyncCollector<RFIDTable>();
            BadRequestObjectResult response = (BadRequestObjectResult)await Create.Run(request, collector, logger);
            Assert.Equal("TagId value is missing", response.Value);
        }

        [Fact]
        public async void Http_trigger_authentication_should_return_true()
        {
            var request = TestFactory.CreateHttpRequestWithQueryString("tagId", "0E00010001");
            OkObjectResult response = (OkObjectResult)await Authentication.Run(request, new MockCloudTable(), logger);
            Assert.Equal(true, response.Value);
        }


        [Fact]
        public async void Http_trigger_authentication_should_return_false()
        {
            var request = TestFactory.CreateHttpRequestWithQueryString("tagId", "0E00010003");
            OkObjectResult response = (OkObjectResult)await Authentication.Run(request, new MockCloudTable(), logger);
            Assert.Equal(false, response.Value);
        }
    }
}
