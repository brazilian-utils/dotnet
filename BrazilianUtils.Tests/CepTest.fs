module BrazilianUtils.Tests.CepTest

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
