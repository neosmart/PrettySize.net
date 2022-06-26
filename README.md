# PrettySize, a .NET library for dealing with and formatting file sizes

PrettySize is a .NET (2.0+ with full .NET Core/.NET 6 support) library for dealing with and formatting/pretty-printing sizes of all kind. It was originally designed to make automatically formatting a file size (in terms of converting to the correct unit and choosing just the right amount of decimal precision) for pretty printing for human consumption a breeze, and has since evolved to include additionally functionality.

## Installation

PrettySize is published as a NuGet package, meaning installation is as simple as looking up "PrettySize" in the Visual Studio package manager or by typing the following in the Visual Studio "Package Manager Console":

```
Install-Package NeoSmart.PrettySize
```

## Use

The PrettySize package exposes the entirety of its surface area via the `PrettySize` struct/class under the `NeoSmart.PrettySize` namespace. `PrettySize` is a `readonly struct` wrapper around a file size and exposes strongly-typed size-related operations via that interface.

Creating a `PrettySize` instance can be done directly (given a raw file size in terms of bytes) or via any of the static helper functions/constructors that provide for unit-based construction. Both base-10 (KB, MB, etc) and base-2 (KiB, MiB, etc) units are supported via the same interface:

```csharp
using NeoSmart.PrettySize;

public void Main() {
    // Initializing directly via the default constructor:
    var size = new PrettySize(200);
    Console.WriteLine($"Size: {size}"); // Prints "Size: 200 bytes"

    // Initializing via a unit-based helper function:
    var size1 = PrettySize.KiB(28);
    var size2 = PrettySize.Bytes(14336);
    var sum = size1 + size2;
    Console.WriteLine($"The total size is {sum}"); // Prints "The total size is 42.00 KiB"
}
```

The benefits of using a strongly-typed object instead of passing around `long` or `ulong` file sizes include (as shown above) the ability to add sizes with different units, being prevented from mixing up types and performing mathematical operations that make no sense (you can safely add/subtract two `PrettySize` values or you can multiply/divide a `PrettySize` value and a scalar `long` value, but you can't do something like accidentally multiply two file sizes together), and having something that you can always just print or convert to a string without worrying about formatting and readability.

For more control over how sizes are formatted as text, you can use `PrettySize.Format()` method, which lets you specify the units to be used (base-2 or base-10) and how unit names are spelled out (abbreviated, unabridged, lowercase, etc):

```csharp
// using NeoSmart.PrettySize
var size = PrettySize.Bytes(2048);

var formatted = size.Format(UnitBase.Base2, UnitStyle.Full);
Console.WriteLine(formatted); // Prints "2.00 Kebibytes"

var formatted2 = size.Format(UnitBase.Base10, UnitStyle.FullLower);
Console.WriteLine(formatted2); // Prints "2.05 kilobytes"
```

### Use with scalar size values

The PrettySize project also includes some constants and static methods that can be used to create and format scalar file sizes (as `long` values expressing the file size in bytes). Their usage is discouraged and is currently included only for legacy compatibility:

```csharp
// using NeoSmart.PrettySize
var untypedSize = 1 * EXABYTE;
var formatted = PrettySize.Format(untypedSize, UnitBase.Base10);
Console.WriteLine(formatted); // Prints "1.00 EB"
```

# License and Credits

PrettySize was developed by Mahmoud Al-Qudsi of NeoSmart Technologies and is released under the terms of the MIT public license. The name "PrettySize" is copyright NeoSmart Technologies 2017 and may not be used without permission (fair use is excluded).
