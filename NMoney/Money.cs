using System;

namespace NMoney
{
	/// <summary>
	/// This structure provides a amount of money in some currency
	/// </summary>
	public readonly struct Money : IComparable<Money>, IEquatable<Money>
	{
		/// <summary>
		/// amount of money
		/// </summary>
		public decimal Amount { get; }

		/// <summary>
		/// currency
		/// </summary>
		public ICurrency Currency { get; }

		/// <summary>
		/// creates a amount of money in any currency
		/// </summary>
		/// <param name="amount">
		/// amount of money
		/// </param>
		/// <param name="currency">
		/// currency
		/// </param>
		public Money(decimal amount, ICurrency currency)
			:this()
		{
			Amount = amount;
			Currency = currency ?? throw new ArgumentNullException(nameof(currency));
		}

		/// <summary>
		/// show amount and character code of currency
		/// </summary>
		public override string ToString()
		{
			if (noCurrency)
				return "0";
			return $"{Amount:G} {Currency.CharCode}";
		}

		/// <summary>
		/// contains an integer number of currency units
		/// </summary>
		public bool IsRounded
		{
			get
			{
				if (noCurrency || Currency.MinorUnit == 0m)
					return true;
				var mu = Amount/Currency.MinorUnit;
				return decimal.Truncate(mu) == mu;
			}
		}

		/// <summary>
		/// the total number minot of unit of currency
		/// </summary>
		public decimal TotalMinorUnit
		{
			get
			{
				if (noCurrency)
					return 0m;
				if(Currency.MinorUnit == 0m)
					throw new InvalidOperationException(string.Format("undefined minor unit in {0} currency", Currency.CharCode));
				return Amount/Currency.MinorUnit;
			}
		}

		/// <summary>
		/// Returns the largest integer less than or equal to minor of unit of this money.
		/// </summary>
		/// <returns>
		/// a new instance with the required amount or the current instance if the operation is not possible
		/// </returns>
		public Money FloorMinorUnit()
		{
			if (noCurrency)
				return Zero;
			if(Currency.MinorUnit == 0m)
				return this;
			return new Money(decimal.Floor(Amount/Currency.MinorUnit)*Currency.MinorUnit, Currency);
		}

		/// <summary>
		/// Returns the largest integer less than or equal to this money.
		/// </summary>
		/// <returns>
		/// a new instance with the required amount or the current instance if the operation is not possible
		/// </returns>
		public Money FloorMajorUnit()
		{
			if (noCurrency)
				return Zero;
			return new Money(decimal.Floor(Amount), Currency);
		}

		/// <summary>
		/// Returns the smallest integral value that is greater than or equal to minor of unit of this money.
		/// </summary>
		/// <returns>
		/// a new instance with the required amount or the current instance if the operation is not possible
		/// </returns>
		public Money CeilingMinorUnit()
		{
			if (noCurrency)
				return Zero;
			if(Currency.MinorUnit == 0m)
				return this;
			return new Money(decimal.Ceiling(Amount/Currency.MinorUnit)*Currency.MinorUnit, Currency);
		}

		/// <summary>
		/// Returns the smallest integral value that is greater than or equal to this money.
		/// </summary>
		/// <returns>
		/// a new instance with the required amount or the current instance if the operation is not possible
		/// </returns>
		public Money CeilingMajorUnit()
		{
			if (noCurrency)
				return Zero;
			return new Money(decimal.Ceiling(Amount), Currency);
		}

		/// <summary>
		/// mutiply amount
		/// </summary>
		public static Money operator *(Money lhs, decimal rhs)
		{
			if (lhs.noCurrency)
				return Zero;
			return new Money(rhs * lhs.Amount, lhs.Currency);
		}

		/// <summary>
		/// mutiply amount
		/// </summary>
		public static Money operator *(decimal lhs, Money rhs)
		{
			if (rhs.noCurrency)
				return Zero;
			return new Money(lhs * rhs.Amount, rhs.Currency);
		}

		/// <summary>
		/// divide amount
		/// </summary>
		public static Money operator /(Money lhs, decimal rhs)
		{
			if (lhs.noCurrency)
				return Zero;
			return new Money(lhs.Amount / rhs, lhs.Currency);
		}

		/// <summary>
		/// Determines whether the specified System.Object is equal to the current Money.
		/// </summary>
		public override bool Equals(object? obj)
		{
			if (obj is Money money)
				return Equals(money);
			
			return false;
		}

		/// <summary>
		/// Determines whether the specified Money is equal to the current Money.
		/// </summary>
		public bool Equals(Money other)
		{
			return Amount == other.Amount &&
				ReferenceEquals(Currency, other.Currency);
		}

		/// <summary>
		/// Compares two Money structures.
		/// </summary>
		public static bool operator ==(Money x, Money y)
		{
			return x.Equals(y);
		}

		/// <summary>
		/// Compares two Money structures.
		/// </summary>
		public static bool operator !=(Money x, Money y)
		{
			return !x.Equals(y);
		}

		/// <summary>
		/// Serves as a hash function for a particular type.
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			return Amount.GetHashCode() ^ (noCurrency ? 0 : Currency.GetHashCode());
		}

		/// <summary>
		///  Compares this instance to a specified AbbyyLS.Payments.Money object and returns a
		///     comparison of their relative values.
		/// </summary>
		public int CompareTo(Money other)
		{
			if(noCurrency)
				return 0m.CompareTo(other.Amount);

			if (other.noCurrency)
				return Amount.CompareTo(0m);

			if (!ReferenceEquals(Currency, other.Currency))
				throw new InvalidOperationException("mismatch currency");

			return Amount.CompareTo(other.Amount);
		}

		/// <summary>
		/// operator Less
		/// </summary>
		public static bool operator <(Money lhs, Money rhs)
		{
			return lhs.CompareTo(rhs) < 0;
		}

		/// <summary>
		/// operator Less or Equal
		/// </summary>
		public static bool operator <=(Money lhs, Money rhs)
		{
			return lhs.CompareTo(rhs) <= 0;
		}

		/// <summary>
		/// operator Great
		/// </summary>
		public static bool operator >(Money lhs, Money rhs)
		{
			return lhs.CompareTo(rhs) > 0;
		}

		/// <summary>
		/// operator Great or Equal
		/// </summary>
		public static bool operator >=(Money lhs, Money rhs)
		{
			return lhs.CompareTo(rhs) >= 0;
		}

		/// <summary>
		/// sum amount
		/// </summary>
		public static Money operator +(Money lhs, Money rhs)
		{
			if (lhs.noCurrency)
				return rhs;

			if (rhs.noCurrency)
				return lhs;

			if (!ReferenceEquals(lhs.Currency, rhs.Currency))
				throw new InvalidOperationException("mismatch currency");

			return new Money(lhs.Amount + rhs.Amount, lhs.Currency);
		}

		/// <summary>
		/// subtraction amount
		/// </summary>
		public static Money operator -(Money lhs, Money rhs)
		{
			if (rhs.noCurrency)
				return lhs;

			if (lhs.noCurrency)
				return new Money(- rhs.Amount, rhs.Currency);

			if (!ReferenceEquals(lhs.Currency, rhs.Currency))
				throw new InvalidOperationException("mismatch currency");

			return new Money(lhs.Amount - rhs.Amount, lhs.Currency);
		}

		private bool noCurrency => Equals(Currency, null);

		/// <summary>
		/// Default value
		/// </summary>
		public static readonly Money Zero = new Money();
	}
}

