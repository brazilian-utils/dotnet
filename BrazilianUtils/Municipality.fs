module BrazilianUtils.Municipality

open System
open System.Text.Json
open Helpers

type MunicipalityInfo =
    { Code: string
      Name: string
      StateCode: string }

let private tamanho = 7

/// Todos os municípios brasileiros publicados pelo IBGE, carregados uma
/// única vez a partir do recurso incorporado `municipalities.json`.
let private todos : Lazy<MunicipalityInfo list> =
    lazy (
        let json = lerRecursoEmbutido "municipalities.json"
        use documento = JsonDocument.Parse(json)
        [ for item in documento.RootElement.GetProperty("data").EnumerateArray() ->
            { Code = item.GetProperty("code").GetString()
              Name = item.GetProperty("name").GetString()
              StateCode = item.GetProperty("stateCode").GetString() } ]
    )

let private comparadorPtBr =
    StringComparer.Create(Globalization.CultureInfo("pt-BR"), true)

/// Consulta um município pelo seu código IBGE de 7 dígitos.
let GetByCode (code: obj) : MunicipalityInfo option =
    let texto =
        match code with
        | :? string as s -> OnlyNumbers s
        | :? int as i -> string i
        | :? int64 as i -> string i
        | _ -> ""
    if texto.Length <> tamanho then
        None
    else
        todos.Value |> List.tryFind (fun m -> m.Code = texto)

/// Retorna os municípios brasileiros publicados pelo IBGE, ordenados pelo
/// nome: todos, ou apenas os de um estado (comparação sensível a maiúsculas,
/// como no contrato).
let List (stateCode: string option) : MunicipalityInfo list =
    let filtrados =
        match stateCode with
        | None -> todos.Value
        | Some uf when uf = "" -> []
        | Some uf -> todos.Value |> List.filter (fun m -> m.StateCode = uf)
    filtrados |> List.sortWith (fun a b -> comparadorPtBr.Compare(a.Name, b.Name))
