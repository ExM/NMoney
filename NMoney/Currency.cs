using System;

namespace NMoney
{
	public class Currency: ICurrency
	{
		public Currency(string charCode, decimal mu, string sym = null)
		{
			if(charCode == null)
				throw new ArgumentNullException(nameof(charCode));
			
			CharCode = charCode;
			Symbol = sym ?? "¤";
			MinorUnit = mu;
		}
		
		public override string ToString()
		{
			return CharCode;
		}

		public virtual bool Equals(ICurrency other)
		{
			if (ReferenceEquals(other, null))
				return false;

			return ReferenceEquals(other, this);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as ICurrency);
		}

		public virtual string ToString(string format, IFormatProvider formatProvider)
		{
			return ToString();
		}

		public string CharCode { get; private set; }

		public string Symbol { get; private set; }

		public decimal MinorUnit { get; private set; }
	}
}

