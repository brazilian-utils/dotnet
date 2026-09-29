module BrazilianUtils.Tests.CstTests

open BrazilianUtils
open Xunit

[<Fact>]
let singleDigitZeroShouldBeValid () =
    Cst.IsValid (box "0") None
    |> Assert.True

[<Theory>]
// Regressão: um único dígito era interpretado como a origem do ICMS (com
// Tabela B "00"), quando deveria ser interpretado como o último dígito da
// forma de 3 dígitos, com origem fixa em 0.
[<InlineData("1")>]
[<InlineData("5")>]
[<InlineData("9")>]
let singleDigitOtherThanZeroShouldBeInvalid (value: string) =
    Cst.IsValid (box value) None
    |> Assert.False
