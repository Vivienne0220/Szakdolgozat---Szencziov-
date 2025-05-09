using szakdolgozat;
using System.Runtime.CompilerServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Forms;
using System.Globalization;
using System.Text.Json.Nodes;

public class MainForm : Form
{
    private bool _isAdmin;
    private Button buttonBack;

    private TextBox eredmenyTextBox;
    private TextBox trzbaTextBox;

    private Panel chartPanel;
    
    private void buttonBack_Click(object sender, EventArgs e)
    {
        this.Hide();
        var selectionForm = new SelectionForm(_isAdmin);
        selectionForm.ShowDialog();
        this.Close();
    }

    public MainForm(bool isBarbi = true)
    {
        _isAdmin = isBarbi;

        this.Text = "Týždenná inventúra!";
        this.Size = new System.Drawing.Size(750, 500);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;

        trzbaTextBox = new TextBox
        {
            Multiline = true,
            Location = new System.Drawing.Point(10, 10),
            Size = new System.Drawing.Size(450, 40),
            Font = new System.Drawing.Font("Consolas", 10),
            ReadOnly = true
        };

        buttonBack = new Button
        {
            Text = "Späť",
            Left = 600,
            Top = 430,
            Width = 100,
        };

        eredmenyTextBox = new TextBox
        {
            Multiline = true,
            Location = new System.Drawing.Point(10, 60),
            Size = new System.Drawing.Size(250, 380),
            Font = new System.Drawing.Font("Consolas", 10),
            ReadOnly = true
        };
        
        var diagram = new DiagramRajzolo
        {
            Location = new System.Drawing.Point(270, 60)
        };
        this.Controls.Add(diagram);
        this.Controls.Add(eredmenyTextBox);
        this.Controls.Add(trzbaTextBox);

        buttonBack.Click += buttonBack_Click;
        this.Controls.Add(buttonBack);
        LoadTopTrzbaFromSmenna();
        mappa();
    }

    private void LoadTopTrzbaFromSmenna()
    {
        var smennaPath = Path.Combine(Environment.GetEnvironmentVariable("SystemDrive") ?? "C:", "Smenna");
        var smennaFiles = Directory.GetFiles(smennaPath, "*.txt")
                                   .OrderByDescending(f => f)
                                   .Take(5);

        decimal maxTrzba = decimal.MinValue;
        decimal minTrzba = decimal.MaxValue;
        string maxDate = "";
        string minDate = "";

        foreach (var file in smennaFiles)
        {
            var lines = File.ReadLines(file);
            decimal currentTrzba = 0;

            foreach (var line in lines)
            {
                if (line.Contains("Tržba"))
                {
                    var parts = line.Split(':');
                    if (parts.Length >= 2)
                    {
                        var trzbaStr = parts[1].Trim();
                        if (decimal.TryParse(trzbaStr.Replace("€", "").Trim(), out currentTrzba))
                        {
                            string date = Path.GetFileNameWithoutExtension(file);

                            if (currentTrzba > maxTrzba)
                            {
                                maxTrzba = currentTrzba;
                                maxDate = date;
                            }

                            if (currentTrzba < minTrzba)
                            {
                                minTrzba = currentTrzba;
                                minDate = date;
                            }
                        }
                    }
                }
            }
        }

        if (maxTrzba != decimal.MinValue && minTrzba != decimal.MaxValue)
        {
            trzbaTextBox.Text =
                $"Najväčší Tržba: {maxTrzba} € ({maxDate}){Environment.NewLine}" +
                $"Najmenší Tržba: {minTrzba} € ({minDate})";
        }
        else
        {
            trzbaTextBox.Text =
                "⚠️ Neexistujú údaje o najvyššom Tržbe.\n⚠️ Neexistujú údaje o najmenšom Tržbe.";
        }
    }


    private void mappa()
    {

        string gyokerMappa = @"C:\databaseBackups";
        Dictionary<string, List<(string termekNev, double eladas)>> mappakTermekek = new();

        var mappak = Directory.GetDirectories(gyokerMappa)
            .Where(mappa =>
            {
                string nev = Path.GetFileName(mappa);
                return DateTime.TryParseExact(nev, "yyyy-MM-dd_HH-mm-ss", null, System.Globalization.DateTimeStyles.None, out _);
            })
            .OrderByDescending(mappa =>
            {
                string nev = Path.GetFileName(mappa);
                DateTime.TryParseExact(nev, "yyyy-MM-dd_HH-mm-ss", null, System.Globalization.DateTimeStyles.None, out DateTime datum);
                return datum;
            })
            .Take(5);

        eredmenyTextBox.Clear();

        foreach (var mappa in mappak)
        {
            string[] jsonFajlokMappaban = Directory.GetFiles(mappa, "*.json");

            foreach (var file in jsonFajlokMappaban)
            {
                try
                {
                    string tartalom = File.ReadAllText(file);
                    JsonNode obj = JsonNode.Parse(tartalom);

                    if (obj is JsonObject jsonObj)
                    {
                        ProcessJsonObject(jsonObj, file, mappakTermekek);
                    }
                    else if (obj is JsonArray jsonArray)
                    {
                        foreach (JsonNode item in jsonArray)
                        {
                            if (item is JsonObject itemObj)
                            {
                                ProcessJsonObject(itemObj, file, mappakTermekek);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"CHYBA: {file}\n{ex.Message}\n");
                }
            }

            string datum = Path.GetFileName(mappa);
            if (mappakTermekek.ContainsKey(datum))
            {
                var top3 = mappakTermekek[datum].OrderByDescending(t => t.eladas).Take(3);

                eredmenyTextBox.AppendText($"📅 {datum}:{Environment.NewLine}");
                foreach (var termek in top3)
                {
                    eredmenyTextBox.AppendText($"   • {termek.termekNev} ({termek.eladas}){Environment.NewLine}");
                }
                eredmenyTextBox.AppendText(Environment.NewLine);
            }
        }
    }


    private void ProcessJsonObject(JsonObject jsonObj, string file, Dictionary<string, List<(string, double)>> mappakTermekek)
    {
        string nev = jsonObj["product_name"]?.ToString() ?? "Ismeretlen";

        double zosPred = SzamotKibont(jsonObj["zosPred"]);
        double prijem = SzamotKibont(jsonObj["prijem"]);
        double uzavZos = SzamotKibont(jsonObj["uzavZos"]);
        double eladas = zosPred + prijem - uzavZos;

        string datum = Path.GetFileName(Path.GetDirectoryName(file));

        if (!mappakTermekek.ContainsKey(datum))
        {
            mappakTermekek[datum] = new List<(string, double)>();
        }

        mappakTermekek[datum].Add((nev, eladas));
    }

    private double SzamotKibont(JsonNode node)
    {
        if (node == null) return 0;

        if (node is JsonObject obj)
        {
            if (obj.TryGetPropertyValue("$numberInt", out JsonNode intNode))
                return double.Parse(intNode.ToString());

            if (obj.TryGetPropertyValue("$numberDecimal", out JsonNode decNode))
                return double.Parse(decNode.ToString());
        }

        return 0;
    }
}





