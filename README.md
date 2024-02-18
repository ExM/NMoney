# Money
Value-type contains the amount of money in decimal and a link to an instance of currency.

# ICurrency
An instance of this interface must contain:
* Name of the currency,
* Currency code (e.g. in ISO 4217),
* Number of characters minor currency (eg cents, a penny ...)
* Symbol of currency sign

# ICurrencySet
Customizable currency collection

[![NuGet Version](http://img.shields.io/nuget/v/NMoney.svg?style=flat)](https://www.nuget.org/packages/NMoney/) 
[![NuGet Downloads](http://img.shields.io/nuget/dt/NMoney.svg?style=flat)](https://www.nuget.org/packages/NMoney/)

Usage
-----
**Initializing money**

```C#
var eur15 = new Money(15m, Iso4217.CurrencySet.EUR);
var usd10 = Iso4217.CurrencySet.USD.Money(10m);
var zero = Money.Zero;
```

**Money operations**

```C#
var usd10 = Iso4217.CurrencySet.USD.Money(10m);
var usd15 = Iso4217.CurrencySet.USD.Money(15m);
var eur1 = Iso4217.CurrencySet.USD.Money(1m);
var eur10 = Iso4217.CurrencySet.USD.Money(10m);
var eur123 = Iso4217.CurrencySet.USD.Money(123m);

// add and substract
var usd25 = usd10 + usd15;
var eur122 = eur123 - eur1;
var wtf = usd10 + eur1; // will throw exception

var usd10 = Money.Zero + usd10;
var usdSum = default(Money); // Money.Zero - default value of this struct
usdSum += usd10;
usdSum += usd15; // now usdSum contain 25 dollars

// multiply and division by constant
var usd5 = usd15 / 3;
var eur15 = eur1 * 15;

// compare money
usd10 == eur10; // false
usd10 != eur10; // true;
eur10 == eur1 * 10; // true
usd15 > usd10; // true;
usd15 <= eur10; // will throw exception

// rounding
Iso4217.CurrencySet.USD.Money(20.953m).CeilingMajorUnit() // 21 USD
Iso4217.CurrencySet.USD.Money(20.953m).FloorMajorUnit()   // 20 USD
Iso4217.CurrencySet.USD.Money(20.953m).CeilingMinorUnit() // 20.96 USD
Iso4217.CurrencySet.USD.Money(20.953m).FloorMinorUnit()   // 20.95 USD
```

# Serializing
* [BSON](NMoney.Bson/README.md)