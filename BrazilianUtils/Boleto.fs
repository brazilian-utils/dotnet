module BrazilianUtils.Boleto

open System
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
    |> (fun x -> (10 - x) % 10)

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

let private paraTexto (value: obj) : string =
    match value with
    | :? string as s -> s
    | :? int as i -> string i
    | :? int64 as i -> string i
    | _ -> ""

/// Remove a formatação do boleto e mantém apenas dígitos, limitado a 47
/// dígitos (48 para um boleto de arrecadação, reconhecido pelo primeiro
/// dígito "8").
let Parse (value: obj) : string =
    let apenasDigitos = OnlyNumbers (paraTexto value)
    let limite = if apenasDigitos.Length > 0 && apenasDigitos.[0] = '8' then 48 else 47
    if apenasDigitos.Length > limite then apenasDigitos.Substring(0, limite) else apenasDigitos

/// Formata um boleto com sua máscara impressa. Não valida (use `IsValid`).
///
/// - Linha digitável de cobrança bancária (47 dígitos): agrupada como
///   `00000.00000 00000.000000 00000.000000 0 00000000000000`.
/// - Linha digitável de arrecadação (48 dígitos, começa com "8"): quatro
///   blocos de 11 dígitos, cada um seguido de seu dígito verificador.
let Format (value: obj) : string =
    let digitos = Parse value
    if digitos = "" then
        ""
    else
        let mutable resultado = digitos
        let aplicar posicoes separadores =
            List.iter2
                (fun posicao separador ->
                    if resultado.Length > posicao then
                        resultado <- resultado.Substring(0, posicao) + separador + resultado.Substring(posicao))
                posicoes
                separadores
        if digitos.[0] = '8' then
            aplicar [ 11; 13; 25; 27; 39; 41; 53 ] [ "-"; " "; "-"; " "; "-"; " "; "-" ]
        else
            aplicar [ 5; 11; 17; 24; 30; 37; 39 ] [ "."; " "; "."; " "; "."; " "; " " ]
        resultado

/// Gera uma linha digitável de cobrança bancária (47 dígitos) aleatória e
/// válida, construída para satisfazer `IsValid` por construção (não apenas
/// por tentativa e erro).
let Generate () : string =
    let random = Random()
    let gerarDigitos n = List.init n (fun _ -> random.Next(0, 10))
    let banco = gerarDigitos 3
    let moeda = [ 9 ]
    let campo1Parte = gerarDigitos 5
    let campo2 = gerarDigitos 10
    let campo3 = gerarDigitos 10
    let fatorVencimento = [ 0; 0; 0; 0 ] // sem fator de vencimento (valor < 1000)
    let valor = gerarDigitos 10

    let baseParaDac = banco @ moeda @ fatorVencimento @ valor @ campo1Parte @ campo2 @ campo3
    let dac = calculateBoletoDigit baseParaDac
    let dv1 = calculatePartialDigit (banco @ moeda @ campo1Parte)
    let dv2 = calculatePartialDigit campo2
    let dv3 = calculatePartialDigit campo3

    banco @ moeda @ campo1Parte @ [ dv1 ] @ campo2 @ [ dv2 ] @ campo3 @ [ dv3 ] @ [ dac ] @ fatorVencimento @ valor
    |> List.map string
    |> String.concat ""
