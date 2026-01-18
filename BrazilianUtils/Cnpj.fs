module BrazilianUtils.Cnpj

open Helpers
open System
open System.Text

let private cnpjLength = 14
let private firstCheckDigitWeights = [ 5; 4; 3; 2; 9; 8; 7; 6; 5; 4; 3; 2 ]
let private secondCheckDigitWeights = [ 6; 5; 4; 3; 2; 9; 8; 7; 6; 5; 4; 3; 2 ]

let private digitRule digit =
    match digit with
    | 0 | 1 -> 0
    | _ -> 11 - digit

let private calculateDigit weights value =
    value
    |> calculateModulus11 weights
    |> digitRule

let private validateCheckDigit value weights checkDigit =
    value
    |> calculateDigit weights
    |> ((=) checkDigit)

let private isValidFirstCheckDigit (value : int list) =
    let checkDigitPos = 12
    let checkDigit = value.[checkDigitPos]
    validateCheckDigit value firstCheckDigitWeights checkDigit

let private isValidSecondCheckDigit (value : int list) =
    let checkDigitPos = 13
    let checkDigit = value.[checkDigitPos]
    validateCheckDigit value secondCheckDigitWeights checkDigit

let private hasCnpjLength = hasLength cnpjLength

// Alphanumeric CNPJ support (Instrução Normativa RFB nº 2.119)
// Converts character to CNPJ value: ASCII code - 48
// Numbers 0-9 map to 0-9, letters A-Z map to 17-42
let private charToCnpjValue (c : char) =
    int (Char.ToUpper c) - 48

let private isValidCnpjChar (c : char) =
    Char.IsDigit c || (Char.ToUpper c >= 'A' && Char.ToUpper c <= 'Z')

let private onlyAlphanumeric (value : string) =
    value
    |> String.filter isValidCnpjChar

let private stringToCnpjValues (value : string) =
    value
    |> onlyAlphanumeric
    |> Seq.map charToCnpjValue
    |> Seq.toList

let private isNotRepdigitCnpj (value : int list) =
    value
    |> Seq.forall (fun elem -> elem = value.[0])
    |> not

//  Visible members
let IsValid cnpj =
    let clearValue = stringToCnpjValues cnpj
    [ hasValue; hasCnpjLength; isNotRepdigitCnpj; isValidFirstCheckDigit; isValidSecondCheckDigit ]
    |> List.forall (fun validator -> validator clearValue)

let Format cnpj =
    let clearValue = onlyAlphanumeric cnpj
    StringBuilder(clearValue).Insert(2, ".").Insert(6, ".").Insert(10, "/").Insert(15, "-").ToString()

let Generate () =
    let baseCnpj = generateRandomNumbers (cnpjLength - 2)
    let firstCheckDigit = calculateDigit firstCheckDigitWeights baseCnpj
    let secondCheckDigit = calculateDigit secondCheckDigitWeights (baseCnpj@[firstCheckDigit])
    baseCnpj @ [firstCheckDigit] @ [secondCheckDigit]
    |> List.map (fun x -> x.ToString())
    |> String.concat ""

let GenerateAlphanumeric () =
    let rnd = Random()
    let chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ"
    let generateAlphanumericChar () = chars.[rnd.Next(chars.Length)]
    let baseCnpj = List.init (cnpjLength - 2) (fun _ -> generateAlphanumericChar())
    let baseCnpjValues = baseCnpj |> List.map charToCnpjValue
    let firstCheckDigit = calculateDigit firstCheckDigitWeights baseCnpjValues
    let secondCheckDigit = calculateDigit secondCheckDigitWeights (baseCnpjValues@[firstCheckDigit])
    (baseCnpj |> List.map string |> String.concat "") + firstCheckDigit.ToString() + secondCheckDigit.ToString()
