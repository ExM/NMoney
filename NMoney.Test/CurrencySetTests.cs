using System;
using NUnit.Framework;

namespace NMoney
{
	[TestFixture]
	public class CurrencySetTests
	{
		private readonly Currency _xa = new Currency("XA", "a", 0.01m);
		private readonly Currency _xb = new Currency("XB", "b", 0.01m);
		private readonly Currency _xc = new Currency("XC", "c", 0.01m);

		[Test]
		public void Create()
		{
			ICurrencySet set = new CurrencySet(new[] { _xa, _xb, _xc });

			Assert.AreEqual(3, set.AllCurencies.Count);

			Assert.IsTrue(set.Contain(_xa));
			Assert.IsTrue(set.Contain(_xb));
			Assert.IsTrue(set.Contain(_xc));
		}

		[Test]
		public void DuplicateCodes()
		{
			var xc = new Currency("XB", "c", 0.01m);

			Assert.Throws<ArgumentException>(() => new CurrencySet(new[] { _xa, _xb, xc }));
		}

		[Test]
		public void Equals()
		{
			Assert.AreNotEqual(_xa, _xb);
			Assert.IsFalse(_xa == _xb);

			ICurrency c3 = _xa;
			Assert.AreEqual(_xa, c3);
			Assert.IsTrue(_xa == c3);
		}

		[Test]
		public void NotContainCurrency()
		{
			var set = new CurrencySet(new[] { _xa, _xb, _xc });

			Assert.IsFalse(set.Contain(new FakeCurrency()));
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

			ICurrency c;
			Assert.IsFalse(set.TryParse("???", out c));
		}

		[Test]
		public void NotContainCode()
		{
			var set = new CurrencySet(new[] { _xa, _xb, _xc });

			Assert.IsFalse(set.Contain("???"));
		}

		[Test]
		public void Contains()
		{
			var set = new CurrencySet(new[] { _xa, _xb, _xc });

			Assert.IsTrue(set.Contain(_xa.CharCode));
		}

		[Test]
		public void ParseCharCode()
		{
			var set = new CurrencySet(new[] { _xa, _xb, _xc });

			Assert.AreEqual("XA", set.Parse("XA").CharCode);
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

			ICurrency c;
			Assert.IsTrue(set.TryParse("XA", out c));
			Assert.AreEqual("XA", c.CharCode);
		}
	}
}
