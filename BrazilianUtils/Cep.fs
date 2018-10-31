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
    StringBuilder(clearValue).Insert(5, "-").ToString()
