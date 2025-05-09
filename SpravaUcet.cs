using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Data;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace szakdolgozat
{
    public class SpravaUcet : Form
    {
        private IMongoCollection<Accounts> fiokTabla = Program.fiokAdatbazis.GetCollection<Accounts>("account");

        private DataGridView dataAccounts;

        private Button btnBack;
        private Button btnAdd;
        private Button btnDelete;
        private Button btnEdit;

        private TextBox txtboxUsername;
        private TextBox txtboxPassword;

        private CheckBox chboxIsAdmin;

        private Label lblUsername;
        private Label lblPassword;
        private Label lblIsAdmin;

        public SpravaUcet() { 
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Správca Učeť";
            this.Size = new System.Drawing.Size(550, 400);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;


            dataAccounts = new DataGridView
            {
                Size = new System.Drawing.Size(510, 200),
                Location = new System.Drawing.Point(10, 20),
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            btnBack = new Button
            {
                Text = "Späť",
                Left = 420,
                Top = 330,
                Width = 100,
            };

            btnBack.Click += btnBack_Click;

            btnAdd = new Button
            {
                Text = "Pridať",
                Left = 320,
                Top = 230,
                Width = 100,
            };

            btnAdd.Click += BtnAdd_Click;

            btnDelete = new Button
            {
                Text = "Vymazať",
                Left = 320,
                Top = 260,
                Width = 100,
            };

            btnDelete.Click += btnDelete_Click;

            btnEdit = new Button
            {
                Text = "Aktualizovať",
                Left = 320,
                Top = 290,
                Width = 100,
            };

            btnEdit.Click += BtnEdit_Click;

            lblUsername = new Label
            {
                Text = "Meno:",
                Location = new System.Drawing.Point(10, 235),
                Size = new System.Drawing.Size(50, 20),
            };

            txtboxUsername = new TextBox
            {
                Size = new System.Drawing.Size(100, 20),
                Location = new System.Drawing.Point(120, 230),
                Text = ""
            };

            lblPassword = new Label
            {
                Text = "Heslo:",
                Location = new System.Drawing.Point(10, 265),
                Size = new System.Drawing.Size(50, 20),
            };

            txtboxPassword = new TextBox
            {
                Size = new System.Drawing.Size(100, 20),
                Location = new System.Drawing.Point(120, 260),
                Text = "",
            };

            lblIsAdmin = new Label
            {
                Text = "Administrátor:",
                Location = new System.Drawing.Point(10, 295),
                Size = new System.Drawing.Size(90, 20),
            };

            chboxIsAdmin = new CheckBox
            { 
                Location = new System.Drawing.Point(120, 290)
            };

            Controls.Add(dataAccounts);
            Controls.Add(btnBack);
            Controls.Add(btnAdd);
            Controls.Add(btnDelete);
            Controls.Add(btnEdit);

            Controls.Add(lblUsername);
            Controls.Add(txtboxUsername);

            Controls.Add(lblPassword);
            Controls.Add(txtboxPassword);

            Controls.Add(lblIsAdmin);
            Controls.Add(chboxIsAdmin);

            RefreshDataGrid();
        }

        private void BtnEdit_Click(object? sender, EventArgs e)
        {
            var filterDefinition = Builders<Accounts>.Filter.Eq(a => a.Username, txtboxUsername.Text);
            var updateDefinition = Builders<Accounts>.Update
                .Set(a => a.Username, txtboxUsername.Text)
                .Set(a => a.Password, txtboxPassword.Text)
                .Set(a => a.isAdmin, chboxIsAdmin.Checked);

            fiokTabla.UpdateOne(filterDefinition, updateDefinition);
            RefreshDataGrid();
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            var newAccount = new Accounts
            {
                AccountID = ObjectId.GenerateNewId().ToString(),
                Username = txtboxUsername.Text,
                Password = txtboxPassword.Text,
                isAdmin = chboxIsAdmin.Checked
            };

            fiokTabla.InsertOne(newAccount);
            RefreshDataGrid();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            var filterDefinition = Builders<Accounts>.Filter.Eq(a => a.Username, txtboxUsername.Text);
             fiokTabla.DeleteOne(filterDefinition);

            RefreshDataGrid();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Hide();
            var selectionForm = new SelectionForm(true);
            selectionForm.ShowDialog();
            this.Close();
        }

        private void RefreshDataGrid()
        {
            var filterDefinition = Builders<Accounts>.Filter.Empty;
            var acc = fiokTabla.Find(filterDefinition).ToList();

            var dataTable = new DataTable();
            dataTable.Columns.Add("Meno");
            dataTable.Columns.Add("Heslo");
            dataTable.Columns.Add("Manažér");

            foreach (var account in acc)
            {
                var row = dataTable.NewRow();
                row["Meno"] = account.Username;
                row["Heslo"] = account.Password;
                if (account.isAdmin == true)
                {
                    row["Manažér"] = "Áno";
                }
                else
                {
                    row["Manažér"] = "Nie";
                }               

                dataTable.Rows.Add(row);
            }

            dataAccounts.DataSource = dataTable;
        }
    }
}