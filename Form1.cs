using Microsoft.VisualBasic.ApplicationServices;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using System.Collections;
using System.Text;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace szakdolgozat
{
    public partial class Form1 : Form
    {
        private bool _isAdmin;
        public Form1(bool isAdmin = false)
        {
            _isAdmin = isAdmin;
            InitializeComponent();
            if (isAdmin) { button9.Enabled = true; }
            else { button9.Enabled = false; }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            this.Hide();
            var selectionForm = new SelectionForm(_isAdmin);
            selectionForm.ShowDialog();
            this.Close();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (button9.Enabled == true)
            {
                this.Hide();
                var dailyForm = new DailyForm();
                dailyForm.ShowDialog();
                this.Close();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            var pivo = new Pivo(_isAdmin);
            pivo.ShowDialog();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            var nealko = new Nealko(_isAdmin);
            nealko.ShowDialog();
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            var destilaty = new Destilaty(_isAdmin);
            destilaty.ShowDialog();
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {

            this.Hide();
            var jedlo = new Jedlo(_isAdmin);
            jedlo.ShowDialog();
            this.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Hide();
            var cigaretta = new Cigaretta(_isAdmin);
            cigaretta.ShowDialog();
            this.Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Hide();
            var napoje = new Napoje(_isAdmin);
            napoje.ShowDialog();
            this.Close();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Hide();
            var ostatne = new Ostatne(_isAdmin);
            ostatne.ShowDialog();
            this.Close();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            this.Hide();
            var vino = new Vino(_isAdmin);
            vino.ShowDialog();
            this.Close();
        }

        private void button11_Click(object sender, EventArgs e)
        {


            DialogResult result = MessageBox.Show("Ste si istí, že ste prepísali všetko v zozname ?", " ", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

            if (result == DialogResult.OK)
            {
                string dateString = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                string systemDrive = Environment.GetEnvironmentVariable("SystemDrive") ?? "C:";
                string filePath = Path.Combine(systemDrive, "databaseBackups", dateString);

                var tablak = Program.termekAdatbazis.ListCollectionNames().ToList();
                foreach (var tabla in tablak)
                {
                    var collection = Program.termekAdatbazis.GetCollection<BsonDocument>(tabla);
                    var documents = collection.Find(new BsonDocument()).ToList();

                    var jsonSettings = new MongoDB.Bson.IO.JsonWriterSettings
                    {
                        Indent = true,
                        OutputMode = MongoDB.Bson.IO.JsonOutputMode.CanonicalExtendedJson
                    };

                    var jsonDocuments = documents.Select(doc =>
                    {
                        var backupDoc = doc.DeepClone().AsBsonDocument;
                        backupDoc.Remove("_id");
                        return backupDoc.ToJson(jsonSettings);
                    }).ToList();

                    var json = "[\n" + string.Join(",\n", jsonDocuments) + "\n]";
                    string backupFilePath = Path.Combine(filePath, $"{tabla}.json");

                    string directoryPath = Path.GetDirectoryName(backupFilePath);
                    if (directoryPath != null && !Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }

                    if (File.Exists(backupFilePath))
                    {
                        File.Delete(backupFilePath);
                    }

                    File.WriteAllText(backupFilePath, json);

                    foreach (var item in documents)
                    {
                        var uzavZosValue = item.GetValue("uzavZos");

                        var filter = Builders<BsonDocument>.Filter.Eq("_id", item.GetValue("_id"));
                        var updateDef = Builders<BsonDocument>.Update.Set("zosPred", uzavZosValue);

                        foreach (var element in item.Elements.ToList())
                        {
                            string name = element.Name;

                            if (name != "_id" && name != "price" && name != "zosPred")
                            {
                                if (element.Value.IsNumeric)
                                {
                                    updateDef = updateDef.Set(name, 0);
                                }
                            }
                        }
                        collection.UpdateOne(filter, updateDef);
                    }
                }
                MessageBox.Show("Uloženie bolo úspešné.", "Informácia", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                this.Hide();
                var form1 = new Form1(_isAdmin);
                form1.ShowDialog();
                this.Close();
            }
        }
    }
}