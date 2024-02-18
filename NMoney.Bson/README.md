# BSON serializer for Money and ICurrency types
[![NuGet Version](http://img.shields.io/nuget/v/NMoney.Bson.svg?style=flat)](https://www.nuget.org/packages/NMoney.Bson/) 
[![NuGet Downloads](http://img.shields.io/nuget/dt/NMoney.Bson.svg?style=flat)](https://www.nuget.org/packages/NMoney.Bson/)

Usage
-----
**Initialize currency serialization**

Use your currency list
```C#
var actualCurrencies = new CurrencySet(new[] {
    Iso4217.CurrencySet.CNY,
    Iso4217.CurrencySet.RUB,
    Iso4217.CurrencySet.BRL,
    Iso4217.CurrencySet.EGP,
    Iso4217.CurrencySet.IRR,
    Iso4217.CurrencySet.AED,
    Iso4217.CurrencySet.ETB,
    Iso4217.CurrencySet.ZAR
});
BsonCurrencySerializer.Register(actualCurrencies);
```
or all currencies of ISO 4217
```C#
BsonCurrencySerializer.Register(NMoney.Iso4217.CurrencySet.Instance);
```

**Initialize Money serialization**

Use default serialization
```C#
BsonMoneySerializer.Register();
```
for BSON document
```JSON
{
  "amount" : { "$numberDecimal" : "4.0" },
  "currency" : "RUB"
}
```

Or use custom serialization
```C#
BsonMoneySerializer.Register(
    currencyFieldName: "cur",
    amountFieldName: "val",
    amountRepresentation: BsonType.Double);
```