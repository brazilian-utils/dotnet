module BrazilianUtils.PixPayload

open System
open System.Globalization

/// Campos de um payload de BR Code (Pix "copia e cola").
type PixPayloadInfo =
    { Key: string option
      Url: string option
      MerchantName: string
      MerchantCity: string
      PointOfInitiation: string
      Amount: decimal option
      Txid: string option
      WithdrawalFacilitator: string option }

/// Parâmetros para gerar um payload de BR Code.
type GeneratePixPayloadParams =
    { Key: string option
      Url: string option
      MerchantName: string
      MerchantCity: string
      Amount: decimal option
      Txid: string option
      Description: string option }

/// Interpreta uma sequência TLV (tag-length-value) do padrão EMV/BR Code em
/// uma lista de pares (id, valor), no nível em que for chamada.
let rec private analisarTlv (texto: string) : (string * string) list =
    if texto.Length < 4 then
        []
    else
        let id = texto.Substring(0, 2)
        match Int32.TryParse(texto.Substring(2, 2)) with
        | false, _ -> []
        | true, tamanho ->
            if texto.Length < 4 + tamanho then
                []
            else
                let valor = texto.Substring(4, tamanho)
                let resto = texto.Substring(4 + tamanho)
                (id, valor) :: analisarTlv resto

/// Calcula o CRC-16/CCITT-FALSE (polinômio 0x1021, valor inicial 0xFFFF)
/// usado para validar o BR Code.
let private crc16 (texto: string) : uint16 =
    let mutable crc = 0xFFFFus
    for c in texto do
        crc <- crc ^^^ (uint16 (int c) <<< 8)
        for _ in 1 .. 8 do
            if crc &&& 0x8000us <> 0us then
                crc <- (crc <<< 1) ^^^ 0x1021us
            else
                crc <- crc <<< 1
    crc

let private buscar (lista: (string * string) list) (id: string) : string option =
    lista |> List.tryFind (fun (i, _) -> i = id) |> Option.map snd

/// Extrai os campos de um payload de BR Code, sem verificar o CRC.
let private extrairCampos (payload: string) (nivelSuperior: (string * string) list) : PixPayloadInfo option =
    let categoria = buscar nivelSuperior "52"
    let moeda = buscar nivelSuperior "53"
    let pais = buscar nivelSuperior "58"
    let nomeComerciante = buscar nivelSuperior "59"
    let cidadeComerciante = buscar nivelSuperior "60"

    match categoria, moeda, pais, nomeComerciante, cidadeComerciante with
    | Some _, Some _, Some "BR", Some nome, Some cidade ->
        // Localiza o template de Merchant Account Information (26 a 51) que
        // carrega o GUI "br.gov.bcb.pix".
        let templatePix =
            [ 26 .. 51 ]
            |> List.tryPick (fun idNumerico ->
                let idTexto = idNumerico.ToString("D2")
                match buscar nivelSuperior idTexto with
                | None -> None
                | Some conteudo ->
                    let subCampos = analisarTlv conteudo
                    match buscar subCampos "00" with
                    | Some "br.gov.bcb.pix" -> Some subCampos
                    | _ -> None)

        match templatePix with
        | None -> None
        | Some subCampos ->
            let chave = buscar subCampos "01"
            let url = buscar subCampos "25"

            let indicadorInicio = buscar nivelSuperior "01"
            let pontoDeIniciacao =
                if indicadorInicio = Some "12" || url.IsSome then "dynamic" else "static"

            let valor =
                buscar nivelSuperior "54"
                |> Option.bind (fun texto ->
                    match Decimal.TryParse(texto, NumberStyles.Number, CultureInfo.InvariantCulture) with
                    | true, numero -> Some numero
                    | false, _ -> None)

            let camposAdicionais = buscar nivelSuperior "62" |> Option.map analisarTlv
            let txid =
                camposAdicionais
                |> Option.bind (fun campos -> buscar campos "05")
                |> Option.bind (fun t -> if t = "***" then None else Some t)
            let facilitadorSaque =
                camposAdicionais |> Option.bind (fun campos -> buscar campos "07")

            // Chave estática e URL dinâmica são mutuamente exclusivas: um
            // payload precisa ter exatamente uma das duas, nunca as duas
            // ausentes nem as duas presentes ao mesmo tempo.
            if chave.IsNone && url.IsNone then
                None
            elif chave.IsSome && url.IsSome then
                None
            else
                Some
                    { Key = chave
                      Url = url
                      MerchantName = nome
                      MerchantCity = cidade
                      PointOfInitiation = pontoDeIniciacao
                      Amount = valor
                      Txid = txid
                      WithdrawalFacilitator = facilitadorSaque }
    | _ -> None

/// Valida um payload de BR Code do Pix: estrutura TLV, CRC-16 e os campos
/// obrigatórios (indicador de formato, código de categoria, moeda, país,
/// nome e cidade do recebedor), e um template de Merchant Account
/// Information (IDs 26 a 51) com o GUI `br.gov.bcb.pix` e uma chave ou URL,
/// nunca as duas. Não valida a chave em si (use `PixKey.IsValid`).
let IsValid (value: string) : bool =
    if String.IsNullOrEmpty value || value.Length < 8 then
        false
    else
        let semCrc = value.Substring(0, value.Length - 4)
        let crcInformado = value.Substring(value.Length - 4)
        let crcCalculado = (crc16 semCrc).ToString("X4")
        if crcCalculado <> crcInformado.ToUpperInvariant() then
            false
        else
            let nivelSuperior = analisarTlv value
            match buscar nivelSuperior "00" with
            | Some "01" -> extrairCampos value nivelSuperior |> Option.isSome
            | _ -> false

/// Interpreta um payload de BR Code do Pix em seus campos; devolve None
/// sempre que `IsValid` devolveria falso, nunca um resultado parcial.
let GetInfo (value: string) : PixPayloadInfo option =
    if not (IsValid value) then None
    else extrairCampos value (analisarTlv value)
