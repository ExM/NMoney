using System;

namespace NMoney
{
	/// <summary>
	/// interface providing information on currency
	/// </summary>
	public interface ICurrency: IEquatable<ICurrency>, IFormattable
	{
		/// <summary>
		/// character code
		/// </summary>
		string CharCode {get;}
		
		/// <summary>
		/// symbol of currency
		/// </summary>
		string Symbol {get;}
		
		/// <summary>
		/// minor of unit scale
		/// </summary>
		decimal MinorUnit {get;}
	}
}

