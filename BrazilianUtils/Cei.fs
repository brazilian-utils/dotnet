module BrazilianUtils.Cei

open System.Text
open Helpers

let private pesos = [ 7; 4; 1; 8; 5; 2; 1; 6; 3; 7; 4 ]

/// Calcula o dígito verificador do CEI a partir dos 11 dígitos-base: soma
/// ponderada, soma as dezenas da soma às suas unidades, e devolve o
/// complemento a 10 do dígito de unidades resultante (10 vira 0).
let private calcularDigitoVerificador (baseNumero: int list) : int =
    let soma =
        List.zip baseNumero pesos
        |> List.sumBy (fun (digito, peso) -> digito * peso)
    let dezenas = (soma / 10) % 10
    let unidades = soma % 10
    let somaParcial = dezenas + unidades
    let digitoUnidades = somaParcial % 10
    (10 - digitoUnidades) % 10

/// Formata um CEI no padrão usual `00.000.00000/00`, aplicando a máscara até
/// onde os dígitos alcançarem.
///
/// Examples:
///     Format "277297118187" = "27.729.71181/87"
let Format (value: string) : string =
    let apenasDigitos = OnlyNumbers value
    let sb = StringBuilder(apenasDigitos)
    if sb.Length > 2 then sb.Insert(2, ".") |> ignore
    if sb.Length > 6 then sb.Insert(6, ".") |> ignore
    if sb.Length > 12 then sb.Insert(12, "/") |> ignore
    sb.ToString()

/// Valida um CEI (Cadastro Específico do INSS): 12 dígitos, 11 dígitos-base
/// e um dígito verificador. Um valor cujos dígitos são todos iguais é
/// rejeitado.
///
/// Examples:
///     IsValid "277297118187" = true
let IsValid (value: string) : bool =
    let digitos = stringToIntList value
    if not (hasLength 12 digitos) || not (isNotRepdigit digitos) then
        false
    else
        let baseNumero = digitos.[0..10]
        calcularDigitoVerificador baseNumero = digitos.[11]

/// Remove a formatação do CEI e mantém apenas dígitos, limitado a 12 dígitos.
let Parse (value: string) : string =
    let apenasDigitos = OnlyNumbers value
    if apenasDigitos.Length > 12 then apenasDigitos.Substring(0, 12) else apenasDigitos
