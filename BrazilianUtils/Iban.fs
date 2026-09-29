module BrazilianUtils.Iban

open System
open System.Text

type IbanInfo =
    { CountryCode: string
      CheckDigits: string
      BankIspb: string
      Branch: string
      Account: string
      AccountType: string
      Owner: string }

let private tamanho = 29

/// Remove a formatação do IBAN, mantém letras e dígitos em maiúsculo,
/// limitado a 29 caracteres.
let Parse (value: string) : string =
    if isNull value then
        ""
    else
        let apenasAlfanumerico =
            value.ToUpperInvariant() |> String.filter Char.IsLetterOrDigit
        if apenasAlfanumerico.Length > tamanho then apenasAlfanumerico.Substring(0, tamanho) else apenasAlfanumerico

/// Formata um IBAN no agrupamento de impressão ISO 13616: blocos de 4
/// caracteres separados por espaço, em maiúsculo. Não valida (use `IsValid`).
let Format (value: string) : string =
    let compacto = Parse value
    if compacto = "" then
        ""
    else
        [ for i in 0 .. 4 .. compacto.Length - 1 -> compacto.Substring(i, min 4 (compacto.Length - i)) ]
        |> String.concat " "

/// Interpreta um IBAN aceitando a forma compacta ou agrupada em blocos de 4
/// por um único separador (espaço, '.', '-' ou '/'); um separador dentro de
/// um grupo invalida o valor.
let private normalizarAgrupado (value: string) : string option =
    if isNull value || value = "" then
        None
    else
        let temSeparador = value |> Seq.exists (fun c -> c = ' ' || c = '.' || c = '-' || c = '/')
        if not temSeparador then
            if value |> Seq.forall Char.IsLetterOrDigit then Some value else None
        else
            let partes = value.Split([| ' '; '.'; '-'; '/' |], StringSplitOptions.RemoveEmptyEntries)
            if partes.Length = 0 then
                None
            else
                let todasAlfanumericas = partes |> Array.forall (fun p -> p |> Seq.forall Char.IsLetterOrDigit)
                let gruposValidos = partes.[0 .. partes.Length - 2] |> Array.forall (fun p -> p.Length = 4)
                let ultima = partes.[partes.Length - 1]
                let ultimaValida = ultima.Length >= 1 && ultima.Length <= 4
                if todasAlfanumericas && gruposValidos && ultimaValida then
                    Some (String.Concat(partes))
                else
                    None

/// Verifica o dígito de controle ISO 7064 MOD 97-10 de um IBAN já
/// normalizado (compacto, maiúsculo, 29 caracteres, começando com "BR").
let private modulo97Valido (compactoMaiusculo: string) : bool =
    let rearranjado = compactoMaiusculo.Substring(4) + compactoMaiusculo.Substring(0, 4)
    let numerico = StringBuilder()
    let mutable valido = true
    for c in rearranjado do
        if Char.IsDigit c then
            numerico.Append(c) |> ignore
        elif c >= 'A' && c <= 'Z' then
            numerico.Append(string (int c - int 'A' + 10)) |> ignore
        else
            valido <- false
    if not valido then
        false
    else
        let mutable resto = 0
        for c in numerico.ToString() do
            resto <- (resto * 10 + (int c - int '0')) % 97
        resto = 1

/// Valida um IBAN brasileiro: `BR` + 2 dígitos verificadores (ISO 7064 MOD
/// 97-10) + ISPB de 8 dígitos + agência de 5 dígitos + conta de 10 dígitos +
/// 1 letra de tipo de conta + 1 indicador de titular.
let IsValid (value: string) : bool =
    match normalizarAgrupado value with
    | None -> false
    | Some compacto ->
        let maiusculo = compacto.ToUpperInvariant()
        maiusculo.Length = tamanho && maiusculo.StartsWith("BR") && modulo97Valido maiusculo

/// Interpreta um IBAN brasileiro em seus campos; devolve None sempre que
/// `IsValid` devolveria falso.
let GetInfo (value: string) : IbanInfo option =
    if not (IsValid value) then
        None
    else
        match normalizarAgrupado value with
        | None -> None
        | Some compacto ->
            let maiusculo = compacto.ToUpperInvariant()
            Some
                { CountryCode = maiusculo.Substring(0, 2)
                  CheckDigits = maiusculo.Substring(2, 2)
                  BankIspb = maiusculo.Substring(4, 8)
                  Branch = maiusculo.Substring(12, 5)
                  Account = maiusculo.Substring(17, 10)
                  AccountType = maiusculo.Substring(27, 1)
                  Owner = maiusculo.Substring(28, 1) }
