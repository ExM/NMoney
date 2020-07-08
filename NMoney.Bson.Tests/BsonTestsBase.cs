using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using NUnit.Framework;

namespace NMoney.Bson.Tests
{
	public abstract class BsonTestsBase
	{
		private static readonly ICurrencySet _currencySet =
			new CurrencySet(new[]
			{
				new Currency("USD", 0.01m, "$"),
				new Currency("EUR", 0.01m, "€"),
				new Currency("RUB", 0.01m, "₽"),
				// Custom
				new Currency("BTC", 0.01m, "₿"),
			});

		public static IEnumerable<ICurrency> AllCurrencies => _currencySet.AllCurencies;

		public static IEnumerable<string> AllCurrencyCodes => AllCurrencies.Select(c => c.CharCode);

		[OneTimeSetUp]
		public void Setup()
		{
			BsonCurrencySerializer.Register(_currencySet);
		}

		protected BsonDocument Serialize<T>(T obj)
		{
			var bsonDoc = new BsonDocument();
			using (var writer = new BsonDocumentWriter(bsonDoc))
				BsonSerializer.Serialize(writer, obj);
			return bsonDoc;
		}

		protected T Deserialize<T>(BsonDocument doc)
		{
			return BsonSerializer.Deserialize<T>(doc);
		}

	}
}
