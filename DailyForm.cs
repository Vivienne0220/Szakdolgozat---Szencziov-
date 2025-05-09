using System;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace szakdolgozat
{
    public class DailyForm : Form
    {
        private Button buttonBack;
        private bool _isAdmin;
        private TextBox textBoxResults;
        private Button buttonCalc;
        private TextBox uver;
        private TextBox vydaj;
        private Label trzbaLabel;

        public DailyForm(bool isAdmin = true)
        {
            _isAdmin = isAdmin;
            InitializeComponent();
            DisplayResults();
        }

        public void InitializeComponent()
        {
            this.Text = "Smenná uzávierka!";
            this.Size = new System.Drawing.Size(600, 400);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;

            textBoxResults = new TextBox
            {
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                Location = new System.Drawing.Point(20, 20),
                Size = new System.Drawing.Size(250, 250),
                Font = new System.Drawing.Font("Consolas", 10),
                ReadOnly = true
            };

            uver = new TextBox
            {
                Size = new System.Drawing.Size(150, 20),
                Location = new System.Drawing.Point(350, 100),
                Text = ""
            };

            Label uverLabel = new Label
            {
                Text = "Úver:",
                Location = new System.Drawing.Point(310, 102),
                Size = new System.Drawing.Size(40, 20),
            };

            vydaj = new TextBox
            {
                Size = new System.Drawing.Size(150, 20),
                Location = new System.Drawing.Point(350, 130),
                Text = ""
            };

            Label vydajLabel = new Label
            {
                Text = "Výdaj:",
                Location = new System.Drawing.Point(310, 132),
                Size = new System.Drawing.Size(40, 20),
            };

            trzbaLabel = new Label
            {
                Text = "Tržba:",
                Location = new System.Drawing.Point(365, 200),
                Size = new System.Drawing.Size(100, 20)
            };

            buttonBack = new Button
            {
                Text = "Späť",
                Left = 450,
                Top = 330,
                Width = 100,
            };

            buttonCalc = new Button
            {
                Text = "Tržba",
                Left = 360,
                Top = 165,
                Width = 100,
            };

            buttonBack.Click += buttonBack_Click;
            buttonCalc.Click += ButtonCalc_Click;

            this.Controls.Add(textBoxResults);
            this.Controls.Add(buttonBack);
            this.Controls.Add(buttonCalc);
            this.Controls.Add(uver);
            this.Controls.Add(uverLabel);
            this.Controls.Add(vydajLabel);
            this.Controls.Add(vydaj);
        }

        private void ButtonCalc_Click(object? sender, EventArgs e)
        {
            Console.WriteLine(decimal.Parse(vydaj.Text) + decimal.Parse(vydaj.Text));
            decimal trzba = GetResults() - (decimal.Parse(uver.Text) + decimal.Parse(vydaj.Text));
            trzbaLabel.Text = "Tržba: " + trzba.ToString("N2") + "€";
            this.Controls.Add(trzbaLabel);

            string dateString = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            string systemDrive = Environment.GetEnvironmentVariable("SystemDrive") ?? "C:";
            string filePath = Path.Combine(systemDrive, "Smenna", dateString+"_uzavierka.txt");

            string directoryPath = Path.GetDirectoryName(filePath);
            if (directoryPath != null && !Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine("Smenný prehľad");
                writer.WriteLine();
                writer.WriteLine($"Pivo:        {new Pivo().celkompivo.ToString("N2")} €");
                writer.WriteLine($"Nealko:      {new Nealko().celkomnealko.ToString("N2")} €");
                writer.WriteLine($"Cigarety:    {new Cigaretta().celkomcigaretta.ToString("N2")} €");
                writer.WriteLine($"Jedlo:       {new Jedlo().celkomjedlo.ToString("N2")} €");
                writer.WriteLine($"Nápoje:      {new Napoje().celkomnapoje.ToString("N2")} €");
                writer.WriteLine($"Ostatné:     {new Ostatne().celkomostatne.ToString("N2")} €");
                writer.WriteLine($"Destiláty:   {new Destilaty().celkomdestilaty.ToString("N2")} €");
                writer.WriteLine($"Vino:        {new Vino().celkomvino.ToString("N2")} €");
                writer.WriteLine("---------------------");
                writer.WriteLine($"Spolu:      {GetResults().ToString("N2")} €");
                writer.WriteLine("---------------------");
                writer.WriteLine($"Úver:       {decimal.Parse(uver.Text).ToString("N2")} €");
                writer.WriteLine($"Výdaj:      {decimal.Parse(vydaj.Text).ToString("N2")} €");
                writer.WriteLine($"Tržba:      {trzba.ToString("N2")} €");
            }
            MessageBox.Show("Uloženie Tržbe bolo úspešné.", "Informácia", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public decimal GetResults()
        {
            decimal celkompivo = new Pivo().celkompivo;
            decimal celkomnealko = new Nealko().celkomnealko;
            decimal celkomcigaretta = new Cigaretta().celkomcigaretta;
            decimal celkomjedlo = new Jedlo().celkomjedlo;
            decimal celkomnapoje = new Napoje().celkomnapoje;
            decimal celkomostatne = new Ostatne().celkomostatne;
            decimal celkomdestilaty = new Destilaty().celkomdestilaty;
            decimal celkomvino = new Vino().celkomvino;

            decimal celkomvsetko = celkompivo + celkomnealko + celkomcigaretta +
                                 celkomjedlo + celkomnapoje + celkomostatne + celkomdestilaty;

            return celkomvsetko;
        }

        private void DisplayResults()
        {

            decimal celkompivo = new Pivo().celkompivo;
            decimal celkomnealko = new Nealko().celkomnealko;
            decimal celkomcigaretta = new Cigaretta().celkomcigaretta;
            decimal celkomjedlo = new Jedlo().celkomjedlo;
            decimal celkomnapoje = new Napoje().celkomnapoje;
            decimal celkomostatne = new Ostatne().celkomostatne;
            decimal celkomdestilaty = new Destilaty().celkomdestilaty;
            decimal celkomvino = new Vino().celkomvino;

            decimal celkomvsetko = GetResults();

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Smenný prehľad");
            sb.AppendLine();
            sb.AppendLine($"Pivo:        {celkompivo.ToString("N2")} €");
            sb.AppendLine($"Nealko:      {celkomnealko.ToString("N2")} €");
            sb.AppendLine($"Cigarety:    {celkomcigaretta.ToString("N2")} €");
            sb.AppendLine($"Jedlo:       {celkomjedlo.ToString("N2")} €");
            sb.AppendLine($"Nápoje:      {celkomnapoje.ToString("N2")} €");
            sb.AppendLine($"Ostatné:     {celkomostatne.ToString("N2")} €");
            sb.AppendLine($"Destiláty:   {celkomdestilaty.ToString("N2")} €");
            sb.AppendLine($"Vino:        {celkomvino.ToString("N2")} €");
            sb.AppendLine("---------------------");
            sb.AppendLine($"Spolu:      {celkomvsetko.ToString("N2")} €");

            textBoxResults.Text = sb.ToString();

        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            this.Hide();
            var Form1 = new Form1(_isAdmin);
            Form1.ShowDialog();
            this.Close();
        }
    }
}