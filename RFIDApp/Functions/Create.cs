using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RFIDApp.Models;

namespace RFIDApp.Functions
{
    public static class Create
    {
        [FunctionName("Create")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            [Table("RFID", Connection = "AzureWebJobsStorage")] IAsyncCollector<RFIDTable> rfidTableCollector,
            ILogger log)
        {
            log.LogInformation("Create HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var rfidDto = JsonConvert.DeserializeObject<CreateRFIDDto>(requestBody);

            if (rfidDto?.TagId == null)
            {
                return new BadRequestObjectResult("TagId value is missing");
            }

            var rfid = new RFIDTable(rfidDto.TagId);

            await rfidTableCollector.AddAsync(rfid);
       
            return new OkObjectResult($"RFID tag id: {rfid.TagId} is saved to table storage");
        }
    }
}
