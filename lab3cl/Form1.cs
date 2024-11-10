using Azure;
using Azure.Storage.Blobs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab3cl
{
    public partial class Form1 : Form
    {
        string storageAccountKey = "p11jLME1O4RwzaxmH+5u1w+bDQwUT8ZCpz6rw0LNN4JgMK2FzTIGBUCE9E/kDRNS281ZuENQxeBF+AStB75qFQ==";
        BlobServiceClient blobServiceClient;
        string containerName = "images";
        public Form1()
        {
            InitializeComponent();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Ext.InsertEntityAsync(new ContactEntity()
            {
                PartitionKey = textBox5.Text,
                RowKey = textBox5.Text,
                Address = textBox3.Text,
                FirstName = textBox1.Text,
                LastName = textBox2.Text,
                UrlPhoto = textBox4.Text
            });

        }

        private async void button2_Click(object sender, EventArgs e)
        {
            var contacts = await Ext.GetAllEntities();
            DataSet dataSet = new DataSet();
            DataTable table = new DataTable("ContactEntity");

            table.Columns.Add("PhoneNumber", typeof(string));
            table.Columns.Add("FirstName", typeof(string));
            table.Columns.Add("Address", typeof(string));

            foreach (var contact in contacts)
            {
                DataRow row = table.NewRow();
                row["PhoneNumber"]=contact.PartitionKey;
                row["FirstName"] = contact.FirstName;
                row["Address"] = contact.Address;
                table.Rows.Add(row);
            }
            dataSet.Tables.Add(table);  
            dataGridView1.DataSource = dataSet.Tables["ContactEntity"];
            //Обробник подій
            dataSet.Tables["ContactEntity"].RowChanged += Form1_RowChanged;

            dataSet.Tables["ContactEntity"].RowDeleted += Form1_RowDeleted;
        }
        //Обробник події RowDeleted (Видалення запису)
        private async void Form1_RowDeleted(object sender, DataRowChangeEventArgs e)
        {
            try
            {
                string partitionKey = e.Row["PhoneNumber"].ToString();
                await Ext.DeleteEntity(partitionKey, partitionKey);
                MessageBox.Show("Дані знищено");
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Помилка при знищенні запису: {ex.Message}");
            }
            
        }

        //Обробник події RowChanged (Оновлення запису)
        private async void Form1_RowChanged(object sender, DataRowChangeEventArgs e)
        {
            try
            {
                var updateEntity = new ContactEntity
                {
                    PartitionKey = e.Row["PhoneNumber"].ToString(),
                    RowKey = e.Row["PhoneNumber"].ToString(),
                    FirstName = e.Row["FirstName"].ToString(),
                    Address = e.Row["Address"].ToString(),
                    ETag = ETag.All
                };
                await Ext.UpdateEntity(updateEntity);
                MessageBox.Show("Запис успішно оновлено");
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Помилка при оновленні запису: {ex.Message}");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            DialogResult dialogResult= openFileDialog1.ShowDialog();
            if(dialogResult == DialogResult.OK)
            {
                BlobClient blobClient = containerClient.GetBlobClient(openFileDialog1.SafeFileName);
                blobClient.UploadAsync(openFileDialog1.FileName, true);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            //Form2 form = new Form2(blobServiceClient, containerName);
            //form.ShowDialog();
            //if (form.DialogResult == DialogResult.OK)
            //{
            //    BlobClient blobClient = containerClient.GetBlobClient(form.fileName);
            //    MemoryStream memoryStream = new MemoryStream();
            //    await blobClient.DownloadToAsync(memoryStream);
            //    Bitmap bitmap = new Bitmap(memoryStream);
            //    pictureBox1.Image = bitmap;
            //}
        }
    }
}
