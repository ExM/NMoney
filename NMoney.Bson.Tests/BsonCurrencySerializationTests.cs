using System;
using NUnit.Framework;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace NMoney.Bson.Tests
{
	[TestFixture, Parallelizable(ParallelScope.Children), TestOf(typeof(BsonCurrencySerializer))]
	public class BsonCurrencySerializationTests : BsonTestsBase
	{
		[Test]
		public void Currency_equality_after_serializing_and_deserializing(
			[ValueSource(nameof(AllCurrencies))]
			ICurrency currency)
		{
			var stub = new Stub() { Currency = currency };
			var bsonDoc = Serialize(stub);
			var deserialized = Deserialize<Stub>(bsonDoc);
			Assert.IsNotNull(deserialized);
			Assert.IsNotNull(deserialized.Currency);
			Assert.AreEqual(currency, deserialized.Currency);
		}

		[Test]
		public void Currency_equality_after_deserializing_from_string(
			[ValueSource(nameof(AllCurrencyCodes))]
			string currencyCode)
		{
			var bsonDoc = new BsonDocument()
			{
				{ "currency", currencyCode }
			};
			var deserialized = Deserialize<Stub>(bsonDoc);
			Assert.IsNotNull(deserialized);
			Assert.IsNotNull(deserialized.Currency);
			Assert.AreEqual(currencyCode, deserialized.Currency.CharCode);
		}

		[Test]
		public void Serialize_null_currency_as_null()
		{
			var stub = new Stub() { Currency = null };
			var bsonDoc = Serialize(stub);
			var value = bsonDoc["currency"];
			Assert.AreEqual(value, BsonNull.Value);
		}

		[Test]
		public void Deserialize_null_as_null_currency()
		{
			var bsonDoc = new BsonDocument()
			{
				{ "currency", BsonNull.Value }
			};
			var deserialized = Deserialize<Stub>(bsonDoc);
			Assert.IsNotNull(deserialized);
			Assert.IsNull(deserialized.Currency);
		}

		[Test]
		public void Exception_when_serializing_unsupported_currency()
		{
			var stub = new Stub() { Currency = NMoney.Iso4217.CurrencySet.GBP };
			Assert.Throws<NotSupportedException>(() => Serialize(stub));
		}

		[Test]
		public void Exception_when_deserializing_unsupported_currency()
		{
			var bsonDoc = new BsonDocument()
			{
				{ "currency", NMoney.Iso4217.CurrencySet.GBP.CharCode }
			};
			var ex = Assert.Catch<FormatException>(() => Deserialize<Stub>(bsonDoc));
			Assert.IsInstanceOf<NotSupportedException>(ex.InnerException);
		}

		private class Stub
		{
			[BsonElement("currency")]
			public ICurrency Currency { get; set; }
		}
	}
}
