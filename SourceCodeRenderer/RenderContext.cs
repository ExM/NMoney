
namespace NMoney.SourceCodeRenderer
{
	public abstract class RenderContext: TemplateClassBase
	{
		public IList<CurrencyEntry>? ActualCurrencies { get; set; }
		public IList<CurrencyEntry>? ObsoleteCurrencies { get; set; }
		public CurrencySymbols? CurrencySymbols { get; set; }
		public NameOverride? NameOverride { get; set; }
	}
}
