using System;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
  
namespace azure_table_storage
{
    class Program
    {
        static CloudStorageAccount storageAccount;
        static CloudTableClient tableClient;
        static CloudTable students;

        static void Main(string[] args)
        {
            storageAccount = CloudStorageAccount.Parse(
                "DefaultEndpointsProtocol=https;AccountName=vishalstorageaccount;AccountKey=BWQ01j8aW8xCUIaCzA8Gn+5fdzJBz81i4//8x4u9ZUyQre6aRsszGR+hGMmTaKk+y5ajBPOaILXNtLAEFnDP/Q==;EndpointSuffix=core.windows.net");
            tableClient = storageAccount.CreateCloudTableClient();
            students = tableClient.GetTableReference("Students");

            students.CreateIfNotExistsAsync();

            InsertOp("John", "Wick");
            InsertOp("Will", "Smith");
            InsertOp("Robert", "Downey");

            QueryOp();

            Console.WriteLine("\n\n");
            Console.WriteLine("Press any key to end");
            Console.ReadKey();
        }


        static void InsertOp(string fname, string lname)
        {
            StudentEntity studentno1 = new StudentEntity(fname, lname);
            TableOperation insertop = TableOperation.Insert(studentno1);
            students.ExecuteAsync(insertop).GetAwaiter().GetResult(); 
        }
        static void QueryOp() 
        {
            TableQuery<StudentEntity> query = new TableQuery<StudentEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "section"));
            Console.WriteLine("Names of all sections");
            Console.WriteLine();
            foreach (StudentEntity std in students.ExecuteQuery(query)) 
            {
                Console.WriteLine(std.RowKey);
            }
        }
        public class StudentEntity : TableEntity
        {
            public StudentEntity(string firstname, string lastname)
            {
                this.PartitionKey = "section";
                this.RowKey = firstname + " " + lastname;
            }
            public StudentEntity() 
            {

            }
        }
    }
}
