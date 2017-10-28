using System;
using System.Collections.Generic;

namespace NMoney.Iso4217
{
	/// <summary>
	/// The currencies standard ISO 4217
	/// </summary>
	public partial class CurrencySet : NMoney.CurrencySet<Iso4217.Currency>
	{
		private static readonly Lazy<CurrencySet> _lazy = new Lazy<CurrencySet>(() => new CurrencySet(), true);

		public static CurrencySet Instance => _lazy.Value;

		private Dictionary<int, Iso4217.Currency> _numMap;

		private CurrencySet() : base(GetAll())
		{
			_numMap = new Dictionary<int, Iso4217.Currency>(AllCurencies.Count);
			foreach (var c in AllCurencies)
				_numMap.Add(c.NumCode, c);
		}

		/// <summary>
		/// Parse number code of currency in ISO4217
		/// </summary>
		/// <param name="numCode">
		/// number code of currency
		/// </param>
		/// <returns>
		/// null if not found<see cref="ICurrency"/>
		/// </returns>
		public Currency TryParse(int numCode)
		{
			Currency currency;
			_numMap.TryGetValue(numCode, out currency);
			return currency;
		}

		/// <summary>
		/// Return currency from number code
		/// </summary>
		/// <param name="numCode">
		/// number code
		/// </param>
		/// <exception cref="System.NotSupportedException">if ISO 4217 not contain this currency</exception>
		public ICurrency Parse(int numCode)
		{
			var currency = TryParse(numCode);
			if (currency != null)
				return currency;

			throw new NotSupportedException("currency code '" + numCode + "' not supported in ISO4217");
		}
		
		/// <summary>
		/// Return currency from number code
		/// </summary>
		/// <param name="numCode">
		/// number code
		/// </param>
		/// <param name="currency">
		/// currency
		/// </param>
		/// <returns>
		/// true if ISO 4217 contain this currency
		/// </returns>
		public bool TryParse(int numCode, out ICurrency currency)
		{
			currency = TryParse(numCode);
			return currency != null;
		}
	}
}