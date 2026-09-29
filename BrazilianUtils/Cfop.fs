module BrazilianUtils.Cfop

open System
open System.Text.Json
open System.Text.RegularExpressions
open Helpers

type CfopInfo =
    { Code: string
      Description: string }

let private padraoMascara = @"^(\d)[ .\-/](\d{3})$"

/// Tabela oficial de CFOP operáveis (exclui cabeçalhos de grupo/subgrupo),
/// carregada uma única vez a partir do recurso incorporado `cfop.json`.
let private tabela : Lazy<System.Collections.Generic.Dictionary<string, string>> =
    lazy (
        let json = lerRecursoEmbutido "cfop.json"
        use documento = JsonDocument.Parse(json)
        let dicionario = System.Collections.Generic.Dictionary<string, string>()
        for item in documento.RootElement.GetProperty("data").EnumerateArray() do
            dicionario.[item.GetProperty("code").GetString()] <- item.GetProperty("description").GetString()
        dicionario
    )

let private paraTexto (value: obj) : string =
    match value with
    | :? string as s -> s.Trim()
    | :? int as i -> string i
    | :? int64 as i -> string i
    | _ -> ""

/// Interpreta um valor para consulta: 4 dígitos nus, ou a forma `N.NNN` com
/// um único separador; nenhum preenchimento com zero (nenhum CFOP começa
/// com zero).
let private analisarParaConsulta (value: obj) : string option =
    let texto = paraTexto value
    if texto = "" then
        None
    elif texto |> Seq.forall Char.IsDigit then
        if texto.Length = 4 then Some texto else None
    else
        let m = Regex.Match(texto, padraoMascara)
        if m.Success then Some (m.Groups.[1].Value + m.Groups.[2].Value) else None

/// Consulta um código CFOP na tabela oficial (Anexo II do Convênio SINIEF
/// s/nº 1970 em vigor).
let Get (value: obj) : CfopInfo option =
    match analisarParaConsulta value with
    | None -> None
    | Some codigo ->
        match tabela.Value.TryGetValue(codigo) with
        | true, descricao -> Some { Code = codigo; Description = descricao }
        | false, _ -> None

/// Valida se um código CFOP existe na tabela oficial (cabeçalhos de
/// grupo/subgrupo não contam).
let IsValid (value: obj) : bool = Get value |> Option.isSome

/// Remove a formatação do CFOP e mantém apenas dígitos, limitado a 4
/// dígitos. Nada é preenchido com zero.
let Parse (value: obj) : string =
    let apenasDigitos = OnlyNumbers (paraTexto value)
    if apenasDigitos.Length > 4 then apenasDigitos.Substring(0, 4) else apenasDigitos
