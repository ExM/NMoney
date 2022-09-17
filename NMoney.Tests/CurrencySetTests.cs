using System;
using NUnit.Framework;

namespace NMoney
{
	[TestFixture]
	public class CurrencySetTests
	{
		private readonly Currency _xa = new Currency("XA", 0.01m, "a");
		private readonly Currency _xb = new Currency("XB", 0.01m, "b");
		private readonly Currency _xc = new Currency("XC", 0.01m, "c");

		[Test]
		public void Create()
		{
			ICurrencySet set = new CurrencySet(new[] { _xa, _xb, _xc });

			Assert.That(set.AllCurencies.Count, Is.EqualTo(3));

			Assert.That(set.Contain(_xa), Is.True);
			Assert.That(set.Contain(_xb), Is.True);
			Assert.That(set.Contain(_xc), Is.True);
		}

		[Test]
		public void DuplicateCodes()
		{
			var xc = new Currency("XB", 0.01m, "c");

			Assert.Throws<ArgumentException>(() => new CurrencySet(new[] { _xa, _xb, xc }));
		}

		[Test]
		public void Equals()
		{
			Assert.That(_xb, Is.Not.EqualTo(_xa));
			Assert.That(_xa == _xb, Is.False);

			ICurrency c3 = _xa;
			Assert.That(c3, Is.EqualTo(_xa));
			Assert.That(_xa == c3, Is.True);
		}

		[Test]
		public void NotContainCurrency()
		{
			var set = new CurrencySet(new[] { _xa, _xb, _xc });

			Assert.That(set.Contain(new FakeCurrency()), Is.False);
		}

		private class FakeCurrency : ICurrency
		{
			public string CharCode => "XA";

			public int NumCode
			{
				get { throw new NotImplementedException(); }
			}

			public string Symbol
			{
				get { throw new NotImplementedException(); }
			}

			public decimal MinorUnit
			{
				get { throw new NotImplementedException(); }
			}

			public bool Equals(ICurrency other)
			{
				throw new NotImplementedException();
			}

			public string ToString(string format, IFormatProvider formatProvider)
			{
				throw new NotImplementedException();
			}
		}

		[Test]
		public void TryParseNumCodeFail()
		{
			var set = new CurrencySet(new[] { _xa, _xb, _xc });

			Assert.That(set.TryParse("???", out _), Is.False);
		}

		[Test]
		public void NotContainCode()
		{
			var set = new CurrencySet(new[] { _xa, _xb, _xc });

			Assert.That(set.Contain("???"), Is.False);
		}

		[Test]
		public void Contains()
		{
			var set = new CurrencySet(new[] { _xa, _xb, _xc });

			Assert.That(set.Contain(_xa.CharCode), Is.True);
		}

		[Test]
		public void ParseCharCode()
		{
			var set = new CurrencySet(new[] { _xa, _xb, _xc });

			Assert.That(set.Parse("XA").CharCode, Is.EqualTo("XA"));
		}

		[Test]
		public void ParseCharFalse()
		{
			var set = new CurrencySet(new[] { _xa, _xb, _xc });

			Assert.Throws<NotSupportedException>(() =>
			{
				set.Parse("???");
			});
		}

		[Test]
		public void TryParseCharCode()
		{
			var set = new CurrencySet(new[] { _xa, _xb, _xc });

			Assert.That(set.TryParse("XA", out var c), Is.True);
			Assert.That(c!.CharCode, Is.EqualTo("XA"));
		}
	}
}
