using System;

namespace NMoney
{
	/// <summary>
	/// interface providing information on currency
	/// </summary>
	public interface ICurrency: IFormattable
	{
		/// <summary>
		/// Character code of currency
		/// </summary>
		string CharCode {get;}
		
		/// <summary>
		/// Symbol of currency
		/// </summary>
		string Symbol {get;}
		
		/// <summary>
		/// Minor of unit scale
		/// </summary>
		decimal MinorUnit {get;}
	}
}

