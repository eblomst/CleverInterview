using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Cosmos.Table;
using RFIDApp.Models;

namespace RFIDApp.Functions
{
    public static class Authentication
    {
        [FunctionName("Authentication")]
        public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
        [Table("RFID", Connection = "AzureWebJobsStorage")] CloudTable cloudTable,
        ILogger log)
        {
            log.LogInformation("Authentication HTTP trigger function processed a request.");

            string tagId = req.Query["tagId"];

            var operation = TableOperation.Retrieve<RFIDTable>("RFID", tagId);

            var query = await cloudTable.ExecuteAsync(operation);

            var rfid = query.Result;

            return new OkObjectResult(rfid != null);

        }
    }
}
