using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;

namespace RFIDApp.Tests.Helpers
{
    public class MockCloudTable : CloudTable
    {

        public MockCloudTable() : base(new Uri("http://127.0.0.1:10002/devstoreaccount1/RFID"))
        {
        }
        public MockCloudTable(Uri tableAddress) : base(tableAddress)
        { }

        public MockCloudTable(StorageUri tableAddress, StorageCredentials credentials) : base(tableAddress, credentials)
        { }

        public MockCloudTable(Uri tableAbsoluteUri, StorageCredentials credentials) : base(tableAbsoluteUri, credentials)
        { }

        public async override Task<TableResult> ExecuteAsync(TableOperation operation)
        {

            var rowKey = operation
                          .GetType()
                          .GetProperties(BindingFlags.NonPublic | BindingFlags.Instance)
                          .FirstOrDefault(prop => prop.Name == "RowKey")
                          .GetValue(operation);

            return await Task.FromResult(new TableResult
            {
                Result = TestFactory.Data().FirstOrDefault(x => x.RowKey.Equals(rowKey)),
                HttpStatusCode = 200
            });
        }

    }
}
