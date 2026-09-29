module BrazilianUtils.Tests.IeTests

open BrazilianUtils
open Xunit

[<Theory>]
[<InlineData("110042490114", "SP")>]
[<InlineData("11004249011.4", "SP")>]
[<InlineData("11004249", "RJ")>]
[<InlineData("P110042490123", "SP")>]
[<InlineData("12345678901", "MT")>]
let shouldBeValid (value: string) (state: string) =
    Ie.IsValid value state
    |> Assert.True

[<Theory>]
[<InlineData("1100424901", "SP")>] // SP espera 12 dígitos, não 10
[<InlineData("110042490", "RJ")>] // RJ espera 8 dígitos, não 9
[<InlineData("110042490114", "XX")>] // UF desconhecida
[<InlineData("", "SP")>]
[<InlineData("   ", "SP")>]
let shouldBeInvalid (value: string) (state: string) =
    Ie.IsValid value state
    |> Assert.False
