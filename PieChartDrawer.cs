using System.Text.Json.Nodes;

public class DiagramRajzolo : Control
{
    private readonly Dictionary<string, double> kategoriakEladas = new();
    private readonly Dictionary<string, Color> kategoriakSzinei = new()
    {
        ["pivo"] = Color.Orange,
        ["nealko"] = Color.LightBlue,
        ["cigaretta"] = Color.Gray,
        ["destilaty"] = Color.Red,
        ["jedlo"] = Color.Green,
        ["vino"] = Color.Purple,
        ["ostatne"] = Color.Brown,
        ["napoje"] = Color.Gold
    };
    public DiagramRajzolo()
    {
        this.Size = new Size(600, 300);
        this.LoadEladasAdatok();
    }

    private void LoadEladasAdatok()
    {
        string gyokerMappa = @"C:\databaseBackups";

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

        foreach (var mappa in mappak)
        {
            string[] jsonFajlok = Directory.GetFiles(mappa, "*.json");

            foreach (var file in jsonFajlok)
            {
                string fajlNev = Path.GetFileNameWithoutExtension(file).ToLower(); 
                if (!kategoriakSzinei.ContainsKey(fajlNev))
                    continue;

                try
                {
                    string tartalom = File.ReadAllText(file);
                    JsonNode obj = JsonNode.Parse(tartalom);

                    if (obj is JsonObject jsonObj)
                    {
                        FeldolgozJson(jsonObj, fajlNev);
                    }
                    else if (obj is JsonArray tomb)
                    {
                        foreach (var elem in tomb)
                        {
                            if (elem is JsonObject itemObj)
                                FeldolgozJson(itemObj, fajlNev);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Chyba pri spracovaní súboru: {file}\n{ex.Message}");
                }
            }
        }
    }

    private void FeldolgozJson(JsonObject jsonObj, string kategoria)
    {
        double zosPred = SzamotKibont(jsonObj["zosPred"]);
        double prijem = SzamotKibont(jsonObj["prijem"]);
        double uzavZos = SzamotKibont(jsonObj["uzavZos"]);
        double eladas = zosPred + prijem - uzavZos;

        if (!kategoriakEladas.ContainsKey(kategoria))
            kategoriakEladas[kategoria] = 0;

        kategoriakEladas[kategoria] += eladas;
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

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);

        if (kategoriakEladas.Count == 0)
        {
            e.Graphics.DrawString("Žiadne údaje pre graf", Font, Brushes.Black, new PointF(10, 10));
            return;
        }

        double osszes = kategoriakEladas.Values.Sum();
        float startAngle = 0;

        Rectangle diagramRect = new Rectangle(10, 10, 250, 250); 
        foreach (var kvp in kategoriakEladas)
        {
            string kat = kvp.Key;
            double ertek = kvp.Value;

            float sweepAngle = (float)(ertek / osszes * 360);
            using Brush brush = new SolidBrush(kategoriakSzinei[kat]);

            e.Graphics.FillPie(brush, diagramRect, startAngle, sweepAngle);
            startAngle += sweepAngle;
        }

        int legendaY = 60;
        foreach (var kvp in kategoriakEladas)
        {
            string kat = kvp.Key;
            Color szin = kategoriakSzinei[kat];

            e.Graphics.FillRectangle(new SolidBrush(szin), 300, legendaY, 20, 10);
            e.Graphics.DrawString($"{kat} ({kvp.Value:F0})", Font, Brushes.Black, 325, legendaY - 2);

            legendaY += 15;
        }
    }
}
