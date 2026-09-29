module BrazilianUtils.Text

open System
open System.Globalization
open System.Text
open System.Text.RegularExpressions

let private preposicoes =
    set [ "de"; "da"; "do"; "das"; "dos"; "e" ]

let private designacoes =
    set [ "LTDA"; "ME"; "EPP"; "CNPJ"; "CPF"; "S.A."; "EIRELI" ]

let private ehNumeroRomano (palavra: string) =
    palavra.Length >= 2 && Regex.IsMatch(palavra.ToUpper(), "^[IVXLCDM]+$")

let private capitalizarPalavra (palavra: string) (primeira: bool) (ultima: bool) : string =
    let maiuscula = palavra.ToUpper()
    if designacoes.Contains(maiuscula) then
        maiuscula
    elif preposicoes.Contains(palavra.ToLower()) && not primeira && not ultima then
        palavra.ToLower()
    elif ehNumeroRomano palavra then
        maiuscula
    else
        let minuscula = palavra.ToLower()
        if minuscula.Length = 0 then
            minuscula
        else
            let primeiroCaractere = minuscula.[0]
            if Char.IsLetter primeiroCaractere then
                string (Char.ToUpper primeiroCaractere) + minuscula.Substring(1)
            else
                minuscula

/// Capitaliza uma string da forma como um nome, razão social ou endereço
/// brasileiro é escrito, sem necessidade de configuração.
///
/// Examples:
///     Capitalize "jose da silva" = "Jose da Silva"
///     Capitalize "empresa ltda" = "Empresa LTDA"
let Capitalize (value: string) : string =
    if String.IsNullOrEmpty value then
        ""
    else
        let colapsado = Regex.Replace(value.Trim(), @"\s+", " ")
        if colapsado = "" then
            ""
        else
            let palavras = colapsado.Split(' ')
            palavras
            |> Array.mapi (fun i palavra -> capitalizarPalavra palavra (i = 0) (i = palavras.Length - 1))
            |> String.concat " "

/// Remove marcas diacríticas (acentos, tis, cedilhas) de uma string,
/// decompondo cada caractere acentuado (Unicode NFD) e descartando toda
/// marca combinante (categoria Unicode M).
///
/// Examples:
///     RemoveAccents "São Paulo" = "Sao Paulo"
///     RemoveAccents "Açaí" = "Acai"
let RemoveAccents (value: string) : string =
    if isNull value then
        ""
    else
        let normalizado = value.Normalize(NormalizationForm.FormD)
        normalizado
        |> Seq.filter (fun c -> CharUnicodeInfo.GetUnicodeCategory(c) <> UnicodeCategory.NonSpacingMark)
        |> Seq.toArray
        |> String
