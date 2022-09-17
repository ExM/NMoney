using System.Collections.Generic;

namespace NMoney
{
	/// <summary>
	/// Supported currency collection in your application or serializer
	/// </summary>
	public interface ICurrencySet
	{
		/// <summary>
		/// Parse char code of currency in this set
		/// </summary>
		/// <param name="charCode">
		/// char code of currency
		/// </param>
		/// <returns>
		/// null if not found
		/// </returns>
		ICurrency? TryParse(string charCode);

		/// <summary>
		/// All contained curencies
		/// </summary>
		IReadOnlyCollection<ICurrency> AllCurencies { get; }
	}
}
