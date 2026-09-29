module BrazilianUtils.Tests.DateTests

open System
open BrazilianUtils
open Xunit

/// Mesmo algoritmo do computus gregoriano usado em `Date.fs`, duplicado
/// aqui só para calcular as datas de Carnaval esperadas; o alvo do teste é
/// `Date.GetHolidays`, não este cálculo.
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

[<Fact>]
let holidaysShouldIncludeBothCarnavalDays () =
    // Regressão: só a Terça-feira de Carnaval (Páscoa-47) era retornada;
    // a Segunda-feira de Carnaval (Páscoa-48), facultativa como a terça,
    // não existia na lista.
    let ano = 2026
    let pascoa = calcularPascoa ano
    let segundaCarnaval = pascoa.AddDays(-48.0).Date
    let tercaCarnaval = pascoa.AddDays(-47.0).Date

    let feriados = Date.GetHolidays ano

    Assert.Contains(feriados, (fun f -> f.Date.Date = segundaCarnaval && f.Type = "optional"))
    Assert.Contains(feriados, (fun f -> f.Date.Date = tercaCarnaval && f.Type = "optional"))

[<Fact>]
let holidayCountShouldBeThirteen () =
    // Antes desta correção eram 12 (faltava a Segunda-feira de Carnaval).
    Date.GetHolidays 2026
    |> List.length
    |> fun total -> Assert.Equal(13, total)
