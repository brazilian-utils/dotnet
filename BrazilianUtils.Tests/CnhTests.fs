module BrazilianUtils.Tests.CnhTests

open Xunit
open BrazilianUtils.Cnh

[<Fact>]
let ``isValidCnh should return true for known valid CNH`` () =
 Assert.True(isValidCnh "12345678900")

[<Fact>]
let ``isValidCnh should return true for formatted CNH with hyphen`` () =
 Assert.True(isValidCnh "123456789-00")

[<Fact>]
let ``isValidCnh should return false for invalid checksum`` () =
 Assert.False(isValidCnh "12345678901")

[<Fact>]
let ``isValidCnh should return false for invalid length`` () =
 Assert.False(isValidCnh "1234567890")

[<Fact>]
let ``isValidCnh should return false for repeated sequence`` () =
 Assert.False(isValidCnh "11111111111")

[<Fact>]
let ``isValidCnh should return false when non-digit characters reduce digits count`` () =
 Assert.False(isValidCnh "A2C45678901")

[<Fact>]
let ``isValidCnh should accept strings with extra non-digits if digits form valid CNH`` () =
 // This input contains non-digit characters but the digit sequence becomes a valid CNH after cleaning
 Assert.True(isValidCnh "12a345678900")
