using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab3cl
{
    public partial class Form2 : Form
    {
        BlobServiceClient blobServiceClient;
        public string fileName { get; set; }
        public Form2(BlobServiceClient blobServiceClient, string containerName)
        {
            InitializeComponent();
            this.blobServiceClient = blobServiceClient;
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            foreach(BlobItem blobItem in containerClient.GetBlobs())
            {
                listView1.Items.Add(blobItem.Name);
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                fileName = listView1.SelectedItems[0].Text;
            }
        }
    }
}
