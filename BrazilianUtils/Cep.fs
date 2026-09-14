module BrazilianUtils.Cep

open System.Text

let private hasCepLength =
    let cepLength = 8;
    Helpers.hasLength cepLength

let IsValid cep =
    let clearValue = Helpers.stringToIntList cep
    hasCepLength clearValue

let Format cep =
    let clearValue = Helpers.OnlyNumbers cep
    let sb = StringBuilder(clearValue)
    if sb.Length >= 5 then sb.Insert(5, "-") |> ignore
    sb.ToString()
