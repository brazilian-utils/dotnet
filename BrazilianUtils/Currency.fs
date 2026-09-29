module BrazilianUtils.Currency

open System
open System.Globalization

// --- analisarValorMonetario ---
// Interpreta uma string monetária no padrão brasileiro ou americano.
// Regra: a última "," ou "." seguida de 1 a `precisao` dígitos é o separador
// decimal; qualquer outra ocorrência de "," ou "." é separador de milhar.
// Um valor sem nenhum separador é tratado conforme `semSeparadorComoCentavos`.
let private ehSeparador (c: char) = c = ',' || c = '.'

let private analisarValorMonetario (valorTexto: string) (semSeparadorComoCentavos: bool) (precisao: int) : decimal option =
    let textoAparado = valorTexto.Trim().Replace("R$", "").Trim()
    if textoAparado = "" then
        Some 0M
    else
        let negativo = textoAparado.StartsWith("-")
        let semSinal = if negativo then textoAparado.Substring(1) else textoAparado
        let indiceUltimoSeparador = semSinal |> Seq.tryFindIndexBack ehSeparador
        match indiceUltimoSeparador with
        | None ->
            let apenasDigitos = semSinal |> String.filter Char.IsDigit
            if apenasDigitos = "" then
                None
            else
                let numero = decimal (Int64.Parse(apenasDigitos))
                let resultado =
                    if semSeparadorComoCentavos then numero / (pown 10M precisao) else numero
                Some (if negativo then -resultado else resultado)
        | Some indice ->
            let candidatoDecimal = semSinal.Substring(indice + 1)
            let ehParteDecimal =
                candidatoDecimal.Length >= 1
                && candidatoDecimal.Length <= (max 2 precisao)
                && candidatoDecimal |> Seq.forall Char.IsDigit
            if not ehParteDecimal then
                None
            else
                let parteInteira = semSinal.Substring(0, indice) |> String.filter Char.IsDigit
                let parteInteiraFinal = if parteInteira = "" then "0" else parteInteira
                let textoNumero = parteInteiraFinal + "." + candidatoDecimal
                match Decimal.TryParse(textoNumero, NumberStyles.Number, CultureInfo.InvariantCulture) with
                | true, numero -> Some (if negativo then -numero else numero)
                | false, _ -> None

let private paraDecimal (valor: obj) : decimal option =
    match valor with
    | :? decimal as d -> Some d
    | :? int as i -> Some (decimal i)
    | :? int64 as i -> Some (decimal i)
    | :? float as f when not (Double.IsNaN f) && not (Double.IsInfinity f) -> Some (decimal f)
    | :? float32 as f -> Some (decimal f)
    | :? string as s -> analisarValorMonetario s false 2
    | _ -> None

/// Formata um valor (string ou número) como moeda brasileira (BRL), padrão `1.234,56`.
///
/// Não adiciona o símbolo "R$"; para entrada inválida (não numérica/não finita)
/// retorna uma string vazia.
///
/// Examples:
///     Format 1000.01M = "1.000,01"
///     Format "R$ 1.234,56" = "1.234,56"
let Format (value: obj) : string =
    match paraDecimal value with
    | None -> ""
    | Some numero ->
        try
            let cultura = CultureInfo("pt-BR")
            numero.ToString("N2", cultura)
        with _ -> ""

/// Interpreta uma string em formato de moeda brasileira e devolve o número correspondente.
///
/// Um valor sem separador decimal é interpretado como centavos (dividido por 100).
///
/// Examples:
///     Parse "R$ 1.234,56" = 1234.56
///     Parse "1234" = 12.34
let Parse (value: string) : float =
    match analisarValorMonetario value true 2 with
    | Some numero -> float numero
    | None -> 0.0

/// Converte um número para sua representação textual em português brasileiro.
let private numeroParaPortugues (n: int64) : string =
    let unidades = [| ""; "um"; "dois"; "três"; "quatro"; "cinco"; "seis"; "sete"; "oito"; "nove" |]
    let dezenasEspeciais = [| "dez"; "onze"; "doze"; "treze"; "quatorze"; "quinze"; "dezesseis"; "dezessete"; "dezoito"; "dezenove" |]
    let dezenas = [| ""; ""; "vinte"; "trinta"; "quarenta"; "cinquenta"; "sessenta"; "setenta"; "oitenta"; "noventa" |]
    let centenas = [| ""; "cento"; "duzentos"; "trezentos"; "quatrocentos"; "quinhentos"; "seiscentos"; "setecentos"; "oitocentos"; "novecentos" |]

    let rec converterAbaixoDeMil (n: int) : string =
        if n = 0 then ""
        elif n = 100 then "cem"
        elif n < 10 then unidades.[n]
        elif n < 20 then dezenasEspeciais.[n - 10]
        elif n < 100 then
            let dezena = n / 10
            let unidade = n % 10
            if unidade = 0 then dezenas.[dezena]
            else sprintf "%s e %s" dezenas.[dezena] unidades.[unidade]
        else
            let centena = n / 100
            let resto = n % 100
            if resto = 0 then centenas.[centena]
            else sprintf "%s e %s" centenas.[centena] (converterAbaixoDeMil resto)

    let rec converter (n: int64) (escala: int) : string =
        if n = 0L then ""
        else
            let nomesEscala = [| ""; "mil"; "milhão"; "bilhão"; "trilhão"; "quatrilhão" |]
            let nomesEscalaPlural = [| ""; "mil"; "milhões"; "bilhões"; "trilhões"; "quatrilhões" |]
            let divisor = pown 1000L escala
            let quociente = n / divisor
            let resto = n % divisor

            if quociente = 0L then converter n (escala - 1)
            else
                let parteAtual = int (quociente % 1000L)
                let partesSuperiores = quociente / 1000L

                let textoParte =
                    if escala = 1 && parteAtual = 1 then ""
                    elif parteAtual = 0 then ""
                    else converterAbaixoDeMil parteAtual

                let nomeEscala =
                    if escala = 0 then ""
                    elif escala = 1 then " mil"
                    elif parteAtual = 1 then sprintf " %s" nomesEscala.[escala]
                    else sprintf " %s" nomesEscalaPlural.[escala]

                let textoAtual =
                    if String.IsNullOrEmpty textoParte then nomeEscala.TrimStart()
                    else textoParte + nomeEscala

                let textoResto = converter resto (escala - 1)

                let textoSuperior =
                    if partesSuperiores > 0L then converter (partesSuperiores * 1000L) escala
                    else ""

                let conector =
                    if not (String.IsNullOrEmpty textoResto) && resto < 100L && resto > 0L then " e "
                    elif not (String.IsNullOrEmpty textoResto) then " "
                    else ""

                if String.IsNullOrEmpty textoSuperior then
                    (if String.IsNullOrEmpty textoAtual then textoResto else textoAtual + conector + textoResto)
                else
                    textoSuperior + " " + textoAtual + conector + textoResto

    if n = 0L then "zero"
    else
        let mutable escalaMaxima = 0
        let mutable temp = n
        while temp >= 1000L do
            temp <- temp / 1000L
            escalaMaxima <- escalaMaxima + 1
        converter n escalaMaxima

/// Converte um valor monetário em Reais para sua representação por extenso, em
/// português brasileiro, minúsculo e sem vírgulas entre os grupos.
///
/// - O valor é truncado (não arredondado) em 2 casas decimais.
/// - Retorna uma string vazia para entrada inválida ou acima de 999 trilhões de reais.
///
/// Examples:
///     ConvertToWords 1523.45M = "mil quinhentos e vinte e três reais e quarenta e cinco centavos"
///     ConvertToWords 1.00M = "um real"
///     ConvertToWords 0.50M = "cinquenta centavos"
///     ConvertToWords 0.00M = "zero reais"
let ConvertToWords (amount: decimal) : string =
    try
        let valorTruncado = Math.Floor(amount * 100M) / 100M

        if valorTruncado <> valorTruncado then ""
        elif abs valorTruncado > 999000000000000.00M then ""
        else
            let negativo = valorTruncado < 0M
            let valorAbsoluto = abs valorTruncado

            let reais = int64 (Math.Floor(valorAbsoluto))
            let centavos = int ((valorAbsoluto - decimal reais) * 100M)

            let partes = ResizeArray<string>()

            if reais > 0L then
                let textoReais = numeroParaPortugues reais
                let textoMoeda = if reais = 1L then "real" else "reais"
                let conector = if textoReais.EndsWith("lhão") || textoReais.EndsWith("lhões") then "de " else ""
                partes.Add(sprintf "%s %s%s" textoReais conector textoMoeda)

            if centavos > 0 then
                let textoCentavos = numeroParaPortugues (int64 centavos)
                let palavraCentavo = if centavos = 1 then "centavo" else "centavos"
                if reais > 0L then
                    partes.Add(sprintf "e %s %s" textoCentavos palavraCentavo)
                else
                    partes.Add(sprintf "%s %s" textoCentavos palavraCentavo)

            if reais = 0L && centavos = 0 then
                partes.Add("zero reais")

            let resultado = String.Join(" ", partes)
            if negativo then sprintf "menos %s" resultado else resultado
    with
    | :? InvalidOperationException
    | :? OverflowException
    | :? ArgumentException -> ""
