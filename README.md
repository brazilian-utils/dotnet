# :brazil: Brazilian Utils

Utils library for specific Brazilian businesses.

| Package | Status | Nuget |
| ------- | ------ | ----- |
| `BrazilianUtils` | [![Build Status](https://travis-ci.org/brazilian-utils/dotnet.svg?branch=master)](https://travis-ci.org/brazilian-utils/dotnet)| ![NuGet](https://img.shields.io/nuget/v/BrazilianUtils.svg) |

## Installation

```sh
# dotnet cli
> dotnet add package BrazilianUtils

# Package Manger
PM> Install-Package BrazilianUtils
```


## Usage
### CPF

```csharp
Cpf.IsValid("52945557789"); // true

Cpf.IsValid("invalid-cpf"); // false
Cpf.IsValid(""); // false
Cpf.IsValid(null); // false

Cpf.IsValid("", isRequired: false); // true
Cpf.IsValid(null, isRequired: false); // true

Cpf.Format("52945557789"); // 529.455.577-89

```
### CNPJ
```csharp
Cnpj.IsValid("11886541000185"); // true

Cnpj.IsValid("invalid-cnpj"); // false
Cnpj.IsValid(""); // false
Cnpj.IsValid(null); // false

Cnpj.IsValid("", isRequired: false); // true
Cnpj.IsValid(null, isRequired: false); // true

Cnpj.Format("11886541000185"); // 11.886.541.0001-85
```
### Utils
```csharp
Helpers.OnlyNumbers("123abc"); // 123
```
