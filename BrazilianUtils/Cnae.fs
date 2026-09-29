module BrazilianUtils.Cnae

open System
open System.Text.Json
open System.Text.RegularExpressions
open Helpers

type CnaeInfo =
    { Code: string
      Description: string }

let private tamanho = 7
let private padraoMascara = @"^(\d{4})[ .\-/](\d{1})[ .\-/](\d{2})$"

/// Tabela oficial CNAE-Subclasses 2.3 (7 dígitos), carregada uma única vez a
/// partir do recurso incorporado `cnae.json`.
let private tabela : Lazy<System.Collections.Generic.Dictionary<string, string>> =
    lazy (
        let json = lerRecursoEmbutido "cnae.json"
        use documento = JsonDocument.Parse(json)
        let dicionario = System.Collections.Generic.Dictionary<string, string>()
        for item in documento.RootElement.GetProperty("data").EnumerateArray() do
            dicionario.[item.GetProperty("code").GetString()] <- item.GetProperty("description").GetString()
        dicionario
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

/// Formata um código CNAE no padrão `NNNN-N/NN`, aplicando a máscara até
/// onde os dígitos alcançarem. Não valida o código (use `IsValid`).
let Format (value: obj) : string =
    let apenasDigitos = OnlyNumbers (paraTexto value)
    let sb = Text.StringBuilder(apenasDigitos)
    if sb.Length > 4 then sb.Insert(4, "-") |> ignore
    if sb.Length > 6 then sb.Insert(6, "/") |> ignore
    sb.ToString()

/// Consulta um código CNAE na tabela oficial CNAE-Subclasses 2.3.
let Get (value: obj) : CnaeInfo option =
    match analisarParaConsulta value with
    | None -> None
    | Some codigo ->
        match tabela.Value.TryGetValue(codigo) with
        | true, descricao -> Some { Code = codigo; Description = descricao }
        | false, _ -> None

/// Valida se um código CNAE existe na tabela oficial CNAE-Subclasses 2.3.
let IsValid (value: obj) : bool = Get value |> Option.isSome

/// Remove a formatação do CNAE e mantém apenas dígitos, limitado a 7 dígitos.
let Parse (value: obj) : string =
    let apenasDigitos = OnlyNumbers (paraTexto value)
    if apenasDigitos.Length > tamanho then apenasDigitos.Substring(0, tamanho) else apenasDigitos
