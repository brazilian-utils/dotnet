module BrazilianUtils.Renavam

open System

// CONSTANTS
let private renavamDvWeights = [| 2; 3; 4; 5; 6; 7; 8; 9; 2; 3 |]

// PRIVATE FUNCTIONS
// =================

/// Validate the format of a RENAVAM string
let private validateRenavamFormat (renavam: string) : bool =
    if String.IsNullOrEmpty(renavam) then
        false
    elif renavam.Length <> 11 then
        false
    elif not (renavam |> Seq.forall Char.IsDigit) then
        false
    elif renavam |> Seq.distinct |> Seq.length = 1 then
        // All digits are the same (e.g., "11111111111")
        false
    else
        true

/// Sum the weighted digits of a RENAVAM
let private sumWeightedDigits (renavam: string) : int =
    let baseDigits = 
        renavam.Substring(0, 10)
        |> Seq.rev
        |> Seq.map (fun c -> int c - int '0')
        |> Seq.toArray
    
    Array.zip baseDigits renavamDvWeights
    |> Array.map (fun (digit, weight) -> digit * weight)
    |> Array.sum

/// Calculate the verification digit for a RENAVAM
let private calculateRenavamDv (renavam: string) : int =
    let weightedSum = sumWeightedDigits renavam
    let dv = 11 - (weightedSum % 11)
    if dv >= 10 then 0 else dv

// PUBLIC FUNCTIONS
// ================

/// Validates the Brazilian vehicle registration number (RENAVAM).
///
/// This function takes a RENAVAM string and checks if it is valid.
/// A valid RENAVAM consists of exactly 11 digits, with the last digit as
/// a verification digit calculated from the previous 10 digits.
///
/// Examples:
///     isValidRenavam "86769597308" = true
///     isValidRenavam "12345678901" = false
///     isValidRenavam "1234567890a" = false
///     isValidRenavam "12345678 901" = false
///     isValidRenavam "12345678" = false
///     isValidRenavam "" = false
let isValidRenavam (renavam: string) : bool =
    if not (validateRenavamFormat renavam) then
        false
    else
        let expectedDv = calculateRenavamDv renavam
        let actualDv = int renavam.[10] - int '0'
        expectedDv = actualDv
