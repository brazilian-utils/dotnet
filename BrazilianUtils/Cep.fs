module BrazilianUtils.Cep

open System
open System.Net.Http
open System.Text.Json
open System.Text

let private temTamanhoCep =
    let tamanhoCep = 8
    Helpers.hasLength tamanhoCep

let IsValid cep =
    let valorLimpo = Helpers.stringToIntList cep
    temTamanhoCep valorLimpo

let Format cep =
    let valorLimpo = Helpers.OnlyNumbers cep
    let sb = StringBuilder(valorLimpo)
    if sb.Length >= 5 then sb.Insert(5, "-") |> ignore
    sb.ToString()

/// Remove os símbolos de formatação (mantém o restante).
///
/// Examples:
///     RemoveSymbols "91906-292" = "91906292"
let RemoveSymbols (cep: string) : string =
    cep.Replace("-", "")

/// Remove a formatação do CEP e mantém apenas dígitos, limitado a 8 dígitos.
///
/// Examples:
///     Parse "01001-000" = "01001000"
let Parse (value: string) : string =
    let apenasDigitos = Helpers.OnlyNumbers value
    if apenasDigitos.Length > 8 then apenasDigitos.Substring(0, 8) else apenasDigitos

/// Gera um CEP aleatório (8 dígitos, sem formatação).
/// Um CEP não possui dígito verificador, então toda sequência de 8 dígitos é
/// estruturalmente válida.
let Generate () : string =
    Helpers.generateRandomNumbers 8
    |> List.map string
    |> String.concat ""

/// Informações de endereço associadas a um CEP.
type AddressInfo =
    { Cep: string
      State: string
      City: string
      Neighborhood: string
      Street: string }

/// Parâmetros para busca de CEPs por endereço.
type GetInfoByAddressParams =
    { State: string
      City: string
      Street: string }

let private clienteHttp = new HttpClient()

let private obterTextoJson (elemento: JsonElement) (propriedade: string) : string =
    match elemento.TryGetProperty(propriedade) with
    | true, valor when valor.ValueKind = JsonValueKind.String -> valor.GetString()
    | _ -> ""

/// Busca as informações de endereço de um CEP na API do ViaCEP (chamada de rede).
///
/// Lança uma exceção quando o CEP é inválido, quando o CEP não é encontrado, ou
/// quando o serviço falha.
let GetAddressInfo (cep: string) : AddressInfo =
    let cepLimpo = Helpers.OnlyNumbers cep
    if cepLimpo.Length <> 8 then
        invalidArg "cep" "CEP inválido"
    else
        let url = sprintf "https://viacep.com.br/ws/%s/json/" cepLimpo
        let resposta = clienteHttp.GetStringAsync(url).GetAwaiter().GetResult()
        use documento = JsonDocument.Parse(resposta)
        let raiz = documento.RootElement
        let temErro =
            match raiz.TryGetProperty("erro") with
            | true, valor -> valor.ValueKind = JsonValueKind.True
            | _ -> false
        if temErro then
            failwithf "CEP %s não encontrado" cepLimpo
        else
            { Cep = obterTextoJson raiz "cep"
              State = obterTextoJson raiz "uf"
              City = obterTextoJson raiz "localidade"
              Neighborhood = obterTextoJson raiz "bairro"
              Street = obterTextoJson raiz "logradouro" }

/// Busca todos os CEPs de uma rua brasileira na API do ViaCEP (chamada de rede).
///
/// Lança uma exceção quando o estado, a cidade ou a rua estão ausentes ou são
/// inválidos, ou quando o serviço falha.
let GetInfoByAddress (parametros: GetInfoByAddressParams) : AddressInfo list =
    if String.IsNullOrWhiteSpace parametros.State
       || String.IsNullOrWhiteSpace parametros.City
       || String.IsNullOrWhiteSpace parametros.Street then
        invalidArg "parametros" "Estado, cidade e rua são obrigatórios"
    else
        let uf = parametros.State.Trim()
        let cidade = Uri.EscapeDataString(parametros.City.Trim())
        let logradouro = Uri.EscapeDataString(parametros.Street.Trim())
        let url = sprintf "https://viacep.com.br/ws/%s/%s/%s/json/" uf cidade logradouro
        let resposta = clienteHttp.GetStringAsync(url).GetAwaiter().GetResult()
        use documento = JsonDocument.Parse(resposta)
        documento.RootElement.EnumerateArray()
        |> Seq.map (fun elemento ->
            { Cep = obterTextoJson elemento "cep"
              State = obterTextoJson elemento "uf"
              City = obterTextoJson elemento "localidade"
              Neighborhood = obterTextoJson elemento "bairro"
              Street = obterTextoJson elemento "logradouro" })
        |> Seq.toList
