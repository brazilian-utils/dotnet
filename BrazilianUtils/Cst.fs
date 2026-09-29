module BrazilianUtils.Cst

open System
open System.Text.RegularExpressions

// Tabela A - Origem da Mercadoria (1º dígito do CST de ICMS de 3 dígitos)
let private origemIcms = set [ "0"; "1"; "2"; "3"; "4"; "5"; "6"; "7"; "8" ]

// Tabela B - Tributação pelo ICMS (2 últimos dígitos do CST de 3 dígitos)
let private tabelaBIcms =
    set [ "00"; "10"; "20"; "30"; "40"; "41"; "50"; "51"; "60"; "70"; "90" ]

// Tabela do IPI (IN RFB 1.009/2010)
let private cstIpi =
    set [ "00"; "01"; "02"; "03"; "04"; "05"; "49"; "50"; "51"; "52"; "53"; "54"; "55"; "99" ]

// Tabela do PIS/COFINS (Guia Prático EFD-Contribuições, compartilhada pelos dois tributos)
let private cstPisCofins =
    set [ "01"; "02"; "03"; "04"; "05"; "06"; "07"; "08"; "09"; "49"; "50"; "51"; "52"; "53"
          "54"; "55"; "56"; "60"; "61"; "62"; "63"; "64"; "65"; "66"; "67"; "70"; "71"; "72"
          "73"; "74"; "75"; "98"; "99" ]

let private paraTexto (value: obj) : string =
    match value with
    | :? string as s -> s.Trim()
    | :? int as i -> string i
    | :? int64 as i -> string i
    | _ -> ""

/// Interpreta o valor: 3 dígitos nus (ICMS), 2 dígitos nus (Tabela B/IPI/
/// PIS-COFINS), um único dígito (preenchido para a forma de 3 dígitos do
/// ICMS) ou a forma mascarada `N-NN` (dígito de origem + separador + Tabela
/// B).
let private analisarCodigo (texto: string) : string option =
    if texto = "" then
        None
    elif texto.Length = 3 && texto |> Seq.forall Char.IsDigit then
        Some texto
    elif texto.Length = 2 && texto |> Seq.forall Char.IsDigit then
        Some texto
    elif texto.Length = 1 && Char.IsDigit texto.[0] then
        // Um único dígito nu é o último dígito da forma de 3 dígitos, com
        // origem fixa em 0 (ex.: "5" -> "005"), não o dígito de origem.
        Some ("00" + texto)
    else
        let m = Regex.Match(texto, @"^(\d)[ .\-/](\d{2})$")
        if m.Success then Some (m.Groups.[1].Value + m.Groups.[2].Value) else None

/// Valida se um código CST é válido para um tributo.
///
/// `tax`: "icms" (3 dígitos), "ipi", "pis" ou "cofins" (2 dígitos, PIS e
/// COFINS compartilham a mesma tabela). Omitido ou desconhecido, qualquer
/// tabela é aceita.
let IsValid (value: obj) (tax: string option) : bool =
    match analisarCodigo (paraTexto value) with
    | None -> false
    | Some codigo when codigo.Length = 3 ->
        let ehIcmsValido = origemIcms.Contains(string codigo.[0]) && tabelaBIcms.Contains(codigo.Substring(1))
        match tax with
        | Some "icms" | None -> ehIcmsValido
        | Some _ -> false
    | Some codigo ->
        let ehIpiValido = cstIpi.Contains(codigo)
        let ehPisCofinsValido = cstPisCofins.Contains(codigo)
        match tax with
        | Some "ipi" -> ehIpiValido
        | Some "pis" | Some "cofins" -> ehPisCofinsValido
        | Some "icms" -> false
        | _ -> ehIpiValido || ehPisCofinsValido
