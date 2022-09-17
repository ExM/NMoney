using System;
using NUnit.Framework;
using System.Threading;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;

namespace NMoney
{
	[TestFixture]
	public class Iso4217CurrenciesTest
	{
		private static readonly Iso4217.CurrencySet _set = Iso4217.CurrencySet.Instance;

		[Test]
		public void ConversionToBaseSet()
		{
			CurrencySet<Iso4217.Currency> convSet = _set;

			var allCur = convSet.AllCurencies;
			var cur = convSet.TryParse("EUR");
		}
		
		[Test]
		public void ConversionToInterfaceSet()
		{
			ICurrencySet<Iso4217.Currency> convSet = _set;

			var allCur = convSet.AllCurencies;
			var cur = convSet.TryParse("EUR");
		}
		
		[Test]
		public void ConversionToInterfaceItem()
		{
			ICurrencySet<ICurrency> convSet = _set;
			
			var allCur = convSet.AllCurencies;
			var cur = convSet.TryParse("EUR");
		}
		
		[Test]
		public void ConversionToBaseInterface()
		{
			ICurrencySet convSet = _set;
			
			var allCur = convSet.AllCurencies;
			var cur = convSet.TryParse("EUR");
		}
		
		[Test]
		public void AllCurrensiesFromIso4217()
		{
			foreach (var c in _set.AllCurencies)
			{
				Assert.That(_set.Parse(c.CharCode), Is.EqualTo(c));
				Assert.That(_set.Parse(c.NumCode), Is.EqualTo(c));
			}
		}

		[Test]
		public void TryParseNumCodeFail()
		{
			Assert.That(_set.TryParse(12345, out _), Is.False);
		}

		[TestCase(784, "AED")]
		[TestCase(971, "AFN")]
		public void TryParseNumCode(int code, string exp)
		{
			Assert.That(_set.TryParse(code, out var c), Is.True);
			Assert.That(c!.CharCode, Is.EqualTo(exp));
		}

		[TestCase(784, "AED")]
		[TestCase(971, "AFN")]
		public void ParseNumCode(int code, string exp)
		{
			Assert.That(_set.Parse(code).CharCode, Is.EqualTo(exp));
		}

		[Test]
		public void ParseNumFalse()
		{
			Assert.Throws<NotSupportedException>(() =>
			{
				_set.Parse(12345);
			});
		}

		[Test]
		public void Equals()
		{
			ICurrency c1 = Iso4217.CurrencySet.RUB;
			ICurrency c2 = Iso4217.CurrencySet.AED;
			Assert.That(c2, Is.Not.EqualTo(c1));
			Assert.That(c1 == c2, Is.False);

			ICurrency c3 = Iso4217.CurrencySet.RUB;
			Assert.That(c3, Is.EqualTo(c1));
			Assert.That(c1 == c3, Is.True);
		}
		
		[Test]
		public void ViewUYU()
		{
			Assert.That(Iso4217.CurrencySet.UYU.CharCode, Is.EqualTo("UYU"));
			Assert.That(Iso4217.CurrencySet.UYU.Symbol, Is.EqualTo("$U"));
			Assert.That(Iso4217.CurrencySet.UYU.NumCode, Is.EqualTo(858));
			Assert.That(Iso4217.CurrencySet.UYU.MinorUnit, Is.EqualTo(0.01m));
		}

		[TestCase("USD", "ru-RU", "Доллар США")]
		[TestCase("USD", "en-GB", "US Dollar")]
		[TestCase("ALL", "ru-RU", "Лек")]
		[TestCase("ALL", "en-GB", "Lek")]
		[TestCase("XDR", "ru-RU", "Специальные права заимствования")]
		public void Localization(string code, string culture, string exp)
		{
			var ci = CultureInfo.GetCultureInfo(culture);
			Thread.CurrentThread.CurrentCulture = ci;
			Thread.CurrentThread.CurrentUICulture = ci;
			Assert.That(_set.Parse(code).ToString(), Is.EqualTo(exp));
		}
	}
}

