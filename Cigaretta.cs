using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Data;
using System.Windows.Forms;

namespace szakdolgozat
{
    public class Cigaretta : Form
    {
        private IMongoCollection<Termekek> cigiTabla = Program.termekAdatbazis.GetCollection<Termekek>("cigaretta");

        private DataGridView datacigiB;

        private Button buttonBack;
        private Button buttonUpdate;

        private TextBox textboxKod;
        private TextBox textboxpred;
        private TextBox textboxprij;
        private TextBox textboxzos;
        private TextBox textboxnev;

        private Button buttonBackB;
        private Button buttonNewB;
        private Button buttonUpdateB;
        private Button buttonDeleteB;

        private TextBox textboxnevB;
        private TextBox textboxpredB;
        private TextBox textboxprijB;
        private TextBox textboxcenaB;
        private TextBox textboxKodB;
        private TextBox textboxzosB;

        private ComboBox containerComboBox;
        private Label labelCelkomB;

        public decimal cigaretta;
        public decimal celkomcigaretta = 0;

        private bool _isAdmin; 

        public Cigaretta(bool isBarbi = true)
        {
            _isAdmin = isBarbi;

            if (isBarbi) { InitializeComponentB(); }

            else { InitializeComponent(); }
        }

        private void InitializeComponent()
        {
            this.Text = "Cigaretta - Pracovník";
            this.Size = new System.Drawing.Size(1100, 450);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            datacigiB = new DataGridView
            {
                Size = new System.Drawing.Size(1060, 200),
                Location = new System.Drawing.Point(10, 20),
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            buttonBack = new Button
            {
                Text = "Späť",
                Left = 850,
                Top = 380,
                Width = 100,
            };

            textboxKod = new TextBox
            {
                Size = new System.Drawing.Size(150, 20),
                Location = new System.Drawing.Point(100, 230),
                Text = ""
            };
            Label labelKod = new Label
            {
                Text = "Produkt kód:",
                Location = new System.Drawing.Point(10, 235),
                Size = new System.Drawing.Size(150, 20),
            };

            textboxpred = new TextBox
            {
                Size = new System.Drawing.Size(150, 20),
                Location = new System.Drawing.Point(100, 280),
                Text = "",
            };
            Label labelpred = new Label
            {
                Text = "Zostatok pred.:",
                Location = new System.Drawing.Point(10, 285),
                Size = new System.Drawing.Size(150, 20),
            };

            textboxprij = new TextBox
            {
                Size = new System.Drawing.Size(150, 20),
                Location = new System.Drawing.Point(100, 305),
                Text = "",
            };
            Label labelprij = new Label
            {
                Text = "Príjem:",
                Location = new System.Drawing.Point(10, 310),
                Size = new System.Drawing.Size(150, 20),
            };

            textboxzos = new TextBox
            {
                Size = new System.Drawing.Size(150, 20),
                Location = new System.Drawing.Point(100, 330),
                Text = "",
            };
            Label labelzos = new Label
            {
                Text = "Zostatok:",
                Location = new System.Drawing.Point(10, 335),
                Size = new System.Drawing.Size(150, 20),
            };

            buttonUpdate = new Button
            {
                Text = "Aktualizovať",
                Left = 850,
                Top = 260,
                Width = 100,
            };

            textboxnev = new TextBox
            {
                Size = new System.Drawing.Size(150, 20),
                Location = new System.Drawing.Point(100, 255),
                Text = "",
                ReadOnly = true,
            };

            Label labelnev = new Label
            {
                Text = "Názov:",
                Location = new System.Drawing.Point(10, 260),
                Size = new System.Drawing.Size(150, 20),
            };

            textboxKod.TextChanged += TextboxKod_TextChanged;

            buttonBack.Click += buttonBack_Click;
            buttonUpdate.Click += buttonUpdate_Click;

            this.Controls.Add(buttonBack);
            this.Controls.Add(buttonUpdate);
            this.Controls.Add(datacigiB);
            this.Controls.Add(textboxpred);
            this.Controls.Add(textboxprij);
            this.Controls.Add(textboxKod);
            this.Controls.Add(textboxzos);
            this.Controls.Add(textboxnev);
            this.Controls.Add(labelKod);
            this.Controls.Add(labelpred);
            this.Controls.Add(labelprij);
            this.Controls.Add(labelzos);
            this.Controls.Add(labelnev);

            refreshDataGrid();
        }

        private void TextboxKod_TextChanged(object? sender, EventArgs e)
        {
            string currentCode = textboxKod.Text.Trim();

            if (string.IsNullOrEmpty(currentCode))
            {
                return;
            }

            var filterDefinition = Builders<Termekek>.Filter.Eq(a => a.ProductCode, currentCode);
            var termek = cigiTabla.Find(filterDefinition).FirstOrDefault();

            if (termek != null)
            {
                textboxnev.Text = termek.ProductName.ToString();
                textboxpred.Text = termek.ZosPred.ToString();
                textboxprij.Text = termek.Prijem.ToString();
                textboxzos.Text = termek.UzavZos.ToString();
            }

            else
            {
                textboxnev.Text = "";
                textboxpred.Text = "";
                textboxprij.Text = "";
                textboxzos.Text = "";
            }
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            var filterDefinition = Builders<Termekek>.Filter.Eq(a => a.ProductCode, textboxKod.Text);
            var updateDefinition = Builders<Termekek>.Update
                .Set(a => a.ZosPred, decimal.Parse(textboxpred.Text))
                .Set(a => a.Prijem, decimal.Parse(textboxprij.Text))
                .Set(a => a.UzavZos, decimal.Parse(textboxzos.Text));

            cigiTabla.UpdateOne(filterDefinition, updateDefinition);
            refreshDataGrid();
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            this.Hide();
            var Form1 = new Form1(_isAdmin);
            Form1.ShowDialog();
            this.Close();
        }

        private void InitializeComponentB()
        {
            this.Text = "Cigaretta - Manažér";
            this.Size = new System.Drawing.Size(1100, 450);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            datacigiB = new DataGridView
            {
                Size = new System.Drawing.Size(1060, 200),
                Location = new System.Drawing.Point(10, 20),
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            containerComboBox = new ComboBox
            {
                Name = "containerComboBox",
                DropDownStyle = ComboBoxStyle.DropDownList, 
                Location = new System.Drawing.Point(100, 380),  
                Size = new System.Drawing.Size(150, 20),
            };
            containerComboBox.DataSource = Enum.GetValues(typeof(CigarettaType));

            buttonBackB = new Button
            {
                Text = "Späť",
                Left = 850,
                Top = 380,
                Width = 100,
            };

            textboxKodB = new TextBox
            {
                Size = new System.Drawing.Size(150, 20),
                Location = new System.Drawing.Point(100, 230),
                Text = ""
            };
            Label labelKodB = new Label
            {
                Text = "Produkt kód:",
                Location = new System.Drawing.Point(10, 235),
                Size = new System.Drawing.Size(150, 20),
            };

            textboxnevB = new TextBox
            {
                Size = new System.Drawing.Size(150, 20),
                Location = new System.Drawing.Point(100, 255),
                Text = "",
            };
            Label labelnevB = new Label
            {
                Text = "Názov:",
                Location = new System.Drawing.Point(10, 260),
                Size = new System.Drawing.Size(150, 20),
            };

            textboxpredB = new TextBox
            {
                Size = new System.Drawing.Size(150, 20),
                Location = new System.Drawing.Point(100, 280),
                Text = "",
            };
            Label labelpredB = new Label
            {
                Text = "Zostatok pred.:",
                Location = new System.Drawing.Point(10, 285),
                Size = new System.Drawing.Size(150, 20),
            };

            textboxprijB = new TextBox
            {
                Size = new System.Drawing.Size(150, 20),
                Location = new System.Drawing.Point(100, 305),
                Text = "",
            };
            Label labelprijB = new Label
            {
                Text = "Príjem:",
                Location = new System.Drawing.Point(10, 310),
                Size = new System.Drawing.Size(150, 20),
            };

            textboxzosB = new TextBox
            {
                Size = new System.Drawing.Size(150, 20),
                Location = new System.Drawing.Point(100, 330),
                Text = "",
            };
            Label labelzosB = new Label
            {
                Text = "Zostatok:",
                Location = new System.Drawing.Point(10, 335),
                Size = new System.Drawing.Size(150, 20),
            };

            textboxcenaB = new TextBox
            {
                Size = new System.Drawing.Size(150, 20),
                Location = new System.Drawing.Point(100, 355),
                Text = "",
            };
            Label labelcenaB = new Label
            {
                Text = "Cena:",
                Location = new System.Drawing.Point(10, 360),
                Size = new System.Drawing.Size(150, 20),
            };

            Label labelkategoria = new Label
            {
                Text = "Kategória:",
                Location = new System.Drawing.Point(10, 385),
                Size = new System.Drawing.Size(80, 20),
            };

            buttonNewB = new Button
            {
                Text = "Pridať",
                Left = 850,
                Top = 230,
                Width = 100
            };

            buttonUpdateB = new Button
            {
                Text = "Aktualizovať",
                Left = 850,
                Top = 260,
                Width = 100,
            };

            buttonDeleteB = new Button
            {
                Text = "Vymazať",
                Left = 850,
                Top = 290,
                Width = 100
            };

            labelCelkomB = new Label
            {
                Text = $"Spolu cigaretta: {celkomcigaretta.ToString("N2")}",
                Location = new System.Drawing.Point(600, 235),
                Size = new System.Drawing.Size(200, 20),
            };

            textboxKodB.TextChanged += TextboxKodB_TextChanged;

            buttonBackB.Click += buttonBackB_Click;
            buttonNewB.Click += buttonNewB_Click;
            buttonUpdateB.Click += buttonUpdateB_Click;
            buttonDeleteB.Click += buttonDeleteB_Click;

            this.Controls.Add(buttonBackB);
            this.Controls.Add(buttonNewB);
            this.Controls.Add(buttonUpdateB);
            this.Controls.Add(buttonDeleteB);
            this.Controls.Add(datacigiB);
            this.Controls.Add(textboxnevB);
            this.Controls.Add(textboxpredB);
            this.Controls.Add(textboxprijB);
            this.Controls.Add(textboxcenaB);
            this.Controls.Add(textboxKodB);
            this.Controls.Add(textboxzosB);
            this.Controls.Add(labelKodB);
            this.Controls.Add(labelnevB);
            this.Controls.Add(labelpredB);
            this.Controls.Add(labelprijB);
            this.Controls.Add(labelzosB);
            this.Controls.Add(labelcenaB);
            this.Controls.Add(labelkategoria);
            this.Controls.Add(containerComboBox);
            this.Controls.Add(labelCelkomB);

            refreshDataGrid();
        }

        private void TextboxKodB_TextChanged(object? sender, EventArgs e)
        {
            string currentCode = textboxKodB.Text.Trim();

            if (string.IsNullOrEmpty(currentCode))
            {
                return;
            }

            var filterDefinition = Builders<Termekek>.Filter.Eq(a => a.ProductCode, currentCode);
            var termek = cigiTabla.Find(filterDefinition).FirstOrDefault();

            if (termek != null)
            {
                textboxnevB.Text = termek.ProductName;
                textboxcenaB.Text = termek.Price.ToString();
                textboxpredB.Text = termek.ZosPred.ToString();
                textboxprijB.Text = termek.Prijem.ToString();
                textboxzosB.Text = termek.UzavZos.ToString();
                containerComboBox.SelectedItem = termek.KategoriaCigi;
            }

            else
            {
                textboxnevB.Text = "";
                textboxcenaB.Text = "";
                textboxpredB.Text = "";
                textboxprijB.Text = "";
                textboxzosB.Text = "";
                containerComboBox.SelectedIndex = -1;
            }
        }

        private void buttonNewB_Click(object sender, EventArgs e)
        {
            var newTermek = new Termekek
            {
                ProductId = ObjectId.GenerateNewId().ToString(),
                ProductCode = textboxKodB.Text,
                ProductName = textboxnevB.Text,
                Price = decimal.Parse(textboxcenaB.Text),
                ZosPred = decimal.Parse(textboxpredB.Text),
                Prijem = decimal.Parse(textboxprijB.Text),
                UzavZos = decimal.Parse(textboxzosB.Text),
                KategoriaCigi = (CigarettaType)containerComboBox.SelectedItem,
            };

            cigiTabla.InsertOne(newTermek);
            refreshDataGrid();
        }

        private void buttonUpdateB_Click(object sender, EventArgs e)
        {
            var filterDefinition = Builders<Termekek>.Filter.Eq(a => a.ProductCode, textboxKodB.Text);
            var updateDefinition = Builders<Termekek>.Update
                .Set(a => a.ProductName, textboxnevB.Text)
                .Set(a => a.Price, decimal.Parse(textboxcenaB.Text))
                .Set(a => a.ZosPred, decimal.Parse(textboxpredB.Text))
                .Set(a => a.Prijem, decimal.Parse(textboxprijB.Text))
                .Set(a => a.UzavZos, decimal.Parse(textboxzosB.Text))
                .Set(a => a.KategoriaCigi, (CigarettaType)containerComboBox.SelectedItem);

            cigiTabla.UpdateOne(filterDefinition, updateDefinition);
            refreshDataGrid();
        }

        private void buttonDeleteB_Click(object sender, EventArgs e)
        {
            var filterDefinition =  Builders<Termekek>.Filter.Eq(a => a.ProductCode, textboxKodB.Text);
            cigiTabla.DeleteOne(filterDefinition);

            refreshDataGrid();

        }

        private void buttonBackB_Click(object sender, EventArgs e)
        {
            this.Hide();
            var Form1 = new Form1(_isAdmin);
            Form1.ShowDialog();
            this.Close();
        }

        private void refreshDataGrid()
        {
            celkomcigaretta = 0;
            var filterDefinition = Builders<Termekek>.Filter.Empty;
            var term = cigiTabla.Find(filterDefinition).ToList();
            
            var dataTable = new DataTable();
            dataTable.Columns.Add("Product Kód");
            dataTable.Columns.Add("Product Mena");
            dataTable.Columns.Add("Cena");
            dataTable.Columns.Add("Zostatok Pred");
            dataTable.Columns.Add("Prijem");
            dataTable.Columns.Add("Pred+Prijem");
            dataTable.Columns.Add("Zostatok uzav.");
            dataTable.Columns.Add("Predaj");

            if (_isAdmin)
            {
                dataTable.Columns.Add("Spolu");
            };

            dataTable.Columns.Add("Kategória");

            foreach (var termek in term)
            {
                var row = dataTable.NewRow();
                row["Product Kód"] = termek.ProductCode;
                row["Product Mena"] = termek.ProductName;
                row["Cena"] = termek.Price.ToString();
                row["Zostatok Pred"] = termek.ZosPred.ToString();
                row["Prijem"] = termek.Prijem.ToString();
                row["Pred+Prijem"] = (termek.ZosPred + termek.Prijem).ToString();
                row["Zostatok uzav."] = termek.UzavZos.ToString();
                row["Predaj"] = ((termek.ZosPred + termek.Prijem) - termek.UzavZos).ToString();

                decimal cigaretta = (((termek.ZosPred + termek.Prijem) - termek.UzavZos) * termek.Price);

                if (_isAdmin)
                {
                    row["Spolu"] = cigaretta.ToString();
                }
                ;

                row["Kategória"] = termek.KategoriaCigi.ToString();

                celkomcigaretta += cigaretta;

                dataTable.Rows.Add(row);
            }
            
            datacigiB.DataSource = dataTable;

            if (_isAdmin)
            {
                labelCelkomB.Text = $"Spolu cigaretta: {celkomcigaretta.ToString("N2")} €";
            }
        }

    }
}


