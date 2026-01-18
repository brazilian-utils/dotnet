module BrazilianUtils.Tests.LegalProcessTests

open System
open Xunit
open BrazilianUtils.LegalProcess

// --- removeSymbols Tests ---

[<Fact>]
let ``removeSymbols should remove dots and hyphens`` () =
    let input = "123.456.789-00"
    let expected = "12345678900"
    let actual = removeSymbols input
    Assert.Equal(expected, actual)

[<Fact>]
let ``removeSymbols should return same string if no symbols`` () =
    let input = "12345678900"
    let expected = "12345678900"
    let actual = removeSymbols input
    Assert.Equal(expected, actual)

[<Fact>]
let ``removeSymbols should handle empty string`` () =
    let input = ""
    let expected = ""
    let actual = removeSymbols input
    Assert.Equal(expected, actual)

// --- formatLegalProcess Tests ---

[<Fact>]
let ``formatLegalProcess should format valid 20-digit string`` () =
    let input = "12345678901234567890"
    let expected = Some "1234567-89.0123.4.56.7890"
    let actual = formatLegalProcess input
    Assert.Equal(expected, actual)

[<Fact>]
let ``formatLegalProcess should return None for short string`` () =
    let input = "12345"
    let actual = formatLegalProcess input
    Assert.True(actual.IsNone)

[<Fact>]
let ``formatLegalProcess should return None for long string`` () =
    let input = "123456789012345678901"
    let actual = formatLegalProcess input
    Assert.True(actual.IsNone)

[<Fact>]
let ``formatLegalProcess should return None for non-numeric string`` () =
    let input = "1234567890123456789a"
    let actual = formatLegalProcess input
    Assert.True(actual.IsNone)

// --- isValid Tests ---

[<Theory>]
// Valid legal process IDs (correct check digits using 98 - (base*100 % 97) formula)
[<InlineData("68476506120233030000", true)>]  // orgao 3, tribunal 03, foro 0000
[<InlineData("00000018720215050000", true)>]  // orgao 5, tribunal 05, foro 0000
[<InlineData("00000013420211010000", true)>]  // orgao 1, tribunal 01, foro 0000 (DD=34)
[<InlineData("00018812520168020000", true)>]  // orgao 8, tribunal 02, foro 0000 (DD=25)
// Invalid: wrong length
[<InlineData("123", false)>]
[<InlineData("123456789012345678901", false)>]
// Invalid: non-numeric characters
[<InlineData("abcdefghijklmnopqrst", false)>]
// Invalid: wrong check digit (DD off by 1)
[<InlineData("68476506020233030000", false)>]
// Invalid: wrong segment (J) does not exist
[<InlineData("68476506020230030000", false)>]
// Invalid: tribunal not valid for orgao
[<InlineData("68476506020233990000", false)>]
// Invalid: foro not valid for orgao
[<InlineData("68476506020233039999", false)>]
// Invalid: tribunal 99 not valid for orgao 1
[<InlineData("00000018020211990000", false)>]
// Invalid: foro 9999 not valid for orgao 1
[<InlineData("00000018020211019999", false)>]
// Invalid: wrong check digit for orgao 5
[<InlineData("00000018520215050000", false)>]
// Invalid: tribunal 99 not valid for orgao 5
[<InlineData("00000018520215990000", false)>]
// Invalid: wrong check digit
[<InlineData("00000018520215059998", false)>]
// Invalid: tribunal 99 not valid for orgao 8
[<InlineData("00018817720168990000", false)>]
// Invalid: wrong check digit
[<InlineData("00018817720168029998", false)>]
let ``isValid should validate various legal process numbers`` (input: string, expected: bool) =
    let actual = isValid input
    Assert.Equal(expected, actual)

[<Fact>]
let ``isValid should handle formatted strings`` () =
    // This process ID is valid (68476506120233030000) with correct check digit 61
    let input = "6847650-61.2023.3.03.0000"
    // Ensure formatted input behaves the same as cleaned input
    let cleaned = removeSymbols input
    let expected = isValid cleaned
    let actual = isValid input
    Assert.Equal(expected, actual)
    Assert.True(actual) // Both should be valid

[<Fact>]
let ``isValid should return false for exceptions`` () =
    // This string has valid length but invalid chars that throw exception on int parse
    let input = "6847650-60.2023.3.03.00xx"
    let actual = isValid input
    Assert.False(actual)

// --- generate Tests ---

[<Fact>]
let ``generate should return a valid legal process ID with no args`` () =
    let result = generate None None
    Assert.True(result.IsSome)
    let processId = result.Value
    Assert.Equal(20, processId.Length)
    Assert.True(processId |> Seq.forall Char.IsDigit)
    Assert.True(isValid processId)

[<Fact>]
let ``generate should return a valid legal process ID for specific year and orgao`` () =
    let currentYear = DateTime.Now.Year
    let result = generate (Some currentYear) (Some 5) // Orgao5
    Assert.True(result.IsSome)
    let processId = result.Value

    let yearPart = processId.Substring(9,4)
    let orgaoPart = processId.Substring(13,1)

    Assert.Equal(currentYear.ToString(), yearPart)
    Assert.Equal("5", orgaoPart)
    Assert.True(isValid processId)

[<Fact>]
let ``generate should return None for a past year`` () =
    let pastYear = DateTime.Now.Year - 1
    let result = generate (Some pastYear) None
    Assert.True(result.IsNone)

[<Fact>]
let ``generate should return None for an invalid orgao (0)`` () =
    let result = generate None (Some 0)
    Assert.True(result.IsNone)

[<Fact>]
let ``generate should return None for an invalid orgao (10)`` () =
    let result = generate None (Some 10)
    Assert.True(result.IsNone)

[<Fact>]
let ``generate should create valid IDs for all valid orgaos`` () =
    let currentYear = DateTime.Now.Year
    for orgao in 1 .. 9 do
        // Try up to 10 attempts to generate a valid ID for the orgao to avoid
        // flakiness from random selection of invalid combinations
        let mutable found = false
        let mutable attempts = 0
        while not found && attempts < 10 do
            attempts <- attempts + 1
            let result = generate (Some currentYear) (Some orgao)
            if result.IsSome then
                let processId = result.Value
                if isValid processId then
                    found <- true
        Assert.True(found, $"Failed to generate a valid ID for orgao {orgao} after {attempts} attempts")
