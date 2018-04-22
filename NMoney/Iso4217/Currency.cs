using System;
using System.Resources;
using System.Globalization;

namespace NMoney.Iso4217
{
	public class Currency: NMoney.Currency
	{
		private static readonly ResourceManager _rMan = new ResourceManager("NMoney.Iso4217.Names", typeof(Currency).Assembly);

		private readonly int _numCode;
		
		internal Currency(string charCode, string sym, int num, decimal mu)
			:base(charCode, sym, mu)
		{
			_numCode = num;
		}
		
		public override string ToString()
		{
			return ToString("n", null);
		}

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
					return _rMan.GetString(CharCode, formatProvider as CultureInfo);
				default:
					throw new FormatException($"unexpected format '{format}'");
			}
		}

		public int NumCode => _numCode;
	}
}

