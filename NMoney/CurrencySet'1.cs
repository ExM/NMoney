using System;
using System.Collections.Generic;

namespace NMoney
{
	/// <summary>
	/// Supported currency collection in your application or any serializer
	/// </summary>
	public class CurrencySet<T>: ICurrencySet<T> where T: class, ICurrency
	{
		private readonly IReadOnlyCollection<T> _currencies;
		private readonly Dictionary<string, T> _codeMap;

		/// <summary>
		/// Supported currency collection in your application or any serializer
		/// </summary>
		public CurrencySet(IReadOnlyCollection<T> currencies)
		{
			_currencies = currencies ?? throw new ArgumentNullException(nameof(currencies));
			_codeMap = new Dictionary<string, T>(currencies.Count, StringComparer.OrdinalIgnoreCase);
			foreach (var c in currencies)
				_codeMap.Add(c.CharCode, c);
		}

		/// <inheritdoc />
		public IReadOnlyCollection<T> AllCurencies => _currencies;

		/// <inheritdoc />
		public T? TryParse(string charCode)
		{
			_codeMap.TryGetValue(charCode, out var currency);
			return currency;
		}

		IReadOnlyCollection<ICurrency> ICurrencySet.AllCurencies => _currencies;

		ICurrency? ICurrencySet.TryParse(string charCode)
		{
			_codeMap.TryGetValue(charCode, out var currency);
			return currency;
		}
	}
}
