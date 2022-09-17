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
			Assert.That(deserialized, Is.Not.Null);
			Assert.That(deserialized.Currency, Is.Not.Null);
			Assert.That(deserialized.Currency, Is.EqualTo(currency));
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
			Assert.That(deserialized, Is.Not.Null);
			Assert.That(deserialized.Currency, Is.Not.Null);
			Assert.That(deserialized.Currency.CharCode, Is.EqualTo(currencyCode));
		}

		[Test]
		public void Serialize_null_currency_as_null()
		{
			var stub = new Stub() { Currency = null };
			var bsonDoc = Serialize(stub);
			var value = bsonDoc["currency"];
			Assert.That(BsonNull.Value, Is.EqualTo(value));
		}

		[Test]
		public void Deserialize_null_as_null_currency()
		{
			var bsonDoc = new BsonDocument()
			{
				{ "currency", BsonNull.Value }
			};
			var deserialized = Deserialize<Stub>(bsonDoc);
			Assert.That(deserialized, Is.Not.Null);
			Assert.That(deserialized.Currency, Is.Null);
		}

		[Test]
		public void Exception_when_serializing_unsupported_currency()
		{
			var stub = new Stub() { Currency = NMoney.Iso4217.CurrencySet.GBP };
			var ex = Assert.Catch<BsonSerializationException>(() => Serialize(stub));
			Assert.That(ex.InnerException, Is.InstanceOf<NotSupportedException>());
		}

		[Test]
		public void Exception_when_deserializing_unsupported_currency()
		{
			var bsonDoc = new BsonDocument()
			{
				{ "currency", NMoney.Iso4217.CurrencySet.GBP.CharCode }
			};
			var ex = Assert.Catch<FormatException>(() => Deserialize<Stub>(bsonDoc));
			Assert.That(ex.InnerException, Is.InstanceOf<NotSupportedException>());
		}

		private class Stub
		{
			[BsonElement("currency")]
			public ICurrency Currency { get; set; }
		}
	}
}
