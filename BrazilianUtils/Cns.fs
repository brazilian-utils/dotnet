module BrazilianUtils.Cns

open System
open System.Text
open Helpers

/// Valida um CNS definitivo (começa com 1 ou 2): módulo 11 sobre os 11
/// dígitos do PIS-base, com o par dígito verificador + preenchimento fixo.
let private ehDefinitivoValido (digitos: int list) : bool =
    let pis = digitos.[0..10]
    let pesos = [ 15; 14; 13; 12; 11; 10; 9; 8; 7; 6; 5 ]
    let soma = List.zip pis pesos |> List.sumBy (fun (d, p) -> d * p)
    let resto = soma % 11
    let dv = 11 - resto
    if dv = 11 then
        digitos.[11..14] = [ 0; 0; 0; 0 ]
    elif dv = 10 then
        let soma2 = soma + 2
        let resto2 = soma2 % 11
        let dv2 = (11 - resto2) % 11
        digitos.[11..14] = [ 0; 0; 1; dv2 ]
    else
        digitos.[11..14] = [ 0; 0; 0; dv ]

/// Valida um CNS provisório (começa com 7, 8 ou 9): módulo 11 sobre os 15
/// dígitos completos.
let private ehProvisorioValido (digitos: int list) : bool =
    let pesos = [ 15; 14; 13; 12; 11; 10; 9; 8; 7; 6; 5; 4; 3; 2; 1 ]
    let soma = List.zip digitos pesos |> List.sumBy (fun (d, p) -> d * p)
    soma % 11 = 0

/// Formata um CNS em grupos de 3-4-4-4 dígitos separados por espaço.
///
/// Examples:
///     Format "123456789010001" = "123 4567 8901 0001"
let Format (value: string) : string =
    let apenasDigitos = OnlyNumbers value
    let sb = StringBuilder(apenasDigitos)
    if sb.Length > 3 then sb.Insert(3, " ") |> ignore
    if sb.Length > 8 then sb.Insert(8, " ") |> ignore
    if sb.Length > 13 then sb.Insert(13, " ") |> ignore
    sb.ToString()

/// Valida um CNS (Cartão Nacional de Saúde): 15 dígitos. Cartões definitivos
/// começam com 1 ou 2, provisórios com 7, 8 ou 9, cada um com sua própria
/// regra de módulo 11; um número começando com 5 é sempre rejeitado.
///
/// Examples:
///     IsValid "123456789010000" = true
let IsValid (value: string) : bool =
    let digitos = stringToIntList value
    if not (hasLength 15 digitos) then
        false
    else
        match digitos.[0] with
        | 1 | 2 -> ehDefinitivoValido digitos
        | 7 | 8 | 9 -> ehProvisorioValido digitos
        | _ -> false

/// Remove a formatação do CNS e mantém apenas dígitos, limitado a 15 dígitos.
let Parse (value: string) : string =
    let apenasDigitos = OnlyNumbers value
    if apenasDigitos.Length > 15 then apenasDigitos.Substring(0, 15) else apenasDigitos
