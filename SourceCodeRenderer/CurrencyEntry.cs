namespace NMoney.SourceCodeRenderer;

public class CurrencyEntry
{
    public required string Code { get; init; }

    public required int NumCode { get; init; }

    public required string Name { get; init; }

    public required string MinorUnit { get; init; }
}