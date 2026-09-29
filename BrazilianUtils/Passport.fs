module BrazilianUtils.Passport

open System
open System.Text.RegularExpressions

let private tamanho = 8

let private paraTexto (value: obj) : string =
    match value with
    | :? string as s -> s
    | :? int as i -> string i
    | :? int64 as i -> string i
    | _ -> ""

/// Remove os caracteres não alfanuméricos, converte para maiúsculo e limita
/// a 8 caracteres.
let Parse (value: obj) : string =
    let apenasAlfanumerico =
        (paraTexto value).ToUpperInvariant() |> String.filter Char.IsLetterOrDigit
    if apenasAlfanumerico.Length > tamanho then apenasAlfanumerico.Substring(0, tamanho) else apenasAlfanumerico

/// Remove os símbolos de formatação (mantém o restante).
let RemoveSymbols (value: string) : string =
    if isNull value then "" else value |> String.filter (fun c -> Char.IsLetterOrDigit c || Char.IsWhiteSpace c)

/// Formata um número de passaporte para exibição: maiúsculo, sem símbolos,
/// limitado a 8 caracteres (a mesma operação de `Parse`).
let Format (value: obj) : string = Parse value

/// Valida um número de passaporte brasileiro: 2 letras seguidas de 6
/// dígitos, após remover os caracteres não alfanuméricos. Não há dígito
/// verificador, então um número bem formado não é necessariamente real. Um
/// número nunca é válido (a forma decimal nunca começa com as duas letras
/// exigidas).
let IsValid (value: obj) : bool =
    match value with
    | :? string as s ->
        let normalizado = (s |> String.filter Char.IsLetterOrDigit).ToUpperInvariant()
        Regex.IsMatch(normalizado, @"^[A-Z]{2}\d{6}$")
    | _ -> false

/// Gera um número de passaporte brasileiro aleatório e válido: 2 letras
/// maiúsculas seguidas de 6 dígitos.
let Generate () : string =
    let random = Random()
    let letra () = char (random.Next(26) + int 'A')
    let digito () = char (random.Next(10) + int '0')
    String([| letra (); letra (); digito (); digito (); digito (); digito (); digito (); digito () |])
