module BrazilianUtils.LicensePlate

open System
open System.Text.RegularExpressions

// FUNÇÕES PRIVADAS DE VALIDAÇÃO
// =============================

/// Verifica se uma string corresponde ao formato antigo de placa brasileira.
/// Padrão: 'LLLNNNN'
let private isValidOldFormat (licensePlate: string) : bool =
    if String.IsNullOrEmpty(licensePlate) then
        false
    else
        let semEspacos = licensePlate.Trim()
        let padrao = @"^[A-Za-z]{3}[0-9]{4}$"
        Regex.IsMatch(semEspacos, padrao)

/// Verifica se uma string corresponde ao formato Mercosul de placa brasileira.
/// Padrão: 'LLLNLNN'
let private isValidMercosul (licensePlate: string) : bool =
    if String.IsNullOrEmpty(licensePlate) then
        false
    else
        let maiusculo = licensePlate.Trim().ToUpper()
        let padrao = @"^[A-Z]{3}\d[A-Z]\d{2}$"
        Regex.IsMatch(maiusculo, padrao)

/// Remove hífen e espaços internos e converte para maiúsculo, para as
/// funções que toleram máscara (convertToMercosul e isValid, conforme o contrato).
let private normalizar (value: string) : string =
    if isNull value then ""
    else value.Replace("-", "").Replace(" ", "").Trim().ToUpper()

// FORMATAÇÃO
// ==========

/// Converte uma placa no padrão antigo (LLLNNNN) para o formato Mercosul (LLLNLNN).
/// Aceita a máscara com hífen. Retorna uma string vazia quando a conversão não é
/// possível (placa já é Mercosul, é inválida, ou está vazia).
///
/// Examples:
///     convertToMercosul "ABC4567" = "ABC4F67"
///     convertToMercosul "ABC-1234" = "ABC1C34"
///     convertToMercosul "ABC4*67" = ""
let convertToMercosul (licensePlate: string) : string =
    if isNull licensePlate then
        ""
    else
        let semTraco = licensePlate.Replace("-", "")
        if not (isValidOldFormat semTraco) then
            ""
        else
            let letrasEDigitos = semTraco.Trim().ToUpper() |> Seq.toArray
            // Converte o 5º caractere (índice 4) de dígito para letra
            letrasEDigitos.[4] <- char (int 'A' + (int letrasEDigitos.[4] - int '0'))
            String(letrasEDigitos)

/// Formata uma placa no padrão correto.
/// Recebe uma placa em qualquer padrão (LLLNNNN ou LLLNLNN) e devolve a versão formatada.
///
/// Examples:
///     formatLicensePlate "ABC1234" = Some "ABC-1234"  // formato antigo (ganha hífen)
///     formatLicensePlate "abc1e34" = Some "ABC1E34"   // formato Mercosul
///     formatLicensePlate "ABC123" = None
let formatLicensePlate (licensePlate: string) : string option =
    let maiusculo = licensePlate.ToUpper()

    if isValidOldFormat licensePlate then
        Some (maiusculo.Substring(0, 3) + "-" + maiusculo.Substring(3))
    elif isValidMercosul licensePlate then
        Some maiusculo
    else
        None

// OPERAÇÕES
// ==========

/// Remove o símbolo de hífen (-) de uma placa.
///
/// Examples:
///     removeSymbols "ABC-123" = "ABC123"
///     removeSymbols "abc123" = "abc123"
let removeSymbols (licensePlateNumber: string) : string =
    licensePlateNumber.Replace("-", "")

/// Devolve o formato de uma placa brasileira. 'LLLNNNN' para o padrão antigo e
/// 'LLLNLNN' para o Mercosul.
///
/// Examples:
///     getFormat "abc1234" = Some "LLLNNNN"
///     getFormat "abc1d23" = Some "LLLNLNN"
///     getFormat "ABCD123" = None
let getFormat (licensePlate: string) : string option =
    if isValidOldFormat licensePlate then
        Some "LLLNNNN"
    elif isValidMercosul licensePlate then
        Some "LLLNLNN"
    else
        None

/// Retorna se um número de placa brasileira é válido (formato antigo ou Mercosul).
/// Não verifica se a placa realmente existe.
///
/// Tolera a máscara com hífen ou espaço e ignora maiúsculas/minúsculas.
///
/// Args:
///     licensePlate: o número da placa a ser validado.
let isValid (licensePlate: string) : bool =
    if String.IsNullOrEmpty licensePlate then
        false
    else
        let normalizado = normalizar licensePlate
        isValidOldFormat normalizado || isValidMercosul normalizado

/// Remove a formatação da placa e devolve apenas letras e dígitos maiúsculos,
/// limitado a 7 caracteres.
///
/// Examples:
///     parse "abc-1234" = "ABC1234"
///     parse "abc123456" = "ABC1234"
let parse (value: string) : string =
    if isNull value then
        ""
    else
        let apenasAlfanumerico =
            value.ToUpper()
            |> String.filter Char.IsLetterOrDigit
        if apenasAlfanumerico.Length > 7 then apenasAlfanumerico.Substring(0, 7) else apenasAlfanumerico

/// Gera uma placa válida no formato informado. Caso nenhum formato seja informado,
/// devolve uma placa no formato Mercosul.
///
/// Args:
///     format: o formato desejado para a placa.
///             'LLLNLNN' para o padrão Mercosul ou 'LLLNNNN' para o antigo.
///             O padrão é 'LLLNLNN'.
///
/// Examples:
///     generate None = "ABC1D23"
///     generate (Some "LLLNLNN") = "ABC4D56"
///     generate (Some "LLLNNNN") = "ABC1234"
///     generate (Some "invalid") = None
let generate (format: string option) : string option =
    let random = Random()
    let selectedFormat = defaultArg format "LLLNLNN"
    let upperFormat = selectedFormat.ToUpper()

    if upperFormat <> "LLLNLNN" && upperFormat <> "LLLNNNN" then
        None
    else
        let generated =
            upperFormat
            |> Seq.map (fun c ->
                if c = 'L' then
                    char (random.Next(26) + int 'A')
                else
                    char (random.Next(10) + int '0')
            )
            |> Seq.toArray
            |> String

        Some generated
