using System.Xml.Serialization;

namespace NMoney.SourceCodeRenderer;

public class Specifications
{
    private const string _xmlSpecUri = "https://www.six-group.com/dam/download/financial-information/data-center/iso-currrency/lists/list-one.xml";
    
    private readonly string _inputPath;

    public Specifications(string inputPath)
    {
        _inputPath = inputPath;
    }

    public void Update()
    {
        Console.WriteLine($"Update specification from {_xmlSpecUri}");

        using var client = new HttpClient();
        
        using var s = client.GetStreamAsync(_xmlSpecUri).Result;
        using var fs = new FileStream(Path.Combine(_inputPath, "list_one.xml"), FileMode.Truncate);
        
        s.CopyTo(fs);
    }

    public IList<CurrencyEntry> LoadActualList() => LoadList("list_one.xml");

    public IList<CurrencyEntry> LoadObsoleteList() => LoadList("list_obsolete.xml");

    private IList<CurrencyEntry> LoadList(string fileName)
    {
        var serializer = new XmlSerializer(typeof(XmlCurrencyEntries));

        using var reader = new StreamReader(Path.Combine(_inputPath, fileName));
        
        var entries = (XmlCurrencyEntries)serializer.Deserialize(reader)!;

        return entries.List!
            .Where(e => !string.IsNullOrWhiteSpace(e.Code))
            .GroupBy(e => e.Code, StringComparer.OrdinalIgnoreCase)
            .Select(g =>
            {
                var e = g.First();
                return new CurrencyEntry()
                {
                    Code = e.Code!,
                    Name = e.Name ?? throw new Exception($"entry {e.Code} without name"),
                    NumCode = e.NumCode ?? throw new Exception($"entry {e.Code} without code"),
                    MinorUnit = XmlMinorUnitToDecimal(e.MinorUnit ?? throw new Exception($"entry {e.Code} without MinorUnit"))
                };
            })
            .OrderBy(e => e.Code, StringComparer.Ordinal)
            .ToList();
    }

    private static string XmlMinorUnitToDecimal(string xmlMinorUnit) =>
        xmlMinorUnit switch
        {
            "N.A." => "0m",
            "0" => "1m",
            "1" => "0.1m",
            "2" => "0.01m",
            "3" => "0.001m",
            "4" => "0.0001m",
            _ => throw new NotSupportedException("unexpected value: " + xmlMinorUnit)
        };
}