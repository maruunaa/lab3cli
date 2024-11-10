using Azure.Data.Tables;
using Azure.Storage.Blobs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab3cl
{
    static class Ext
    {
        static string storageUri = "https://chechelnytska.table.core.windows.net/";
        static string accountName = "chechelnytska";
        static string storageAccountKey = "p11jLME1O4RwzaxmH+5u1w+bDQwUT8ZCpz6rw0LNN4JgMK2FzTIGBUCE9E/kDRNS281ZuENQxeBF+AStB75qFQ==";
        private static readonly string connString = "DefaultEndpointsProtocol=https;AccountName=chechelnytska;AccountKey=p11jLME1O4RwzaxmH+5u1w+bDQwUT8ZCpz6rw0LNN4JgMK2FzTIGBUCE9E/kDRNS281ZuENQxeBF+AStB75qFQ==;EndpointSuffix=core.windows.net";

        public static TableServiceClient serviceClient = new TableServiceClient(new Uri(storageUri),
            new TableSharedKeyCredential(accountName, storageAccountKey));


        static string containerName = "images";
        public static BlobServiceClient blobServiceClient = new BlobServiceClient(connString);
       // static BlobServiceClient blobServiceClient;
        //Get table Client
        public static async Task<TableClient> GetTableClient(string TableName)
        {
            var tableClient = serviceClient?.GetTableClient(TableName);
            await tableClient?.CreateIfNotExistsAsync();
            return tableClient;
        }

        //Insert Entity
        public static async Task<ContactEntity> InsertEntityAsync(ContactEntity contactEntity)
        {
            var tableClient = await GetTableClient("listphones");
            await tableClient.AddEntityAsync(contactEntity);
            return contactEntity;
        }

        public static async Task<List<ContactEntity>> GetAllEntities()
        {
            var tableClient = await GetTableClient("listphones");
            var contacts = tableClient.Query<ContactEntity>().ToList();
            return contacts;
        }
        //Delete Entity
        public static async Task DeleteEntity(string partionKey, string rowKey)
        {
            var tableClient = await GetTableClient("listphones");
            await tableClient.DeleteEntityAsync(partionKey, rowKey);
        }

        //Update Entity
        public static async Task<ContactEntity> UpdateEntity(ContactEntity contactEntity)
        {
            var tableClient = await GetTableClient("listphones");
            await tableClient.UpsertEntityAsync(contactEntity);
            return contactEntity;
        }

        
    }
}
