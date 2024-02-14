using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace NMoney.Bson
{
	/// <summary>
	/// Bson serializer for <see cref="ICurrency"/> by <see cref="ICurrencySet"/>
	/// </summary>
	public class BsonCurrencySerializer : SerializerBase<ICurrency?>
	{
		private static readonly object _sync = new object();
		private readonly ICurrencySet _currencySet;

		/// <summary>
		/// Initializes a new instance of the BsonCurrencySerializer class
		/// </summary>
		/// <param name="currencySet">Available currencies set (required)</param>
		private BsonCurrencySerializer(ICurrencySet currencySet)
		{
			_currencySet = currencySet ?? throw new ArgumentNullException(nameof(currencySet));
		}

		/// <summary>
		/// Serializer is registered in registry
		/// </summary>
		public static bool IsRegistered { get; private set; }

		/// <summary>
		/// Register Bson serializer with all ISO4217-supported currencies
		/// </summary>
		public static void RegisterIsoSet() => Register(Iso4217.CurrencySet.Instance);

		/// <summary>
		/// Register Bson serializer with custom currency set
		/// </summary>
		public static void Register(ICurrencySet currencySet)
		{
			if (IsRegistered)
				return;

			lock (_sync)
			{
				if (IsRegistered)
					return;
				IsRegistered = true;
			}

			Register(currencySet, (BsonSerializerRegistry)BsonSerializer.SerializerRegistry);
		}

		/// <summary>
		/// Register Bson serializer with custom currency set into custom registry
		/// </summary>
		public static void Register(ICurrencySet currencySet, BsonSerializerRegistry registry)
		{
			var serializer = new BsonCurrencySerializer(currencySet);
			registry.RegisterSerializer(typeof(ICurrency), serializer);
			registry.RegisterSerializer(typeof(Currency), serializer);
			registry.RegisterSerializer(typeof(Iso4217.Currency), serializer);
		}

		/// <inheritdoc/>
		public override ICurrency? Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
		{
			var bsonReader = context.Reader;
			var bsonType = context.Reader.GetCurrentBsonType();

			switch (bsonType)
			{
				case BsonType.Null:
					bsonReader.ReadNull();
					return null;

				case BsonType.String:
					var charCode = bsonReader.ReadString();
					return _currencySet.TryParse(charCode) ??
						throw new NotSupportedException("currency code '" + charCode + "' not supported");

				default:
					throw new FormatException($"Cannot deserialize BsonString from BsonType {bsonType}.");
			}
		}

		/// <inheritdoc/>
		public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, ICurrency? value)
		{
			if (value is null)
			{
				context.Writer.WriteNull();
				return;
			}
			if (!_currencySet.Contain(value))
				throw new NotSupportedException("currency code '" + value.CharCode + "' not supported");
			context.Writer.WriteString(value.CharCode);
		}
	}
}
