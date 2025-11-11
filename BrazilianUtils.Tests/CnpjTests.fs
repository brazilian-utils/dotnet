module BrazilianUtils.Tests.CnpjTests

open BrazilianUtils
open Xunit

[<Theory>]
[<InlineData"81.202.136/0001-86">]
[<InlineData"46.238.497/0001-81">]
[<InlineData"18240603000126">]
[<InlineData"77973689000165">]
let shouldBeValid cnpj =
    cnpj
    |> Cnpj.IsValid
    |> Assert.True

[<Theory>]
[<InlineData"">]
[<InlineData null>]
[<InlineData"123456">]
[<InlineData"11257245286">]
[<InlineData"abcabcabcde">]
[<InlineData"11111111111">]
[<InlineData"77973689000163">]
[<InlineData"77173389000163">]
let shouldBeInvalid cnpj =
    cnpj
    |> Cnpj.IsValid
    |> Assert.False

[<Theory>]
[<InlineData"22938962000129">]
[<InlineData"42.999.072/0001-34">]
let shouldFormattedStringMatchRegex cnpj =
    Assert.Matches(@"^[0-9]{2}(\.[0-9]{3}){2}\/[0-9]{4}\-[0-9]{2}$", Cnpj.Format cnpj)

[<Fact>]
let generatedCnpjShouldBeValid() =
    [ 1..100 ]
    |> Seq.forall (fun _ -> Cnpj.Generate() |> Cnpj.IsValid)
    |> Assert.True
