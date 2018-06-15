﻿using System;
using System.Collections.Generic;

namespace NMoney
{
	/// <summary>
	/// Supported currency collection in your application or any serializer
	/// </summary>
	public class CurrencySet<T>: ICurrencySet<T> where T: class, ICurrency
	{
		private IReadOnlyCollection<T> _currencies;
		private Dictionary<string, T> _codeMap;

		/// <inheritdoc />
		public CurrencySet(IReadOnlyCollection<T> currencies)
		{
			if (ReferenceEquals(currencies, null))
				throw new ArgumentNullException(nameof(currencies));
			_currencies = currencies;
			_codeMap = new Dictionary<string, T>(currencies.Count, StringComparer.OrdinalIgnoreCase);
			foreach (var c in currencies)
				_codeMap.Add(c.CharCode, c);
		}

		/// <inheritdoc />
		public IReadOnlyCollection<T> AllCurencies => _currencies;

		/// <inheritdoc />
		public T TryParse(string charCode)
		{
			T currency;
			_codeMap.TryGetValue(charCode, out currency);
			return currency;
		}

		IReadOnlyCollection<ICurrency> ICurrencySet.AllCurencies => _currencies;

		ICurrency ICurrencySet.TryParse(string charCode)
		{
			T currency;
			_codeMap.TryGetValue(charCode, out currency);
			return currency;
		}
	}
}