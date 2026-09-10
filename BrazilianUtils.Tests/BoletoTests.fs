module BrazilianUtils.Tests.BoletoTests

open BrazilianUtils
open Xunit

[<Theory>]
[<InlineData("0019000009 01149.718601 68524.522114 6 75860000102656")>]
[<InlineData("42297 03006 00002 695286 85088 140422 1 75840000044415")>]
[<InlineData("34191000000000000001753980229122525005423000")>]
[<InlineData("00198.10001 00030.212237 00217.236553 1 35742800321323")>]
// Teste de regressão: cálculo do dígito verificador parcial resultava em 10 em vez de 0
// quando o resto da divisão por 10 era 0 (ver issue #11 do GitHub).
[<InlineData("97371333807262473178110801326777363116566701065")>]
let shouldBeValid boleto =
    boleto
    |> Boleto.IsValid
    |> Assert.True

[<Theory>]
[<InlineData("")>]
[<InlineData(null)>]
[<InlineData("0019000009 01149.718601 68524.522112 6 75860000102656")>]
[<InlineData("42297 03006 00002 695286 85088 140422 9 75840000044415")>]
[<InlineData("4191000000000000001753980229122525005423000")>]
[<InlineData("00198.10005 00030.212237 00217.236553 1 35742800321323")>]
let shouldBeInvalid boleto =
    boleto
    |> Boleto.IsValid
    |> Assert.False

