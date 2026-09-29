module BrazilianUtils.Ncm

open System
open System.Text.Json
open System.Text.RegularExpressions
open Helpers

let private tamanho = 8
let private padraoMascara = @"^(\d{4})[ .\-/](\d{2})[ .\-/](\d{2})$"

/// Tabela oficial NCM (Nomenclatura Comum do Mercosul, nível 8 dígitos),
/// carregada uma única vez a partir do recurso incorporado `ncm.json`.
let private tabela : Lazy<System.Collections.Generic.HashSet<string>> =
    lazy (
        let json = lerRecursoEmbutido "ncm.json"
        use documento = JsonDocument.Parse(json)
        let conjunto = System.Collections.Generic.HashSet<string>()
        for item in documento.RootElement.GetProperty("data").EnumerateArray() do
            conjunto.Add(item.GetProperty("code").GetString()) |> ignore
        conjunto
    )

let private paraTexto (value: obj) : string =
    match value with
    | :? string as s -> s
    | :? int as i -> string i
    | :? int64 as i -> string i
    | _ -> ""

let private analisarParaConsulta (value: obj) : string option =
    let texto = (paraTexto value).Trim()
    if texto = "" then
        None
    elif texto |> Seq.forall Char.IsDigit then
        if texto.Length <= tamanho then Some (texto.PadLeft(tamanho, '0')) else None
    else
        let m = Regex.Match(texto, padraoMascara)
        if m.Success then
            Some (m.Groups.[1].Value + m.Groups.[2].Value + m.Groups.[3].Value)
        else
            None

/// Formata um código NCM no padrão `NNNN.NN.NN`, aplicando a máscara até
/// onde os dígitos alcançarem. Não valida o código (use `IsValid`).
let Format (value: obj) : string =
    let apenasDigitos = OnlyNumbers (paraTexto value)
    let sb = Text.StringBuilder(apenasDigitos)
    if sb.Length > 4 then sb.Insert(4, ".") |> ignore
    if sb.Length > 7 then sb.Insert(7, ".") |> ignore
    sb.ToString()

/// Valida se um código NCM existe na tabela vigente publicada pelo Siscomex.
let IsValid (value: obj) : bool =
    match analisarParaConsulta value with
    | None -> false
    | Some codigo -> tabela.Value.Contains(codigo)

/// Remove a formatação do NCM e mantém apenas dígitos, limitado a 8 dígitos.
let Parse (value: obj) : string =
    let apenasDigitos = OnlyNumbers (paraTexto value)
    if apenasDigitos.Length > tamanho then apenasDigitos.Substring(0, tamanho) else apenasDigitos
