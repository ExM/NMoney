using System.Collections.Generic;

namespace NMoney
{
	/// <summary>
	/// Supported currency collection in your application or any serializer
	/// </summary>
	public class CurrencySet: CurrencySet<ICurrency>
	{
		/// <inheritdoc />
		public CurrencySet(IReadOnlyCollection<ICurrency> currencies)
			:base(currencies)
		{
		}
	}
}
