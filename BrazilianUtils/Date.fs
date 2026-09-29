module BrazilianUtils.Date

open System
open System.Globalization
open BrazilianUtils

/// Um feriado brasileiro: nome, data e tipo (nacional, estadual, facultativo
/// ou religioso).
type Holiday =
    { Name: string
      Date: DateTime
      Type: string }

/// Opções para as funções de dia útil: se os feriados facultativos
/// (Carnaval, Corpus Christi) contam (por padrão sim) e um estado cujos
/// feriados também devem contar.
type BusinessDayOptions =
    { IncludeOptionalHolidays: bool option
      State: string option }

/// Parâmetros para verificar se uma data é feriado.
type IsHolidayParams =
    { Date: DateTime
      State: string option }

/// Calcula a data da Páscoa pelo algoritmo do computus gregoriano
/// (algoritmo anônimo/Meeus).
let private calcularPascoa (ano: int) : DateTime =
    let a = ano % 19
    let b = ano / 100
    let c = ano % 100
    let d = b / 4
    let e = b % 4
    let f = (b + 8) / 25
    let g = (b - f + 1) / 3
    let h = (19 * a + b - d - g + 15) % 30
    let i = c / 4
    let k = c % 4
    let l = (32 + 2 * e + 2 * i - h - k) % 7
    let m = (a + 11 * h + 22 * l) / 451
    let mes = (h + l - 7 * m + 114) / 31
    let dia = ((h + l - 7 * m + 114) % 31) + 1
    DateTime(ano, mes, dia)

let private anoValido (ano: int) : bool = ano >= 1900 && ano <= 2099

/// Retorna os feriados nacionais brasileiros de um ano, ordenados por data.
/// "Dia da Consciência Negra" (20 de novembro) é nacional a partir de 2024.
/// Devolve uma lista vazia para um ano fora do intervalo 1900-2099.
let GetHolidays (year: int) : Holiday list =
    if not (anoValido year) then
        []
    else
        let pascoa = calcularPascoa year
        let fixos =
            [ "Confraternização Universal", DateTime(year, 1, 1), "national"
              "Tiradentes", DateTime(year, 4, 21), "national"
              "Dia do Trabalho", DateTime(year, 5, 1), "national"
              "Independência do Brasil", DateTime(year, 9, 7), "national"
              "Nossa Senhora Aparecida", DateTime(year, 10, 12), "national"
              "Finados", DateTime(year, 11, 2), "national"
              "Proclamação da República", DateTime(year, 11, 15), "national"
              "Natal", DateTime(year, 12, 25), "national" ]

        let consciênciaNegra =
            if year >= 2024 then
                [ "Dia da Consciência Negra", DateTime(year, 11, 20), "national" ]
            else
                []

        let moveis =
            [ "Carnaval", pascoa.AddDays(-48.0), "optional"
              "Carnaval", pascoa.AddDays(-47.0), "optional"
              "Sexta-feira Santa", pascoa.AddDays(-2.0), "national"
              "Corpus Christi", pascoa.AddDays(60.0), "optional" ]

        (fixos @ consciênciaNegra @ moveis)
        |> List.map (fun (nome, data, tipo) -> { Name = nome; Date = data; Type = tipo })
        |> List.sortBy (fun feriado -> feriado.Date)

/// Verifica se uma data é feriado brasileiro (pela data local do calendário).
/// Um estado desconhecido é ignorado; não há tabela de feriados estaduais
/// nesta versão, então apenas feriados nacionais e facultativos são
/// considerados.
let IsHoliday (options: IsHolidayParams option) : bool =
    match options with
    | None -> false
    | Some parametros ->
        if not (anoValido parametros.Date.Year) then
            false
        else
            GetHolidays parametros.Date.Year
            |> List.exists (fun feriado -> feriado.Date.Date = parametros.Date.Date)

/// Verifica se uma data é dia útil brasileiro: não é sábado, domingo, nem um
/// feriado de `GetHolidays`.
let IsBusinessDay (value: DateTime) (options: BusinessDayOptions option) : bool =
    if not (anoValido value.Year) then
        false
    elif value.DayOfWeek = DayOfWeek.Saturday || value.DayOfWeek = DayOfWeek.Sunday then
        false
    else
        let incluirFacultativos =
            options |> Option.bind (fun o -> o.IncludeOptionalHolidays) |> Option.defaultValue true

        GetHolidays value.Year
        |> List.filter (fun feriado -> incluirFacultativos || feriado.Type <> "optional")
        |> List.forall (fun feriado -> feriado.Date.Date <> value.Date)

/// Soma uma quantidade de dias úteis brasileiros a uma data, pulando fins de
/// semana e feriados. Um `amount` de 0 devolve a mesma data, mesmo que não
/// seja dia útil; `amount` negativo anda para trás.
let AddBusinessDays (date: DateTime) (amount: int) (options: BusinessDayOptions option) : DateTime option =
    if not (anoValido date.Year) then
        None
    else
        let passo = if amount >= 0 then 1.0 else -1.0
        let mutable atual = date
        let mutable restante = abs amount
        let mutable valido = true
        while restante > 0 && valido do
            atual <- atual.AddDays(passo)
            if not (anoValido atual.Year) then
                valido <- false
            elif IsBusinessDay atual options then
                restante <- restante - 1
        if valido then Some atual else None

/// Subtrai uma quantidade de dias úteis brasileiros de uma data (o mesmo que
/// `AddBusinessDays` com o `amount` invertido).
let SubBusinessDays (date: DateTime) (amount: int) (options: BusinessDayOptions option) : DateTime option =
    AddBusinessDays date (-amount) options

/// Conta os dias úteis brasileiros entre duas datas: conta `earlierDate`
/// quando é dia útil e todo dia útil estritamente entre as duas;
/// `laterDate` nunca é contado. Negativo quando `laterDate` é anterior a
/// `earlierDate`.
let DifferenceInBusinessDays (laterDate: DateTime) (earlierDate: DateTime) (options: BusinessDayOptions option) : int option =
    if not (anoValido laterDate.Year) || not (anoValido earlierDate.Year) then
        None
    else
        let inicio = earlierDate.Date
        let fim = laterDate.Date
        if inicio = fim then
            Some 0
        else
            let de, ate, sinal = if inicio < fim then inicio, fim, 1 else fim, inicio, -1
            let mutable contagem = 0
            let mutable atual = de
            while atual < ate do
                if IsBusinessDay atual options then
                    contagem <- contagem + 1
                atual <- atual.AddDays(1.0)
            Some (sinal * contagem)

let private mesesPorExtenso =
    [| ""; "janeiro"; "fevereiro"; "março"; "abril"; "maio"; "junho"
       "julho"; "agosto"; "setembro"; "outubro"; "novembro"; "dezembro" |]

let private tentarAnalisarData (texto: string) : DateTime option =
    let formatos = [| "dd/MM/yyyy"; "yyyy-MM-dd" |]
    match DateTime.TryParseExact(texto, formatos, CultureInfo.InvariantCulture, DateTimeStyles.None) with
    | true, data -> Some data
    | false, _ -> None

/// Escreve uma data por extenso, em português brasileiro, minúsculo (ex.:
/// "primeiro de janeiro de dois mil e vinte e quatro").
///
/// `value` é `dd/mm/yyyy` ou ISO `yyyy-mm-dd`. Retorna uma string vazia para
/// uma data inválida ou malformada.
let ConvertToWords (value: string) : string =
    if isNull value then
        ""
    else
        match tentarAnalisarData (value.Trim()) with
        | None -> ""
        | Some data ->
            let diaTexto =
                if data.Day = 1 then "primeiro" else Number.ConvertToWords (float data.Day)
            sprintf "%s de %s de %s" diaTexto mesesPorExtenso.[data.Month] (Number.ConvertToWords (float data.Year))
