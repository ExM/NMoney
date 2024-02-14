namespace NMoney.SourceCodeRenderer;

public class NameOverride
{
    private readonly Dictionary<string, string> _map;

    public NameOverride(string csvFile)
    {
        _map = new Dictionary<string, string>();

        foreach(var line in File.ReadAllLines(csvFile).Skip(1))
        {
            var cells = line.Split(';');
            _map.Add(cells[0], cells[1]);
        }
    }

    public string? Get(string code) => _map.GetValueOrDefault(code);
}