module BrazilianUtils.Tests.HelpersTests

open BrazilianUtils
open Xunit

[<Theory>]
[<InlineData("abc89320100^7")>]
[<InlineData("021.154.020-06")>]
let shouldFormattedStringMatchRegex value =
    Assert.Matches(@"^[0-9]*$", Helpers.OnlyNumbers(value));
