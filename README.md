# Brazilian Utils .NET

Biblioteca de utilitários para regras de negócio específicas do Brasil.

[![Build Status](https://travis-ci.org/brazilian-utils/dotnet.svg?branch=master)](https://travis-ci.org/brazilian-utils/dotnet)
![NuGet](https://img.shields.io/nuget/v/BrazilianUtils.svg)

*[English version](README_EN.md)*

## Instalação

```sh
# dotnet CLI
dotnet add package BrazilianUtils

# Package Manager
PM> Install-Package BrazilianUtils
```

## Utilização

- [CPF](#cpf)
- [CNPJ](#cnpj)
- [CEP](#cep)
- [Boleto](#boleto)
- [Telefone](#telefone)
- [CNH](#cnh)
- [PIS](#pis)
- [RENAVAM](#renavam)
- [Título de Eleitor](#título-de-eleitor)
- [Processo Jurídico](#processo-jurídico)
- [Placa de Veículo](#placa-de-veículo)
- [Moeda](#moeda)
- [Utilitários](#utilitários)

---

### CPF

O CPF (Cadastro de Pessoas Físicas) é o registro de identificação do contribuinte individual brasileiro.

#### Validar

```csharp
using BrazilianUtils;

Cpf.IsValid("529.455.577-89");  // true
Cpf.IsValid("52945557789");     // true

Cpf.IsValid("cpf-invalido");    // false
Cpf.IsValid("111.111.111-11");  // false (dígitos repetidos)
Cpf.IsValid("");                // false
Cpf.IsValid(null);              // false
```

#### Formatar

```csharp
Cpf.Format("52945557789");  // "529.455.577-89"
```

#### Gerar

```csharp
Cpf.Generate();  // "38041016588"
```

---

### CNPJ

O CNPJ (Cadastro Nacional da Pessoa Jurídica) é o número de identificação de entidades jurídicas no Brasil.

#### Validar

```csharp
using BrazilianUtils;

Cnpj.IsValid("11.886.541/0001-85");  // true
Cnpj.IsValid("11886541000185");      // true

// CNPJ Alfanumérico (Instrução Normativa RFB nº 2.119)
Cnpj.IsValid("AA.BBB.CCC/DDDD-01");  // true (se os dígitos verificadores forem válidos)

Cnpj.IsValid("cnpj-invalido");       // false
Cnpj.IsValid("");                    // false
Cnpj.IsValid(null);                  // false
```

#### Formatar

```csharp
Cnpj.Format("11886541000185");  // "11.886.541/0001-85"
```

#### Gerar

```csharp
// Gerar CNPJ numérico
Cnpj.Generate();             // "13401551551768"

// Gerar CNPJ alfanumérico
Cnpj.GenerateAlphanumeric(); // "AB12CD34EF5601"
```

---

### CEP

O CEP (Código de Endereçamento Postal) é o código postal brasileiro.

#### Validar

```csharp
using BrazilianUtils;

Cep.IsValid("92990-000");  // true
Cep.IsValid("92990000");   // true

Cep.IsValid("12345");      // false (tamanho incorreto)
Cep.IsValid("invalido");   // false
```

#### Formatar

```csharp
Cep.Format("92990000");  // "92990-000"
```

---

### Boleto

Valida boletos bancários brasileiros nos formatos de código de barras (44 dígitos) e linha digitável (47 dígitos).

#### Validar

```csharp
using BrazilianUtils;

// Linha digitável (47 dígitos)
Boleto.IsValid("00198.10001 00030.212237 00217.236553 1 35742800321323");  // true

// Código de barras (44 dígitos)
Boleto.IsValid("00193357428003213230001000302122700021723655");  // true

Boleto.IsValid("boleto-invalido");  // false
```

---

### Telefone

Valida números de telefone brasileiros, incluindo formatos de celular e telefone fixo.

#### Validar

```csharp
using BrazilianUtils;

// Celular (11 dígitos com o nono dígito)
Phone.IsValid("(51) 99922-3344");  // true
Phone.IsValid("51999223344");      // true

// Telefone fixo (10 dígitos)
Phone.IsValid("(11) 3344-5566");   // true
Phone.IsValid("1133445566");       // true

Phone.IsValid("(11) 9000-0000");   // false (primeiro dígito inválido)
Phone.IsValid("1234567");          // false (tamanho incorreto)
```

---

### CNH

A CNH (Carteira Nacional de Habilitação) é a carteira de motorista brasileira. Valida o formato de 2022 em diante.

#### Validar

```csharp
using BrazilianUtils;

Cnh.isValidCnh("98765432100");   // true
Cnh.isValidCnh("987654321-00");  // true (símbolos são ignorados)

Cnh.isValidCnh("12345678901");   // false (dígitos verificadores inválidos)
Cnh.isValidCnh("00000000000");   // false (dígitos repetidos)
Cnh.isValidCnh("A2C45678901");   // false (não numérico)
```

---

### PIS

O PIS (Programa de Integração Social) é o número de identificação do programa de integração social brasileiro.

#### Validar

```csharp
using BrazilianUtils;

Pis.isValid("82178537464");  // true
Pis.isValid("55550207753");  // true

Pis.isValid("12345678901");  // false (dígito verificador inválido)
Pis.isValid("1234567");      // false (tamanho incorreto)
```

#### Formatar

```csharp
Pis.formatPis("12345678909");  // Some "123.45678.90-9"
```

#### Gerar

```csharp
Pis.generate();  // "82178537464"
```

---

### RENAVAM

O RENAVAM (Registro Nacional de Veículos Automotores) é o número de registro de veículos no Brasil.

#### Validar

```csharp
using BrazilianUtils;

Renavam.isValidRenavam("86769597308");  // true

Renavam.isValidRenavam("12345678901");  // false (dígito verificador inválido)
Renavam.isValidRenavam("11111111111");  // false (dígitos repetidos)
Renavam.isValidRenavam("1234567");      // false (tamanho incorreto)
```

---

### Título de Eleitor

O Título de Eleitor é o documento de registro eleitoral brasileiro.

#### Validar

```csharp
using BrazilianUtils;

VoterId.isValid("690847092828");  // true
VoterId.isValid("163204010922");  // true

VoterId.isValid("123456789012");  // false (dígitos verificadores inválidos)
VoterId.isValid("12345");         // false (tamanho incorreto)
```

#### Formatar

```csharp
VoterId.formatVoterId("690847092828");  // Some "6908 4709 28 28"
```

#### Gerar

```csharp
VoterId.generate("SP");  // Some "123456780101" (São Paulo)
VoterId.generate("RJ");  // Some "987654320352" (Rio de Janeiro)
VoterId.generate("ZZ");  // Some "112233440028" (Estrangeiros)
```

---

### Processo Jurídico

Valida números de processos jurídicos brasileiros seguindo o padrão de numeração única do CNJ (Conselho Nacional de Justiça).

Formato: `NNNNNNN-DD.AAAA.J.TR.OOOO`

- `NNNNNNN`: Número sequencial de 7 dígitos
- `DD`: Dígitos verificadores de 2 dígitos (Módulo 97 Base 10, ISO 7064:2003)
- `AAAA`: Ano com 4 dígitos
- `J`: Segmento de justiça de 1 dígito (1-9)
- `TR`: Tribunal de 2 dígitos
- `OOOO`: Unidade de origem de 4 dígitos

#### Validar

```csharp
using BrazilianUtils;

LegalProcess.isValid("6847650-61.2023.3.03.0000");  // true
LegalProcess.isValid("68476506120233030000");       // true

LegalProcess.isValid("68476506020233030000");       // false (dígito verificador incorreto)
LegalProcess.isValid("123");                         // false (tamanho incorreto)
```

#### Formatar

```csharp
LegalProcess.formatLegalProcess("68476506120233030000");  // Some "6847650-61.2023.3.03.0000"
```

#### Gerar

```csharp
LegalProcess.generate(Some 2024, Some 5);  // Some "12345678720245050000"
LegalProcess.generate(None, None);          // ID de processo jurídico válido aleatório
```

---

### Placa de Veículo

Valida e formata placas de veículos brasileiros no formato antigo (LLLNNNN) e no formato Mercosul (LLLNLNN).

#### Validar

```csharp
using BrazilianUtils;

// Formato antigo (LLLNNNN)
LicensePlate.isValid("ABC1234", Some "old_format");  // true
LicensePlate.isValid("ABC1234", None);               // true

// Formato Mercosul (LLLNLNN)
LicensePlate.isValid("ABC1D23", Some "mercosul");    // true
LicensePlate.isValid("ABC1D23", None);               // true

LicensePlate.isValid("ABCD123", None);               // false
```

#### Formatar

```csharp
LicensePlate.formatLicensePlate("ABC1234");  // Some "ABC-1234" (formato antigo)
LicensePlate.formatLicensePlate("abc1e34");  // Some "ABC1E34"  (Mercosul)
```

#### Converter para Mercosul

```csharp
LicensePlate.convertToMercosul("ABC4567");  // Some "ABC4F67"
```

#### Obter Formato

```csharp
LicensePlate.getFormat("ABC1234");  // Some "LLLNNNN"
LicensePlate.getFormat("ABC1D23");  // Some "LLLNLNN"
```

#### Gerar

```csharp
LicensePlate.generate(None);                // Some "ABC1D23" (Mercosul por padrão)
LicensePlate.generate(Some "LLLNLNN");      // Some "XYZ2E45" (Mercosul)
LicensePlate.generate(Some "LLLNNNN");      // Some "ABC1234" (Formato antigo)
```

---

### Moeda

Utilitários para formatação e conversão de texto do Real brasileiro (BRL).

#### Formatar Moeda

```csharp
using BrazilianUtils;

Currency.formatCurrency(1234.56M);   // Some "R$ 1.234,56"
Currency.formatCurrency(-1234.56M);  // Some "R$ -1.234,56"
Currency.formatCurrency(0M);         // Some "R$ 0,00"
```

#### Converter para Texto

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

### Utilitários

Funções utilitárias gerais para manipulação de strings.

#### Extrair Apenas Números

```csharp
using BrazilianUtils;

Helpers.OnlyNumbers("123abc456");          // "123456"
Helpers.OnlyNumbers("(11) 9999-0000");     // "1199990000"
Helpers.OnlyNumbers("CPF: 529.455.577-89"); // "52945557789"
```

---

## Contribuindo

Contribuições são bem-vindas! Sinta-se à vontade para enviar um Pull Request. Para mudanças significativas, por favor abra uma issue primeiro para discutir o que você gostaria de alterar.

## Licença

Este projeto é open source e está disponível sob a [Licença MIT](LICENSE).
