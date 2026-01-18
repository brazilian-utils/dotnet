module BrazilianUtils.Tests.CnpjTests

open BrazilianUtils
open Xunit

[<Theory>]
[<InlineData"81.202.136/0001-86">]
[<InlineData"46.238.497/0001-81">]
[<InlineData"18240603000126">]
[<InlineData"77973689000165">]
// Alphanumeric CNPJs (Instrução Normativa RFB nº 2.119)
[<InlineData"12.ABC.345/01DE-35">]
[<InlineData"12ABC34501DE35">]
let shouldBeValid cnpj =
    cnpj
    |> Cnpj.IsValid
    |> Assert.True

[<Theory>]
[<InlineData"">]
[<InlineData null>]
[<InlineData"123456">]
[<InlineData"11257245286">]
[<InlineData"11111111111">]
[<InlineData"77973689000163">]
[<InlineData"77173389000163">]
// Invalid alphanumeric CNPJs
[<InlineData"12.ABC.345/01DE-99">]
[<InlineData"AAAAAAAAAAAA00">]
let shouldBeInvalid cnpj =
    cnpj
    |> Cnpj.IsValid
    |> Assert.False

[<Theory>]
[<InlineData"22938962000129">]
[<InlineData"42.999.072/0001-34">]
let shouldFormattedNumericStringMatchRegex cnpj =
    Assert.Matches(@"^[0-9]{2}(\.[0-9]{3}){2}\/[0-9]{4}\-[0-9]{2}$", Cnpj.Format cnpj)

[<Theory>]
[<InlineData"12ABC34501DE35">]
[<InlineData"12.ABC.345/01DE-35">]
let shouldFormattedAlphanumericStringMatchRegex cnpj =
    Assert.Matches(@"^[0-9A-Z]{2}(\.[0-9A-Z]{3}){2}\/[0-9A-Z]{4}\-[0-9]{2}$", Cnpj.Format cnpj)

[<Fact>]
let generatedCnpjShouldBeValid() =
    [ 1..100 ]
    |> Seq.forall (fun _ -> Cnpj.Generate() |> Cnpj.IsValid)
    |> Assert.True

[<Fact>]
let generatedAlphanumericCnpjShouldBeValid() =
    [ 1..100 ]
    |> Seq.forall (fun _ -> Cnpj.GenerateAlphanumeric() |> Cnpj.IsValid)
    |> Assert.True
