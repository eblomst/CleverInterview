using Microsoft.Azure.Cosmos.Table;


namespace RFIDApp.Models
{
    public class RFIDTable : TableEntity
    {
        public RFIDTable() { }
        public RFIDTable(string tagId)
        {
            RowKey = tagId;
            PartitionKey = "RFID";
            TagId = tagId;
        }

        public string TagId { get; set; }
    }
}
