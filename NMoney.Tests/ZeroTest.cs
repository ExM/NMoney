using System;
using NUnit.Framework;
using System.Threading;
using System.Globalization;

namespace NMoney
{
	[TestFixture]
	public class ZeroTest
	{
		[Test]
		public void CreateWithNoCurrency()
		{
			Assert.Throws<ArgumentNullException>(() => new Money(0m, null));
		}

		[Test]
		public void IsRounded()
		{
			Assert.That(Money.Zero.IsRounded, Is.True);
		}
		
		[Test]
		public void TotalMinorUnit()
		{
			Assert.That(Money.Zero.TotalMinorUnit, Is.EqualTo(0));
		}
		
		[Test]
		public void FloorMinorUnit()
		{
			Assert.That(Money.Zero.FloorMinorUnit(), Is.EqualTo(Money.Zero));
		}
		
		[Test]
		public void CeilingMinorUnit()
		{
			Assert.That(Money.Zero.CeilingMinorUnit(), Is.EqualTo(Money.Zero));
		}
		
		[Test]
		public void FloorMajorUnit()
		{
			Assert.That(Money.Zero.FloorMajorUnit(), Is.EqualTo(Money.Zero));
		}
		
		[Test]
		public void CeilingMajorUnit()
		{
			Assert.That(Money.Zero.CeilingMajorUnit(), Is.EqualTo(Money.Zero));
		}
		
		[Test]
		public void Equal()
		{
			Assert.That(Money.Zero, Is.Not.EqualTo(Iso4217.CurrencySet.RUB.Money(0m)));
			Assert.That(Money.Zero, Is.EqualTo(Money.Zero));
		}
		
		[Test]
		public void Muliply()
		{
			var m = Iso4217.CurrencySet.EUR.Money(1.23m);
			Assert.That(Money.Zero * 2, Is.EqualTo(Money.Zero));
			Assert.That(2 * Money.Zero, Is.EqualTo(Money.Zero));
			Assert.That(m * 0, Is.Not.EqualTo(Money.Zero));
			Assert.That(0 * m, Is.Not.EqualTo(Money.Zero));
		}

		[Test]
		public void Division()
		{
			Assert.That(Money.Zero / 2, Is.EqualTo(Money.Zero));
		}

		[Test]
		public void Additional()
		{
			var m = Iso4217.CurrencySet.EUR.Money(1.23m);
			Assert.That(Money.Zero + Money.Zero, Is.EqualTo(Money.Zero));
			Assert.That(m + Money.Zero, Is.EqualTo(m));
			Assert.That(Money.Zero + m, Is.EqualTo(m));
		}

		[Test]
		public void Subtract()
		{
			var m = Iso4217.CurrencySet.EUR.Money(1.23m);
			Assert.That(Money.Zero - Money.Zero, Is.EqualTo(Money.Zero));
			Assert.That(m - m, Is.Not.EqualTo(Money.Zero));
			Assert.That(m - Money.Zero, Is.EqualTo(m));
			Assert.That((Money.Zero - m).Amount, Is.EqualTo(-m.Amount));
		}
		
		[Test]
		public void Show()
		{
			var ci = CultureInfo.InvariantCulture;
			Thread.CurrentThread.CurrentCulture = ci;
			Thread.CurrentThread.CurrentUICulture = ci;

			Assert.That(Money.Zero.ToString(), Is.EqualTo("0"));
		}

		[Test]
		public void MatchOperators()
		{
			var l = Money.Zero;
			var lE = Money.Zero;
			var r = Iso4217.CurrencySet.RUB.Money(2.23m);

			Assert.That(l < r, Is.True);
			Assert.That(l <= r, Is.True);
			Assert.That(l <= lE, Is.True);

			Assert.That(r < l, Is.False);
			Assert.That(r <= l, Is.False);

			Assert.That(l > r, Is.False);
			Assert.That(l >= r, Is.False);
			Assert.That(l >= lE, Is.True);

			Assert.That(r > l, Is.True);
			Assert.That(r >= l, Is.True);
		}

		[Test]
		public void HashCode()
		{
			var m1 = Money.Zero.GetHashCode();
			var m1E = (Money.Zero * 2).GetHashCode();
			var m2 = Iso4217.CurrencySet.RUB.Money(1m).GetHashCode();
			var m3 = (Iso4217.CurrencySet.RUB.Money(1m) * 0).GetHashCode();

			Assert.That(m1E, Is.EqualTo(m1));
			Assert.That(m2, Is.Not.EqualTo(m1));
			Assert.That(m3, Is.Not.EqualTo(m1));
		}
	}
}

