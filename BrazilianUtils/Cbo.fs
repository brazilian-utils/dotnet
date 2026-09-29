module BrazilianUtils.Cbo

open System
open System.Text
open System.Text.Json
open System.Text.RegularExpressions
open Helpers

type CboInfo =
    { Code: string
      Description: string }

let private tamanho = 6
let private padraoMascara = @"^(\d{4})[ .\-/](\d{2})$"

/// Tabela oficial CBO 2002 (6 dígitos), carregada uma única vez a partir do
/// recurso incorporado `cbo.json`.
let private tabela : Lazy<System.Collections.Generic.Dictionary<string, string>> =
    lazy (
        let json = lerRecursoEmbutido "cbo.json"
        use documento = JsonDocument.Parse(json)
        let dicionario = System.Collections.Generic.Dictionary<string, string>()
        for item in documento.RootElement.GetProperty("data").EnumerateArray() do
            dicionario.[item.GetProperty("code").GetString()] <- item.GetProperty("description").GetString()
        dicionario
    )

/// Interpreta um valor (string ou número) para consulta na tabela: dígitos
/// nus são preenchidos com zeros à esquerda até `tamanho`; um valor com
/// máscara (um único separador entre os grupos) é lido como está escrito;
/// qualquer outra string é rejeitada.
let private analisarParaConsulta (value: obj) : string option =
    let texto =
        match value with
        | :? string as s -> s.Trim()
        | :? int as i -> string i
        | :? int64 as i -> string i
        | _ -> ""
    if texto = "" then
        None
    elif texto |> Seq.forall Char.IsDigit then
        if texto.Length <= tamanho then Some (texto.PadLeft(tamanho, '0')) else None
    else
        let m = Regex.Match(texto, padraoMascara)
        if m.Success then
            Some (m.Groups.[1].Value + m.Groups.[2].Value)
        else
            None

/// Consulta um código CBO na tabela oficial CBO 2002.
let Get (value: obj) : CboInfo option =
    match analisarParaConsulta value with
    | None -> None
    | Some codigo ->
        match tabela.Value.TryGetValue(codigo) with
        | true, descricao -> Some { Code = codigo; Description = descricao }
        | false, _ -> None

/// Valida se um código CBO existe na tabela oficial CBO 2002.
let IsValid (value: obj) : bool = Get value |> Option.isSome

/// Remove a formatação do CBO e mantém apenas dígitos, limitado a 6 dígitos.
/// Nada é preenchido com zeros: um zero à esquerda precisa estar escrito.
let Parse (value: obj) : string =
    let texto =
        match value with
        | :? string as s -> s
        | :? int as i -> string i
        | :? int64 as i -> string i
        | _ -> ""
    let apenasDigitos = OnlyNumbers texto
    if apenasDigitos.Length > tamanho then apenasDigitos.Substring(0, tamanho) else apenasDigitos
