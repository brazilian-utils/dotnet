module BrazilianUtils.LicensePlate

open System
open System.Text.RegularExpressions

// PRIVATE VALIDATION FUNCTIONS
// =============================

/// Checks whether a string matches the old format of Brazilian license plate.
/// Pattern: 'LLLNNNN'
let private isValidOldFormat (licensePlate: string) : bool =
    if String.IsNullOrEmpty(licensePlate) then
        false
    else
        let trimmed = licensePlate.Trim()
        let pattern = @"^[A-Za-z]{3}[0-9]{4}$"
        Regex.IsMatch(trimmed, pattern)

/// Checks whether a string matches the Mercosul format of Brazilian license plate.
/// Pattern: 'LLLNLNN'
let private isValidMercosul (licensePlate: string) : bool =
    if String.IsNullOrEmpty(licensePlate) then
        false
    else
        let trimmed = licensePlate.Trim().ToUpper()
        let pattern = @"^[A-Z]{3}\d[A-Z]\d{2}$"
        Regex.IsMatch(trimmed, pattern)

// FORMATTING
// ==========

/// Converts an old pattern license plate (LLLNNNN) to a Mercosul format (LLLNLNN).
///
/// Examples:
///     convertToMercosul "ABC4567" = Some "ABC4F67"
///     convertToMercosul "ABC4*67" = None
let convertToMercosul (licensePlate: string) : string option =
    if not (isValidOldFormat licensePlate) then
        None
    else
        let digits = licensePlate.ToUpper() |> Seq.toArray
        // Convert the 5th character (index 4) from digit to letter
        digits.[4] <- char (int 'A' + (int digits.[4] - int '0'))
        Some (String(digits))

/// Formats a license plate into the correct pattern.
/// This function receives a license plate in any pattern (LLLNNNN or LLLNLNN)
/// and returns a formatted version.
///
/// Examples:
///     formatLicensePlate "ABC1234" = Some "ABC-1234"  // old format (contains a dash)
///     formatLicensePlate "abc1e34" = Some "ABC1E34"   // mercosul format
///     formatLicensePlate "ABC123" = None
let formatLicensePlate (licensePlate: string) : string option =
    let upper = licensePlate.ToUpper()
    
    if isValidOldFormat licensePlate then
        Some (upper.Substring(0, 3) + "-" + upper.Substring(3))
    elif isValidMercosul licensePlate then
        Some upper
    else
        None

// OPERATIONS
// ==========

/// Removes the dash (-) symbol from a license plate string.
///
/// Examples:
///     removeSymbols "ABC-123" = "ABC123"
///     removeSymbols "abc123" = "abc123"
///     removeSymbols "ABCD123" = "ABCD123"
let removeSymbols (licensePlateNumber: string) : string =
    licensePlateNumber.Replace("-", "")

/// Return the format of a license plate. 'LLLNNNN' for the old pattern and
/// 'LLLNLNN' for the Mercosul one.
///
/// Examples:
///     getFormat "abc1234" = Some "LLLNNNN"
///     getFormat "abc1d23" = Some "LLLNLNN"
///     getFormat "ABCD123" = None
let getFormat (licensePlate: string) : string option =
    if isValidOldFormat licensePlate then
        Some "LLLNNNN"
    elif isValidMercosul licensePlate then
        Some "LLLNLNN"
    else
        None

/// Returns if a Brazilian license plate number is valid.
/// It does not verify if the plate actually exists.
///
/// Args:
///     licensePlate: The license plate number to be validated.
///     plateType: "old_format", "mercosul", or None.
///                If not specified, checks for one or another.
let isValid (licensePlate: string) (plateType: string option) : bool =
    match plateType with
    | Some "old_format" -> isValidOldFormat licensePlate
    | Some "mercosul" -> isValidMercosul licensePlate
    | _ -> isValidOldFormat licensePlate || isValidMercosul licensePlate

/// Generate a valid license plate in the given format. In case no format is
/// provided, it will return a license plate in the Mercosul format.
///
/// Args:
///     format: The desired format for the license plate.
///             'LLLNNNN' for the old pattern or 'LLLNLNN' for the
///             Mercosul one. Default is 'LLLNLNN'
///
/// Examples:
///     generate None = "ABC1D23"
///     generate (Some "LLLNLNN") = "ABC4D56"
///     generate (Some "LLLNNNN") = "ABC1234"
///     generate (Some "invalid") = None
let generate (format: string option) : string option =
    let random = Random()
    let selectedFormat = defaultArg format "LLLNLNN"
    let upperFormat = selectedFormat.ToUpper()
    
    if upperFormat <> "LLLNLNN" && upperFormat <> "LLLNNNN" then
        None
    else
        let generated = 
            upperFormat
            |> Seq.map (fun c ->
                if c = 'L' then
                    char (random.Next(26) + int 'A')
                else
                    char (random.Next(10) + int '0')
            )
            |> Seq.toArray
            |> String
        
        Some generated
