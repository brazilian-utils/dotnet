module BrazilianUtils.Pis

open System

// CONSTANTS
let private weights = [| 3; 2; 9; 8; 7; 6; 5; 4; 3; 2 |]

// PRIVATE FUNCTIONS
// =================

/// Calculate the checksum digit of the given `basePis` string.
let private checksum (basePis: string) : int =
    let pisDigits = basePis |> Seq.map (fun c -> int c - int '0') |> Seq.toArray
    let pisSum = 
        Array.zip pisDigits weights
        |> Array.map (fun (digit, weight) -> digit * weight)
        |> Array.sum
    
    let checkDigit = 11 - (pisSum % 11)
    
    if checkDigit = 10 || checkDigit = 11 then 0 else checkDigit

// FORMATTING
// ==========

/// Remove formatting symbols from a PIS.
///
/// This function takes a PIS (Programa de Integração Social) string with
/// formatting symbols and returns a cleaned version with no symbols.
///
/// Examples:
///     removeSymbols "123.456.789-09" = "12345678909"
///     removeSymbols "98765432100" = "98765432100"
let removeSymbols (pis: string) : string =
    pis.Replace(".", "").Replace("-", "")

// OPERATIONS
// ==========

/// Returns whether or not the verifying checksum digit of the
/// given `PIS` match its base number.
///
/// Examples:
///     isValid "82178537464" = true
///     isValid "55550207753" = true
let isValid (pis: string) : bool =
    if String.IsNullOrEmpty(pis) then
        false
    elif pis.Length <> 11 then
        false
    elif not (pis |> Seq.forall Char.IsDigit) then
        false
    else
        let calculatedChecksum = checksum (pis.Substring(0, 10))
        let lastDigit = int pis.[10] - int '0'
        calculatedChecksum = lastDigit

/// Format a valid PIS (Programa de Integração Social) string with
/// standard visual aid symbols.
///
/// This function takes a valid numbers-only PIS string as input
/// and adds standard formatting visual aid symbols for display.
///
/// Examples:
///     formatPis "12345678909" = Some "123.45678.90-9"
///     formatPis "98765432100" = Some "987.65432.10-0"
let formatPis (pis: string) : string option =
    if not (isValid pis) then
        None
    else
        Some (sprintf "%s.%s.%s-%s" 
            (pis.Substring(0, 3))
            (pis.Substring(3, 5))
            (pis.Substring(8, 2))
            (pis.Substring(10, 1)))

/// Generate a random valid Brazilian PIS number.
///
/// This function generates a random PIS number with the following characteristics:
/// - It has 11 digits
/// - It passes the weight calculation check
///
/// Examples:
///     generate() = "12345678909"
///     generate() = "98765432100"
let generate () : string =
    let random = Random()
    let baseNumber = random.Next(0, 1000000000).ToString().PadLeft(10, '0')
    let checksumDigit = checksum baseNumber
    
    baseNumber + checksumDigit.ToString()
