module BrazilianUtils.Tests.CpfTests

open Xunit
open BrazilianUtils
open Xunit

[<Theory>]
[<InlineData"01234567890">]
[<InlineData"73615755790">]
[<InlineData"96818343481">]
[<InlineData"012.345.678-90">]
[<InlineData"308.411.120-02">]
[<InlineData"656.562.663-46">]
let shouldBeValid cpf =
    cpf
    |> Cpf.IsValid
    |> Assert.True

[<Theory>]
[<InlineData"">]
[<InlineData null>]
[<InlineData"123456">]
[<InlineData"11257245286">]
[<InlineData"abcabcabcde">]
[<InlineData"11111111111">]
let shouldBeInvalid cpf =
    cpf
    |> Cpf.IsValid
    |> Assert.False

[<Theory>]
[<InlineData"02115402006">]
[<InlineData"021.154.020-06">]
let shouldFormattedStringMatchRegex cpf = Assert.Matches(@"^(\d{3}\.){2}\d{3}-\d{2}$", Cpf.Format cpf)

[<Fact>]
let generatedCpfShouldBeValid() =
    [ 1..100 ]
    |> Seq.forall (fun _ -> Cpf.Generate() |> Cpf.IsValid)
    |> Assert.True
