module BrazilianUtils.Tests.PhoneTests

open BrazilianUtils
open Xunit

[<Theory>]
[<InlineData("(11) 9 0000-0000")>]
[<InlineData("(11) 3000-0000")>]
[<InlineData("11900000000")>]
[<InlineData("1130000000")>]
let shouldBeValid phone =
    phone
    |> Phone.IsValid
    |> Assert.True

[<Theory>]
[<InlineData("(00) 3 0000-0000")>]
[<InlineData("(11) 9000-0000")>]
[<InlineData("(20) 9000-0000")>]
[<InlineData("(11) 3 0000-0000")>]
[<InlineData("(11) 9000-0000")>]
[<InlineData("11")>]
[<InlineData("11300000001130000000")>]
let shouldBeInvalid phone =
    phone
    |> Phone.IsValid
    |> Assert.False
