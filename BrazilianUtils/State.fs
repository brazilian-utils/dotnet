module BrazilianUtils.State

open System
open System.Text.RegularExpressions

type StateInfo =
    { Code: string
      Name: string
      RegionCode: string
      RegionName: string
      IbgeCode: int
      Timezone: string }

/// As 27 unidades federativas brasileiras, na ordem alfabética pelo nome.
let private estados =
    [ { Code = "AC"; Name = "Acre"; RegionCode = "N"; RegionName = "Norte"; IbgeCode = 12; Timezone = "America/Rio_Branco" }
      { Code = "AL"; Name = "Alagoas"; RegionCode = "NE"; RegionName = "Nordeste"; IbgeCode = 27; Timezone = "America/Maceio" }
      { Code = "AP"; Name = "Amapá"; RegionCode = "N"; RegionName = "Norte"; IbgeCode = 16; Timezone = "America/Belem" }
      { Code = "AM"; Name = "Amazonas"; RegionCode = "N"; RegionName = "Norte"; IbgeCode = 13; Timezone = "America/Manaus" }
      { Code = "BA"; Name = "Bahia"; RegionCode = "NE"; RegionName = "Nordeste"; IbgeCode = 29; Timezone = "America/Bahia" }
      { Code = "CE"; Name = "Ceará"; RegionCode = "NE"; RegionName = "Nordeste"; IbgeCode = 23; Timezone = "America/Fortaleza" }
      { Code = "DF"; Name = "Distrito Federal"; RegionCode = "CO"; RegionName = "Centro-Oeste"; IbgeCode = 53; Timezone = "America/Sao_Paulo" }
      { Code = "ES"; Name = "Espírito Santo"; RegionCode = "SE"; RegionName = "Sudeste"; IbgeCode = 32; Timezone = "America/Sao_Paulo" }
      { Code = "GO"; Name = "Goiás"; RegionCode = "CO"; RegionName = "Centro-Oeste"; IbgeCode = 52; Timezone = "America/Sao_Paulo" }
      { Code = "MA"; Name = "Maranhão"; RegionCode = "NE"; RegionName = "Nordeste"; IbgeCode = 21; Timezone = "America/Fortaleza" }
      { Code = "MT"; Name = "Mato Grosso"; RegionCode = "CO"; RegionName = "Centro-Oeste"; IbgeCode = 51; Timezone = "America/Cuiaba" }
      { Code = "MS"; Name = "Mato Grosso do Sul"; RegionCode = "CO"; RegionName = "Centro-Oeste"; IbgeCode = 50; Timezone = "America/Campo_Grande" }
      { Code = "MG"; Name = "Minas Gerais"; RegionCode = "SE"; RegionName = "Sudeste"; IbgeCode = 31; Timezone = "America/Sao_Paulo" }
      { Code = "PA"; Name = "Pará"; RegionCode = "N"; RegionName = "Norte"; IbgeCode = 15; Timezone = "America/Belem" }
      { Code = "PB"; Name = "Paraíba"; RegionCode = "NE"; RegionName = "Nordeste"; IbgeCode = 25; Timezone = "America/Fortaleza" }
      { Code = "PR"; Name = "Paraná"; RegionCode = "S"; RegionName = "Sul"; IbgeCode = 41; Timezone = "America/Sao_Paulo" }
      { Code = "PE"; Name = "Pernambuco"; RegionCode = "NE"; RegionName = "Nordeste"; IbgeCode = 26; Timezone = "America/Recife" }
      { Code = "PI"; Name = "Piauí"; RegionCode = "NE"; RegionName = "Nordeste"; IbgeCode = 22; Timezone = "America/Fortaleza" }
      { Code = "RJ"; Name = "Rio de Janeiro"; RegionCode = "SE"; RegionName = "Sudeste"; IbgeCode = 33; Timezone = "America/Sao_Paulo" }
      { Code = "RN"; Name = "Rio Grande do Norte"; RegionCode = "NE"; RegionName = "Nordeste"; IbgeCode = 24; Timezone = "America/Fortaleza" }
      { Code = "RS"; Name = "Rio Grande do Sul"; RegionCode = "S"; RegionName = "Sul"; IbgeCode = 43; Timezone = "America/Sao_Paulo" }
      { Code = "RO"; Name = "Rondônia"; RegionCode = "N"; RegionName = "Norte"; IbgeCode = 11; Timezone = "America/Porto_Velho" }
      { Code = "RR"; Name = "Roraima"; RegionCode = "N"; RegionName = "Norte"; IbgeCode = 14; Timezone = "America/Boa_Vista" }
      { Code = "SC"; Name = "Santa Catarina"; RegionCode = "S"; RegionName = "Sul"; IbgeCode = 42; Timezone = "America/Sao_Paulo" }
      { Code = "SP"; Name = "São Paulo"; RegionCode = "SE"; RegionName = "Sudeste"; IbgeCode = 35; Timezone = "America/Sao_Paulo" }
      { Code = "SE"; Name = "Sergipe"; RegionCode = "NE"; RegionName = "Nordeste"; IbgeCode = 28; Timezone = "America/Maceio" }
      { Code = "TO"; Name = "Tocantins"; RegionCode = "N"; RegionName = "Norte"; IbgeCode = 17; Timezone = "America/Araguaina" } ]

let private normalizarNome (nome: string) : string =
    if isNull nome then
        ""
    else
        nome
        |> Text.RemoveAccents
        |> fun s -> Regex.Replace(s.Trim(), @"\s+", " ")
        |> fun s -> s.ToUpperInvariant()

/// Retorna o estado cujo código IBGE de 2 dígitos (cUF) corresponde ao valor
/// informado, ou None quando nenhum corresponder.
let GetByIbgeCode (code: obj) : StateInfo option =
    let codigo =
        match code with
        | :? string as s ->
            match Int32.TryParse(s.Trim()) with
            | true, n -> Some n
            | false, _ -> None
        | :? int as i -> Some i
        | _ -> None
    match codigo with
    | None -> None
    | Some n -> estados |> List.tryFind (fun e -> e.IbgeCode = n)

/// Retorna a sigla de um estado a partir do nome completo, ignorando
/// acentos, maiúsculas/minúsculas e espaços nas pontas.
let GetCodeByName (name: string) : string option =
    if String.IsNullOrWhiteSpace name then
        None
    else
        let alvo = normalizarNome name
        estados
        |> List.tryFind (fun e -> normalizarNome e.Name = alvo)
        |> Option.map (fun e -> e.Code)

/// Retorna o nome completo de um estado a partir da sigla, ignorando
/// maiúsculas/minúsculas e espaços nas pontas.
let GetNameByCode (code: string) : string option =
    if String.IsNullOrWhiteSpace code then
        None
    else
        let alvo = code.Trim().ToUpperInvariant()
        estados
        |> List.tryFind (fun e -> e.Code = alvo)
        |> Option.map (fun e -> e.Name)

/// Retorna o fuso horário IANA (tzdata) da capital de um estado, a partir da
/// sigla, ignorando maiúsculas/minúsculas e espaços nas pontas.
let GetTimezone (stateCode: string) : string option =
    if String.IsNullOrWhiteSpace stateCode then
        None
    else
        let alvo = stateCode.Trim().ToUpperInvariant()
        estados
        |> List.tryFind (fun e -> e.Code = alvo)
        |> Option.map (fun e -> e.Timezone)

/// Retorna as 27 unidades federativas brasileiras, ordenadas pelo nome.
let List () : StateInfo list = estados
