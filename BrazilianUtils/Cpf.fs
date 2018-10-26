module BrazilianUtils.Cpf

open Helpers
open System.Text

let private cpfLength = 11
let private firstCheckDigitWeights = [ 10..-1..2 ]
let private secondCheckDigitWeights = [ 11..-1..2 ]

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
    let checkDigitPos = 9
    let checkDigit = value.[checkDigitPos]
    validateCheckDigit value firstCheckDigitWeights checkDigit

let private isValidSecondCheckDigit (value : int list) =
    let checkDigitPos = 10
    let checkDigit = value.[checkDigitPos]
    validateCheckDigit value secondCheckDigitWeights checkDigit

let private hasCpfLength = hasLength cpfLength

//  Visible members
let IsValid cpf =
    let cpf' = stringToIntList cpf
    [ hasValue; hasCpfLength; isNotRepdigit; isValidFirstCheckDigit; isValidSecondCheckDigit ]
    |> Seq.forall (fun validator -> validator cpf')

let Format cpf =
    let clearValue = OnlyNumbers cpf
    StringBuilder(clearValue).Insert(3, ".").Insert(7, ".").Insert(11, "-").ToString()

let Generate () =
    let baseCpf = generateRandomNumbers (cpfLength - 2)
    let firstCheckDigit = calculateDigit firstCheckDigitWeights baseCpf
    let secondCheckDigit = calculateDigit secondCheckDigitWeights (baseCpf@[firstCheckDigit])
    baseCpf @ [firstCheckDigit] @ [secondCheckDigit]
    |> List.map (fun x -> x.ToString())
    |> String.concat ""
