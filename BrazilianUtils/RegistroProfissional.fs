module BrazilianUtils.RegistroProfissional

open System.Text.RegularExpressions

/// Parâmetros para a validação estrutural de um registro/inscrição
/// profissional (OAB, CRM, CRO, CRP ou CRC).
type IsValidRegistroProfissionalParams =
    { Value: string
      Council: string
      State: string option }

let private ufsValidas =
    set [ "AC"; "AL"; "AP"; "AM"; "BA"; "CE"; "DF"; "ES"; "GO"; "MA"; "MT"; "MS"; "MG"
          "PA"; "PB"; "PR"; "PE"; "PI"; "RJ"; "RN"; "RS"; "RO"; "RR"; "SC"; "SP"; "SE"; "TO" ]

let private estadoConfere (esperado: string option) (uf: string) : bool =
    match esperado with
    | None -> true
    | Some estado -> estado.Trim().ToUpperInvariant() = uf

/// Verifica apenas a estrutura de um número de registro profissional
/// (quantidade de dígitos e UF), nunca um dígito verificador.
///
/// - OAB e CRM: 4 a 6 dígitos e a UF (`123456/SP` ou `123456-SP`).
/// - CRO: 3 a 6 dígitos e a UF.
/// - CRP: código regional de 2 dígitos (01 a 24) e 4 a 6 dígitos
///   (`06/12345`); o estado esperado é ignorado.
/// - CRC: UF, 6 dígitos, tipo de inscrição (`O` ou `P`) e um dígito
///   (`SP-123456/O-3`), opcionalmente um sufixo de transferência
///   (`T-MG` ou `S-MG`); o estado esperado deve casar com a UF de origem.
let IsValid (parametros: IsValidRegistroProfissionalParams) : bool =
    let valor = if isNull parametros.Value then "" else parametros.Value.Trim()
    let conselho = if isNull parametros.Council then "" else parametros.Council.Trim().ToUpperInvariant()
    match conselho with
    | "OAB" | "CRM" ->
        let m = Regex.Match(valor, @"^(\d{4,6})[/-]([A-Za-z]{2})$")
        m.Success
        && ufsValidas.Contains(m.Groups.[2].Value.ToUpperInvariant())
        && estadoConfere parametros.State (m.Groups.[2].Value.ToUpperInvariant())
    | "CRO" ->
        let m = Regex.Match(valor, @"^(\d{3,6})[/-]([A-Za-z]{2})$")
        m.Success
        && ufsValidas.Contains(m.Groups.[2].Value.ToUpperInvariant())
        && estadoConfere parametros.State (m.Groups.[2].Value.ToUpperInvariant())
    | "CRP" ->
        let m = Regex.Match(valor, @"^(\d{2})[/-](\d{4,6})$")
        if not m.Success then
            false
        else
            let regional = int m.Groups.[1].Value
            regional >= 1 && regional <= 24
    | "CRC" ->
        let m = Regex.Match(valor, @"^([A-Za-z]{2})-(\d{6})/([OPop])-(\d)(?:/[TSts]-([A-Za-z]{2}))?$")
        m.Success
        && ufsValidas.Contains(m.Groups.[1].Value.ToUpperInvariant())
        && estadoConfere parametros.State (m.Groups.[1].Value.ToUpperInvariant())
    | _ -> false
