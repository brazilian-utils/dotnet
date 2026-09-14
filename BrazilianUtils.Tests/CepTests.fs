module BrazilianUtils.Tests.CepTests

open BrazilianUtils
open Xunit

[<Theory>]
[<InlineData("92990000")>]
[<InlineData("92990-000")>]
let shouldBeValid cep =
    cep
    |> Cep.IsValid
    |> Assert.True

[<Theory>]
[<InlineData("")>]
[<InlineData(null)>]
[<InlineData("9299000")>]
[<InlineData("929900000")>]
[<InlineData("92990-32")>]
let shouldBeInvalid cep =
    cep
    |> Cep.IsValid
    |> Assert.False

[<Theory>]
[<InlineData"92990000">]
[<InlineData"92990-000">]
let shouldFormattedStringMatchRegex cep =
    Assert.Matches(@"^\d{5}-\d{3}$", Cep.Format cep)

[<Theory>]
[<InlineData"">]
[<InlineData"1">]
[<InlineData"1234">]
let formatShouldNotThrowOnShortInput cep =
    // Regressão: Format lançava ArgumentOutOfRangeException para menos de 5 caracteres.
    Cep.Format cep |> ignore
