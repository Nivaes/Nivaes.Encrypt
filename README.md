# Encrypt helper

Encryption helper.

``` C#
var message = "hello world.";
var pass = "secret phrase";

var encriptedMessage = await EncryptHelper.Encrypt(message, pass);

var decryptMessage = await EncryptHelper.Decrypt(encriptedMessage, "pass");
```

## Packages

| NuGet Package | Latest Versions |
| --- | --- |
| [Nivaes.Encrypt](https://www.nuget.org/packages/Nivaes.Encrypt) <br /> Encryption helper | [![latest stable version](https://img.shields.io/nuget/v/Nivaes.Encrypt.svg)](https://www.nuget.org/packages/Nivaes.Encrypt) |

### Actions

![CI](https://github.com/Nivaes/Nivaes.Encrypt/workflows/CI/badge.svg)

![Build Release](https://github.com/Nivaes/Nivaes.Encrypt/workflows/Build%20Release/badge.svg)

![Publish Release](https://github.com/Nivaes/Nivaes.Encrypt/workflows/Publish%20Release/badge.svg)

