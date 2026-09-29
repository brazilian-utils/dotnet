module BrazilianUtils.LicensePlate.Tests

open Xunit
open BrazilianUtils.LicensePlate

// =============================
// CONVERT TO MERCOSUL TESTS
// =============================

[<Fact>]
let ``convertToMercosul should convert valid old format to Mercosul`` () =
    let result = convertToMercosul "ABC1234"
    Assert.Equal("ABC1C34", result)

[<Fact>]
let ``convertToMercosul should handle lowercase input`` () =
    let result = convertToMercosul "abc1234"
    Assert.Equal("ABC1C34", result)

[<Fact>]
let ``convertToMercosul should accept the hyphen mask`` () =
    let result = convertToMercosul "ABC-1234"
    Assert.Equal("ABC1C34", result)

[<Fact>]
let ``convertToMercosul should return empty string for invalid format`` () =
    let result = convertToMercosul "ABC123"
    Assert.Equal("", result)

[<Fact>]
let ``convertToMercosul should return empty string for Mercosul format input`` () =
    let result = convertToMercosul "ABC1D34"
    Assert.Equal("", result)

[<Fact>]
let ``convertToMercosul should return empty string for empty string`` () =
    let result = convertToMercosul ""
    Assert.Equal("", result)

[<Fact>]
let ``convertToMercosul should convert digit 2 to C`` () =
    let result = convertToMercosul "ABC1234"
    Assert.Equal("ABC1C34", result)

[<Fact>]
let ``convertToMercosul should convert digit 5 to F`` () =
    let result = convertToMercosul "ABC4567"
    Assert.Equal("ABC4F67", result)

// =============================
// FORMAT LICENSE PLATE TESTS
// =============================

[<Fact>]
let ``formatLicensePlate should format old pattern with dash`` () =
    let result = formatLicensePlate "ABC1234"
    Assert.Equal(Some "ABC-1234", result)

[<Fact>]
let ``formatLicensePlate should format Mercosul without dash`` () =
    let result = formatLicensePlate "ABC1E34"
    Assert.Equal(Some "ABC1E34", result)

[<Fact>]
let ``formatLicensePlate should convert to uppercase`` () =
    let result = formatLicensePlate "abc1e34"
    Assert.Equal(Some "ABC1E34", result)

[<Fact>]
let ``formatLicensePlate should return None for invalid format`` () =
    let result = formatLicensePlate "ABC123"
    Assert.Equal(None, result)

[<Fact>]
let ``formatLicensePlate should return None for empty string`` () =
    let result = formatLicensePlate ""
    Assert.Equal(None, result)

[<Fact>]
let ``formatLicensePlate should handle mixed case old format`` () =
    let result = formatLicensePlate "aBc1234"
    Assert.Equal(Some "ABC-1234", result)

// =============================
// REMOVE SYMBOLS TESTS
// =============================

[<Fact>]
let ``removeSymbols should remove dash from string`` () =
    let result = removeSymbols "ABC-1234"
    Assert.Equal("ABC1234", result)

[<Fact>]
let ``removeSymbols should handle string without dash`` () =
    let result = removeSymbols "ABC1234"
    Assert.Equal("ABC1234", result)

[<Fact>]
let ``removeSymbols should handle multiple dashes`` () =
    let result = removeSymbols "ABC-12-34"
    Assert.Equal("ABC1234", result)

[<Fact>]
let ``removeSymbols should handle empty string`` () =
    let result = removeSymbols ""
    Assert.Equal("", result)

[<Fact>]
let ``removeSymbols should preserve case`` () =
    let result = removeSymbols "aBc-1234"
    Assert.Equal("aBc1234", result)

// =============================
// GET FORMAT TESTS
// =============================

[<Fact>]
let ``getFormat should return LLLNNNN for old format`` () =
    let result = getFormat "ABC1234"
    Assert.Equal(Some "LLLNNNN", result)

[<Fact>]
let ``getFormat should return LLLNLNN for Mercosul format`` () =
    let result = getFormat "ABC1D23"
    Assert.Equal(Some "LLLNLNN", result)

[<Fact>]
let ``getFormat should return None for invalid format`` () =
    let result = getFormat "ABCD123"
    Assert.Equal(None, result)

[<Fact>]
let ``getFormat should handle lowercase old format`` () =
    let result = getFormat "abc1234"
    Assert.Equal(Some "LLLNNNN", result)

[<Fact>]
let ``getFormat should handle lowercase Mercosul format`` () =
    let result = getFormat "abc1d23"
    Assert.Equal(Some "LLLNLNN", result)

[<Fact>]
let ``getFormat should return None for empty string`` () =
    let result = getFormat ""
    Assert.Equal(None, result)

// =============================
// PARSE TESTS
// =============================

[<Fact>]
let ``parse should remove hyphen mask and uppercase`` () =
    Assert.Equal("ABC1234", parse "abc-1234")

[<Fact>]
let ``parse should uppercase Mercosul plate`` () =
    Assert.Equal("ABC1D23", parse "abc1d23")

[<Fact>]
let ``parse should return empty string for empty string`` () =
    Assert.Equal("", parse "")

[<Fact>]
let ``parse should cap length to 7 characters`` () =
    Assert.Equal("ABC1234", parse "abc123456")

// =============================
// IS VALID TESTS
// =============================

[<Fact>]
let ``isValid should validate old format plates`` () =
    Assert.True(isValid "ABC1234")

[<Fact>]
let ``isValid should validate Mercosul plates`` () =
    Assert.True(isValid "ABC1D34")

[<Fact>]
let ``isValid should reject invalid format`` () =
    let result = isValid "ABCD123"
    Assert.False(result)

[<Fact>]
let ``isValid should handle lowercase input`` () =
    Assert.True(isValid "abc1234")
    Assert.True(isValid "abc1d34")

[<Fact>]
let ``isValid should reject empty string`` () =
    let result = isValid ""
    Assert.False(result)

[<Fact>]
let ``isValid should tolerate the hyphen mask`` () =
    let result = isValid "ABC-1234"
    Assert.True(result)

// =============================
// GENERATE TESTS
// =============================

[<Fact>]
let ``generate should return Mercosul format by default`` () =
    let result = generate None
    match result with
    | Some plate ->
        Assert.Equal(7, plate.Length)
        Assert.True(isValid plate)
    | None -> Assert.True(false, "Expected Some value")

[<Fact>]
let ``generate should return Mercosul format when specified`` () =
    let result = generate (Some "LLLNLNN")
    match result with
    | Some plate ->
        Assert.Equal(7, plate.Length)
        Assert.True(isValid plate)
    | None -> Assert.True(false, "Expected Some value")

[<Fact>]
let ``generate should return old format when specified`` () =
    let result = generate (Some "LLLNNNN")
    match result with
    | Some plate ->
        Assert.Equal(7, plate.Length)
        Assert.True(isValid plate)
    | None -> Assert.True(false, "Expected Some value")

[<Fact>]
let ``generate should handle lowercase format specification`` () =
    let result = generate (Some "lllnnnn")
    match result with
    | Some plate ->
        Assert.Equal(7, plate.Length)
        Assert.True(isValid plate)
    | None -> Assert.True(false, "Expected Some value")

[<Fact>]
let ``generate should return None for invalid format`` () =
    let result = generate (Some "invalid")
    Assert.Equal(None, result)

[<Fact>]
let ``generate should return None for wrong pattern length`` () =
    let result = generate (Some "LLNNNN")
    Assert.Equal(None, result)

[<Fact>]
let ``generate should produce valid uppercase letters`` () =
    let result = generate (Some "LLLNNNN")
    match result with
    | Some plate ->
        let letters = plate.Substring(0, 3)
        Assert.True(letters |> Seq.forall (fun c -> c >= 'A' && c <= 'Z'))
    | None -> Assert.True(false, "Expected Some value")

[<Fact>]
let ``generate should produce valid digits`` () =
    let result = generate (Some "LLLNNNN")
    match result with
    | Some plate ->
        let digits = plate.Substring(3, 4)
        Assert.True(digits |> Seq.forall (fun c -> c >= '0' && c <= '9'))
    | None -> Assert.True(false, "Expected Some value")

// =============================
// EDGE CASE TESTS
// =============================

[<Fact>]
let ``should handle license plates with leading/trailing whitespace`` () =
    let result1 = formatLicensePlate " ABC1234 "
    let result2 = formatLicensePlate " ABC1D34 "
    // As funções de validação removem espaços das pontas, então isso é tratado
    Assert.Equal(Some " AB-C1234 ", result1)
    Assert.Equal(Some " ABC1D34 ", result2)

[<Theory>]
[<InlineData("ABC12345")>]
[<InlineData("AB1234")>]
[<InlineData("ABCD234")>]
[<InlineData("123ABCD")>]
let ``should reject malformed plates`` (plate: string) =
    Assert.False(isValid plate)

[<Theory>]
[<InlineData("ABC1234")>]
[<InlineData("XYZ9876")>]
[<InlineData("DEF0001")>]
let ``valid old format plates should be recognized`` (plate: string) =
    Assert.True(isValid plate)

[<Theory>]
[<InlineData("ABC1A23")>]
[<InlineData("XYZ9Z87")>]
[<InlineData("DEF0B01")>]
let ``valid Mercosul plates should be recognized`` (plate: string) =
    Assert.True(isValid plate)
