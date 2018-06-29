using System.Collections.Generic;

namespace NMoney
{
	/// <summary>
	/// Supported currency collection in your application or any serializer
	/// </summary>
	public interface ICurrencySet<out T>: ICurrencySet where T: class, ICurrency
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
		new T TryParse(string charCode);

		/// <summary>
		/// All contained curencies
		/// </summary>
		new IReadOnlyCollection<T> AllCurencies { get; }
	}
}
