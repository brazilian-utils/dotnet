module BrazilianUtils.Caepf

open System.Text
open Helpers

let private pesosPrimeiroDigito = [ 5; 4; 3; 2; 9; 8; 7; 6; 5; 4; 3; 2 ]
let private pesosSegundoDigito = [ 6; 5; 4; 3; 2; 9; 8; 7; 6; 5; 4; 3; 2 ]

let private regraDigito digito =
    match digito with
    | 0 | 1 -> 0
    | _ -> 11 - digito

let private calcularDigito pesos valor =
    valor |> calculateModulus11 pesos |> regraDigito

/// Formata um CAEPF no padrão `000.000.000/000-00`, aplicando a máscara até
/// onde os dígitos alcançarem.
///
/// Examples:
///     Format "29311861000184" = "293.118.610/001-84"
let Format (value: string) : string =
    let apenasDigitos = OnlyNumbers value
    let sb = StringBuilder(apenasDigitos)
    if sb.Length > 3 then sb.Insert(3, ".") |> ignore
    if sb.Length > 7 then sb.Insert(7, ".") |> ignore
    if sb.Length > 11 then sb.Insert(11, "/") |> ignore
    if sb.Length > 15 then sb.Insert(15, "-") |> ignore
    sb.ToString()

/// Valida um CAEPF (Cadastro de Atividade Econômica da Pessoa Física): 14
/// dígitos, sendo os 12 primeiros a base (CPF do titular + 3 dígitos de
/// sequência) e os 2 últimos os dígitos verificadores, calculados no padrão
/// do CNPJ e então deslocados em 12 unidades (módulo 100).
///
/// Examples:
///     IsValid "29311861000184" = true
let IsValid (value: string) : bool =
    let digitos = stringToIntList value
    if not (hasLength 14 digitos) || not (isNotRepdigit digitos.[0..11]) then
        false
    else
        let baseNumero = digitos.[0..11]
        let digito1 = calcularDigito pesosPrimeiroDigito baseNumero
        let digito2 = calcularDigito pesosSegundoDigito (baseNumero @ [ digito1 ])
        let parEstiloCnpj = digito1 * 10 + digito2
        let parEsperado = (parEstiloCnpj + 12) % 100
        digitos.[12] = parEsperado / 10 && digitos.[13] = parEsperado % 10

/// Remove a formatação do CAEPF e mantém apenas dígitos, limitado a 14 dígitos.
let Parse (value: string) : string =
    let apenasDigitos = OnlyNumbers value
    if apenasDigitos.Length > 14 then apenasDigitos.Substring(0, 14) else apenasDigitos
