module BrazilianUtils.Number

open System

let private unidades = [| ""; "um"; "dois"; "três"; "quatro"; "cinco"; "seis"; "sete"; "oito"; "nove" |]
let private dezenasEspeciais = [| "dez"; "onze"; "doze"; "treze"; "quatorze"; "quinze"; "dezesseis"; "dezessete"; "dezoito"; "dezenove" |]
let private dezenas = [| ""; ""; "vinte"; "trinta"; "quarenta"; "cinquenta"; "sessenta"; "setenta"; "oitenta"; "noventa" |]
let private centenas = [| ""; "cento"; "duzentos"; "trezentos"; "quatrocentos"; "quinhentos"; "seiscentos"; "setecentos"; "oitocentos"; "novecentos" |]

let rec private converterAbaixoDeMil (n: int) : string =
    if n = 0 then ""
    elif n = 100 then "cem"
    elif n < 10 then unidades.[n]
    elif n < 20 then dezenasEspeciais.[n - 10]
    elif n < 100 then
        let dezena = n / 10
        let unidade = n % 10
        if unidade = 0 then dezenas.[dezena] else sprintf "%s e %s" dezenas.[dezena] unidades.[unidade]
    else
        let centena = n / 100
        let resto = n % 100
        if resto = 0 then centenas.[centena] else sprintf "%s e %s" centenas.[centena] (converterAbaixoDeMil resto)

let rec private converter (n: int64) (escala: int) : string =
    if n = 0L then
        ""
    else
        let nomesEscala = [| ""; "mil"; "milhão"; "bilhão"; "trilhão" |]
        let nomesEscalaPlural = [| ""; "mil"; "milhões"; "bilhões"; "trilhões" |]
        let divisor = pown 1000L escala
        let quociente = n / divisor
        let resto = n % divisor

        if quociente = 0L then
            converter n (escala - 1)
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
                if partesSuperiores > 0L then converter (partesSuperiores * 1000L) escala else ""

            let conector =
                if not (String.IsNullOrEmpty textoResto) && resto < 100L && resto > 0L then " e "
                elif not (String.IsNullOrEmpty textoResto) then " "
                else ""

            if String.IsNullOrEmpty textoSuperior then
                (if String.IsNullOrEmpty textoAtual then textoResto else textoAtual + conector + textoResto)
            else
                textoSuperior + " " + textoAtual + conector + textoResto

/// Converte um número inteiro (a parte fracionária é truncada) para sua
/// representação por extenso em português brasileiro.
///
/// Retorna uma string vazia para um valor fora do intervalo suportado
/// (-999.999.999.999.999 a 999.999.999.999.999) ou não finito.
///
/// Examples:
///     ConvertToWords 1235.0 = "mil duzentos e trinta e cinco"
///     ConvertToWords -3.0 = "menos três"
let ConvertToWords (value: float) : string =
    if Double.IsNaN value || Double.IsInfinity value then
        ""
    elif abs value > 999999999999999.0 then
        ""
    else
        let truncado = int64 (Math.Truncate value)
        if truncado = 0L then
            "zero"
        else
            let negativo = truncado < 0L
            let absoluto = abs truncado
            let mutable escalaMaxima = 0
            let mutable temp = absoluto
            while temp >= 1000L do
                temp <- temp / 1000L
                escalaMaxima <- escalaMaxima + 1
            let texto = converter absoluto escalaMaxima
            if negativo then sprintf "menos %s" texto else texto
