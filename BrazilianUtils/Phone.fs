module BrazilianUtils.Phone

open System
open BrazilianUtils

let private areaCodes =
    [ [ 1; 1 ]
      [ 1; 2 ]
      [ 1; 3 ]
      [ 1; 4 ]
      [ 1; 5 ]
      [ 1; 6 ]
      [ 1; 7 ]
      [ 1; 8 ]
      [ 1; 9 ]
      [ 2; 1 ]
      [ 2; 2 ]
      [ 2; 4 ]
      [ 2; 7 ]
      [ 2; 8 ]
      [ 3; 1 ]
      [ 3; 2 ]
      [ 3; 3 ]
      [ 3; 4 ]
      [ 3; 5 ]
      [ 3; 7 ]
      [ 3; 8 ]
      [ 4; 1 ]
      [ 4; 2 ]
      [ 4; 3 ]
      [ 4; 4 ]
      [ 4; 5 ]
      [ 4; 6 ]
      [ 4; 7 ]
      [ 4; 8 ]
      [ 4; 9 ]
      [ 5; 1 ]
      [ 5; 3 ]
      [ 5; 4 ]
      [ 5; 5 ]
      [ 6; 1 ]
      [ 6; 2 ]
      [ 6; 3 ]
      [ 6; 4 ]
      [ 6; 5 ]
      [ 6; 6 ]
      [ 6; 7 ]
      [ 6; 8 ]
      [ 6; 9 ]
      [ 7; 1 ]
      [ 7; 3 ]
      [ 7; 4 ]
      [ 7; 5 ]
      [ 7; 7 ]
      [ 7; 9 ]
      [ 8; 1 ]
      [ 8; 2 ]
      [ 8; 3 ]
      [ 8; 4 ]
      [ 8; 5 ]
      [ 8; 6 ]
      [ 8; 7 ]
      [ 8; 8 ]
      [ 8; 9 ]
      [ 9; 1 ]
      [ 9; 2 ]
      [ 9; 3 ]
      [ 9; 4 ]
      [ 9; 5 ]
      [ 9; 6 ]
      [ 9; 7 ]
      [ 9; 8 ]
      [ 9; 9 ] ]



let private mobilePhoneLength = 11

let private landlinePhoneLength = 10

let private hasLenght (phone : int list) =
    match phone.Length with
    | x when x = mobilePhoneLength -> true
    | x when x = landlinePhoneLength -> true
    | _ -> false

let private isValidDigit (phone : int list) =
    let isValidLandlineDigit digit =
        [ 2; 3; 4; 5 ] |> List.exists (fun x -> x = digit)
        
    let isValidMobileDigit digit =
        [ 6; 7; 8; 9 ] |> List.exists (fun x -> x = digit)

    match phone.Length with
    | x when x = mobilePhoneLength -> phone.[2] |> isValidMobileDigit
    | x when x = landlinePhoneLength -> phone.[2] |> isValidLandlineDigit
    | _ -> false

let private isValidCode (phone: int list) =
    areaCodes
    |> List.exists (fun x -> x.[0] = phone.[0] && x.[1] = phone.[1])

let IsValid phone =
    let clearValue = Helpers.stringToIntList phone
    [ hasLenght; isValidCode; isValidDigit ] |> Seq.forall (fun validator -> validator clearValue)

// SÍMBOLOS E MÁSCARA
// ===================

/// Remove os símbolos de formatação de telefone (espaço, parênteses, hífen e
/// o sinal de mais), mantendo o restante.
///
/// Examples:
///     RemoveSymbols "(11) 3000-0000" = "1130000000"
let RemoveSymbols (value: string) : string =
    if isNull value then ""
    else value |> String.filter (fun c -> c <> ' ' && c <> '(' && c <> ')' && c <> '-' && c <> '+')

/// Remove um código de discagem internacional (+55 / 0055 / 55) à esquerda do
/// número, quando presente e quando restarem 10 ou 11 dígitos (para não
/// confundir o DDD 55 com o código do país).
let RemoveInternationalDialingCode (value: string) : string =
    if isNull value then ""
    else
        let apenasDigitos = Helpers.OnlyNumbers value
        if apenasDigitos.StartsWith("0055") && apenasDigitos.Length > 11 then
            apenasDigitos.Substring(4)
        elif apenasDigitos.StartsWith("55") && apenasDigitos.Length > 11 then
            apenasDigitos.Substring(2)
        else
            value

/// Remove a formatação do telefone e mantém apenas dígitos, limitado a 11
/// dígitos. Remove o código de discagem internacional quando restarem 10 ou
/// 11 dígitos.
///
/// Examples:
///     Parse "(11) 98888-7777" = "11988887777"
///     Parse "+55 11 98888-7777" = "11988887777"
let Parse (value: string) : string =
    if isNull value then ""
    else
        let apenasDigitos = Helpers.OnlyNumbers value
        let semCodigoPais =
            if apenasDigitos.StartsWith("0055") && apenasDigitos.Length > 11 then
                apenasDigitos.Substring(4)
            elif apenasDigitos.StartsWith("55") && apenasDigitos.Length > 11 then
                apenasDigitos.Substring(2)
            else
                apenasDigitos
        if semCodigoPais.Length > 11 then semCodigoPais.Substring(0, 11) else semCodigoPais

/// Formata um número de telefone no padrão de número de assinante (5 + 3 ou
/// 5 + 4 dígitos), ignorando o DDD, se houver.
///
/// Examples:
///     Format "988887777" = "98888-7777"
///     Format "1130000000" = "11300-0000"
let Format (value: string) : string =
    let digitos = Helpers.OnlyNumbers value
    let truncado = if digitos.Length > 9 then digitos.Substring(0, 9) else digitos
    if truncado.Length = 0 then
        ""
    else
        let pontoDeCorte = min 5 truncado.Length
        let primeiraParte = truncado.Substring(0, pontoDeCorte)
        let resto = truncado.Substring(pontoDeCorte)
        if resto.Length = 0 then primeiraParte else primeiraParte + "-" + resto

/// Valida se um número é um telefone fixo brasileiro válido (DDD + 8 dígitos,
/// primeiro dígito do assinante entre 2 e 5). Aceita código de discagem
/// internacional.
let IsValidLandline (value: string) : bool =
    let semCodigoPais = Parse value
    let listaDigitos = semCodigoPais |> Seq.map Helpers.charToInt |> Seq.toList
    listaDigitos.Length = landlinePhoneLength
    && isValidCode listaDigitos
    && [ 2; 3; 4; 5 ] |> List.contains listaDigitos.[2]

/// Valida se um número é um celular brasileiro válido (DDD + 9 dígitos,
/// primeiro dígito do assinante entre 6 e 9). Aceita código de discagem
/// internacional.
let IsValidMobile (value: string) : bool =
    let semCodigoPais = Parse value
    let listaDigitos = semCodigoPais |> Seq.map Helpers.charToInt |> Seq.toList
    listaDigitos.Length = mobilePhoneLength
    && isValidCode listaDigitos
    && [ 6; 7; 8; 9 ] |> List.contains listaDigitos.[2]

/// Valida se um número é um número de serviço brasileiro válido (sem DDD):
/// códigos não geográficos (0300/0303/0500/0800/0900 + 7 dígitos), números
/// abreviados 300X/400X (8 dígitos), ou códigos públicos de 3 dígitos.
let IsValidService (value: string) : bool =
    let apenasDigitos = Helpers.OnlyNumbers value
    let prefixosNaoGeograficos = [ "0300"; "0303"; "0500"; "0800"; "0900" ]
    let ehNaoGeografico =
        apenasDigitos.Length = 11
        && prefixosNaoGeograficos |> List.contains (apenasDigitos.Substring(0, 4))
    let ehAbreviado =
        apenasDigitos.Length = 8
        && (apenasDigitos.StartsWith("300") || apenasDigitos.StartsWith("400"))
    let codigosPublicos = [ "100"; "128"; "129"; "130"; "131"; "132"; "133"; "135"; "136"; "137"; "138"; "180"; "181"; "185"; "188"; "190"; "191"; "192"; "193"; "197"; "198"; "199" ]
    let ehCodigoPublico = apenasDigitos.Length = 3 && codigosPublicos |> List.contains apenasDigitos
    ehNaoGeografico || ehAbreviado || ehCodigoPublico

/// Gera um número de telefone brasileiro aleatório e estruturalmente válido
/// (DDD + número, sem formatação).
///
/// Args:
///     tipo: "mobile", "landline" ou "service". Se omitido, sorteia entre
///           celular e fixo.
let rec Generate (tipo: string option) : string =
    let rnd = Random()
    let gerarDigitos quantidade =
        List.init quantidade (fun _ -> rnd.Next(0, 10))
        |> List.map string
        |> String.concat ""
    let codigoDeArea () =
        areaCodes.[rnd.Next(areaCodes.Length)] |> List.map string |> String.concat ""
    match tipo with
    | Some "service" ->
        let servicos = [ "0300"; "0500"; "0800"; "0900" ]
        servicos.[rnd.Next(servicos.Length)] + gerarDigitos 7
    | Some "landline" ->
        let primeiroDigito = rnd.Next(2, 6)
        codigoDeArea () + string primeiroDigito + gerarDigitos 7
    | Some "mobile" ->
        let primeiroDigito = rnd.Next(6, 10)
        codigoDeArea () + string primeiroDigito + gerarDigitos 8
    | _ ->
        if rnd.Next(2) = 0 then Generate (Some "mobile") else Generate (Some "landline")
