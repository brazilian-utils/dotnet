module BrazilianUtils.AreaCode

open System

type AreaCodeInfo =
    { AreaCode: int
      StateCode: string
      StateName: string
      RegionCode: string
      RegionName: string
      StateCodes: string list }

/// Os 67 DDDs em uso no Plano Geral de Numeração da Anatel. Os 4 DDDs que
/// cruzam fronteira estadual (61, 42, 47, 49) listam o estado-sede primeiro.
let private codigosDeArea =
    [ { AreaCode = 11; StateCode = "SP"; StateName = "São Paulo"; RegionCode = "SE"; RegionName = "Sudeste"; StateCodes = [ "SP" ] }
      { AreaCode = 12; StateCode = "SP"; StateName = "São Paulo"; RegionCode = "SE"; RegionName = "Sudeste"; StateCodes = [ "SP" ] }
      { AreaCode = 13; StateCode = "SP"; StateName = "São Paulo"; RegionCode = "SE"; RegionName = "Sudeste"; StateCodes = [ "SP" ] }
      { AreaCode = 14; StateCode = "SP"; StateName = "São Paulo"; RegionCode = "SE"; RegionName = "Sudeste"; StateCodes = [ "SP" ] }
      { AreaCode = 15; StateCode = "SP"; StateName = "São Paulo"; RegionCode = "SE"; RegionName = "Sudeste"; StateCodes = [ "SP" ] }
      { AreaCode = 16; StateCode = "SP"; StateName = "São Paulo"; RegionCode = "SE"; RegionName = "Sudeste"; StateCodes = [ "SP" ] }
      { AreaCode = 17; StateCode = "SP"; StateName = "São Paulo"; RegionCode = "SE"; RegionName = "Sudeste"; StateCodes = [ "SP" ] }
      { AreaCode = 18; StateCode = "SP"; StateName = "São Paulo"; RegionCode = "SE"; RegionName = "Sudeste"; StateCodes = [ "SP" ] }
      { AreaCode = 19; StateCode = "SP"; StateName = "São Paulo"; RegionCode = "SE"; RegionName = "Sudeste"; StateCodes = [ "SP" ] }
      { AreaCode = 21; StateCode = "RJ"; StateName = "Rio de Janeiro"; RegionCode = "SE"; RegionName = "Sudeste"; StateCodes = [ "RJ" ] }
      { AreaCode = 22; StateCode = "RJ"; StateName = "Rio de Janeiro"; RegionCode = "SE"; RegionName = "Sudeste"; StateCodes = [ "RJ" ] }
      { AreaCode = 24; StateCode = "RJ"; StateName = "Rio de Janeiro"; RegionCode = "SE"; RegionName = "Sudeste"; StateCodes = [ "RJ" ] }
      { AreaCode = 27; StateCode = "ES"; StateName = "Espírito Santo"; RegionCode = "SE"; RegionName = "Sudeste"; StateCodes = [ "ES" ] }
      { AreaCode = 28; StateCode = "ES"; StateName = "Espírito Santo"; RegionCode = "SE"; RegionName = "Sudeste"; StateCodes = [ "ES" ] }
      { AreaCode = 31; StateCode = "MG"; StateName = "Minas Gerais"; RegionCode = "SE"; RegionName = "Sudeste"; StateCodes = [ "MG" ] }
      { AreaCode = 32; StateCode = "MG"; StateName = "Minas Gerais"; RegionCode = "SE"; RegionName = "Sudeste"; StateCodes = [ "MG" ] }
      { AreaCode = 33; StateCode = "MG"; StateName = "Minas Gerais"; RegionCode = "SE"; RegionName = "Sudeste"; StateCodes = [ "MG" ] }
      { AreaCode = 34; StateCode = "MG"; StateName = "Minas Gerais"; RegionCode = "SE"; RegionName = "Sudeste"; StateCodes = [ "MG" ] }
      { AreaCode = 35; StateCode = "MG"; StateName = "Minas Gerais"; RegionCode = "SE"; RegionName = "Sudeste"; StateCodes = [ "MG" ] }
      { AreaCode = 37; StateCode = "MG"; StateName = "Minas Gerais"; RegionCode = "SE"; RegionName = "Sudeste"; StateCodes = [ "MG" ] }
      { AreaCode = 38; StateCode = "MG"; StateName = "Minas Gerais"; RegionCode = "SE"; RegionName = "Sudeste"; StateCodes = [ "MG" ] }
      { AreaCode = 41; StateCode = "PR"; StateName = "Paraná"; RegionCode = "S"; RegionName = "Sul"; StateCodes = [ "PR" ] }
      { AreaCode = 42; StateCode = "PR"; StateName = "Paraná"; RegionCode = "S"; RegionName = "Sul"; StateCodes = [ "PR"; "SC" ] }
      { AreaCode = 43; StateCode = "PR"; StateName = "Paraná"; RegionCode = "S"; RegionName = "Sul"; StateCodes = [ "PR" ] }
      { AreaCode = 44; StateCode = "PR"; StateName = "Paraná"; RegionCode = "S"; RegionName = "Sul"; StateCodes = [ "PR" ] }
      { AreaCode = 45; StateCode = "PR"; StateName = "Paraná"; RegionCode = "S"; RegionName = "Sul"; StateCodes = [ "PR" ] }
      { AreaCode = 46; StateCode = "PR"; StateName = "Paraná"; RegionCode = "S"; RegionName = "Sul"; StateCodes = [ "PR" ] }
      { AreaCode = 47; StateCode = "SC"; StateName = "Santa Catarina"; RegionCode = "S"; RegionName = "Sul"; StateCodes = [ "SC"; "PR" ] }
      { AreaCode = 48; StateCode = "SC"; StateName = "Santa Catarina"; RegionCode = "S"; RegionName = "Sul"; StateCodes = [ "SC" ] }
      { AreaCode = 49; StateCode = "SC"; StateName = "Santa Catarina"; RegionCode = "S"; RegionName = "Sul"; StateCodes = [ "SC"; "PR" ] }
      { AreaCode = 51; StateCode = "RS"; StateName = "Rio Grande do Sul"; RegionCode = "S"; RegionName = "Sul"; StateCodes = [ "RS" ] }
      { AreaCode = 53; StateCode = "RS"; StateName = "Rio Grande do Sul"; RegionCode = "S"; RegionName = "Sul"; StateCodes = [ "RS" ] }
      { AreaCode = 54; StateCode = "RS"; StateName = "Rio Grande do Sul"; RegionCode = "S"; RegionName = "Sul"; StateCodes = [ "RS" ] }
      { AreaCode = 55; StateCode = "RS"; StateName = "Rio Grande do Sul"; RegionCode = "S"; RegionName = "Sul"; StateCodes = [ "RS" ] }
      { AreaCode = 61; StateCode = "DF"; StateName = "Distrito Federal"; RegionCode = "CO"; RegionName = "Centro-Oeste"; StateCodes = [ "DF"; "GO" ] }
      { AreaCode = 62; StateCode = "GO"; StateName = "Goiás"; RegionCode = "CO"; RegionName = "Centro-Oeste"; StateCodes = [ "GO" ] }
      { AreaCode = 63; StateCode = "TO"; StateName = "Tocantins"; RegionCode = "N"; RegionName = "Norte"; StateCodes = [ "TO" ] }
      { AreaCode = 64; StateCode = "GO"; StateName = "Goiás"; RegionCode = "CO"; RegionName = "Centro-Oeste"; StateCodes = [ "GO" ] }
      { AreaCode = 65; StateCode = "MT"; StateName = "Mato Grosso"; RegionCode = "CO"; RegionName = "Centro-Oeste"; StateCodes = [ "MT" ] }
      { AreaCode = 66; StateCode = "MT"; StateName = "Mato Grosso"; RegionCode = "CO"; RegionName = "Centro-Oeste"; StateCodes = [ "MT" ] }
      { AreaCode = 67; StateCode = "MS"; StateName = "Mato Grosso do Sul"; RegionCode = "CO"; RegionName = "Centro-Oeste"; StateCodes = [ "MS" ] }
      { AreaCode = 68; StateCode = "AC"; StateName = "Acre"; RegionCode = "N"; RegionName = "Norte"; StateCodes = [ "AC" ] }
      { AreaCode = 69; StateCode = "RO"; StateName = "Rondônia"; RegionCode = "N"; RegionName = "Norte"; StateCodes = [ "RO" ] }
      { AreaCode = 71; StateCode = "BA"; StateName = "Bahia"; RegionCode = "NE"; RegionName = "Nordeste"; StateCodes = [ "BA" ] }
      { AreaCode = 73; StateCode = "BA"; StateName = "Bahia"; RegionCode = "NE"; RegionName = "Nordeste"; StateCodes = [ "BA" ] }
      { AreaCode = 74; StateCode = "BA"; StateName = "Bahia"; RegionCode = "NE"; RegionName = "Nordeste"; StateCodes = [ "BA" ] }
      { AreaCode = 75; StateCode = "BA"; StateName = "Bahia"; RegionCode = "NE"; RegionName = "Nordeste"; StateCodes = [ "BA" ] }
      { AreaCode = 77; StateCode = "BA"; StateName = "Bahia"; RegionCode = "NE"; RegionName = "Nordeste"; StateCodes = [ "BA" ] }
      { AreaCode = 79; StateCode = "SE"; StateName = "Sergipe"; RegionCode = "NE"; RegionName = "Nordeste"; StateCodes = [ "SE" ] }
      { AreaCode = 81; StateCode = "PE"; StateName = "Pernambuco"; RegionCode = "NE"; RegionName = "Nordeste"; StateCodes = [ "PE" ] }
      { AreaCode = 82; StateCode = "AL"; StateName = "Alagoas"; RegionCode = "NE"; RegionName = "Nordeste"; StateCodes = [ "AL" ] }
      { AreaCode = 83; StateCode = "PB"; StateName = "Paraíba"; RegionCode = "NE"; RegionName = "Nordeste"; StateCodes = [ "PB" ] }
      { AreaCode = 84; StateCode = "RN"; StateName = "Rio Grande do Norte"; RegionCode = "NE"; RegionName = "Nordeste"; StateCodes = [ "RN" ] }
      { AreaCode = 85; StateCode = "CE"; StateName = "Ceará"; RegionCode = "NE"; RegionName = "Nordeste"; StateCodes = [ "CE" ] }
      { AreaCode = 86; StateCode = "PI"; StateName = "Piauí"; RegionCode = "NE"; RegionName = "Nordeste"; StateCodes = [ "PI" ] }
      { AreaCode = 87; StateCode = "PE"; StateName = "Pernambuco"; RegionCode = "NE"; RegionName = "Nordeste"; StateCodes = [ "PE" ] }
      { AreaCode = 88; StateCode = "CE"; StateName = "Ceará"; RegionCode = "NE"; RegionName = "Nordeste"; StateCodes = [ "CE" ] }
      { AreaCode = 89; StateCode = "PI"; StateName = "Piauí"; RegionCode = "NE"; RegionName = "Nordeste"; StateCodes = [ "PI" ] }
      { AreaCode = 91; StateCode = "PA"; StateName = "Pará"; RegionCode = "N"; RegionName = "Norte"; StateCodes = [ "PA" ] }
      { AreaCode = 92; StateCode = "AM"; StateName = "Amazonas"; RegionCode = "N"; RegionName = "Norte"; StateCodes = [ "AM" ] }
      { AreaCode = 93; StateCode = "PA"; StateName = "Pará"; RegionCode = "N"; RegionName = "Norte"; StateCodes = [ "PA" ] }
      { AreaCode = 94; StateCode = "PA"; StateName = "Pará"; RegionCode = "N"; RegionName = "Norte"; StateCodes = [ "PA" ] }
      { AreaCode = 95; StateCode = "RR"; StateName = "Roraima"; RegionCode = "N"; RegionName = "Norte"; StateCodes = [ "RR" ] }
      { AreaCode = 96; StateCode = "AP"; StateName = "Amapá"; RegionCode = "N"; RegionName = "Norte"; StateCodes = [ "AP" ] }
      { AreaCode = 97; StateCode = "AM"; StateName = "Amazonas"; RegionCode = "N"; RegionName = "Norte"; StateCodes = [ "AM" ] }
      { AreaCode = 98; StateCode = "MA"; StateName = "Maranhão"; RegionCode = "NE"; RegionName = "Nordeste"; StateCodes = [ "MA" ] }
      { AreaCode = 99; StateCode = "MA"; StateName = "Maranhão"; RegionCode = "NE"; RegionName = "Nordeste"; StateCodes = [ "MA" ] } ]

/// Retorna o estado (e a região) a que pertence um DDD brasileiro, ou None
/// quando o DDD não estiver em uso.
let GetInfo (areaCode: obj) : AreaCodeInfo option =
    let codigo =
        match areaCode with
        | :? string as s ->
            match Int32.TryParse(s.Trim()) with
            | true, n -> Some n
            | false, _ -> None
        | :? int as i -> Some i
        | _ -> None
    match codigo with
    | None -> None
    | Some n -> codigosDeArea |> List.tryFind (fun c -> c.AreaCode = n)

/// Retorna todos os DDDs que atendem a um estado brasileiro, em ordem
/// crescente.
let ListByState (stateCode: string) : int list =
    if String.IsNullOrWhiteSpace stateCode then
        []
    else
        let alvo = stateCode.Trim().ToUpperInvariant()
        codigosDeArea
        |> List.filter (fun c -> c.StateCodes |> List.contains alvo)
        |> List.map (fun c -> c.AreaCode)
        |> List.sort
