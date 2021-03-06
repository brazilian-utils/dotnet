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

Cpf.Format("52945557789"); // 529.455.577-89

Cpf.Generate(); // 38041016588
```
### CNPJ
```csharp
Cnpj.IsValid("11886541000185"); // true

Cnpj.IsValid("invalid-cnpj"); // false
Cnpj.IsValid(""); // false
Cnpj.IsValid(null); // false

Cnpj.Format("11886541000185"); // 11.886.541.0001-85

Cnpj.Generate(); // 13401551551768
```
### Boleto
```csharp
Boleto.IsValid("00198.10001 00030.212237 00217.236553 1 35742800321323"); // true
```
### Phone
```csharp
Phone.IsValid("(51) 99922-3344"); // true
Phone.IsValid("51999223344"); // true
```
### Cep
```csharp
Cep.IsValid("92990000"); // true
Cep.IsValid("92990-000"); // true

Cep.Format("92990000"); // 92990-000
```
### Utils
```csharp
Helpers.OnlyNumbers("123abc"); // 123
```
