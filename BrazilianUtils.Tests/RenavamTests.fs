module BrazilianUtils.Tests.RenavamTests

open Xunit
open BrazilianUtils.Renavam

[<Fact>]
let ``test valid renavam`` () =
    Assert.True(isValidRenavam "86769597308")

[<Fact>]
let ``test invalid renavam with wrong check digit`` () =
    Assert.False(isValidRenavam "12345678901")

[<Fact>]
let ``test invalid renavam with letter`` () =
    Assert.False(isValidRenavam "1234567890a")

[<Fact>]
let ``test invalid renavam with space`` () =
    Assert.False(isValidRenavam "12345678 901")

[<Fact>]
let ``test invalid renavam too short`` () =
    Assert.False(isValidRenavam "12345678")

[<Fact>]
let ``test invalid renavam empty string`` () =
    Assert.False(isValidRenavam "")

[<Fact>]
let ``test invalid renavam too long`` () =
    Assert.False(isValidRenavam "123456789012")

[<Fact>]
let ``test invalid renavam all letters`` () =
    Assert.False(isValidRenavam "abcdefghijk")

[<Fact>]
let ``test invalid renavam with special character`` () =
    Assert.False(isValidRenavam "12345678901!")

[<Fact>]
let ``test invalid renavam all zeros`` () =
    Assert.False(isValidRenavam "00000000000")

[<Fact>]
let ``test invalid renavam all ones`` () =
    Assert.False(isValidRenavam "11111111111")
