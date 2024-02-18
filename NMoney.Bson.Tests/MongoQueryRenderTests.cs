using System.Linq;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using NUnit.Framework;

namespace NMoney.Bson
{
	[TestFixture, Parallelizable(ParallelScope.Children), TestOf(typeof(BsonMoneySerializer))]
	public class MongoQueryRenderTests
	{
		private const string _customAmountFieldName = "sum";
		private const string _customCurrencyFieldName = "cur";

		private IBsonSerializerRegistry _registry;
		private IBsonSerializer<Money> _serializer;

		[OneTimeSetUp]
		public void Setup()
		{
			var registry = new BsonSerializerRegistry();
			registry.RegisterSerializationProvider(new BsonObjectModelSerializationProvider());
			BsonCurrencySerializer.Register(Iso4217.CurrencySet.Instance, registry);
			_serializer = new BsonMoneySerializer(
				amountFieldName: _customAmountFieldName,
				currencyFieldName: _customCurrencyFieldName,
				registry: registry);
			registry.RegisterSerializer(typeof(Money), _serializer);
			_registry = registry;
		}

		[Test]
		public void RenderEqualityByCurrency()
		{
			var filter = Builders<Money>.Filter.Eq(m => m.Currency, Iso4217.CurrencySet.USD);
			var expected = "{ 'cur' : 'USD' }";
			var rendered = Render(filter);
			Assert.That(rendered, Is.EqualTo(expected));
		}

		[Test]
		public void RenderEqualityByCurrencyExpression()
		{
			var filter = Builders<Money>.Filter.Where(m => m.Currency == Iso4217.CurrencySet.USD);
			var expected = "{ 'cur' : 'USD' }";
			var rendered = Render(filter);
			Assert.That(rendered, Is.EqualTo(expected));
		}

		[Test]
		public void RenderContainsByCurrencyExpression()
		{
			var currencies = new[]
			{
				Iso4217.CurrencySet.USD,
				Iso4217.CurrencySet.EUR,
				Iso4217.CurrencySet.RUB,
			};
			var filter = Builders<Money>.Filter.Where(m => currencies.Contains(m.Currency));
			var expected = "{ 'cur' : { '$in' : ['USD', 'EUR', 'RUB'] } }";
			var rendered = Render(filter);
			Assert.That(rendered, Is.EqualTo(expected));
		}

		[Test]
		public void RenderGreaterThanAmount()
		{
			var filter = Builders<Money>.Filter.Where(m => m.Amount > 100);
			var expected = "{ 'sum' : { '$gt' : NumberDecimal('100') } }";
			var rendered = Render(filter);
			Assert.That(rendered, Is.EqualTo(expected));
		}

		[Test]
		public void RenderIndexDefinition()
		{
			var index = Builders<Money>.IndexKeys
				.Ascending(m => m.Currency)
				.Ascending(m => m.Amount);
			var expected = "{ 'cur' : 1, 'sum' : 1 }";
			var rendered = index.Render(_serializer, _registry).ToJson().Replace('"', '\'');
			Assert.That(rendered, Is.EqualTo(expected));
		}

		[Test]
		public void RenderUpdateCurrency()
		{
			var update = Builders<Money>.Update
				.Set(m => m.Currency, Iso4217.CurrencySet.USD);
			var expected = "{ '$set' : { 'cur' : 'USD' } }";
			var rendered = update.Render(_serializer, _registry).ToJson().Replace('"', '\'');
			Assert.That(rendered, Is.EqualTo(expected));
		}

		[Test]
		public void RenderIncrementAmount()
		{
			var update = Builders<Money>.Update
				.Inc(m => m.Amount, 100m);
			var expected = "{ '$inc' : { 'sum' : NumberDecimal('100') } }";
			var rendered = update.Render(_serializer, _registry).ToJson().Replace('"', '\'');
			Assert.That(rendered, Is.EqualTo(expected));
		}

		private string Render(FilterDefinition<Money> filter) =>
			filter
			.Render(_serializer, _registry)
			.ToJson()
			.Replace('"', '\'');

	}
}
