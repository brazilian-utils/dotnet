module BrazilianUtils.Tests.VoterIdTests

open Xunit
open BrazilianUtils.VoterId

[<Fact>]
let ``test valid voter id`` () =
    // test a valid voter id number
    let voterId = "217633460930"
    Assert.True(isValid voterId)
    Assert.IsType<bool>(isValid voterId) |> ignore

[<Fact>]
let ``test invalid voter id`` () =
    // test an invalid voter id number (dv1 & UF fail)
    let voterId = "123456789011"
    Assert.False(isValid voterId)

[<Fact>]
let ``test invalid length`` () =
    // Test an invalid length for voter id
    let invalidLengthShort = "12345678901"
    let invalidLengthLong = "1234567890123"
    Assert.False(isValid invalidLengthShort)
    Assert.False(isValid invalidLengthLong)

[<Fact>]
let ``test invalid characters`` () =
    // Test voter id with non-numeric characters
    let invalidCharacters = "ABCD56789012"
    let invalidCharactersSpace = "217633 460 930"
    Assert.False(isValid invalidCharacters)
    Assert.False(isValid invalidCharactersSpace)

[<Fact>]
let ``test valid special case`` () =
    // Test a valid edge case (SP & MG with 13 digits)
    let validSpecial = "3244567800167"
    Assert.True(isValid validSpecial)

[<Fact>]
let ``test invalid vd1`` () =
    let voterId = "427503840223"
    Assert.False(isValid voterId)

[<Fact>]
let ``test invalid vd2`` () =
    let voterId = "427503840214"
    Assert.False(isValid voterId)

[<Fact>]
let ``test format voter id`` () =
    Assert.Equal(Some "2776 2712 28 52", formatVoterId "277627122852")
    Assert.IsType<string option>(formatVoterId "277627122852") |> ignore
    Assert.Equal(None, formatVoterId "00000000000")
    Assert.Equal(None, formatVoterId "0000000000a")
    Assert.Equal(None, formatVoterId "000000000000")
    Assert.Equal(None, formatVoterId "800911840197")

[<Fact>]
let ``test generate voter id MG`` () =
    // test if is_valid a voter id from MG
    let voterId = generate "MG"
    match voterId with
    | Some id -> Assert.True(isValid id)
    | None -> Assert.True(false, "Failed to generate voter ID for MG")

[<Fact>]
let ``test generate voter id AC`` () =
    // test if is_valid a voter id from AC
    let voterId = generate "AC"
    match voterId with
    | Some id -> Assert.True(isValid id)
    | None -> Assert.True(false, "Failed to generate voter ID for AC")

[<Fact>]
let ``test generate voter id foreigner`` () =
    // test if is_valid a voter id from foreigner
    let voterId = generate "ZZ"
    match voterId with
    | Some id -> 
        Assert.True(isValid id)
        Assert.IsType<string>(id) |> ignore
    | None -> Assert.True(false, "Failed to generate voter ID for ZZ")

[<Fact>]
let ``test generate voter id invalid UF`` () =
    // test if UF is not valid
    let voterId = generate "XX"
    Assert.Equal(None, voterId)
