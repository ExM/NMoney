using System;

namespace NMoney
{
	/// <summary>
	/// any extensions
	/// </summary>
	public static class CurrencySetExtensions
	{
		/// <summary>
		/// Currency set is contains specified currency
		/// </summary>
		public static bool Contain(this ICurrencySet currencySet, ICurrency currency)
		{
			var candidate = currencySet.TryParse(currency.CharCode);
			return candidate != null && ReferenceEquals(candidate, currency);
		}

		/// <summary>
		/// Currency set is contains specified currency
		/// </summary>
		public static bool Contain<T>(this ICurrencySet<T> currencySet, T currency) where T: class, ICurrency
		{
			var candidate = currencySet.TryParse(currency.CharCode);
			return candidate != null && ReferenceEquals(candidate, currency);
		}

		/// <summary>
		/// Currency set is contains an instance of a character code
		/// </summary>
		/// <param name="currencySet">this currency set</param>
		/// <param name="charCode">
		/// character code
		/// </param>
		public static bool Contain<T>(this ICurrencySet<T> currencySet, string charCode) where T : class, ICurrency
		{
			return currencySet.TryParse(charCode) != null;
		}

		/// <summary>
		/// Return currency from character code
		/// </summary>
		/// <param name="currencySet">this currency set</param>
		/// <param name="charCode">
		/// character code
		/// </param>
		/// <exception cref="System.NotSupportedException">if currrency set is not contain this currency</exception>
		public static ICurrency Parse<T>(this ICurrencySet<T> currencySet, string charCode) where T : class, ICurrency
		{
			var currency = currencySet.TryParse(charCode);
			if (currency != null)
				return currency;

			throw new NotSupportedException("currency code '" + charCode + "' not supported");
		}

		/// <summary>
		/// Return currency from character code
		/// </summary>
		/// <param name="currencySet">
		/// currency set
		/// </param>
		/// <param name="charCode">
		/// character code
		/// </param>
		/// <param name="currency">
		/// currency
		/// </param>
		/// <returns>
		/// true if set contain this currency
		/// </returns>
		public static bool TryParse<T>(this ICurrencySet<T> currencySet, string charCode, out T currency) where T : class, ICurrency
		{
			currency = currencySet.TryParse(charCode);
			return currency != null;
		}
	}
}

