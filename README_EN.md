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
- [CAEPF](#caepf)
- [Legal Nature](#legal-nature)
- [CEP](#cep)
- [Municipality](#municipality)
- [State](#state)
- [Area Code](#area-code)
- [Boleto](#boleto)
- [Credit Card](#credit-card)
- [Bank](#bank)
- [Bank Account](#bank-account)
- [IBAN](#iban)
- [Pix Key](#pix-key)
- [BR Code (Pix Copy-and-Paste)](#br-code-pix-copy-and-paste)
- [Phone](#phone)
- [Email](#email)
- [CNH](#cnh)
- [VIN (Chassis)](#vin-chassis)
- [PIS](#pis)
- [RENAVAM](#renavam)
- [CNO](#cno)
- [CEI](#cei)
- [Voter ID](#voter-id)
- [Legal Process](#legal-process)
- [NF-e Access Key](#nf-e-access-key)
- [Civil Registry Certificate](#civil-registry-certificate)
- [Passport](#passport)
- [Professional License](#professional-license)
- [CNS](#cns)
- [License Plate](#license-plate)
- [CBO](#cbo)
- [CNAE](#cnae)
- [NCM](#ncm)
- [CFOP](#cfop)
- [CST](#cst)
- [CSOSN](#csosn)
- [Currency](#currency)
- [Number to Words](#number-to-words)
- [Date](#date)
- [Helpers](#helpers)
- [Text](#text)

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

#### Remove Formatting

```csharp
Cpf.RemoveSymbols("831.595.621-31");  // "83159562131"
Cpf.Parse("943.895.751-04");          // "94389575104"
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

#### Remove Formatting

```csharp
Cnpj.RemoveSymbols("10.799.163/9892-71");  // "10799163989271"
Cnpj.Parse("46.843.485/0001-86");          // "46843485000186"
```

#### Generate

```csharp
// Generate numeric CNPJ
Cnpj.Generate();             // "13401551551768"

// Generate alphanumeric CNPJ
Cnpj.GenerateAlphanumeric(); // "AB12CD34EF5601"
```

---

### CAEPF

The CAEPF (Cadastro de Atividade Econômica da Pessoa Física) identifies the economic activity of an individual before the Brazilian Federal Revenue.

#### Validate

```csharp
using BrazilianUtils;

Caepf.IsValid("29311861000184");  // true
Caepf.IsValid("invalid-caepf");   // false
```

#### Format

```csharp
Caepf.Format("29311861000184");  // "293.118.610/001-84"
```

---

### Legal Nature

The Legal Nature identifies an entity's type of incorporation according to the CONCLA (Comissão Nacional de Classificação) table published by IBGE.

#### Validate

```csharp
using BrazilianUtils;

LegalNature.IsValid("2062");  // true
LegalNature.IsValid("9999");  // false
```

#### Format

```csharp
LegalNature.Format("2062");  // "206-2"
```

#### Query

```csharp
LegalNature.GetDescription("2062");  // Some "Sociedade Empresária Limitada"
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

#### Remove Formatting

```csharp
Cep.RemoveSymbols("91906-292");  // "91906292"
Cep.Parse("01001-000");          // "01001000"
```

#### Generate

```csharp
Cep.Generate();  // "06156069"
```

---

### Municipality

Looks up Brazilian municipalities published by IBGE by their 7-digit code, or lists every municipality of a state.

#### Query

```csharp
using BrazilianUtils;

Municipality.GetByCode("5200050");  // Some { Code = "5200050"; Name = "Abadia de Goiás"; StateCode = "GO" }
Municipality.List(Some "GO");       // every municipality of Goiás, sorted by name
```

---

### State

The 27 Brazilian federative units, with region, IBGE code and timezone.

#### Query

```csharp
using BrazilianUtils;

State.GetNameByCode("SP");         // Some "São Paulo"
State.GetCodeByName("São Paulo");  // Some "SP"
State.GetTimezone("SP");           // Some "America/Sao_Paulo"
```

---

### Area Code

Looks up the state and region a Brazilian area code (DDD) belongs to.

#### Query

```csharp
using BrazilianUtils;

AreaCode.GetInfo(11).Value.StateName;  // "São Paulo"
AreaCode.ListByState("SP");            // [11; 12; 13; 14; 15; 16; 17; 18; 19]
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

#### Format

```csharp
Boleto.Format("03694928065284362782017695692230200005860034350");
// "03694.92806 52843.627820 17695.692230 2 00005860034350"
```

#### Generate

```csharp
Boleto.Generate();  // "03694928065284362782017695692230200005860034350"
```

---

### Credit Card

Validates a payment card number (credit or debit) using the Luhn algorithm: 12 to 19 digits and a correct check digit.

#### Validate

```csharp
using BrazilianUtils;

CreditCard.IsValid("4111 1111 1111 1111");  // true
CreditCard.IsValid("1234567890123456");     // false
CreditCard.IsValid("1111111111111111");     // false (repdigit)
```

---

### Bank

Looks up STR (Sistema de Transferência de Reservas) participants published by the Central Bank of Brazil, by COMPE code or by ISPB.

#### Query

```csharp
using BrazilianUtils;

Bank.GetByCode("341");      // Some { Code = "341"; Ispb = "60701190"; Name = "ITAÚ UNIBANCO S.A." }
Bank.GetByIspb("60701190"); // Some { Code = "341"; Ispb = "60701190"; Name = "ITAÚ UNIBANCO S.A." }
```

---

### Bank Account

Validates a Brazilian bank account (bank, branch, account and check digit), recognizing the specific algorithms published for some banks.

#### Validate

```csharp
using BrazilianUtils;

// Generic fallback (modulus 10 or modulus 11)
BankAccount.IsValid({ BankCode = "341"; Agency = "1234"; Account = "12345"; Digit = "5" });  // true

// Nubank: check digit via the Verhoeff algorithm
BankAccount.IsValid({ BankCode = "260"; Agency = "0001"; Account = "1234567"; Digit = "9" }); // true
```

---

### IBAN

Validates and formats a Brazilian IBAN: `BR` + check digits (ISO 7064 MOD 97-10) + ISPB + branch + account + account type + owner indicator.

#### Validate

```csharp
using BrazilianUtils;

Iban.IsValid("BR46 0000 0000 0000 1000 1234 567C 1");  // true
```

#### Format

```csharp
Iban.Format("BR4600000000000010001234567C1");
// "BR46 0000 0000 0000 1000 1234 567C 1"
```

#### Query

```csharp
Iban.GetInfo("BR4600000000000010001234567C1").Value.BankIspb;  // "00000000"
```

---

### Pix Key

Identifies and validates a Pix key (CPF, CNPJ, e-mail, Brazilian mobile number or random EVP key), following the DICT formats.

#### Validate

```csharp
using BrazilianUtils;

PixKey.IsValid("11988887777", None);  // true (mobile)
PixKey.GetInfo("usuario@exemplo.com");
// Some { Type = "email"; Value = "usuario@exemplo.com" }
```

---

### BR Code (Pix Copy-and-Paste)

Validates a Pix BR Code payload ("copia e cola"): TLV structure, CRC-16 and the receiver's required fields.

#### Validate

```csharp
using BrazilianUtils;

var payload = "00020101021126380014br.gov.bcb.pix0116user@example.com5204000053039865802BR5910LOJA TESTE6009SAO PAULO6304C50B";

PixPayload.IsValid(payload);  // true
PixPayload.GetInfo(payload).Value.MerchantName;  // "LOJA TESTE"
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

// Type-specific validation
Phone.IsValidMobile("(51) 99922-3344");    // true
Phone.IsValidLandline("(11) 3344-5566");   // true
```

#### Remove Formatting

```csharp
Phone.RemoveSymbols("(11) 3000-0000");  // "1130000000"
Phone.Parse("+55 11 98888-7777");       // "11988887777"
```

#### Format

```csharp
Phone.Format("988887777");   // "98888-7777"
Phone.Format("1130000000");  // "11300-0000"
```

#### Generate

```csharp
Phone.Generate(Some "mobile");    // "77955978581"
Phone.Generate(Some "landline");  // "8837470481"
```

---

### Email

Validates an e-mail address (a practical subset of the WHATWG HTML "valid e-mail address" standard).

#### Validate

```csharp
using BrazilianUtils;

Email.IsValid("usuario@exemplo.com");  // true
Email.IsValid("usuario@dominio");      // false (no top-level domain)
Email.IsValid("");                     // false
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

#### Format

```csharp
Cnh.Format("98765432100");  // "987654321-00"
```

#### Remove Formatting

```csharp
Cnh.Parse("987654321-00");  // "98765432100"
```

#### Generate

```csharp
Cnh.Generate();  // "04253983910"
```

---

### VIN (Chassis)

Structurally validates a VIN (Vehicle Identification Number / chassis): 17 characters, none of the excluded letters `I`, `O`, `Q`, and the check digit at position 9 (North American rule).

#### Validate

```csharp
using BrazilianUtils;

Vin.IsValid("1HGCM82633A004352");  // true

Vin.IsValid("IO1CM82633A004352");     // false (letters I/O/Q are not allowed)
Vin.IsValid("11111111111111111");    // false (repeated characters)
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

#### Remove Formatting

```csharp
Pis.parse("123.45678.90-1");  // "12345678901"
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

#### Generate

```csharp
Renavam.Generate();  // "61747177505"
```

---

### CNO

The CNO (Cadastro Nacional de Obras) replaced the CEI for civil construction works and keeps the same numbering and check digit rule.

#### Validate

```csharp
using BrazilianUtils;

Cno.IsValid("110840168062");  // true
```

#### Format

```csharp
Cno.Format("111130137368");  // "11.113.01373/68"
```

---

### CEI

The CEI (Cadastro Específico do INSS) identifies a taxpayer before Social Security.

#### Validate

```csharp
using BrazilianUtils;

Cei.IsValid("277297118187");  // true
```

#### Format

```csharp
Cei.Format("277297118187");  // "27.729.71181/87"
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

#### Remove Formatting

```csharp
VoterId.Parse("690.8470.92.828");         // "690847092828"
VoterId.RemoveSymbols("6908 4709 28 28"); // "690847092828"
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

#### Remove Formatting

```csharp
LegalProcess.parse("0002080-25.2012.5.15.0049");  // "00020802520125150049"
```

#### Generate

```csharp
LegalProcess.generate(Some 2024, Some 5);  // Some "12345678720245050000"
LegalProcess.generate(None, None);          // Random valid legal process ID
```

---

### NF-e Access Key

Validates and parses the 44-digit access key of a DF-e: NF-e (55), NFC-e (65), CT-e (57), MDF-e (58), CT-e OS (67), GTV-e (64), BP-e (63), NF3e (66) and NFCom (62).

#### Validate

```csharp
using BrazilianUtils;

NfeKey.IsValid("35240912345678000199570010000000011111111114");  // true
```

#### Format

```csharp
NfeKey.Format("35240912345678000199570010000000011111111114");
// "3524 0912 3456 7800 0199 5700 1000 0000 0111 1111 1114"
```

#### Query

```csharp
NfeKey.GetInfo("35240912345678000199570010000000011111111114").Value.StateCode;  // "SP"
```

---

### Civil Registry Certificate

Validates the 32-digit registration number (matrícula) of a Brazilian civil registry certificate (art. 473 of the CNJ National Registrar Code).

#### Validate

```csharp
using BrazilianUtils;

Certidao.IsValid("104539 01 55 2013 1 00012 021 0000123 21", None);  // true
```

#### Format

```csharp
Certidao.Format("10453901552013100012021000012321");
// "104539 01 55 2013 1 00012 021 0000123 21"
```

---

### Passport

Validates a Brazilian passport number: 2 letters followed by 6 digits. There is no check digit, so a well-formed number is not necessarily real.

#### Validate

```csharp
using BrazilianUtils;

Passport.IsValid("FZ123456");  // true
Passport.IsValid("12345678");  // false (the decimal form is never valid)
```

#### Generate

```csharp
Passport.Generate();  // "IH753095"
```

---

### Professional License

Checks only the structure of a professional license/registration (OAB, CRM, CRO, CRP or CRC) — digit count and state — never a check digit.

#### Validate

```csharp
using BrazilianUtils;

RegistroProfissional.IsValid({ Value = "123456/SP"; Council = "OAB"; State = None });     // true
RegistroProfissional.IsValid({ Value = "SP-123456/O-3"; Council = "CRC"; State = None }); // true
```

---

### CNS

The CNS (Cartão Nacional de Saúde) identifies a user of Brazil's public health system (SUS).

#### Validate

```csharp
using BrazilianUtils;

Cns.IsValid("123456789010000");  // true
```

#### Format

```csharp
Cns.Format("123456789010001");  // "123 4567 8901 0001"
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

### CBO

Looks up the official CBO 2002 table (Brazilian Classification of Occupations) by its 6-digit code.

#### Query

```csharp
using BrazilianUtils;

Cbo.IsValid("010105");  // true
Cbo.Get("010105");      // Some { Code = "010105"; Description = "Oficial general da aeronáutica" }
```

---

### CNAE

Looks up the official CNAE-Subclasses 2.3 table (National Classification of Economic Activities) by its 7-digit code.

#### Query

```csharp
using BrazilianUtils;

Cnae.IsValid("0111301");  // true
Cnae.Get("0111301");      // Some { Code = "0111301"; Description = "CULTIVO DE ARROZ" }
```

#### Format

```csharp
Cnae.Format("0111301");  // "0111-3/01"
```

---

### NCM

Validates an 8-digit NCM code (Nomenclatura Comum do Mercosul) against the current table published by Siscomex.

#### Validate

```csharp
using BrazilianUtils;

Ncm.IsValid("01012100");  // true
```

#### Format

```csharp
Ncm.Format("01012100");  // "0101.21.00"
```

---

### CFOP

Looks up the official CFOP table (Código Fiscal de Operações e Prestações), excluding group/subgroup headers.

#### Query

```csharp
using BrazilianUtils;

Cfop.IsValid("1101");  // true
Cfop.Get("1101");      // Some { Code = "1101"; Description = "Compra para industrialização ou produção rural" }
```

---

### CST

Validates a CST code (Código de Situação Tributária) for ICMS, IPI, PIS or COFINS.

#### Validate

```csharp
using BrazilianUtils;

Cst.IsValid("060", Some "icms");  // true
Cst.IsValid("01", Some "pis");    // true
Cst.IsValid("5", None);           // false (a single digit only applies to ICMS, with origin "0")
```

---

### CSOSN

Validates a CSOSN code (Código de Situação da Operação no Simples Nacional) against the official table (Ajuste SINIEF 07/2005).

#### Validate

```csharp
using BrazilianUtils;

Csosn.IsValid("101");  // true
Csosn.IsValid("999");  // false
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

### Number to Words

Converts an integer (the fractional part is truncated) to its Brazilian Portuguese written-out form.

#### Convert to Text

```csharp
using BrazilianUtils;

Number.ConvertToWords(1235.0);  // "mil duzentos e trinta e cinco"
Number.ConvertToWords(-3.0);    // "menos três"
```

---

### Date

Brazilian national holidays and business day calculations (adding, subtracting and diffing dates).

#### Check Holidays and Business Days

```csharp
using BrazilianUtils;

Date.IsBusinessDay(DateTime(2026, 9, 29), None);  // true
Date.IsHoliday(Some { Date = DateTime(2026, 1, 1); State = None });  // true
```

#### Add and Subtract Business Days

```csharp
Date.AddBusinessDays(DateTime(2026, 9, 29), 5, None);
// Some 2026-10-06

Date.DifferenceInBusinessDays(DateTime(2026, 10, 10), DateTime(2026, 9, 29), None);
// Some 9
```

#### Convert to Text

```csharp
Date.ConvertToWords("01/01/2024");  // "primeiro de janeiro de dois mil e vinte e quatro"
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

### Text

General utilities for capitalizing and removing accents from Brazilian text (names, company names, addresses).

#### Capitalize

```csharp
using BrazilianUtils;

Text.Capitalize("jose da silva");  // "Jose da Silva"
Text.Capitalize("empresa ltda");   // "Empresa LTDA"
```

#### Remove Accents

```csharp
Text.RemoveAccents("São Paulo");  // "Sao Paulo"
Text.RemoveAccents("Açaí");       // "Acai"
```

---

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request. For major changes, please open an issue first to discuss what you would like to change.

## License

This project is open source and available under the [MIT License](LICENSE).
