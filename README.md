This library deals with **prime numbers** (generating collections of them), getting the **prime factors** for arbitrary positive integers and also provides **fraction** data type which may be used to represent, format and calculate with *rational numbers*.

*All types described below are in the namespace `CSF`.*

## Getting prime numbers
To get a collection of prime numbers, use the service `PrimeNumberGenerator`, which implements the interface `CSF.IGetsPrimeNumbers`.  The method `GetPrimeNumbers` will return a collection of prime numbers up to a specified upper limit.  If you instead wish to get the first X amount of prime numbers, use an arbitrarily high upper limit combined with Linq `.Take`.  For example:

```csharp
var firstTwentyPrimes = new PrimeNumberGenerator().GetPrimeNumbers(10000).Take(20);
```

This mechanism will not result in extra work, calculating primes beyond the 20th.

## Getting prime factors
To calculate the prime factors for an arbitrary positive integer, use the service `PrimeFactorProvider`, which implements the interface `IGetsPrimeFactors`.  The method `GetPrimeFactors` will return a collection of all of the prime factors for the specified number.  It will return duplicates if the same prime factor goes more than once into the number.  For example:

```csharp
var primeFactors = new PrimeFactorProvider().GetPrimeFactors(18);
// primeFactors will be equivalent to new [] { 2, 3, 3, 3 }
```

## Getting common prime factors
To get the prime factors *which are common to two specified positive integers*, use the service `CommonPrimeFactorProvider`, which implements `IGetsCommonPrimeFactors`.  The `GetPrimeFactors` method will return a collection of prime factors which are common to both specified inputs.  For example:

```csharp
var commonFactors = new CommonPrimeFactorProvider().GetPrimeFactors(18, 6);
// commonFactors will be equivalent to new [] { 2, 3 }
```

## Fractions
The `Fraction` struct is a powerful representation of rational numbers, both positive and negative.  It may represent both a whole-number/integer component as well as a fractional (numerator/denominator) component.  The fractional component may be a **vulgar fraction**, whereby the numerator is greater than the denominator.

### Creating a fraction
```csharp
// Via a constructor
var fraction = new Fraction(3, 4);              // Three-quarters

// Via a constructor with a whole-number part
var fraction = new Fraction(2, 3, 4);           // Two and three-quarters

// Via a constructor with a negative amount
var fraction = new Fraction(-2, 3, 4);          // Negative two and three-quarters

// Via a constructor with a negative fraction-only
var fraction = new Fraction(-3, 4);             // Negative three-quarters

// Parsing a fraction from string
var fraction = Fraction.Parse("3/4");           // Three-quarters

// Parsing a fraction from string with a whole-number part
var fraction = Fraction.Parse("2 3/4");         // Two and three-quarters

// Parsing a negative fraction from string
var fraction = Fraction.Parse("-3/4");          // Negative three-quarters

// Parsing a fraction using a service; any parsing
// example above may be performed this way.  The FractionParser
// implements the interface IParsesFraction.
new FractionParser().Parse("3/4")               // Three-quarters

// Whole numbers may also be implicitly cast as fractions
Fraction fraction = 5;                          // Five
```

### Math with fractions
Fractions may be added, subtracted, multiplied and divided:

```csharp
Fraction.Parse("3/4") * 2;                      // One and one-half
```

### Comparing fractions
Fractions implement `IComparable<Fraction>`, `IComparable` and provide the operators `<`, `>`, `<=` and `>=`:

```csharp
Fraction.Parse("3/4") > Fraction.Parse("3/5")   // True
```

Fractions may also be tested for equality with one another; the operators `==` and `!=` are also implemented.  When performing *any kind of comparison*, fractions are **simplified** (see below) before the comparison is made.  This means that (for example), one-quarter is correctly treated as equal to two-eights.

### Simplifying fractions
Fractions may be **simplified** to their smallest form.  An optional parameter specifies whether or not prefer simplification to a **vulgar fraction** (if applicable) or not.  By default vulgar fractions are not preferred.

```csharp
// Simplify a fraction
Fraction.Parse("50/100").Simplify()             // One-half

// Simplify a vulgar fraction to become mixed-whole-number-and-fraction
Fraction.Parse("125/50").Simplify()             // Two and one-half

// Simplify a fraction and prefer vulgar fractions
Fraction.Parse("125/50").Simplify(true)         // Five-halves
```

### Formatting fractions
Fractions may be formatted as strings.  The `Fraction.ToString()` method internally uses the **standard** mechanism described below, without simplification (the first example).  For more functionality use the service `FractionFormatter`, which implements `IFormatsFraction`.

```csharp
// Standard mechanism, without simplification
var result = new FractionFormatter().Format(Fraction.Parse("125/100"), "S");
// result is "125/100"

// Standard mechanism, with simplification; this is also the
// default if the format specifier is omitted
var result = new FractionFormatter().Format(Fraction.Parse("125/100"), "s");
// result is "1 1/4"

// Force a leading zero, without simplification
var result = new FractionFormatter().Format(Fraction.Parse("25/100"), "Z");
// result is "0 25/100"

// Force a leading zero, with simplification
var result = new FractionFormatter().Format(Fraction.Parse("25/100"), "z");
// result is "0 1/4"

// Prefer vulgar fraction (implies simplification)
var result = new FractionFormatter().Format(Fraction.Parse("125/100"), "v");
// result is "5/4"
```

### Converting to plain numbers
Fractions may be cast/converted to numeric types.  Because of the risk of data-loss, casts must be explicit.

```csharp
decimal number = (decimal) Fraction.Parse("3/4");
// number is 0.75m
```

The same principle applies to `double` and `float`.  Fractions may also be cast to integer types: `sbyte`, `short`, `int`, `long`, `byte`, `ushort`, `uint` or `ulong`, but when doing so only the whole-number portion of the fraction will be used.
