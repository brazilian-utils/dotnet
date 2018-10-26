module BrazilianUtils.Boleto

open Helpers

type private PartialPosition(startPos: int, endPos: int, checkDigitPos: int) =
    member this.StartPos = startPos
    member this.EndPos = endPos
    member this.CheckDigitPos = checkDigitPos

type private Partial(value: int list, checkDigit: int) =
    member this.Value = value
    member this.CheckDigit = checkDigit

let private partialPositions =
    [ new PartialPosition(0, 8, 9)
      new PartialPosition(10, 19, 20)
      new PartialPosition(21, 30, 31) ]


let private calculatePartialDigit (value : int list) =
    let weights = [ 2; 1 ]
    let accumulatorRule index digit =
        let weight = weights.[index % 2]
        let res = digit * weight
        if res > 9 then 1 + (res % 10) else res
    value
    |> List.rev
    |> List.mapi accumulatorRule
    |> List.sum
    |> (fun x -> x % 10)
    |> (fun x -> 10 - x)

let private validatePartialCheckDigit (partial: Partial) =
    partial.Value
    |> calculatePartialDigit
    |> ((=) partial.CheckDigit)

let boletoWeights =
    let initialWeight = 2;
    let nextWeightRule value =
        match value with
        | value when value < 9 -> value + 1
        | _ -> initialWeight
    let nextWeight acc =
        acc
        |> Seq.last
        |> nextWeightRule
    [0..41]
    |> List.fold(fun acc elem ->  acc @ [acc |> nextWeight]) [initialWeight]
    |> List.rev

let private calculateBoletoDigit (value: int list) =
    let digitRule digit =
        match digit with
        | 0 | 1 -> 1
        | _ -> 11 - digit
    value
    |> calculateModulus11 boletoWeights
    |> digitRule

let private getPartials (dl : int list) =
    partialPositions
    |> Seq.map (fun p -> new Partial(dl.[p.StartPos..p.EndPos], dl.[p.CheckDigitPos]))

let private validateDigitableLinePartials dl =
    dl |> getPartials
    |> Seq.forall validatePartialCheckDigit

let private convertDigitableLineToBoleto (value: int list) =
    value.[0..3] @ value.[32..46] @ value.[4..8] @ value.[10..19] @ value.[21..30]

let private validateBoleto (boleto: int list) =
    boleto.[0..3] @ boleto.[5..43]
    |> calculateBoletoDigit
    |> ((=) boleto.[4])

let private validateDigitableLine dl  =
    let boleto = convertDigitableLineToBoleto dl
    validateDigitableLinePartials dl && validateBoleto boleto

//  Visible members
let IsValid value =
    let clearValue = stringToIntList value
    let digitableLineLength = 47
    let boletoLength = 44
    match clearValue.Length with
    | x when x = digitableLineLength -> clearValue |> validateDigitableLine
    | x when x = boletoLength -> clearValue |> validateBoleto
    | _ -> false
