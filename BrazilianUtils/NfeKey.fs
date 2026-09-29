module BrazilianUtils.NfeKey

open System
open System.Text.RegularExpressions
open Helpers

/// Informações extraídas de uma chave de acesso de DF-e.
type NfeKeyInfo =
    { StateCode: string
      Year: int
      Month: int
      TaxId: string
      Model: string
      Series: int
      Number: int
      EmissionType: int
      Code: string
      CheckDigit: int
      AuthorizationSite: int option }

let private tamanho = 44
let private modelosAceitos = set [ "55"; "65"; "57"; "58"; "67"; "64"; "63"; "66"; "62" ]
let private prefixosXml = [ "NFCom"; "NF3e"; "MDFe"; "BPe"; "CTe"; "NFe" ]

/// Remove os prefixos de `Id` do XML (`NFe`, `CTe`, `MDFe`, `BPe`, `NF3e`,
/// `NFCom`), se presentes no início do valor.
let private removerPrefixo (value: string) : string =
    let encontrado = prefixosXml |> List.tryFind (fun p -> value.StartsWith(p, StringComparison.OrdinalIgnoreCase))
    match encontrado with
    | Some prefixo -> value.Substring(prefixo.Length)
    | None -> value

/// Remove a formatação da chave de acesso e mantém apenas dígitos, limitado
/// a 44 dígitos.
let Parse (value: string) : string =
    if isNull value then
        ""
    else
        let semPrefixo = removerPrefixo value
        let apenasDigitos = OnlyNumbers semPrefixo
        if apenasDigitos.Length > tamanho then apenasDigitos.Substring(0, tamanho) else apenasDigitos

/// Formata uma chave de acesso de DF-e em grupos de 4 dígitos separados por
/// espaço, como o DANFE e os demais documentos auxiliares imprimem. Não
/// valida (use `IsValid`).
let Format (value: string) : string =
    let digitos = Parse value
    if digitos = "" then
        ""
    else
        [ for i in 0 .. 4 .. digitos.Length - 1 -> digitos.Substring(i, min 4 (digitos.Length - i)) ]
        |> String.concat " "

/// Modelos de documento aos quais a regra B03-10 do MOC (código numérico
/// `cNF` não pode ser repetido, sequencial ou igual ao número do documento)
/// se aplica: NF-e (55) e NFC-e (65).
let private modelosComRegraCodigoNumerico = set [ "55"; "65" ]

/// Verifica se todos os dígitos de uma sequência numérica são iguais entre
/// si (ex.: "11111111").
let private todosDigitosIguais (numero: string) : bool =
    numero |> Seq.forall (fun c -> c = numero.[0])

/// Verifica se uma sequência de dígitos é estritamente sequencial crescente,
/// com rollover de 9 para 0 (ex.: "01234567" ou "56789012").
let private sequencialCrescente (numero: string) : bool =
    numero
    |> Seq.mapi (fun i c -> (int c - int numero.[0] + 10) % 10 = i % 10)
    |> Seq.forall id

/// Verifica se uma sequência de dígitos é estritamente sequencial
/// decrescente, com rollover de 0 para 9 (ex.: "87654321" ou "76543210").
let private sequencialDecrescente (numero: string) : bool =
    numero
    |> Seq.mapi (fun i c -> (int numero.[0] - int c + 10) % 10 = i % 10)
    |> Seq.forall id

/// Regra B03-10 do MOC: o código numérico (`cNF`) não pode ser todo
/// repetido, sequencial crescente, sequencial decrescente, nem igual ao
/// número do documento (`nNF`) — só se aplica a NF-e (55) e NFC-e (65).
/// A igualdade com `nNF` é comparada pelo valor numérico (os campos têm
/// tamanhos diferentes: 8 dígitos para `cNF`, 9 para `nNF`).
let private codigoNumericoValido (modelo: string) (codigoNumerico: string) (numeroDocumento: string) : bool =
    if not (modelosComRegraCodigoNumerico.Contains modelo) then
        true
    else
        not (todosDigitosIguais codigoNumerico)
        && not (sequencialCrescente codigoNumerico)
        && not (sequencialDecrescente codigoNumerico)
        && int codigoNumerico <> int numeroDocumento

/// Calcula o dígito verificador (módulo 11) sobre os 43 primeiros dígitos.
let private calcularDigitoVerificador (base43: string) : int =
    let mutable soma = 0
    let mutable peso = 2
    for i in (base43.Length - 1) .. -1 .. 0 do
        soma <- soma + (int base43.[i] - int '0') * peso
        peso <- if peso = 9 then 2 else peso + 1
    let resto = soma % 11
    if resto < 2 then 0 else 11 - resto

/// Extrai os campos de uma chave de acesso, se ela tiver 44 dígitos; não
/// valida.
let private extrairCampos (digitos: string) : NfeKeyInfo option =
    if digitos.Length <> tamanho then
        None
    else
        let cUF = digitos.Substring(0, 2)
        match State.GetByIbgeCode (box cUF) with
        | None -> None
        | Some estado ->
            let aamm = digitos.Substring(2, 4)
            let taxId = digitos.Substring(6, 14)
            let modelo = digitos.Substring(20, 2)
            let serie = int (digitos.Substring(22, 3))
            let numero = int (digitos.Substring(25, 9))
            let tipoEmissao = int (digitos.Substring(34, 1))
            let codigo = digitos.Substring(35, 8)
            let digitoVerificador = int (digitos.Substring(43, 1))
            Some
                { StateCode = estado.Code
                  Year = 2000 + int (aamm.Substring(0, 2))
                  Month = int (aamm.Substring(2, 2))
                  TaxId = taxId
                  Model = modelo
                  Series = serie
                  Number = numero
                  EmissionType = tipoEmissao
                  Code = codigo
                  CheckDigit = digitoVerificador
                  AuthorizationSite = if modelo = "62" || modelo = "66" then Some 0 else None }

/// Valida uma chave de acesso de DF-e (44 dígitos): NF-e (55), NFC-e (65),
/// CT-e (57), MDF-e (58), CT-e OS (67), GTV-e (64), BP-e (63), NF3e (66) e
/// NFCom (62); o CF-e-SAT (59) fica de fora.
///
/// - O `cUF` precisa ser um estado; o modelo, um dos citados acima; o tipo
///   de emissão, de 1 a 9.
/// - Um número de documento (`nNF`) todo em zero é rejeitado.
/// - O dígito verificador é um módulo 11 sobre os 43 primeiros dígitos.
/// - Regra B03-10 do MOC (só para NF-e/55 e NFC-e/65): o código numérico
///   (`cNF`) não pode ser todo repetido, sequencial crescente, sequencial
///   decrescente, nem igual ao número do documento (`nNF`).
let IsValid (value: string) : bool =
    let digitos = Parse value
    match extrairCampos digitos with
    | None -> false
    | Some campos ->
        let numeroDocumento = digitos.Substring(25, 9)
        let tipoEmissaoValido = campos.EmissionType >= 1 && campos.EmissionType <= 9
        let numeroNaoZerado = numeroDocumento |> Seq.exists (fun c -> c <> '0')
        let digitoEsperado = calcularDigitoVerificador (digitos.Substring(0, 43))
        modelosAceitos.Contains(campos.Model)
        && tipoEmissaoValido
        && numeroNaoZerado
        && campos.CheckDigit = digitoEsperado
        && codigoNumericoValido campos.Model campos.Code numeroDocumento

/// Interpreta uma chave de acesso de DF-e em seus campos; devolve None
/// sempre que `IsValid` devolveria falso.
let GetInfo (value: string) : NfeKeyInfo option =
    if IsValid value then extrairCampos (Parse value) else None
