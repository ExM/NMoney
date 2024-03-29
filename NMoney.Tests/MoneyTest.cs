using System;
using NUnit.Framework;
using System.Threading;
using System.Globalization;

namespace NMoney
{
	[TestFixture]
	public class MoneyTest
	{
		[Test]
		public void InvalidArgsInConstructor()
		{
			Assert.Throws<ArgumentNullException>(() =>
			{
				var mu = new Money(99.99m, null);
			});
		}

		[TestCase("USD", -0.991)]
		[TestCase("USD", 20.999)]
		[TestCase("JPY", 21.00001)]
		public void NotRounded(string code, decimal amount)
		{
			Assert.That(Iso4217.CurrencySet.Instance.Parse(code).Money(amount).IsRounded, Is.False);
		}

		[TestCase("USD", 20.95)]
		[TestCase("USD", 20.5)]
		[TestCase("USD", -20)]
		[TestCase("USD", -0.99)]
		[TestCase("JPY", 20)]
		[TestCase("JPY", 21)]
		[TestCase("XAU", 21.00001)]
		public void IsRounded(string code, decimal amount)
		{
			Assert.That(Iso4217.CurrencySet.Instance.Parse(code).Money(amount).IsRounded, Is.True);
		}
		
		[TestCase("USD", 20.95, 2095)]
		[TestCase("USD", 20.5, 2050)]
		[TestCase("USD", -20, -2000)]
		[TestCase("USD", -0.99, -99)]
		[TestCase("USD", -0.991, -99.1)]
		[TestCase("USD", 20.999, 2099.9)]
		[TestCase("JPY", 20, 20)]
		[TestCase("JPY", 21, 21)]
		[TestCase("JPY", 21.00001, 21.00001)]
		public void TotalMinorUnit(string code, decimal amount, decimal exp)
		{
			Assert.That(Iso4217.CurrencySet.Instance.Parse(code).Money(amount).TotalMinorUnit, Is.EqualTo(exp));
		}
		
		[TestCase("USD", 20.95, 20.95)]
		[TestCase("USD", 20.953, 20.95)]
		[TestCase("USD", -20.953, -20.96)]
		[TestCase("JPY", 20, 20)]
		[TestCase("JPY", 20.1, 20)]
		[TestCase("JPY", 20.00001, 20)]
		[TestCase("JPY", -20.00001, -21)]
		[TestCase("XDR", 20.00001, 20.00001)]
		public void FloorMinorUnit(string code, decimal amount, decimal exp)
		{
			Assert.That(Iso4217.CurrencySet.Instance.Parse(code).Money(amount).FloorMinorUnit().Amount, Is.EqualTo(exp));
		}
		
		[TestCase("USD", 20.95, 20.95)]
		[TestCase("USD", 20.953, 20.96)]
		[TestCase("USD", -20.953, -20.95)]
		[TestCase("JPY", 20, 20)]
		[TestCase("JPY", 20.1, 21)]
		[TestCase("JPY", 20.00001, 21)]
		[TestCase("JPY", -20.00001, -20)]
		[TestCase("XDR", 20.00001, 20.00001)]
		[TestCase("BYN", 20.001, 20.01)]
		public void CeilingMinorUnit(string code, decimal amount, decimal exp)
		{
			Assert.That(Iso4217.CurrencySet.Instance.Parse(code).Money(amount).CeilingMinorUnit().Amount, Is.EqualTo(exp));
		}
		
		[TestCase("USD", 20, 20)]
		[TestCase("USD", 20.953, 20)]
		[TestCase("USD", -20.953, -21)]
		[TestCase("JPY", 20, 20)]
		[TestCase("JPY", 20.1, 20)]
		[TestCase("JPY", 20.00001, 20)]
		[TestCase("JPY", -20.00001, -21)]
		[TestCase("BYN", 20.015, 20)]
		public void FloorMajorUnit(string code, decimal amount, decimal exp)
		{
			Assert.That(Iso4217.CurrencySet.Instance.Parse(code).Money(amount).FloorMajorUnit().Amount, Is.EqualTo(exp));
		}
		
		[TestCase("USD", 20, 20)]
		[TestCase("USD", 20.953, 21)]
		[TestCase("USD", -20.953, -20)]
		[TestCase("JPY", 20, 20)]
		[TestCase("JPY", 20.1, 21)]
		[TestCase("JPY", 20.00001, 21)]
		[TestCase("JPY", -20.00001, -20)]
		public void CeilingMajorUnit(string code, decimal amount, decimal exp)
		{
			Assert.That(Iso4217.CurrencySet.Instance.Parse(code).Money(amount).CeilingMajorUnit().Amount, Is.EqualTo(exp));
		}
		
		[Test]
		public void NoTotalMinorUnit()
		{
			Assert.Throws<InvalidOperationException>(() =>
			{
				var mu = Iso4217.CurrencySet.XAU.Money(99.99m).TotalMinorUnit;
			});
		}
		
		[Test]
		public void Equal()
		{
			var m1 = Iso4217.CurrencySet.RUB.Money(1.23m);
			var m1E = Iso4217.CurrencySet.RUB.Money(1.23m);
			var m2 = Iso4217.CurrencySet.RUB.Money(1.24m);
			var m3 = Iso4217.CurrencySet.USD.Money(1.23m);

			Assert.That(m1, Is.EqualTo(m1));
			Assert.That(m1, Is.Not.EqualTo(m2));
			Assert.That(m3, Is.Not.EqualTo(m1));

			Assert.That(m1 == m1E, Is.True);
			Assert.That(m1 != m1E, Is.False);

			Assert.That(m1 != m3, Is.True);
			Assert.That(m2 != m1, Is.True);

			Assert.That(m1.Equals((object)m3), Is.False);
			Assert.That(m1.Equals((object)"123"), Is.False);
			Assert.That(m1.Equals((object)m1E), Is.True);
		}

		[Test]
		public void ZeroAmountEquality()
		{
			var rub1 = Iso4217.CurrencySet.RUB.Money(0m);
			var rub2 = Iso4217.CurrencySet.RUB.Money(0m);
			var usd = Iso4217.CurrencySet.USD.Money(0m);

			Assert.That(rub2, Is.EqualTo(rub1));
			Assert.That(usd, Is.Not.EqualTo(rub1));
			Assert.That(usd, Is.Not.EqualTo(rub2));
			Assert.That(Money.Zero, Is.Not.EqualTo(rub1));
			Assert.That(Money.Zero, Is.Not.EqualTo(usd));
		}

		[Test]
		public void Division()
		{
			Assert.That(Iso4217.CurrencySet.RUB.Money(2.46m) / 2, Is.EqualTo(Iso4217.CurrencySet.RUB.Money(1.23m)));
		}

		[Test]
		public void Multiply()
		{
			Assert.That(Iso4217.CurrencySet.RUB.Money(1.23m) * 2, Is.EqualTo(Iso4217.CurrencySet.RUB.Money(2.46m)));
			Assert.That(2 * Iso4217.CurrencySet.RUB.Money(1.23m), Is.EqualTo(Iso4217.CurrencySet.RUB.Money(2.46m)));
		}

		[Test]
		public void MultiplyWithZero()
		{
			Assert.That((Iso4217.CurrencySet.RUB.Money(1.23m) * 0).Currency, Is.EqualTo(Iso4217.CurrencySet.RUB));
			Assert.That((0 * Iso4217.CurrencySet.RUB.Money(1.23m)).Currency, Is.EqualTo(Iso4217.CurrencySet.RUB));
		}
		
		[Test]
		public void Show()
		{
			var ci = CultureInfo.InvariantCulture;
			Thread.CurrentThread.CurrentCulture = ci;
			Thread.CurrentThread.CurrentUICulture = ci;
			
			Assert.That(Iso4217.CurrencySet.RUB.Money(1.23m).ToString(), Is.EqualTo("1.23 RUB"));
			Assert.That(Iso4217.CurrencySet.USD.Money(1.123456789012345678900876523m).ToString(), Is.EqualTo("1.123456789012345678900876523 USD"));
		}

		[Test]
		public void InvalidMatch()
		{
			var l = Iso4217.CurrencySet.EUR.Money(1.23m);
			var r = Iso4217.CurrencySet.RUB.Money(2.23m);

			Assert.Throws<InvalidOperationException>(() =>
			{
				var res = l < r;
			});
		}

		[Test]
		public void InvalidMatchZero()
		{
			var l = Iso4217.CurrencySet.EUR.Money(0m);
			var r = Iso4217.CurrencySet.RUB.Money(0m);
			Assert.Throws<InvalidOperationException>(() =>
			{
				var res = l < r;
			});
		}

		[Test]
		public void MatchOperators()
		{
			var l = Iso4217.CurrencySet.RUB.Money(1.23m);
			var r = Iso4217.CurrencySet.RUB.Money(2.23m);
			var rE = Iso4217.CurrencySet.RUB.Money(2.23m);

			Assert.That(l < r, Is.True);
			Assert.That(l <= r, Is.True);
			Assert.That(r <= rE, Is.True);

			Assert.That(r < l, Is.False);
			Assert.That(r <= l, Is.False);

			Assert.That(l > r, Is.False);
			Assert.That(l >= r, Is.False);
			Assert.That(r >= rE, Is.True);

			Assert.That(r > l, Is.True);
			Assert.That(r >= l, Is.True);
		}

		[Test]
		public void InvalidAdditional()
		{
			var m1 = Iso4217.CurrencySet.EUR.Money(1m);
			var m2 = Iso4217.CurrencySet.RUB.Money(2m);
			Assert.Throws<InvalidOperationException>(() =>
			{
				var res = m1 + m2;
			});
		}

		[Test]
		public void Additional()
		{
			var m1 = Iso4217.CurrencySet.EUR.Money(1m);
			var m2 = Iso4217.CurrencySet.EUR.Money(2m);
			var m3 = Iso4217.CurrencySet.EUR.Money(3m);

			Assert.That(m1 + m2, Is.EqualTo(m3));
		}

		[Test]
		public void InvalidSubtract()
		{
			var m1 = Iso4217.CurrencySet.EUR.Money(1m);
			var m2 = Iso4217.CurrencySet.RUB.Money(2m);
			Assert.Throws<InvalidOperationException>(() =>
			{
				var res = m1 - m2;
			});
		}

		[Test]
		public void Subtract()
		{
			var m1 = Iso4217.CurrencySet.EUR.Money(1m);
			var m2 = Iso4217.CurrencySet.EUR.Money(2m);
			var m3 = Iso4217.CurrencySet.EUR.Money(-1m);

			Assert.That(m1 - m2, Is.EqualTo(m3));

			Assert.That((m1 - m1).Currency, Is.EqualTo(Iso4217.CurrencySet.EUR));
		}

		[Test]
		public void HashCode()
		{
			var m1 = Iso4217.CurrencySet.RUB.Money(1m).GetHashCode();
			var m1E = Iso4217.CurrencySet.RUB.Money(1m).GetHashCode();
			var m2 = Iso4217.CurrencySet.EUR.Money(2m).GetHashCode();
			var m3 = Iso4217.CurrencySet.EUR.Money(-1m).GetHashCode();

			Assert.That(m1E, Is.EqualTo(m1));
			Assert.That(m2, Is.Not.EqualTo(m1));
			Assert.That(m2, Is.Not.EqualTo(m3));
		}
	}
}

