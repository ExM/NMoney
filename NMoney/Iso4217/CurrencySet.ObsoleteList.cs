using System;
using System.Collections.Generic;

namespace NMoney.Iso4217
{
	public partial class CurrencySet
	{
		/// <summary>
		/// Ouguiya
		/// </summary>
		[Obsolete]
		public static Currency MRO => MROCache.Instance;
		/// <summary>
		/// Dobra
		/// </summary>
		[Obsolete]
		public static Currency STD => STDCache.Instance;

		
		private static class MROCache
		{
			internal static readonly Currency Instance = new ObsoleteCurrency("MRO", "UM", 478, 0.01m);
		}

		private static class STDCache
		{
			internal static readonly Currency Instance = new ObsoleteCurrency("STD", "Db", 678, 0.01m);
		}
	}
}
