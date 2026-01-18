# Brazilian Utils .NET

Utils library for specific Brazilian businesses.

[![Build Status](https://travis-ci.org/brazilian-utils/dotnet.svg?branch=master)](https://travis-ci.org/brazilian-utils/dotnet)
![NuGet](https://img.shields.io/nuget/v/BrazilianUtils.svg)

*[Versão em Português](README.md)*

## Installation

```sh
# dotnet CLI
dotnet add package BrazilianUtils

# Package Manager
PM> Install-Package BrazilianUtils
```

## Usage

- [CPF](#cpf)
- [CNPJ](#cnpj)
- [CEP](#cep)
- [Boleto](#boleto)
- [Phone](#phone)
- [CNH](#cnh)
- [PIS](#pis)
- [RENAVAM](#renavam)
- [Voter ID](#voter-id)
- [Legal Process](#legal-process)
- [License Plate](#license-plate)
- [Currency](#currency)
- [Helpers](#helpers)

---

### CPF

The CPF (Cadastro de Pessoas Físicas) is the Brazilian individual taxpayer registry identification.

#### Validate

```csharp
using BrazilianUtils;

Cpf.IsValid("529.455.577-89");  // true
Cpf.IsValid("52945557789");     // true

Cpf.IsValid("invalid-cpf");     // false
Cpf.IsValid("111.111.111-11");  // false (repdigit)
Cpf.IsValid("");                // false
Cpf.IsValid(null);              // false
```

#### Format

```csharp
Cpf.Format("52945557789");  // "529.455.577-89"
```

#### Generate

```csharp
Cpf.Generate();  // "38041016588"
```

---

### CNPJ

The CNPJ (Cadastro Nacional da Pessoa Jurídica) is the Brazilian company's legal entity identification number.

#### Validate

```csharp
using BrazilianUtils;

Cnpj.IsValid("11.886.541/0001-85");  // true
Cnpj.IsValid("11886541000185");      // true

// Alphanumeric CNPJ (Instrução Normativa RFB nº 2.119)
Cnpj.IsValid("AA.BBB.CCC/DDDD-01");  // true (if check digits are valid)

Cnpj.IsValid("invalid-cnpj");        // false
Cnpj.IsValid("");                    // false
Cnpj.IsValid(null);                  // false
```

#### Format

```csharp
Cnpj.Format("11886541000185");  // "11.886.541/0001-85"
```

#### Generate

```csharp
// Generate numeric CNPJ
Cnpj.Generate();             // "13401551551768"

// Generate alphanumeric CNPJ
Cnpj.GenerateAlphanumeric(); // "AB12CD34EF5601"
```

---

### CEP

The CEP (Código de Endereçamento Postal) is the Brazilian postal code.

#### Validate

```csharp
using BrazilianUtils;

Cep.IsValid("92990-000");  // true
Cep.IsValid("92990000");   // true

Cep.IsValid("12345");      // false (wrong length)
Cep.IsValid("invalid");    // false
```

#### Format

```csharp
Cep.Format("92990000");  // "92990-000"
```

---

### Boleto

Validates Brazilian bank slips (boleto bancário) in both 44-digit barcode and 47-digit digitable line formats.

#### Validate

```csharp
using BrazilianUtils;

// Digitable line (47 digits)
Boleto.IsValid("00198.10001 00030.212237 00217.236553 1 35742800321323");  // true

// Barcode (44 digits)
Boleto.IsValid("00193357428003213230001000302122700021723655");  // true

Boleto.IsValid("invalid-boleto");  // false
```

---

### Phone

Validates Brazilian phone numbers including both mobile and landline formats.

#### Validate

```csharp
using BrazilianUtils;

// Mobile (11 digits with 9th digit)
Phone.IsValid("(51) 99922-3344");  // true
Phone.IsValid("51999223344");      // true

// Landline (10 digits)
Phone.IsValid("(11) 3344-5566");   // true
Phone.IsValid("1133445566");       // true

Phone.IsValid("(11) 9000-0000");   // false (invalid first digit)
Phone.IsValid("1234567");          // false (wrong length)
```

---

### CNH

The CNH (Carteira Nacional de Habilitação) is the Brazilian driver's license. Validates the 2022+ format.

#### Validate

```csharp
using BrazilianUtils;

Cnh.isValidCnh("98765432100");   // true
Cnh.isValidCnh("987654321-00");  // true (symbols are ignored)

Cnh.isValidCnh("12345678901");   // false (invalid check digits)
Cnh.isValidCnh("00000000000");   // false (repdigit)
Cnh.isValidCnh("A2C45678901");   // false (non-numeric)
```

---

### PIS

The PIS (Programa de Integração Social) is the Brazilian social integration program identification number.

#### Validate

```csharp
using BrazilianUtils;

Pis.isValid("82178537464");  // true
Pis.isValid("55550207753");  // true

Pis.isValid("12345678901");  // false (invalid check digit)
Pis.isValid("1234567");      // false (wrong length)
```

#### Format

```csharp
Pis.formatPis("12345678909");  // Some "123.45678.90-9"
```

#### Generate

```csharp
Pis.generate();  // "82178537464"
```

---

### RENAVAM

The RENAVAM (Registro Nacional de Veículos Automotores) is the Brazilian vehicle registration number.

#### Validate

```csharp
using BrazilianUtils;

Renavam.isValidRenavam("86769597308");  // true

Renavam.isValidRenavam("12345678901");  // false (invalid check digit)
Renavam.isValidRenavam("11111111111");  // false (repdigit)
Renavam.isValidRenavam("1234567");      // false (wrong length)
```

---

### Voter ID

The Voter ID (Título de Eleitor) is the Brazilian voter registration document.

#### Validate

```csharp
using BrazilianUtils;

VoterId.isValid("690847092828");  // true
VoterId.isValid("163204010922");  // true

VoterId.isValid("123456789012");  // false (invalid check digits)
VoterId.isValid("12345");         // false (wrong length)
```

#### Format

```csharp
VoterId.formatVoterId("690847092828");  // Some "6908 4709 28 28"
```

#### Generate

```csharp
VoterId.generate("SP");  // Some "123456780101" (São Paulo)
VoterId.generate("RJ");  // Some "987654320352" (Rio de Janeiro)
VoterId.generate("ZZ");  // Some "112233440028" (Foreigners)
```

---

### Legal Process

Validates Brazilian legal process numbers following the CNJ (Conselho Nacional de Justiça) unified numbering standard.

Format: `NNNNNNN-DD.AAAA.J.TR.OOOO`

- `NNNNNNN`: 7-digit sequential number
- `DD`: 2-digit check digits (Module 97 Base 10, ISO 7064:2003)
- `AAAA`: 4-digit year
- `J`: 1-digit justice segment (1-9)
- `TR`: 2-digit court/tribunal
- `OOOO`: 4-digit origin unit

#### Validate

```csharp
using BrazilianUtils;

LegalProcess.isValid("6847650-61.2023.3.03.0000");  // true
LegalProcess.isValid("68476506120233030000");       // true

LegalProcess.isValid("68476506020233030000");       // false (wrong check digit)
LegalProcess.isValid("123");                         // false (wrong length)
```

#### Format

```csharp
LegalProcess.formatLegalProcess("68476506120233030000");  // Some "6847650-61.2023.3.03.0000"
```

#### Generate

```csharp
LegalProcess.generate(Some 2024, Some 5);  // Some "12345678720245050000"
LegalProcess.generate(None, None);          // Random valid legal process ID
```

---

### License Plate

Validates and formats Brazilian vehicle license plates in both old format (LLLNNNN) and Mercosul format (LLLNLNN).

#### Validate

```csharp
using BrazilianUtils;

// Old format (LLLNNNN)
LicensePlate.isValid("ABC1234", Some "old_format");  // true
LicensePlate.isValid("ABC1234", None);               // true

// Mercosul format (LLLNLNN)
LicensePlate.isValid("ABC1D23", Some "mercosul");    // true
LicensePlate.isValid("ABC1D23", None);               // true

LicensePlate.isValid("ABCD123", None);               // false
```

#### Format

```csharp
LicensePlate.formatLicensePlate("ABC1234");  // Some "ABC-1234" (old format)
LicensePlate.formatLicensePlate("abc1e34");  // Some "ABC1E34"  (Mercosul)
```

#### Convert to Mercosul

```csharp
LicensePlate.convertToMercosul("ABC4567");  // Some "ABC4F67"
```

#### Get Format

```csharp
LicensePlate.getFormat("ABC1234");  // Some "LLLNNNN"
LicensePlate.getFormat("ABC1D23");  // Some "LLLNLNN"
```

#### Generate

```csharp
LicensePlate.generate(None);                // Some "ABC1D23" (Mercosul by default)
LicensePlate.generate(Some "LLLNLNN");      // Some "XYZ2E45" (Mercosul)
LicensePlate.generate(Some "LLLNNNN");      // Some "ABC1234" (Old format)
```

---

### Currency

Utilities for Brazilian Real (BRL) currency formatting and text conversion.

#### Format Currency

```csharp
using BrazilianUtils;

Currency.formatCurrency(1234.56M);   // Some "R$ 1.234,56"
Currency.formatCurrency(-1234.56M);  // Some "R$ -1.234,56"
Currency.formatCurrency(0M);         // Some "R$ 0,00"
```

#### Convert to Text

```csharp
Currency.convertRealToText(1523.45M);
// Some "Mil, quinhentos e vinte e três reais e quarenta e cinco centavos"

Currency.convertRealToText(1.00M);
// Some "Um real"

Currency.convertRealToText(0.50M);
// Some "Cinquenta centavos"

Currency.convertRealToText(0.00M);
// Some "Zero reais"

Currency.convertRealToText(-100.00M);
// Some "Menos cem reais"

Currency.convertRealToText(1000000.00M);
// Some "Um milhão de reais"
```

---

### Helpers

General utility functions for string manipulation.

#### Extract Only Numbers

```csharp
using BrazilianUtils;

Helpers.OnlyNumbers("123abc456");     // "123456"
Helpers.OnlyNumbers("(11) 9999-0000"); // "1199990000"
Helpers.OnlyNumbers("CPF: 529.455.577-89"); // "52945557789"
```

---

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request. For major changes, please open an issue first to discuss what you would like to change.

## License

This project is open source and available under the [MIT License](LICENSE).
