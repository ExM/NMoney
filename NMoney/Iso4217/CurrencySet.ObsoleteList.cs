using System;

namespace NMoney.Iso4217
{
	public partial class CurrencySet
	{
		/// <summary>
		/// Netherlands Antillean Guilder
		/// </summary>
		[Obsolete]
		public static Currency ANG => ANGCache.Instance;
		/// <summary>
		/// Belarusian Ruble
		/// </summary>
		[Obsolete]
		public static Currency BYR => BYRCache.Instance;
		/// <summary>
		/// Peso Convertible
		/// </summary>
		[Obsolete]
		public static Currency CUC => CUCCache.Instance;
		/// <summary>
		/// Kuna
		/// </summary>
		[Obsolete]
		public static Currency HRK => HRKCache.Instance;
		/// <summary>
		/// Ouguiya
		/// </summary>
		[Obsolete]
		public static Currency MRO => MROCache.Instance;
		/// <summary>
		/// Leone
		/// </summary>
		[Obsolete]
		public static Currency SLL => SLLCache.Instance;
		/// <summary>
		/// Dobra
		/// </summary>
		[Obsolete]
		public static Currency STD => STDCache.Instance;
		/// <summary>
		/// Bolívar
		/// </summary>
		[Obsolete]
		public static Currency VEF => VEFCache.Instance;
		/// <summary>
		/// Zimbabwe Dollar
		/// </summary>
		[Obsolete]
		public static Currency ZWL => ZWLCache.Instance;

		
		private static class ANGCache
		{
			internal static readonly Currency Instance = new ObsoleteCurrency("ANG", "ƒ", 532, 0.01m);
		}

		private static class BYRCache
		{
			internal static readonly Currency Instance = new ObsoleteCurrency("BYR", "¤", 974, 1m);
		}

		private static class CUCCache
		{
			internal static readonly Currency Instance = new ObsoleteCurrency("CUC", "$", 931, 0.01m);
		}

		private static class HRKCache
		{
			internal static readonly Currency Instance = new ObsoleteCurrency("HRK", "kn", 191, 0.01m);
		}

		private static class MROCache
		{
			internal static readonly Currency Instance = new ObsoleteCurrency("MRO", "UM", 478, 0.01m);
		}

		private static class SLLCache
		{
			internal static readonly Currency Instance = new ObsoleteCurrency("SLL", "Le", 694, 0.01m);
		}

		private static class STDCache
		{
			internal static readonly Currency Instance = new ObsoleteCurrency("STD", "Db", 678, 0.01m);
		}

		private static class VEFCache
		{
			internal static readonly Currency Instance = new ObsoleteCurrency("VEF", "Bs", 937, 0.01m);
		}

		private static class ZWLCache
		{
			internal static readonly Currency Instance = new ObsoleteCurrency("ZWL", "¤", 932, 0.01m);
		}
	}
}
