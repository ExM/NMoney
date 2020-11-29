using System.Linq;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using NUnit.Framework;

namespace NMoney.Bson.Tests
{
	[TestFixture, Parallelizable(ParallelScope.Children), TestOf(typeof(BsonMoneySerializer))]
	public class BsonMoneySerializerTests : BsonTestsBase
	{
		private const string _customAmountFieldName = "sum";
		private const string _customCurrencyFieldName = "cur";
		private const string _moneyField = "money";

		[OneTimeSetUp]
		public new void Setup()
		{
			BsonMoneySerializer.Register(
				amountFieldName: _customAmountFieldName,
				currencyFieldName: _customCurrencyFieldName);
		}

		[Test]
		public void SerializingValidValue(
			[ValueSource(nameof(AllCurrencies))]
			ICurrency currency)
		{
			var amount = 1.23m;
			var value = new Money(amount, currency);
			var doc = Serialize(value);
			BsonValue amountValue = null;
			BsonValue currencyValue = null;

			Assert.Multiple(() =>
			{
				Assert.DoesNotThrow(() => amountValue = doc.GetElement(_customAmountFieldName).Value);
				Assert.DoesNotThrow(() => currencyValue = doc.GetElement(_customCurrencyFieldName).Value);
			});
			Assert.Multiple(() =>
			{
				Assert.That(amountValue.BsonType, Is.EqualTo(BsonType.Decimal128));
				Assert.That(amountValue.AsDecimal, Is.EqualTo(amount));
				Assert.That(currencyValue.BsonType, Is.EqualTo(BsonType.String));
				Assert.That(currencyValue.AsString, Is.EqualTo(currency.CharCode));
			});
		}

		[Test]
		public void SerializingZeroValueAsNull()
		{
			var value = new Stub() { Money = Money.Zero };
			var doc = Serialize(value);
			var element = doc.GetElement(_moneyField).Value;
			Assert.That(element.IsBsonNull, Is.True);
		}

		[Test]
		public void SerializingZeroAmountValueAsDocument(
			[ValueSource(nameof(AllCurrencies))]
			ICurrency currency)
		{
			var value = currency.Money(0m);
			var doc = Serialize(value);
			BsonValue amountValue = null;
			BsonValue currencyValue = null;

			Assert.Multiple(() =>
			{
				Assert.DoesNotThrow(() => amountValue = doc.GetElement(_customAmountFieldName).Value);
				Assert.DoesNotThrow(() => currencyValue = doc.GetElement(_customCurrencyFieldName).Value);
			});
			Assert.Multiple(() =>
			{
				Assert.That(amountValue.AsDecimal, Is.Zero);
				Assert.That(currencyValue.AsString, Is.EqualTo(currency.CharCode));
			});
		}

		[Test]
		public void DontSerializingZeroValueWhenIgnoringDefaultValue()
		{
			var value = new StubWithIgnoreIfDefault() { Money = Money.Zero };
			var doc = Serialize(value);
			var hasElement = doc.Elements.Any(el => el.Name == _moneyField);
			Assert.That(hasElement, Is.False);
		}

		[Test]
		public void SerializingZeroAmountValueWhenIgnoringDefaultValue(
			[ValueSource(nameof(AllCurrencies))]
			ICurrency currency)
		{
			var value = new StubWithIgnoreIfDefault() { Money = currency.Money(0m) };
			var doc = Serialize(value);
			var hasElement = doc.Elements.Any(el => el.Name == _moneyField);
			Assert.That(hasElement, Is.True);
		}

		[Test]
		public void SerializingValidValueWithDoubleRepresentation(
			[ValueSource(nameof(AllCurrencies))]
			ICurrency currency)
		{
			var amount = 1.23m;
			var value = new Money(amount, currency);
			var stub = new StubWithDoubleRepresentation() { Money = value };
			var doc = Serialize(stub);
			doc = doc.GetValue(_moneyField).AsBsonDocument;
			BsonValue amountValue = null;
			BsonValue currencyValue = null;

			Assert.Multiple(() =>
			{
				Assert.DoesNotThrow(() => amountValue = doc.GetElement(_customAmountFieldName).Value);
				Assert.DoesNotThrow(() => currencyValue = doc.GetElement(_customCurrencyFieldName).Value);
			});
			Assert.Multiple(() =>
			{
				Assert.That(amountValue.BsonType, Is.EqualTo(BsonType.Double));
				Assert.That(amountValue.AsDouble, Is.EqualTo((double)amount));
				Assert.That(currencyValue.BsonType, Is.EqualTo(BsonType.String));
				Assert.That(currencyValue.AsString, Is.EqualTo(currency.CharCode));
			});
		}

		[Test]
		public void DeserializingValidValue(
			[ValueSource(nameof(AllCurrencies))]
			ICurrency currency)
		{
			var amount = 1.23m;
			var bsonDoc = new BsonDocument()
			{
				{ _customCurrencyFieldName, currency.CharCode },
				{ _customAmountFieldName, new BsonDecimal128(amount) },
			};
			var value = default(Money);
			Assert.DoesNotThrow(() => value = BsonSerializer.Deserialize<Money>(bsonDoc));
			Assert.Multiple(() =>
			{
				Assert.That(value.Amount, Is.EqualTo(amount));
				Assert.That(value.Currency, Is.EqualTo(currency));
			});
		}

		[Test]
		public void DeserializingNullAsDefaultValue()
		{
			var bsonDoc = new BsonDocument()
			{
				{ _moneyField, BsonNull.Value },
			};
			Stub value = null;
			Assert.DoesNotThrow(() => value = BsonSerializer.Deserialize<Stub>(bsonDoc));
			Assert.That(value, Is.Not.Null);
			Assert.That(value.Money, Is.EqualTo(Money.Zero));
		}

		[Test]
		public void DeserializingNoElementAsDefaultValue()
		{
			var bsonDoc = new BsonDocument();
			StubWithIgnoreIfDefault value = null;
			Assert.DoesNotThrow(() => value = BsonSerializer.Deserialize<StubWithIgnoreIfDefault>(bsonDoc));
			Assert.That(value, Is.Not.Null);
			Assert.That(value.Money, Is.EqualTo(Money.Zero));
		}

		[Test]
		public void ThrowWhenNoCurrencyElement()
		{
			var amount = 1.23m;
			var bsonDoc = new BsonDocument()
			{
				{ _customAmountFieldName, new BsonDecimal128(amount) },
			};
			var ex = Assert.Catch<BsonSerializationException>(() => BsonSerializer.Deserialize<Money>(bsonDoc));
			Assert.That(ex.Message, Is.EqualTo($"Document does not contains field {_customCurrencyFieldName}"));
		}

		[Test]
		public void ThrowWhenNoAmountElement()
		{
			var currency = Iso4217.CurrencySet.USD;
			var bsonDoc = new BsonDocument()
			{
				{ _customCurrencyFieldName, currency.CharCode },
			};
			var ex = Assert.Catch<BsonSerializationException>(() => BsonSerializer.Deserialize<Money>(bsonDoc));
			Assert.That(ex.Message, Is.EqualTo($"Document does not contains field {_customAmountFieldName}"));
		}

		[Test]
		public void ThrowWhenInvalidElement()
		{
			var amount = 1.23m;
			var currency = Iso4217.CurrencySet.USD;
			var invalidElement = "field";
			var bsonDoc = new BsonDocument()
			{
				{ _customAmountFieldName, new BsonDecimal128(amount) },
				{ _customCurrencyFieldName, currency.CharCode },
				{ invalidElement, "value" }
			};
			var ex = Assert.Catch<BsonSerializationException>(() => BsonSerializer.Deserialize<Money>(bsonDoc));
			Assert.That(ex.Message, Is.EqualTo($"Invalid element: '{invalidElement}'."));
		}
		private class Stub
		{
			[BsonElement(_moneyField)]
			public Money Money { get; set; }
		}

		private class StubWithIgnoreIfDefault
		{
			[BsonElement(_moneyField), BsonIgnoreIfDefault]
			public Money Money { get; set; }
		}

		private class StubWithDoubleRepresentation
		{
			[BsonElement(_moneyField), BsonRepresentation(BsonType.Double)]
			public Money Money { get; set; }
		}
	}
}
