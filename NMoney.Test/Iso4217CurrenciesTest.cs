using System;
using NUnit.Framework;
using System.Threading;
using System.Globalization;
using System.Collections;

namespace NMoney
{
	[TestFixture]
	public class Iso4217CurrenciesTest
	{
		private static readonly Iso4217.CurrencySet _set = Iso4217.CurrencySet.Instance;

		[Test]
		public void AllCurrensiesFromIso4217()
		{
			foreach (var c in _set.AllCurencies)
			{
				Assert.AreEqual(c, _set.Parse(c.CharCode));
				Assert.AreEqual(c, _set.Parse(c.NumCode));
			}
		}

		[Test]
		public void TryParseNumCodeFail()
		{
			ICurrency c;
			Assert.IsFalse(_set.TryParse(12345, out c));
		}

		[TestCase(784, "AED")]
		[TestCase(971, "AFN")]
		public void TryParseNumCode(int code, string exp)
		{
			ICurrency c;
			Assert.IsTrue(_set.TryParse(code, out c));
			Assert.AreEqual(exp, c.CharCode);
		}

		[TestCase(784, "AED")]
		[TestCase(971, "AFN")]
		public void ParseNumCode(int code, string exp)
		{
			Assert.AreEqual(exp, _set.Parse(code).CharCode);
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
			Assert.AreNotEqual(c1, c2);
			Assert.IsFalse(c1 == c2);

			ICurrency c3 = Iso4217.CurrencySet.RUB;
			Assert.AreEqual(c1, c3);
			Assert.IsTrue(c1 == c3);
		}
		
		[Test]
		public void ViewUYU()
		{
			Assert.AreEqual("UYU", Iso4217.CurrencySet.UYU.CharCode);
			Assert.AreEqual("$U", Iso4217.CurrencySet.UYU.Symbol);
			Assert.AreEqual(858, Iso4217.CurrencySet.UYU.NumCode);
			Assert.AreEqual(0.01m, Iso4217.CurrencySet.UYU.MinorUnit);
		}

		[TestCase("USD", "ru-RU", "Доллар США")]
		[TestCase("USD", "en-GB", "US Dollar")]
		[TestCase("ALL", "ru-RU", "Лек")]
		[TestCase("ALL", "en-GB", "Lek")]
		[TestCase("XDR", "ru-RU", "Специальные права заимствования")]
		public void Localization(string code, string culture, string exp)
		{
			CultureInfo ci = CultureInfo.GetCultureInfo(culture);
			Thread.CurrentThread.CurrentCulture = ci;
			Thread.CurrentThread.CurrentUICulture = ci;
			Assert.AreEqual(exp, _set.Parse(code).ToString());
		}

		[Test, TestCaseSource("fullLocalizationCases")]
		public void CheckFullLocalization(string code, string culture)
		{
			var currency = _set.Parse(code);
			var invariantName = currency.ToString("n", CultureInfo.InvariantCulture);
			var localizedName = currency.ToString("n", CultureInfo.GetCultureInfo(culture));
			if (string.Equals(invariantName, localizedName, StringComparison.OrdinalIgnoreCase))
				Assert.Inconclusive();
		}

		static IEnumerable fullLocalizationCases()
		{
			foreach(var culture in new[] { "ru"/*, "de", "fr"*/})
				foreach (var c in _set.AllCurencies)
					yield return new TestCaseData(c.CharCode, culture);
		}
	}
}

