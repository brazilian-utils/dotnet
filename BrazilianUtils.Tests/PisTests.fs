module BrazilianUtils.Tests.PisTests

open Xunit
open BrazilianUtils.Pis

[<Fact>]
let ``test is valid with non-string types returns false`` () =
    // In F#, we can't pass non-string types to a function that expects string
    // So we test with invalid string inputs instead
    Assert.False(isValid null)
    Assert.False(isValid "")

[<Fact>]
let ``test is valid with wrong length returns false`` () =
    Assert.False(isValid "123456789")

[<Fact>]
let ``test is valid with non-digit characters returns false`` () =
    Assert.False(isValid "123pis")
    Assert.False(isValid "123456789ab")

[<Fact>]
let ``test is valid with invalid checksum returns false`` () =
    Assert.False(isValid "11111111111")
    Assert.False(isValid "11111111215")
    Assert.False(isValid "12038619493")

[<Fact>]
let ``test is valid with valid PIS returns true`` () =
    Assert.True(isValid "12038619494")
    Assert.True(isValid "12016784018")
    Assert.True(isValid "12083210826")

[<Fact>]
let ``test generate produces valid PIS`` () =
    // Test 100 generated PIS numbers
    for _ in 1..100 do
        let pis = generate()
        Assert.True(isValid pis)

[<Fact>]
let ``test remove symbols removes dots dashes and slashes`` () =
    Assert.Equal("00000000000", removeSymbols "00000000000")
    Assert.Equal("17033259504", removeSymbols "170.33259.50-4")
    Assert.Equal("1342435/1892", removeSymbols "134..2435/.-1892.-")
    Assert.Equal("abc1230916*!*&#", removeSymbols "abc1230916*!*&#")
    Assert.Equal("", removeSymbols "...---...")

[<Fact>]
let ``test format valid PIS`` () =
    Assert.Equal(Some "143.72195.53-9", formatPis "14372195539")
    Assert.Equal(Some "120.38619.49-4", formatPis "12038619494")
    Assert.Equal(Some "120.16784.01-8", formatPis "12016784018")

[<Fact>]
let ``test format invalid PIS returns None`` () =
    Assert.Equal(None, formatPis "14372195530")
    Assert.Equal(None, formatPis "11111111111")
    Assert.Equal(None, formatPis "123456789")
    Assert.Equal(None, formatPis "123pis")
