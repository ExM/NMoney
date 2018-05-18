using System;

namespace NMoney
{
	/// <summary>
	/// Base class that represents a currency
	/// </summary>
	public class Currency: ICurrency
	{
		/// <summary>
		/// Base class that represents a currency
		/// </summary>
		public Currency(string charCode, decimal mu, string sym = null)
		{
			if(charCode == null)
				throw new ArgumentNullException(nameof(charCode));
			
			CharCode = charCode;
			Symbol = sym ?? "¤";
			MinorUnit = mu;
		}
		
		/// <summary>
		/// Converts this <see cref="Money"/> instance to its equivalent <see cref="string"/> representation.
		/// </summary>
		public override string ToString()
		{
			return CharCode;
		}

		/// <inheritdoc />
		public virtual string ToString(string format, IFormatProvider formatProvider)
		{
			return ToString();
		}

		/// <inheritdoc />
		public string CharCode { get; private set; }

		/// <inheritdoc />
		public string Symbol { get; private set; }

		/// <inheritdoc />
		public decimal MinorUnit { get; private set; }
	}
}

