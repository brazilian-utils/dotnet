module BrazilianUtils.Certidao

open System
open Helpers

type CertidaoInfo =
    { RegistryCns: string
      Acervo: string
      Service: string
      Year: int
      Type: string
      TypeCode: int
      Book: string
      Page: string
      Term: string
      CheckDigits: string }

let private tamanho = 32
let private posicoesMascara = [ 6; 9; 12; 17; 19; 25; 29; 37 ]

let private tiposLivro =
    Map.ofList
        [ 1, "birth"
          2, "marriage"
          3, "religiousMarriage"
          4, "death"
          5, "stillbirth"
          6, "banns"
          7, "other"
          8, "emancipation"
          9, "interdiction" ]

let private paraTexto (value: obj) : string =
    match value with
    | :? string as s -> s
    | :? int as i -> string i
    | :? int64 as i -> string i
    | _ -> ""

/// Calcula um dígito verificador módulo 11 sobre uma sequência de dígitos:
/// peso 2 para o dígito mais à direita, crescendo 1 a cada posição para a
/// esquerda (sem ciclo).
let private calcularDigitoVerificador (digitos: int list) : int =
    let n = digitos.Length
    let soma =
        digitos
        |> List.mapi (fun i digito -> digito * (n - i + 1))
        |> List.sum
    let resto = soma % 11
    if resto < 2 then 0 else 11 - resto

/// Remove a formatação da matrícula e mantém apenas dígitos, limitado a 32
/// dígitos.
let Parse (value: obj) : string =
    let apenasDigitos = OnlyNumbers (paraTexto value)
    if apenasDigitos.Length > tamanho then apenasDigitos.Substring(0, tamanho) else apenasDigitos

/// Formata a matrícula de uma certidão de registro civil no padrão impresso
/// do art. 473 (grupos 6-2-2-4-1-5-3-7-2 separados por espaço), aplicando a
/// máscara até onde os dígitos alcançarem. Não valida (use `IsValid`).
let Format (value: obj) : string =
    let digitos = OnlyNumbers (paraTexto value)
    let mutable resultado = digitos
    for posicao in posicoesMascara do
        if resultado.Length > posicao then
            resultado <- resultado.Substring(0, posicao) + " " + resultado.Substring(posicao)
    resultado

/// Separadores tolerados pela limpeza estrita usada por `IsValid`/`GetInfo`.
let private separadoresAceitosCertidao = set [ ' '; '.'; '-'; '/' ]

/// Remove a formatação da matrícula de forma estrita: tolera apenas espaço,
/// ponto, hífen e barra como separadores e rejeita (devolve `None`) assim que
/// encontra qualquer outro caractere que não seja dígito, em qualquer
/// posição. Usada só por `IsValid`/`GetInfo`; `Parse`/`Format` continuam
/// usando a limpeza permissiva de `Helpers.OnlyNumbers`.
let private limparMascaraCertidao (texto: string) : string option =
    if texto |> Seq.forall (fun c -> Char.IsDigit c || separadoresAceitosCertidao.Contains c) then
        Some (texto |> String.filter Char.IsDigit)
    else
        None

/// Extrai os campos de uma matrícula de 32 dígitos, sem validar.
let private extrairCampos (digitos: string) : CertidaoInfo option =
    if digitos.Length <> tamanho then
        None
    else
        let tipoLivroDigito = int (string digitos.[14])
        match tiposLivro |> Map.tryFind tipoLivroDigito with
        | None -> None
        | Some tipo ->
            Some
                { RegistryCns = digitos.Substring(0, 6)
                  Acervo = digitos.Substring(6, 2)
                  Service = digitos.Substring(8, 2)
                  Year = int (digitos.Substring(10, 4))
                  Type = tipo
                  TypeCode = tipoLivroDigito
                  Book = digitos.Substring(15, 5)
                  Page = digitos.Substring(20, 3)
                  Term = digitos.Substring(23, 7)
                  CheckDigits = digitos.Substring(30, 2) }

/// Valida a matrícula de 32 dígitos de uma certidão de registro civil (art.
/// 473 do Código Nacional de Normas da Corregedoria Nacional de Justiça).
///
/// - A limpeza é estrita: só espaço, ponto, hífen e barra são tolerados como
///   separadores; qualquer outro caractere não numérico em qualquer posição
///   invalida o valor inteiro (diferente de `Parse`, que apenas descarta
///   letras silenciosamente).
/// - O serviço precisa ser `55` e o tipo do livro, um dos nove livros
///   (`0` é rejeitado).
/// - `aceitar` restringe os tipos de livro aceitos (por padrão, todos).
let IsValid (value: obj) (aceitar: int list option) : bool =
    match limparMascaraCertidao (paraTexto value) with
    | None -> false
    | Some digitosBrutos ->
        let digitos =
            if digitosBrutos.Length > tamanho then digitosBrutos.Substring(0, tamanho) else digitosBrutos
        match extrairCampos digitos with
        | None -> false
        | Some campos ->
            let listaDigitos = digitos |> Seq.map (fun c -> int c - int '0') |> Seq.toList
            let base30 = listaDigitos.[0..29]
            let dv1 = calcularDigitoVerificador base30
            let dv2 = calcularDigitoVerificador (base30 @ [ dv1 ])
            let digitosEsperados = sprintf "%d%d" dv1 dv2
            let tipoAceito = aceitar |> Option.forall (fun lista -> List.contains campos.TypeCode lista)
            campos.Service = "55" && campos.CheckDigits = digitosEsperados && tipoAceito

/// Interpreta uma matrícula de certidão de registro civil em seus campos;
/// devolve None sempre que `IsValid` devolveria falso.
let GetInfo (value: obj) : CertidaoInfo option =
    if IsValid value None then extrairCampos (Parse value) else None
