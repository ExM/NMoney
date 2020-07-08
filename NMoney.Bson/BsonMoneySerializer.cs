using System;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace NMoney.Bson
{
	/// <summary>
	/// Bson serializer for <see cref="Money"/>
	/// </summary>
	public class BsonMoneySerializer : StructSerializerBase<Money>,
		IRepresentationConfigurable<BsonMoneySerializer>,
		IBsonDocumentSerializer
	{
		private const string DefaultCurrencyFieldName = "currency";
		private const string DefaultAmountFieldName = "amount";
		private const BsonType DefaultAmountRepresentation = BsonType.Decimal128;

		private static readonly object _sync = new object();
		private static bool _isRegistered = false;

		private readonly string _currencyFieldName;
		private readonly IBsonSerializer<ICurrency> _currencySerializer;
		private readonly string _amountFieldName;
		private readonly DecimalSerializer _amountSerializer;

		/// <summary>
		/// Initializes a new instance of the BsonMoneySerializer class
		/// </summary>
		/// <param name="currencyFieldName">Field name for <see cref="Money.Currency"/> property</param>
		/// <param name="amountFieldName">Field name for <see cref="Money.Amount"/> property</param>
		/// <param name="amountRepresentation">Bson representation for <see cref="Money.Amount"/> value</param>
		/// <param name="registry">Custom serializer registry</param>
		public BsonMoneySerializer(
			string currencyFieldName = DefaultCurrencyFieldName,
			string amountFieldName = DefaultAmountFieldName,
			BsonType amountRepresentation = DefaultAmountRepresentation,
			IBsonSerializerRegistry registry = null)
		{
			_currencyFieldName = currencyFieldName;
			_amountFieldName = amountFieldName;
			Representation = amountRepresentation;
			_amountSerializer = new DecimalSerializer(Representation);
			_currencySerializer = (registry ?? BsonSerializer.SerializerRegistry).GetSerializer<ICurrency>();
		}

		/// <inheritdoc/>
		public BsonType Representation { get; }

		/// <inheritdoc/>
		public BsonMoneySerializer WithRepresentation(BsonType representation) =>
			new BsonMoneySerializer(_currencyFieldName, _amountFieldName, representation);

		/// <inheritdoc/>
		IBsonSerializer IRepresentationConfigurable.WithRepresentation(BsonType representation) =>
			WithRepresentation(representation);

		/// <inheritdoc/>
		public bool TryGetMemberSerializationInfo(string memberName, out BsonSerializationInfo serializationInfo)
		{
			switch (memberName)
			{
				case nameof(Money.Currency):
					serializationInfo = new BsonSerializationInfo(
						_currencyFieldName, _currencySerializer, typeof(ICurrency));
					return true;

				case nameof(Money.Amount):
					serializationInfo = new BsonSerializationInfo(
						_amountFieldName, _amountSerializer, typeof(decimal));
					return true;
			}

			serializationInfo = null;
			return false;
		}

		/// <inheritdoc/>
		public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, Money value)
		{
			var writer = context.Writer;

			if (value == Money.Zero)
			{
				writer.WriteNull();
				return;
			}

			writer.WriteStartDocument();
			writer.WriteName(_amountFieldName);
			_amountSerializer.Serialize(context, args, value.Amount);
			writer.WriteName(_currencyFieldName);
			_currencySerializer.Serialize(context, args, value.Currency);
			writer.WriteEndDocument();
		}

		/// <inheritdoc/>
		public override Money Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
		{
			var reader = context.Reader;
			var bsonType = reader.GetCurrentBsonType();

			if (bsonType == BsonType.Null)
			{
				reader.ReadNull();
				return Money.Zero;
			}

			if (bsonType != BsonType.Document)
				throw new FormatException($"Cannot deserialize Money from BsonType {bsonType}.");

			reader.ReadStartDocument();

			decimal? amount = null;
			ICurrency currency = null;
			while (reader.ReadBsonType() != BsonType.EndOfDocument)
			{
				var name = reader.ReadName();

				if (name == _amountFieldName)
					amount = _amountSerializer.Deserialize(context, args);
				else if (name == _currencyFieldName)
					currency = _currencySerializer.Deserialize(context, args);
				else
					throw new BsonSerializationException($"Invalid element: '{name}'.");
			}

			reader.ReadEndDocument();

			if (currency is null)
				throw new BsonSerializationException($"Document does not contains field {_currencyFieldName}");
			if (amount is null)
				throw new BsonSerializationException($"Document does not contains field {_amountFieldName}");

			return new Money(amount.Value, currency);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="currencyFieldName"></param>
		/// <param name="amountFieldName"></param>
		/// <param name="amountRepresentation"></param>
		public static void Register(
			string currencyFieldName = DefaultCurrencyFieldName,
			string amountFieldName = DefaultAmountFieldName,
			BsonType amountRepresentation = DefaultAmountRepresentation)
		{
			if (_isRegistered)
				return;

			if (!BsonCurrencySerializer.IsRegistered)
				throw new InvalidOperationException($"{nameof(BsonCurrencySerializer)} must be registered first");

			lock (_sync)
			{
				if (_isRegistered)
					return;
				_isRegistered = true;
			}

			BsonSerializer.RegisterSerializer(
				typeof(Money),
				new BsonMoneySerializer(currencyFieldName, amountFieldName, amountRepresentation));
		}
	}
}
