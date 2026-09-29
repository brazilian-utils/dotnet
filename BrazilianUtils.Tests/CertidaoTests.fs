module BrazilianUtils.Tests.CertidaoTests

open BrazilianUtils
open Xunit

[<Theory>]
[<InlineData("10453901552013100012021000012321")>]
[<InlineData("104539 01 55 2013 1 00012 021 0000123 21")>]
[<InlineData("104539/01/55/2013/1/00012/021/0000123/21")>]
let shouldBeValid (value: string) =
    Certidao.IsValid (box value) None
    |> Assert.True

[<Theory>]
// Regressão: letra solta no meio do valor era descartada silenciosamente
// pela limpeza permissiva, validando o que deveria ser rejeitado.
[<InlineData("1045390A1552013100012021000012321")>]
[<InlineData("104539*01*55*2013*1*00012*021*0000123*21")>]
[<InlineData("XYZ10453901552013100012021000012321")>]
[<InlineData("104539 Q 01552013100012021000012321")>]
let shouldBeInvalid (value: string) =
    Certidao.IsValid (box value) None
    |> Assert.False
