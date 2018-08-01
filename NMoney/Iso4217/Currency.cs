using System;
using System.Resources;
using System.Globalization;

namespace NMoney.Iso4217
{
	/// <summary>
	/// Base class that represents a currency from ISO 4217 
	/// </summary>
	public class Currency: NMoney.Currency
	{
		private static readonly ResourceManager _rMan = new ResourceManager("NMoney.Iso4217.Names", typeof(Currency).Assembly);

		/// <inheritdoc />
		public Currency(string charCode, string sym, int num, decimal mu)
			:base(charCode, mu, sym)
		{
			NumCode = num;
		}
		
		/// <summary>
		/// Initialize of copy of exists currency
		/// </summary>
		public Currency(Currency source)
			:this(source.CharCode, source.Symbol, source.NumCode, source.MinorUnit)
		{
		}

		/// <inheritdoc />
		public override string ToString()
		{
			return ToString("n", null);
		}

		/// <inheritdoc />
		public override string ToString(string format, IFormatProvider formatProvider)
		{
			switch (format)
			{
				case "s":
					return Symbol;
				case "c":
					return CharCode;
				case null:
				case "":
				case "n":
					return GetLocalizedName(formatProvider as CultureInfo);
				default:
					throw new FormatException($"unexpected format '{format}'");
			}
		}

		/// <summary>
		/// Number code of currency from ISO 4217
		/// </summary>
		public int NumCode { get; }

		/// <summary>
		/// Get localized name of currency
		/// </summary>
		protected virtual string GetLocalizedName(CultureInfo cultureInfo)
		{
			return _rMan.GetString(CharCode, cultureInfo);
		}
	}
}

