A lightweight and reliable library for representing and working with monetary values in .NET, based on ISO 4217 currency definitions.

[![NuGet Version](http://img.shields.io/nuget/v/NMoney.svg?style=flat)](https://www.nuget.org/packages/NMoney/)
[![NuGet Downloads](http://img.shields.io/nuget/dt/NMoney.svg?style=flat)](https://www.nuget.org/packages/NMoney/)

# ✨ Features
* Strongly-typed monetary values
* Full support for ISO 4217 currencies
* Safe arithmetic operations (prevents mixing currencies)
* Built-in rounding (major and minor units)
* Value-type (struct) for performance and immutability
* Designed for financial and business applications

# 🚀 Quick Start

Install NuGet package
```bash
dotnet add package NMoney
```

Write code
```C#
using NMoney;
using Iso4217List = NMoney.Iso4217.CurrencySet;

var price = Iso4217List.USD.Money(10m);
var quantity = 3;

var total = price * quantity;

Console.WriteLine(total); // 30 USD
```

# 💰 Core Concepts

## Money

[Money](NMoney/Money.cs) is a value type that represents a monetary amount.

It consists of:
* A decimal amount
* A currency (ICurrency)

Key behaviors
* Immutable
* Supports arithmetic operations
* Throws an exception when mixing different currencies

## Currency

[ICurrency](NMoney/ICurrency.cs) represents a currency and provides:
* Name (e.g. "US Dollar")
* ISO 4217 code (e.g. "USD")
* Number of decimal places for minor units (e.g. 2 for cents)
* Currency symbol (e.g. "$")

## Currency Sets

[CurrencySet](NMoney/CurrencySet'1.cs) represents a collection of currencies used for:
* list of currencies used in the application
* serialization and deserialization

```C#
using NMoney;
using Iso4217List = NMoney.Iso4217.CurrencySet;

ICurrencySet actualSet =
new CurrencySet([
   Iso4217List.CNY,
   Iso4217List.RUB,
   Iso4217List.INR,  
   new Currency("BTC", 0.01m, "₿")
]);

Console.WriteLine(actualSet.TryParse("CNY")); // Yuan Renminbi
```
# 🌍 ISO 4217

Currency definitions are based on the official ISO 4217 standard.

Updates are sourced from:
* https://www.iso.org/iso-4217-currency-codes.html
* https://www.six-group.com/dam/download/financial-information/data-center/iso-currrency/lists/list-one.xml

The XML list is used to generate currency definitions in C#.

[Iso4217.CurrencySet](NMoney/Iso4217/CurrencySet.cs) provides:
* All active ISO 4217 currencies
* Obsolete currencies (where applicable)

```C#
using NMoney;

var allIso4217Set = NMoney.Iso4217.CurrencySet.Instance;

Console.WriteLine(allIso4217Set.TryParse("AUD")); //Australian Dollar
```

# 🔢 Operations

## Arithmetic
```C#
var usd = NMoney.Iso4217.CurrencySet.USD;

var usd10 = usd.Money(10m);
var usd15 = usd.Money(15m);

var sum = usd10 + usd15;     // 25 USD
var diff = usd15 - usd10;    // 5 USD
var scaled = usd10 * 2;      // 20 USD
var divided = usd15 / 3;     // 5 USD
```

## Currency safety
```C#
var usd = NMoney.Iso4217.CurrencySet.USD.Money(10m);
var eur = NMoney.Iso4217.CurrencySet.EUR.Money(10m);

var invalid = usd + eur; // throws InvalidOperationException
```

## Comparison
```C#
var usd = NMoney.Iso4217.CurrencySet.USD;

var usd10 = usd.Money(10m);
var usd15 = usd.Money(15m);

usd10 == usd15; // false
usd10 < usd15;  // true

usd10 == CurrencySet.EUR.Money(10m); // false
usd10 > CurrencySet.EUR.Money(10m);  // throws InvalidOperationException
```

## Aggregation

```C#
var usd = NMoney.Iso4217.CurrencySet.USD;

var total = Money.Zero;

total += usd.Money(10m);
total += usd.Money(15m);

// total = 25 USD
```
`Money.Zero` represents a neutral monetary value used as a starting point for aggregations.

When combined with another Money value, it adopts that value’s currency.

## Rounding
```C#
var value = NMoney.Iso4217.CurrencySet.USD.Money(20.953m);

value.CeilingMajorUnit(); // 21 USD
value.FloorMajorUnit();   // 20 USD
value.CeilingMinorUnit(); // 20.96 USD
value.FloorMinorUnit();   // 20.95 USD
```

# 🧩 Design Principles
* Correctness over convenience — prevents invalid monetary operations
* Explicit currency handling — no implicit conversions
* Precision — uses decimal for financial accuracy
* Immutability — safe for concurrent and functional scenarios

# 📦 Serialization
* BSON support: see [NMoney.Bson](NMoney.Bson/README.md)