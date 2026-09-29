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
- [CAEPF](#caepf)
- [Natureza Jurídica](#natureza-jurídica)
- [CEP](#cep)
- [Município](#município)
- [Estado](#estado)
- [DDD](#ddd)
- [Boleto](#boleto)
- [Cartão de Crédito](#cartão-de-crédito)
- [Banco](#banco)
- [Conta Bancária](#conta-bancária)
- [IBAN](#iban)
- [Chave Pix](#chave-pix)
- [BR Code (Pix Copia e Cola)](#br-code-pix-copia-e-cola)
- [Telefone](#telefone)
- [E-mail](#e-mail)
- [CNH](#cnh)
- [Chassi (VIN)](#chassi-vin)
- [PIS](#pis)
- [RENAVAM](#renavam)
- [CNO](#cno)
- [CEI](#cei)
- [Título de Eleitor](#título-de-eleitor)
- [Processo Jurídico](#processo-jurídico)
- [Chave de Acesso da NF-e](#chave-de-acesso-da-nf-e)
- [Certidão de Registro Civil](#certidão-de-registro-civil)
- [Passaporte](#passaporte)
- [Registro Profissional](#registro-profissional)
- [CNS](#cns)
- [Placa de Veículo](#placa-de-veículo)
- [CBO](#cbo)
- [CNAE](#cnae)
- [NCM](#ncm)
- [CFOP](#cfop)
- [CST](#cst)
- [CSOSN](#csosn)
- [Moeda](#moeda)
- [Números por Extenso](#números-por-extenso)
- [Data](#data)
- [Utilitários](#utilitários)
- [Texto](#texto)

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

#### Remover Formatação

```csharp
Cpf.RemoveSymbols("831.595.621-31");  // "83159562131"
Cpf.Parse("943.895.751-04");          // "94389575104"
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

#### Remover Formatação

```csharp
Cnpj.RemoveSymbols("10.799.163/9892-71");  // "10799163989271"
Cnpj.Parse("46.843.485/0001-86");          // "46843485000186"
```

#### Gerar

```csharp
// Gerar CNPJ numérico
Cnpj.Generate();             // "13401551551768"

// Gerar CNPJ alfanumérico
Cnpj.GenerateAlphanumeric(); // "AB12CD34EF5601"
```

---

### CAEPF

O CAEPF (Cadastro de Atividade Econômica da Pessoa Física) identifica a atividade econômica de uma pessoa física perante a Receita Federal.

#### Validar

```csharp
using BrazilianUtils;

Caepf.IsValid("29311861000184");  // true
Caepf.IsValid("caepf-invalido");  // false
```

#### Formatar

```csharp
Caepf.Format("29311861000184");  // "293.118.610/001-84"
```

---

### Natureza Jurídica

A Natureza Jurídica identifica o tipo de constituição de uma entidade perante a tabela CONCLA (Comissão Nacional de Classificação) do IBGE.

#### Validar

```csharp
using BrazilianUtils;

LegalNature.IsValid("2062");  // true
LegalNature.IsValid("9999");  // false
```

#### Formatar

```csharp
LegalNature.Format("2062");  // "206-2"
```

#### Consultar

```csharp
LegalNature.GetDescription("2062");  // Some "Sociedade Empresária Limitada"
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

#### Remover Formatação

```csharp
Cep.RemoveSymbols("91906-292");  // "91906292"
Cep.Parse("01001-000");          // "01001000"
```

#### Gerar

```csharp
Cep.Generate();  // "06156069"
```

---

### Município

Consulta municípios brasileiros publicados pelo IBGE pelo código de 7 dígitos, ou lista todos os municípios de um estado.

#### Consultar

```csharp
using BrazilianUtils;

Municipality.GetByCode("5200050");  // Some { Code = "5200050"; Name = "Abadia de Goiás"; StateCode = "GO" }
Municipality.List(Some "GO");       // todos os municípios de Goiás, ordenados por nome
```

---

### Estado

As 27 unidades federativas brasileiras, com região, código IBGE (cUF) e fuso horário.

#### Consultar

```csharp
using BrazilianUtils;

State.GetNameByCode("SP");         // Some "São Paulo"
State.GetCodeByName("São Paulo");  // Some "SP"
State.GetTimezone("SP");           // Some "America/Sao_Paulo"
```

---

### DDD

Consulta o estado e a região correspondentes a um DDD (código de área telefônico) brasileiro.

#### Consultar

```csharp
using BrazilianUtils;

AreaCode.GetInfo(11).Value.StateName;  // "São Paulo"
AreaCode.ListByState("SP");            // [11; 12; 13; 14; 15; 16; 17; 18; 19]
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

#### Formatar

```csharp
Boleto.Format("03694928065284362782017695692230200005860034350");
// "03694.92806 52843.627820 17695.692230 2 00005860034350"
```

#### Gerar

```csharp
Boleto.Generate();  // "03694928065284362782017695692230200005860034350"
```

---

### Cartão de Crédito

Valida um número de cartão de pagamento (crédito ou débito) pelo algoritmo de Luhn: 12 a 19 dígitos e dígito verificador correto.

#### Validar

```csharp
using BrazilianUtils;

CreditCard.IsValid("4111 1111 1111 1111");  // true
CreditCard.IsValid("1234567890123456");     // false
CreditCard.IsValid("1111111111111111");     // false (dígitos repetidos)
```

---

### Banco

Consulta os bancos participantes do STR (Sistema de Transferência de Reservas) publicados pelo Banco Central, pelo código COMPE ou pelo ISPB.

#### Consultar

```csharp
using BrazilianUtils;

Bank.GetByCode("341");      // Some { Code = "341"; Ispb = "60701190"; Name = "ITAÚ UNIBANCO S.A." }
Bank.GetByIspb("60701190"); // Some { Code = "341"; Ispb = "60701190"; Name = "ITAÚ UNIBANCO S.A." }
```

---

### Conta Bancária

Valida uma conta bancária brasileira (banco, agência, conta e dígito verificador), reconhecendo os algoritmos específicos publicados para alguns bancos.

#### Validar

```csharp
using BrazilianUtils;

// Fallback genérico (módulo 10 ou módulo 11)
BankAccount.IsValid({ BankCode = "341"; Agency = "1234"; Account = "12345"; Digit = "5" });  // true

// Nubank: dígito verificador pelo algoritmo de Verhoeff
BankAccount.IsValid({ BankCode = "260"; Agency = "0001"; Account = "1234567"; Digit = "9" }); // true
```

---

### IBAN

Valida e formata um IBAN brasileiro: `BR` + dígitos verificadores (ISO 7064 MOD 97-10) + ISPB + agência + conta + tipo de conta + titular.

#### Validar

```csharp
using BrazilianUtils;

Iban.IsValid("BR46 0000 0000 0000 1000 1234 567C 1");  // true
```

#### Formatar

```csharp
Iban.Format("BR4600000000000010001234567C1");
// "BR46 0000 0000 0000 1000 1234 567C 1"
```

#### Consultar

```csharp
Iban.GetInfo("BR4600000000000010001234567C1").Value.BankIspb;  // "00000000"
```

---

### Chave Pix

Identifica e valida uma chave Pix (CPF, CNPJ, e-mail, celular brasileiro ou chave aleatória EVP), conforme os formatos do DICT.

#### Validar

```csharp
using BrazilianUtils;

PixKey.IsValid("11988887777", None);  // true (celular)
PixKey.GetInfo("usuario@exemplo.com");
// Some { Type = "email"; Value = "usuario@exemplo.com" }
```

---

### BR Code (Pix Copia e Cola)

Valida um payload de BR Code do Pix ("copia e cola"): estrutura TLV, CRC-16 e os campos obrigatórios do recebedor.

#### Validar

```csharp
using BrazilianUtils;

var payload = "00020101021126380014br.gov.bcb.pix0116user@example.com5204000053039865802BR5910LOJA TESTE6009SAO PAULO6304C50B";

PixPayload.IsValid(payload);  // true
PixPayload.GetInfo(payload).Value.MerchantName;  // "LOJA TESTE"
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

// Validação específica por tipo
Phone.IsValidMobile("(51) 99922-3344");    // true
Phone.IsValidLandline("(11) 3344-5566");   // true
```

#### Remover Formatação

```csharp
Phone.RemoveSymbols("(11) 3000-0000");  // "1130000000"
Phone.Parse("+55 11 98888-7777");       // "11988887777"
```

#### Formatar

```csharp
Phone.Format("988887777");   // "98888-7777"
Phone.Format("1130000000");  // "11300-0000"
```

#### Gerar

```csharp
Phone.Generate(Some "mobile");    // "77955978581"
Phone.Generate(Some "landline");  // "8837470481"
```

---

### E-mail

Valida um endereço de e-mail (subconjunto prático do padrão "valid e-mail address" do WHATWG HTML).

#### Validar

```csharp
using BrazilianUtils;

Email.IsValid("usuario@exemplo.com");  // true
Email.IsValid("usuario@dominio");      // false (sem domínio de topo)
Email.IsValid("");                     // false
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

#### Formatar

```csharp
Cnh.Format("98765432100");  // "987654321-00"
```

#### Remover Formatação

```csharp
Cnh.Parse("987654321-00");  // "98765432100"
```

#### Gerar

```csharp
Cnh.Generate();  // "04253983910"
```

---

### Chassi (VIN)

Valida estruturalmente um VIN (Vehicle Identification Number / chassi): 17 caracteres, nenhuma das letras excluídas `I`, `O`, `Q`, e o dígito verificador na posição 9 (regra norte-americana).

#### Validar

```csharp
using BrazilianUtils;

Vin.IsValid("1HGCM82633A004352");  // true

Vin.IsValid("IO1CM82633A004352");     // false (letras I/O/Q não são permitidas)
Vin.IsValid("11111111111111111");    // false (caracteres repetidos)
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

#### Remover Formatação

```csharp
Pis.parse("123.45678.90-1");  // "12345678901"
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

#### Gerar

```csharp
Renavam.Generate();  // "61747177505"
```

---

### CNO

O CNO (Cadastro Nacional de Obras) substituiu o CEI para obras de construção civil e mantém a mesma numeração e regra de dígito verificador.

#### Validar

```csharp
using BrazilianUtils;

Cno.IsValid("110840168062");  // true
```

#### Formatar

```csharp
Cno.Format("111130137368");  // "11.113.01373/68"
```

---

### CEI

O CEI (Cadastro Específico do INSS) identifica um contribuinte perante a Previdência Social.

#### Validar

```csharp
using BrazilianUtils;

Cei.IsValid("277297118187");  // true
```

#### Formatar

```csharp
Cei.Format("277297118187");  // "27.729.71181/87"
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

#### Remover Formatação

```csharp
VoterId.Parse("690.8470.92.828");         // "690847092828"
VoterId.RemoveSymbols("6908 4709 28 28"); // "690847092828"
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

#### Remover Formatação

```csharp
LegalProcess.parse("0002080-25.2012.5.15.0049");  // "00020802520125150049"
```

#### Gerar

```csharp
LegalProcess.generate(Some 2024, Some 5);  // Some "12345678720245050000"
LegalProcess.generate(None, None);          // ID de processo jurídico válido aleatório
```

---

### Chave de Acesso da NF-e

Valida e interpreta a chave de acesso de 44 dígitos de um DF-e: NF-e (55), NFC-e (65), CT-e (57), MDF-e (58), CT-e OS (67), GTV-e (64), BP-e (63), NF3e (66) e NFCom (62).

#### Validar

```csharp
using BrazilianUtils;

NfeKey.IsValid("35240912345678000199570010000000011111111114");  // true
```

#### Formatar

```csharp
NfeKey.Format("35240912345678000199570010000000011111111114");
// "3524 0912 3456 7800 0199 5700 1000 0000 0111 1111 1114"
```

#### Consultar

```csharp
NfeKey.GetInfo("35240912345678000199570010000000011111111114").Value.StateCode;  // "SP"
```

---

### Certidão de Registro Civil

Valida a matrícula de 32 dígitos de uma certidão de registro civil (art. 473 do Código Nacional de Normas da Corregedoria Nacional de Justiça).

#### Validar

```csharp
using BrazilianUtils;

Certidao.IsValid("104539 01 55 2013 1 00012 021 0000123 21", None);  // true
```

#### Formatar

```csharp
Certidao.Format("10453901552013100012021000012321");
// "104539 01 55 2013 1 00012 021 0000123 21"
```

---

### Passaporte

Valida um número de passaporte brasileiro: 2 letras seguidas de 6 dígitos. Não há dígito verificador, então um número bem formado não é necessariamente real.

#### Validar

```csharp
using BrazilianUtils;

Passport.IsValid("FZ123456");  // true
Passport.IsValid("12345678");  // false (a forma decimal nunca é válida)
```

#### Gerar

```csharp
Passport.Generate();  // "IH753095"
```

---

### Registro Profissional

Verifica apenas a estrutura de um registro/inscrição profissional (OAB, CRM, CRO, CRP ou CRC) — quantidade de dígitos e UF —, nunca um dígito verificador.

#### Validar

```csharp
using BrazilianUtils;

RegistroProfissional.IsValid({ Value = "123456/SP"; Council = "OAB"; State = None });     // true
RegistroProfissional.IsValid({ Value = "SP-123456/O-3"; Council = "CRC"; State = None }); // true
```

---

### CNS

O CNS (Cartão Nacional de Saúde) identifica um usuário do SUS.

#### Validar

```csharp
using BrazilianUtils;

Cns.IsValid("123456789010000");  // true
```

#### Formatar

```csharp
Cns.Format("123456789010001");  // "123 4567 8901 0001"
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

### CBO

Consulta a tabela oficial CBO 2002 (Classificação Brasileira de Ocupações) por código de 6 dígitos.

#### Consultar

```csharp
using BrazilianUtils;

Cbo.IsValid("010105");  // true
Cbo.Get("010105");      // Some { Code = "010105"; Description = "Oficial general da aeronáutica" }
```

---

### CNAE

Consulta a tabela oficial CNAE-Subclasses 2.3 (Classificação Nacional de Atividades Econômicas) por código de 7 dígitos.

#### Consultar

```csharp
using BrazilianUtils;

Cnae.IsValid("0111301");  // true
Cnae.Get("0111301");      // Some { Code = "0111301"; Description = "CULTIVO DE ARROZ" }
```

#### Formatar

```csharp
Cnae.Format("0111301");  // "0111-3/01"
```

---

### NCM

Valida um código NCM (Nomenclatura Comum do Mercosul) de 8 dígitos na tabela vigente publicada pelo Siscomex.

#### Validar

```csharp
using BrazilianUtils;

Ncm.IsValid("01012100");  // true
```

#### Formatar

```csharp
Ncm.Format("01012100");  // "0101.21.00"
```

---

### CFOP

Consulta a tabela oficial de CFOP (Código Fiscal de Operações e Prestações), excluindo cabeçalhos de grupo/subgrupo.

#### Consultar

```csharp
using BrazilianUtils;

Cfop.IsValid("1101");  // true
Cfop.Get("1101");      // Some { Code = "1101"; Description = "Compra para industrialização ou produção rural" }
```

---

### CST

Valida um código CST (Código de Situação Tributária) de ICMS, IPI, PIS ou COFINS.

#### Validar

```csharp
using BrazilianUtils;

Cst.IsValid("060", Some "icms");  // true
Cst.IsValid("01", Some "pis");    // true
Cst.IsValid("5", None);           // false (um único dígito só serve para o ICMS, com origem "0")
```

---

### CSOSN

Valida um código CSOSN (Código de Situação da Operação no Simples Nacional) na tabela oficial (Ajuste SINIEF 07/2005).

#### Validar

```csharp
using BrazilianUtils;

Csosn.IsValid("101");  // true
Csosn.IsValid("999");  // false
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

### Números por Extenso

Converte um número inteiro (a parte fracionária é truncada) para sua representação por extenso em português brasileiro.

#### Converter para Texto

```csharp
using BrazilianUtils;

Number.ConvertToWords(1235.0);  // "mil duzentos e trinta e cinco"
Number.ConvertToWords(-3.0);    // "menos três"
```

---

### Data

Feriados nacionais brasileiros e cálculo de dias úteis (soma, subtração e diferença entre datas).

#### Verificar Feriados e Dias Úteis

```csharp
using BrazilianUtils;

Date.IsBusinessDay(DateTime(2026, 9, 29), None);  // true
Date.IsHoliday(Some { Date = DateTime(2026, 1, 1); State = None });  // true
```

#### Somar e Subtrair Dias Úteis

```csharp
Date.AddBusinessDays(DateTime(2026, 9, 29), 5, None);
// Some 2026-10-06

Date.DifferenceInBusinessDays(DateTime(2026, 10, 10), DateTime(2026, 9, 29), None);
// Some 9
```

#### Converter para Texto

```csharp
Date.ConvertToWords("01/01/2024");  // "primeiro de janeiro de dois mil e vinte e quatro"
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

### Texto

Utilitários gerais para capitalização e remoção de acentos de textos brasileiros (nomes, razão social, endereços).

#### Capitalizar

```csharp
using BrazilianUtils;

Text.Capitalize("jose da silva");  // "Jose da Silva"
Text.Capitalize("empresa ltda");   // "Empresa LTDA"
```

#### Remover Acentos

```csharp
Text.RemoveAccents("São Paulo");  // "Sao Paulo"
Text.RemoveAccents("Açaí");       // "Acai"
```

---

## Contribuindo

Contribuições são bem-vindas! Sinta-se à vontade para enviar um Pull Request. Para mudanças significativas, por favor abra uma issue primeiro para discutir o que você gostaria de alterar.

## Licença

Este projeto é open source e está disponível sob a [Licença MIT](LICENSE).
