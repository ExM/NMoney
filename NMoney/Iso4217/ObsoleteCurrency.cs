using System;
using System.Resources;
using System.Globalization;

namespace NMoney.Iso4217
{
	/// <summary>
	/// Base class that represents a currency from ISO 4217 
	/// </summary>
	internal class ObsoleteCurrency: Currency
	{
		private static readonly ResourceManager _rMan = new ResourceManager("NMoney.Iso4217.Names.Obsolete", typeof(Currency).Assembly);

		public ObsoleteCurrency(string charCode, string sym, int num, decimal mu)
			:base(charCode, sym, num, mu)
		{
		}

		protected override string GetLocalizedName(CultureInfo? cultureInfo)
		{
			return _rMan.GetString(CharCode, cultureInfo) ?? CharCode;
		}
	}
}

