using System;
using System.Drawing.Text;
using System.Windows.Forms;

namespace szakdolgozat
{
    public class SelectionForm : Form
    {
        private Button buttonDaily;
        private Button buttonMonthly;
        private Button buttonBack;
        private Button buttonRegister;
        private bool _isAdmin;

        public SelectionForm(bool isAdmin = false)
        {
            _isAdmin = isAdmin;

            InitializeComponent();
            if (isAdmin) { buttonMonthly.Enabled = true; buttonRegister.Enabled = true; }
            else { buttonMonthly.Enabled = false; buttonRegister.Enabled = false; }
        }

        private void InitializeComponent()
        {
            buttonDaily = new Button();
            buttonMonthly = new Button();
            buttonBack = new Button();
            buttonRegister = new Button();
            SuspendLayout();

            buttonDaily.Location = new Point(5, 5);
            buttonDaily.Name = "buttonDaily";
            buttonDaily.Size = new Size(150, 25);
            buttonDaily.TabIndex = 0;
            buttonDaily.Text = "Smenná inventúra";
            buttonDaily.Click += buttonDaily_Click;
 
            buttonMonthly.Location = new Point(5, 30);
            buttonMonthly.Name = "buttonMonthly";
            buttonMonthly.Size = new Size(150, 25);
            buttonMonthly.TabIndex = 1;
            buttonMonthly.Text = "Týždenná Inventúra";
            buttonMonthly.Click += buttonMonthly_Click;
 
            buttonBack.Location = new Point(220, 120);
            buttonBack.Name = "buttonBack";
            buttonBack.Size = new Size(75, 25);
            buttonBack.TabIndex = 2;
            buttonBack.Text = "Späť";
            buttonBack.Click += buttonBack_Click;

            buttonRegister.Location = new Point(5, 55);
            buttonRegister.Name = "buttonRegister";
            buttonRegister.Size = new Size(150, 25);
            buttonRegister.TabIndex = 2;
            buttonRegister.Text = "Správa účtov";
            buttonRegister.Click += buttonRegister_Click;
 
            ClientSize = new Size(300, 150);
            Controls.Add(buttonDaily);
            Controls.Add(buttonMonthly);
            Controls.Add(buttonBack);
            Controls.Add(buttonRegister);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "SelectionForm";
            Text = "Výber";
            ResumeLayout(false);
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            this.Hide();
            var loginForm = new LoginForm();
            loginForm.ShowDialog();
            this.Close();
        }


        private void buttonRegister_Click(object sender, EventArgs e)
        {
            this.Hide();
            var spravaUcet = new SpravaUcet();
            spravaUcet.ShowDialog();
            this.Close();
        }

        private void buttonMonthly_Click(object sender, EventArgs e)
        {
            if (buttonMonthly.Enabled == true)
            {
                this.Hide();
                var mainForm = new MainForm();
                mainForm.ShowDialog();
                this.Close();
            }

        }

        private void buttonDaily_Click(object sender, EventArgs e)
        {
            this.Hide();
            var Form1 = new Form1(_isAdmin);
            Form1.ShowDialog();
            this.Close();
        }
    }
}


